using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;

using Microsoft.AspNet.Identity;

namespace Jarboo.Admin.Web.Infrastructure
{
    public class DataBaseInitializer : IDatabaseInitializer<Context>
    {
        public void InitializeDatabase(Context context)
        {
            if (!context.Database.Exists())
            {
                throw new Exception("Missing database");
            }

            if (!context.Database.CompatibleWithModel(true))
            {
                throw new Exception("Incompatible database");
            }

            EnsureCustomers(context);
            EnsureAdmin(context);
        }

        private void EnsureCustomers(Context context)
        {
            foreach (var customer in Configuration.Instance.PredefinedCustomers)
            {
                if (context.Customers.Any(x => x.Name == customer))
                {
                    continue;
                }

                context.Customers.Add(new Customer()
                                          {
                                              Name = customer,
                                              DateCreated = DateTime.Now,
                                              DateModified = DateTime.Now
                                          });
            }
            context.SaveChanges();
        }
        private void EnsureAdmin(Context context)
        {
            var userManager = new UserManager(context);

            var admin = userManager.FindByName(Configuration.Instance.AdminEmail);
            if (admin == null)
            {
                admin = new User()
                               {
                                   DisplayName = "Administrator",
                                   UserName = Configuration.Instance.AdminEmail,
                                   Email = Configuration.Instance.AdminEmail,
                               };
                var result = userManager.Create(admin, Configuration.Instance.AdminPassword);
                CheckResult(result);
            }
            else
            {
                var newPasswordHash = userManager.PasswordHasher.HashPassword(Configuration.Instance.AdminPassword);
                if (newPasswordHash != admin.PasswordHash)
                {
                    admin.PasswordHash = newPasswordHash;
                    var result = userManager.Update(admin);
                    CheckResult(result);
                }
            }

            if (!userManager.IsInRole(admin.Id, UserRoles.Admin.ToString()))
            {
                userManager.AddToRole(admin.Id, UserRoles.Admin.ToString());
            }

            context.SaveChanges();
        }

        private static void CheckResult(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new Exception("Errors: " + String.Join(";", result.Errors));
            }
        }
    }
}