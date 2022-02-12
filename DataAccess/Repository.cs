namespace Accounts;

public class Repository
{
    public static Write<Accounts.User> User
    {
        get
        {
            return new Write<Accounts.User>(new AccountsContext());
        }
    }

    public static Write<Accounts.UserView> UserView
    {
        get
        {
            return new Write<Accounts.UserView>(new AccountsContext());
        }
    }
}
