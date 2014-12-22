using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jarboo.Admin.BL.Models;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.Web.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterMappers()
        {
            BL.AutoMapperConfig.RegisterMappers();
        }
    }
}