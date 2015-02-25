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
            EnsureSettings(context);
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

        private void EnsureSettings(Context context)
        {
            if (!context.Settings.Any(a => a.Configuration == "Debug"))
            {
                var debug = new Setting
                {
                    Configuration = "Debug",
                    JarbooInfoEmail = "info@jarboo.com",

                    UseGoogleDrive = false,
                    GoogleClientId = "396782749590-7aikes2ama76giic9ck1ntt8tdbrr6t8.apps.googleusercontent.com",
                    GoogleClientSecret = "d45IRhkND0S0OQIJI5gS0pcy",
                    GoogleRefreshToken = "1/6hpzsG_HnbHWB_m_xm1Qwd02EUnCWzxlN5Rq-bbdFZA",
                    GoogleLocalUserId = "user",

                    UseMandrill = false,
                    MandrillApiKey = "IRWMe1g1dCTrG6uOZEy7gQ",
                    MandrillFrom = "admin@jarboo.com",
                    MandrillNewTaskTemplate = "a-customer-created-a-new-task",
                    MandrillPasswordRecoveryTemplate = "forgot-password-e-mail",
                    MandrillTaskResponsibleChangedNotificationSubject = "Task was assigned to you",
                    MandrillTaskResponsibleNotificationTemplate = "task-update"
                };
                context.Settings.Add(debug);
            }
            if (!context.Settings.Any(a => a.Configuration == "Staging"))
            {
                var staging = new Setting
                {
                    Configuration = "Staging",
                    JarbooInfoEmail = "info@jarboo.com",

                    UseGoogleDrive = true,
                    GoogleClientId = "396782749590-7aikes2ama76giic9ck1ntt8tdbrr6t8.apps.googleusercontent.com",
                    GoogleClientSecret = "d45IRhkND0S0OQIJI5gS0pcy",
                    GoogleRefreshToken = "1/uFhKbK-2wWnsLdpecq0QZnF7F5RrUYIWWupZoj9ew0MMEudVrK5jSpoR30zcRFq6",
                    GoogleLocalUserId = "user",

                    UseMandrill = true,
                    MandrillApiKey = "IRWMe1g1dCTrG6uOZEy7gQ",
                    MandrillFrom = "admin@jarboo.com",
                    MandrillNewTaskTemplate = "a-customer-created-a-new-task",
                    MandrillPasswordRecoveryTemplate = "forgot-password-e-mail",
                    MandrillTaskResponsibleChangedNotificationSubject = "Task was assigned to you",
                    MandrillTaskResponsibleNotificationTemplate = "task-update"
                };
                context.Settings.Add(staging);
            }
            if (!context.Settings.Any(a => a.Configuration == "Release"))
            {
                var release = new Setting
                {
                    Configuration = "Release",
                    JarbooInfoEmail = "info@jarboo.com",

                    UseGoogleDrive = true,
                    GoogleClientId = "396782749590-7aikes2ama76giic9ck1ntt8tdbrr6t8.apps.googleusercontent.com",
                    GoogleClientSecret = "d45IRhkND0S0OQIJI5gS0pcy",
                    GoogleRefreshToken = "1/uFhKbK-2wWnsLdpecq0QZnF7F5RrUYIWWupZoj9ew0MMEudVrK5jSpoR30zcRFq6",
                    GoogleLocalUserId = "user",

                    UseMandrill = true,
                    MandrillApiKey = "IRWMe1g1dCTrG6uOZEy7gQ",
                    MandrillFrom = "admin@jarboo.com",
                    MandrillNewTaskTemplate = "a-customer-created-a-new-task",
                    MandrillPasswordRecoveryTemplate = "forgot-password-e-mail",
                    MandrillTaskResponsibleChangedNotificationSubject = "Task was assigned to you",
                    MandrillTaskResponsibleNotificationTemplate = "task-update"
                };
                context.Settings.Add(release);
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
