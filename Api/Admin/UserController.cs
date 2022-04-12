namespace Accounts;

public class UserController : ReadController<UserView>
{
    public override ReadBusiness<UserView> ReadBusiness => new UserReadBusiness();

    [HttpPost]
    public IActionResult Sync()
    {
        UserBusiness.SyncUsers();
        return OkJson();
    }
}
