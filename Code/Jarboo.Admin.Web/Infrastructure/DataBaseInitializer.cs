using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;

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
        }

        private void EnsureCustomers(Context context)
        {
            foreach (var customer in Configuration.PredefinedCustomers)
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
    }
}