namespace Holism.Accounts.DataAccess;

public class Repository
{
    public static Repository<User> User
    {
        get
        {
            return new Repository<User>(new AccountsContext());
        }
    }

    public static Repository<UserView> UserView
    {
        get
        {
            return new Repository<UserView>(new AccountsContext());
        }
    }
}
