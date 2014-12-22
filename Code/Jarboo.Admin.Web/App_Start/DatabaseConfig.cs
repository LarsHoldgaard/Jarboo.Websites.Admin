using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using Jarboo.Admin.DAL;
using Jarboo.Admin.Web.Infrastructure;

namespace Jarboo.Admin.Web.App_Start
{
    public static class DatabaseConfig
    {
        public static void ConfigureDatabase()
        {
            //Database.SetInitializer<Context>(new DropCreateDatabaseAlways<Context>());
            System.Data.Entity.Database.SetInitializer(new DataBaseInitializer());

            using (var context = new Context())
            {
                context.Database.Initialize(true);
            }
        }
    }
}