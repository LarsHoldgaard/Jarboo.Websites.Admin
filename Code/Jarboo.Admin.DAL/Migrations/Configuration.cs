using Jarboo.Admin.DAL.Entities;

using Microsoft.AspNet.Identity;

namespace Jarboo.Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Jarboo.Admin.DAL.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Jarboo.Admin.DAL.Context";
        }

        protected override void Seed(Jarboo.Admin.DAL.Context context)
        {
            var roleManager = new RoleManager(context);

            foreach (var roleName in Enum.GetNames(typeof(Entities.UserRoles)))
            {
                if (roleManager.RoleExists(roleName))
                {
                    continue;
                }

                roleManager.Create(new UserRole()
                                       {
                                           Name = roleName,
                                       });
            }

            context.SaveChanges();
        }
    }
}
