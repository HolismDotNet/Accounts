namespace Accounts;

public class User : IEntity
{
    public User()
    {
        RelatedItems = new ExpandoObject();
    }

    public long Id { get; set; }

    public Guid KeycloakGuid { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public bool? IsEmailVerified { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public Guid? ProfilePictureGuid { get; set; }

    public DateTime? LastSyncUtcDate { get; set; }

    public dynamic RelatedItems { get; set; }
}
