namespace Accounts;

public class Repository
{
    public static Repository<Accounts.User> User
    {
        get
        {
            return new Repository<Accounts.User>(new AccountsContext());
        }
    }

    public static Repository<Accounts.UserView> UserView
    {
        get
        {
            return new Repository<Accounts.UserView>(new AccountsContext());
        }
    }
}
