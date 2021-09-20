using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Holism.Accounts.DataAccess;
using Holism.Accounts.Models;
using Holism.Business;
using Holism.DataAccess;
using Holism.Infra;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;

namespace Holism.Accounts.Business
{
    public class UserViewBusiness : ReadBusiness<UserView>
    {
        protected override ReadRepository<UserView> ReadRepository => Repository.UserView;
    }
}