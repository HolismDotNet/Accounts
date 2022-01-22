












namespace Holism.Accounts.Business
{
    public class UserViewBusiness : ReadBusiness<UserView>
    {
        protected override ReadRepository<UserView> ReadRepository => Repository.UserView;
    }
}