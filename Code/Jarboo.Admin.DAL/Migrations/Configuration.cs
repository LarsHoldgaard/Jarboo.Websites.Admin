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
            EnsureRoles(context);
            EnsureEmployeeUsers(context);
        }
        private void EnsureRoles(Context context)
        {
            var roleManager = new RoleManager(context);

            foreach (var roleName in Enum.GetNames(typeof(Entities.UserRoles)))
            {
                if (roleManager.RoleExists(roleName))
                {
                    continue;
                }

                var result = roleManager.Create(new UserRole() { Name = roleName, });
                CheckResult(result);
            }

            context.SaveChanges();
        }
        private void EnsureEmployeeUsers(Context context)
        {
            var userManager = new UserManager(context);

            foreach (var userlessEmployee in context.Employees.Where(x => x.User == null).ToList())
            {
                var user = new User()
                               {
                                   PasswordHash = userManager.PasswordHasher.HashPassword("test1234"),
                                   UserName = userlessEmployee.Email,
                                   Email = userlessEmployee.Email,
                                   DisplayName = userlessEmployee.FullName,
                                   Employee = userlessEmployee
                               };

                var result = userManager.Create(user);
                this.CheckResult(result);

                result = userManager.AddToRole(user.Id, Entities.UserRoles.Employee.ToString());
                this.CheckResult(result);
            }

            context.SaveChanges();
        }

        private void CheckResult(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new Exception("Errors: " + string.Join(";", result.Errors));
            }
        }
    }
}
