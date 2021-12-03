using Holism.Api;
using Holism.Business;
using Holism.Accounts.Business;
using Holism.Accounts.Models;
using Microsoft.AspNetCore.Mvc;

namespace Holism.Accounts.AdminApi
{
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
}
