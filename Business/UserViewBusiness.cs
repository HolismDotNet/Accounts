namespace Accounts;

public class UserViewBusiness : ReadBusiness<UserView>
{
    protected override ReadRepository<UserView> ReadRepository => Repository.UserView;
}
