using AutoMapper;
using Jarboo.Admin.Web.Models.Report;
using Jarboo.Admin.Web.Models.Task;
using Jarboo.Admin.Web.Models.Account;

namespace Jarboo.Admin.Web.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterMappers()
        {
            BL.AutoMapperConfig.RegisterMappers();

            Mapper.CreateMap<DAL.Entities.Task, TaskViewModel>();
            Mapper.CreateMap<DAL.Entities.User, UserViewModel>();
            Mapper.CreateMap<DAL.Entities.Task, ReportViewModel>();
        }
    }
}