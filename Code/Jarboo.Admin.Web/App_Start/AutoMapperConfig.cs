using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;

using Jarboo.Admin.BL.Models;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Web.Models.Task;
using Jarboo.Admin.Web.Models.Account;

namespace Jarboo.Admin.Web.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterMappers()
        {
            BL.AutoMapperConfig.RegisterMappers();

            Mapper.CreateMap<DAL.Entities.Task, TaskVM>();
            Mapper.CreateMap<DAL.Entities.User, UserVM>();
        }
    }
}