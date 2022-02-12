namespace Accounts;

public class UserViewBusiness : ReadBusiness<UserView>
{
    protected override Read<UserView> Read => Repository.UserView;
}
