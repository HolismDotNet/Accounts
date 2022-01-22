namespace Accounts;

public class UserView : IGuidEntity
{
    public UserView()
    {
        RelatedItems = new ExpandoObject();
    }

    public long Id { get; set; }

    public string UserName { get; set; }

    public Guid Guid { get; set; }

    public string Email { get; set; }

    public bool? IsEmailVerified { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string DisplayName { get; set; }

    public DateTime? LastSyncUtcDate { get; set; }

    public dynamic RelatedItems { get; set; }
}
