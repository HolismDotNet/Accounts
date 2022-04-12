namespace Accounts;

public class UserReadBusiness : ReadBusiness<UserView>
{
    protected override Read<UserView> Read => Repository.UserView;
}
