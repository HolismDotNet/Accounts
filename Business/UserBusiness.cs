using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Holism.Business;
using Holism.DataAccess;
using Holism.Accounts.DataAccess;
using Holism.Accounts.Models;
using Holism.Infra;

namespace Holism.Accounts
{
    public class UserBusiness : Business<User, User>
    {
        protected override Repository<User> WriteRepository =>
            Repository.User;

        protected override ReadRepository<User> ReadRepository =>
            Repository.User;

        public User GetUserByKeycloakGuid(Guid keycloakGuid)
        {
            var user = Get(i => i.KeycloakGuid == keycloakGuid);
            if (user == null)
            {
                user = new User
                {
                    KeycloakGuid = keycloakGuid,
                    FirstName = "",
                    LastName = ""
                };
                Create(user);
            }
            return user;
        }
    }
}
