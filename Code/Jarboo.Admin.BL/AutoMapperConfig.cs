using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using Jarboo.Admin.BL.Models;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL
{
    public class AutoMapperConfig
    {
        public class NullStringConverter : ITypeConverter<string, string>
        {
            public string Convert(ResolutionContext context)
            {
                return context.SourceValue as string ?? string.Empty;
            }
        }

        public static void RegisterMappers()
        {
            Mapper.CreateMap<string, string>().ConvertUsing<NullStringConverter>();

            Mapper.CreateMap<Customer, CustomerCreate>();
            Mapper.CreateMap<CustomerCreate, Customer>()
                .ForMember(x => x.CustomerId, x => x.Ignore());

            Mapper.CreateMap<Project, ProjectCreate>();
            Mapper.CreateMap<ProjectCreate, Project>()
                .ForMember(x => x.ProjectId, x => x.Ignore());

            Mapper.CreateMap<DAL.Entities.Task, TaskCreate>();
            Mapper.CreateMap<TaskCreate, DAL.Entities.Task>()
                .ForMember(x => x.TaskId, x => x.Ignore());
        }
    }
}
