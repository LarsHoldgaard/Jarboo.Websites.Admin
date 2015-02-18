using AutoMapper;
using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Web.Models.Comment;
using Jarboo.Admin.Web.Models.Question;
using Jarboo.Admin.Web.Models.Report;
using Jarboo.Admin.Web.Models.Task;
using Jarboo.Admin.Web.Models.Account;
using Jarboo.Admin.Web.Models.Time;

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

            Mapper.CreateMap<DAL.Entities.Question, QuestionViewModel>();
            Mapper.CreateMap<DAL.Entities.SpentTime, TimeViewModel>();
            Mapper.CreateMap<DAL.Entities.Comment, CommentViewModel>();
        }
    }
}