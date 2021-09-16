using Holism.Accounts.Models;
using Holism.DataAccess;

namespace Holism.Accounts.DataAccess
{
    public class Repository
    {
        public static Repository<User> User
        {
            get
            {
                return new Holism.DataAccess.Repository<User
                >(new AccountsContext());
            }
        }
    }
}
