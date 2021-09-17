using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Holism.Accounts.DataAccess;
using Holism.Accounts.Models;
using Holism.Business;
using Holism.DataAccess;
using Holism.Infra;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;

namespace Holism.Accounts
{
    public class UserBusiness : Business<User, User>
    {
        protected override Repository<User> WriteRepository => Repository.User;

        protected override ReadRepository<User> ReadRepository =>
            Repository.User;

        private static Dictionary<Guid, DateTime> syncs = new Dictionary<Guid, DateTime>();

        public User GetUserByKeycloakGuid(Guid keycloakGuid)
        {
            var user = Get(i => i.KeycloakGuid == keycloakGuid);
            if (user == null)
            {
                user =
                    new User {
                        KeycloakGuid = keycloakGuid
                    };
                Create (user);
            }
            return user;
        }

        public void SyncUser(Guid keycloakGuid)
        {
            var user = GetUserByKeycloakGuid(keycloakGuid);
            if (syncs.ContainsKey(user.KeycloakGuid) && DateTime.Now.Subtract(syncs[user.KeycloakGuid]).TotalHours < 12)
            {
                return;
            }
            if (user.LastSyncDate != null && DateTime.Now.Subtract(user.LastSyncDate.Value).TotalHours < 12)
            {
                return;
            }
            var parameters = new Dictionary<string, string>();
            parameters.Add("client_id", "admin-cli");
            parameters.Add("username", Config.GetSetting("KeycloakAdminUser"));
            parameters.Add("password", Config.GetSetting("KeycloakAdminPassword"));
            parameters.Add("grant_type", "password");
            var client = new HttpClient();
            var req =
                new HttpRequestMessage(HttpMethod.Post,
                    @$"{Config.GetSetting("KeycloakUrl").Trim('/')}/auth/realms/master/protocol/openid-connect/token")
                { Content = new FormUrlEncodedContent(parameters) };
            var response = client.SendAsync(req).Result;
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ServerException(@$"Keycloak error {response.StatusCode}");
            }
            var json = response.Content.ReadAsStringAsync().Result.Deserialize();
            var token = json.GetProperty("access_token").GetString();
            

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            response = client.GetAsync(@$"{Config.GetSetting("KeycloakUrl").Trim('/')}/auth/admin/realms/{Config.GetSetting("Realm")}/users/{user.KeycloakGuid}").Result;
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ServerException(@$"Keycloak error {response.StatusCode}");
            }
            json = response.Content.ReadAsStringAsync().Result.Deserialize();
            if (json.TryGetProperty("firstName", out var firstName))
            {
                user.FirstName = firstName.GetString();
            }
            if (json.TryGetProperty("lastName", out var lastName))
            {
                user.LastName = lastName.GetString();
            }
            if (json.TryGetProperty("email", out var email)) 
            {
                user.Email = email.GetString();
            }
            if (json.TryGetProperty("emailVerified", out var emailVerified))
            {
                user.IsEmailVerified = emailVerified.GetBoolean();
            }
            user.LastSyncDate = DateTime.Now;
            Update(user);
            if (syncs.ContainsKey(user.KeycloakGuid))
            {
                syncs[user.KeycloakGuid] = DateTime.Now;
            }
            else 
            {
                syncs.Add(user.KeycloakGuid, DateTime.Now);
            }
        }
    }
}
