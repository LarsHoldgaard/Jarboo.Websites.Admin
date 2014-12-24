using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using Jarboo.Admin.BL.Models;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL
{
    public static class AutoMapperConfig
    {
        public class NullStringConverter : ITypeConverter<string, string>
        {
            public string Convert(ResolutionContext context)
            {
                return context.SourceValue as string ?? string.Empty;
            }
        }

        /*public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);
            var existingMaps = Mapper.GetAllTypeMaps().First(x => x.SourceType.Equals(sourceType) && x.DestinationType.Equals(destinationType));
            foreach (var property in existingMaps.GetUnmappedPropertyNames())
            {
                expression.ForMember(property, opt => opt.Ignore());
            }
            return expression;
        }
        public static IMappingExpression<TSource, TDestination> IgnoreAll<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            foreach (var property in typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                expression.ForMember(property.Name, opt => opt.Ignore());
            }
            return expression;
        }*/

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

            Mapper.CreateMap<Employee, EmployeeEdit>();
            Mapper.CreateMap<EmployeeEdit, Employee>();
        }
    }
}
