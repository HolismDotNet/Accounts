namespace Accounts;

public class UserController : ReadController<UserView>
{
    public override ReadBusiness<UserView> ReadBusiness => new UserViewBusiness();

    [HttpPost]
    public IActionResult Sync()
    {
        UserBusiness.SyncUsers();
        return OkJson();
    }
}
