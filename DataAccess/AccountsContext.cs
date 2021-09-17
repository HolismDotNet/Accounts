using System.Collections.Generic;
using Holism.Accounts.Models;
using Holism.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Holism.Accounts.DataAccess
{
    public class AccountsContext : DatabaseContext
    {
        public override string ConnectionStringName => "Accounts";

        public DbSet<User> Users { get; set; }

        public DbSet<UserView> UserViews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
