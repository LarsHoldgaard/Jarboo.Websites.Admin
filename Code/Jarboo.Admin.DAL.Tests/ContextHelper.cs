using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

using FakeItEasy;

using Jarboo.Admin.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Jarboo.Admin.DAL.Tests
{
    public static class ContextHelper
    {
        #region FillDb

        public static User AddUser(this IUnitOfWork context, Action<User> beforeSave = null)
        {
            var id = Guid.NewGuid().ToString();
            var user = new User()
                           {
                               Id = id,
                               UserName = id,
                               DisplayName = id
                           };

            if (beforeSave != null)
            {
                beforeSave(user);
            }

            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }
        public static Customer AddCustomer(this IUnitOfWork context, Action<Customer> beforeSave = null)
        {
            var customer = new Customer()
                               {
                                   Name = "Customer",
                                   Country = "Country",
                                   Creator = "Creator",
                                   User = context.AddUser(
                                       x => x.Roles.Add(new IdentityUserRole()
                                                            {
                                                                RoleId = UserRoles.Customer.ToString(),
                                                                UserId = x.Id
                                                            })),
                               };

            if (beforeSave != null)
            {
                beforeSave(customer);
            }

            context.Customers.Add(customer);
            context.SaveChanges();
            return customer;
        }
        public static Project AddProject(this IUnitOfWork context, Action<Project> beforeSave = null)
        {
            var project = new Project()
                              {
                                  Name = "Project",
                                  BoardName = "BoardName",
                                  Customer = context.EnsureCustomer()
                              };

            if (beforeSave != null)
            {
                beforeSave(project);
            }

            context.Projects.Add(project);
            context.SaveChanges();
            return project;
        }
        public static Employee AddEmployee(this IUnitOfWork context, Action<Employee> beforeSave = null)
        {
            var employee = new Employee()
                               {
                                   FullName = "Employee",
                                   TrelloId = "TrelloId",
                                   Email = "email@email.com",
                                   Country = "Country",
                                   User = context.AddUser(
                                       x => x.Roles.Add(new IdentityUserRole()
                                       {
                                           RoleId = UserRoles.Employee.ToString(),
                                           UserId = x.Id
                                       })),
                               };

            if (beforeSave != null)
            {
                beforeSave(employee);
            }

            context.Employees.Add(employee);
            context.SaveChanges();
            return employee;
        }
        public static EmployeePosition AddEmployeePosition(this IUnitOfWork context, Action<EmployeePosition> beforeSave = null)
        {
            var position = new EmployeePosition()
                               {
                                   Employee = context.EnsureEmployee(),
                                   Position = Position.Architecture,
                               };
            if (beforeSave != null)
            {
                beforeSave(position);
            }

            context.EmployeePositions.Add(position);
            context.SaveChanges();

            return position;
        }
        public static Task AddTask(this IUnitOfWork context, Action<Task> beforeSave = null)
        {
            var task = new Task()
            {
                Title = "Task",
                Type = TaskType.Bug,
                Urgency = TaskUrgency.Medium,
                CardLink = "card",
                FolderLink = "folder",
                Size = 1,
                Project = context.EnsureProject(),
            };

            if (beforeSave != null)
            {
                beforeSave(task);
            }

            context.Tasks.Add(task);
            context.SaveChanges();

            return task;
        }
        public static TaskStep AddTaskStep(this IUnitOfWork context, Action<TaskStep> beforeSave = null)
        {
            var taskStep = new TaskStep()
            {
                Employee = context.EnsureEmployee(),
                Task = context.EnsureTask(),
                Step = TaskStep.First()
            };

            if (beforeSave != null)
            {
                beforeSave(taskStep);
            }

            context.TaskSteps.Add(taskStep);
            context.SaveChanges();

            return taskStep;
        }

        public static T Ensure<T>(this IQueryable<T> query, Func<T> create)
            where T : class, IBaseEntity
        {
            if (!query.Any())
            {
                return create();
            }

            return query.LastCreated();
        }
        internal static T LastCreated<T>(this IQueryable<T> query)
            where T : class, IBaseEntity
        {
            return query.OrderBy(x => x.DateCreated).AsEnumerable().Last();
        }

        public static User EnsureUser(this IUnitOfWork context, Action<User> beforeSave = null)
        {
            return Ensure(context.Users, () => context.AddUser(beforeSave));
        }
        public static Customer EnsureCustomer(this IUnitOfWork context, Action<Customer> beforeSave = null)
        {
            return Ensure(context.Customers, () => context.AddCustomer(beforeSave));
        }
        public static Project EnsureProject(this IUnitOfWork context, Action<Project> beforeSave = null)
        {
            return Ensure(context.Projects, () => context.AddProject(beforeSave));
        }
        public static Employee EnsureEmployee(this IUnitOfWork context, Action<Employee> beforeSave = null)
        {
            return Ensure(context.Employees, () => context.AddEmployee(beforeSave));
        }
        public static Task EnsureTask(this IUnitOfWork context, Action<Task> beforeSave = null)
        {
            return Ensure(context.Tasks, () => context.AddTask(beforeSave));
        }

        #endregion

        #region internal

        internal static void IterateDbSets(IUnitOfWork context, MethodInfo callback, object caller)
        {
            foreach (var property in typeof(IUnitOfWork).GetProperties())
            {
                if (!property.PropertyType.IsGenericType || property.PropertyType.GetGenericTypeDefinition() != typeof(IDbSet<>))
                {
                    continue;
                }

                var exp = Expression.MakeMemberAccess(Expression.Constant(context), property);

                callback.MakeGenericMethod(property.PropertyType.GetGenericArguments()[0])
                    .Invoke(caller, new object[] { exp });
            }
        }

        #endregion

        public static IUnitOfWork Create()
        {
#if DEBUG
            var unitOfWork = FakeContext.Create();
#else
            var unitOfWork = RealContextWrapper.Create();
#endif
            foreach (var userRole in unitOfWork.Roles)
            {
                unitOfWork.Roles.Remove(userRole);
            }
            foreach (var roleName in Enum.GetNames(typeof(UserRoles)))
            {
                unitOfWork.Roles.Add(new UserRole()
                {
                    Id = roleName,
                    Name = roleName
                });
            }
            unitOfWork.SaveChanges();

            return unitOfWork;
        }
        public static IUserStore<User, string> CreateUserStore(this IUnitOfWork context)
        {
#if DEBUG
            return new FakeUserStore(context);
#else
            return new UserStore((context as IUnitOfWorkEx).GetContext());
#endif
        }
    }
}
