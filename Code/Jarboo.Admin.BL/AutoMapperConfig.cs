﻿using System;
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
            Mapper.CreateMap<CustomerCreate, User>()
                .ForMember(x => x.DisplayName, x => x.MapFrom(y => y.Name))
                .ForMember(x => x.UserName, x => x.MapFrom(y => y.Email));

            Mapper.CreateMap<Project, ProjectEdit>();
            Mapper.CreateMap<ProjectEdit, Project>();

            Mapper.CreateMap<Setting, SettingEdit>();
            Mapper.CreateMap<SettingEdit, Setting>();

            Mapper.CreateMap<DAL.Entities.Task, TaskEdit>();
            Mapper.CreateMap<TaskEdit, DAL.Entities.Task>();

            Mapper.CreateMap<Employee, EmployeeEdit>()
                .ForMember(x => x.Positions, x => x.MapFrom(y => y.Positions.Select(z => z.Position).ToList()));
            Mapper.CreateMap<EmployeeEdit, Employee>()
                .ForMember(x => x.Positions, x => x.MapFrom(y =>
                    y.Positions.Select(z => new EmployeePosition()
                    {
                        EmployeeId = y.EmployeeId,
                        Position = z
                    })));

            Mapper.CreateMap<Employee, EmployeeCreate>()
                .ForMember(x => x.Positions, x => x.MapFrom(y => y.Positions.Select(z => z.Position).ToList()));
            Mapper.CreateMap<EmployeeCreate, Employee>()
                .ForMember(x => x.EmployeeId, x => x.Ignore())
                .ForMember(x => x.Positions, x => x.MapFrom(y =>
                    y.Positions.Select(z => new EmployeePosition()
                    {
                        EmployeeId = y.EmployeeId,
                        Position = z
                    })));

            Mapper.CreateMap<Documentation, DocumentationEdit>();
            Mapper.CreateMap<DocumentationEdit, Documentation>();

            Mapper.CreateMap<User, UserEdit>()
                .ForMember(x => x.UserId, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.Name, x => x.MapFrom(y => y.DisplayName));
            Mapper.CreateMap<UserEdit, User>()
                .ForMember(x => x.DisplayName, x => x.MapFrom(y => y.Name))
                .ForMember(x => x.UserName, x => x.MapFrom(y => y.Email));
            Mapper.CreateMap<User, UserCustomerEdit>()
                .ForMember(x => x.UserId, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.Name, x => x.MapFrom(y => y.DisplayName))
                .ForMember(x => x.Country, x => x.MapFrom(y => y.Customer.Country))
                .ForMember(x => x.Creator, x => x.MapFrom(y => y.Customer.Creator));

            Mapper.CreateMap<User, UserPasswordChange>()
                .ForMember(x => x.UserId, x => x.MapFrom(y => y.Id));
            
            Mapper.CreateMap<User, UserPasswordSet>()
                .ForMember(x => x.UserId, x => x.MapFrom(y => y.Id));

            Mapper.CreateMap<SpentTimeOnTask, SpentTime>()
                    .ForMember(x => x.Role, x => x.MapFrom(y => y.Roles)); 
            Mapper.CreateMap<SpentTime, SpentTimeOnTask>()
                     .ForMember(x => x.Roles, x => x.MapFrom(y => y.Role)); 

            Mapper.CreateMap<SpentTimeOnProject, SpentTime>();
            Mapper.CreateMap<SpentTime, SpentTimeOnProject>();

            Mapper.CreateMap<Question, Question>();
            Mapper.CreateMap<Question, Question>();

            Mapper.CreateMap<Answer, Answer>();
            Mapper.CreateMap<Answer, Answer>();

            Mapper.CreateMap<Comment, Comment>();
            Mapper.CreateMap<Comment, Comment>();
 
        }
    }
}
