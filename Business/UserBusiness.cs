using System.Net.Http.Headers;

namespace Accounts;

public class UserBusiness : Business<User, User>
{
    protected override Write<User> Write => Repository.User;
    
    protected override Read<User> Read => Repository.User;

    private static Dictionary<Guid, DateTime> syncs = new Dictionary<Guid, DateTime>();

    private static string GetKeycloakToken()
    {
        var parameters = new Dictionary<string, string>();
        parameters.Add("client_id", "admin-cli");
        parameters.Add("username", InfraConfig.GetSetting("KeycloakAdminUser"));
        parameters.Add("password", InfraConfig.GetSetting("KeycloakAdminPassword"));
        parameters.Add("grant_type", "password");
        var client = new HttpClient();
        var req =
            new HttpRequestMessage(HttpMethod.Post,
                @$"{InfraConfig.GetSetting("KeycloakUrl").Trim('/')}/auth/realms/master/protocol/openid-connect/token")
            { Content = new FormUrlEncodedContent(parameters) };
        var response = client.SendAsync(req).Result;
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new ServerException(@$"Keycloak error {response.StatusCode}");
        }
        var json = response.Content.ReadAsStringAsync().Result.Deserialize();
        return json.GetProperty("access_token").GetString();

    }

    private static System.Text.Json.JsonElement CallKeycloak(string url)
    {
        var token = GetKeycloakToken();
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        var response = client.GetAsync(@$"{InfraConfig.GetSetting("KeycloakUrl").Trim('/')}/auth/admin/realms/{InfraConfig.GetSetting("Realm")}/{url}").Result;
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new ServerException(@$"Keycloak error {response.StatusCode}");
        }
        return response.Content.ReadAsStringAsync().Result.Deserialize();
    }

    private User GetUserByKeycloakGuid(Guid keycloakGuid)
    {
        var user = Read.Get(i => i.KeycloakGuid == keycloakGuid);
        if (user == null)
        {
            user =
                new User
                {
                    KeycloakGuid = keycloakGuid
                };
            Create(user);
        }
        return user;
    }

    public static void SyncUser(Guid keycloakGuid)
    {
        if (InfraConfig.IsDeveloping)
        {
            return;
        }
        lock (lockToken)
        {
            if (syncs.ContainsKey(keycloakGuid) && UniversalDateTime.Now.Subtract(syncs[keycloakGuid]).TotalHours < 12)
            {
                return;
            }
        }
        var user = new UserBusiness().GetUserByKeycloakGuid(keycloakGuid);
        if (user.LastSyncUtcDate != null && UniversalDateTime.Now.Subtract(user.LastSyncUtcDate.Value).TotalHours < 12)
        {
            lock (lockToken)
            {
                if (syncs.ContainsKey(keycloakGuid))
                {
                    syncs[keycloakGuid] = user.LastSyncUtcDate.Value;
                }
                else
                {
                    syncs.Add(keycloakGuid, user.LastSyncUtcDate.Value);
                }
            }
            return;
        }


        var response = CallKeycloak($"users/{user.KeycloakGuid}");
        var json = response;

        if (json.TryGetProperty("firstName", out var firstName))
        {
            user.FirstName = firstName.GetString();
        }
        if (json.TryGetProperty("lastName", out var lastName))
        {
            user.LastName = lastName.GetString();
        }

        if (json.TryGetProperty("email", out var username))
        {
            user.UserName = username.GetString();
        }
        if (json.TryGetProperty("email", out var email))
        {
            user.Email = email.GetString();
        }
        if (json.TryGetProperty("emailVerified", out var emailVerified))
        {
            user.IsEmailVerified = emailVerified.GetBoolean();
        }
        user.LastSyncUtcDate = UniversalDateTime.Now;
        new UserBusiness().Update(user);
        lock (lockToken)
        {
            if (syncs.ContainsKey(user.KeycloakGuid))
            {
                syncs[user.KeycloakGuid] = UniversalDateTime.Now;
            }
            else
            {
                syncs.Add(user.KeycloakGuid, UniversalDateTime.Now);
            }
        }
    }

    public static void SyncUsers()
    {
        var response = CallKeycloak($"users");
        var usersKeycloak = response.EnumerateArray();

        foreach (var userItem in usersKeycloak)
        {
            var user = new User();
            if (userItem.TryGetProperty("id", out var keycloakGuid))
            {
                var userGuid = keycloakGuid.GetGuid();
                user = new UserBusiness().GetUserByKeycloakGuid(userGuid);
            }
            else
            {
                continue;
            }
            if (userItem.TryGetProperty("firstName", out var firstName))
            {
                user.FirstName = firstName.GetString();
            }
            if (userItem.TryGetProperty("lastName", out var lastName))
            {
                user.LastName = lastName.GetString();
            }
            if (userItem.TryGetProperty("email", out var username))
            {
                user.UserName = username.GetString();
            }
            if (userItem.TryGetProperty("email", out var email))
            {
                user.Email = email.GetString();
            }
            if (userItem.TryGetProperty("emailVerified", out var emailVerified))
            {
                user.IsEmailVerified = emailVerified.GetBoolean();
            }
            user.LastSyncUtcDate = UniversalDateTime.Now;

            new UserBusiness().Update(user);
        }
    }
}
