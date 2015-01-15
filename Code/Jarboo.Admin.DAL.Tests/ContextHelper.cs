using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

using Jarboo.Admin.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;

namespace Jarboo.Admin.DAL.Tests
{
    public static class ContextHelper
    {
        #region FillDb

        public static Customer AddCustomer(this IUnitOfWork context, Action<Customer> beforeSave = null)
        {
            var customer = new Customer()
                               {
                                   Name = "Customer",
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
            if (!context.Customers.Any())
            {
                context.AddCustomer();
            }

            var customer = context.Customers.OrderBy(x => x.DateCreated).AsEnumerable().Last();
            var project = new Project()
                              {
                                  Name = "Project",
                                  Customer = customer
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
            if (!context.Employees.Any())
            {
                context.AddEmployee();
            }

            var employee = context.Employees.OrderBy(x => x.DateCreated).AsEnumerable().Last();
            var position = new EmployeePosition()
                               {
                                   Employee = employee,
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
            if (!context.Projects.Any())
            {
                context.AddProject();
            }

            var project = context.Projects.OrderBy(x => x.DateCreated).AsEnumerable().Last();
            var task = new Task()
            {
                Title = "Task",
                Type = TaskType.Bug,
                Urgency = TaskUrgency.Medium,
                CardLink = "card",
                FolderLink = "folder",
                Size = 1,
                Project = project,
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
            if (!context.Employees.Any())
            {
                context.AddEmployee();
            }
            var employee = context.Employees.OrderBy(x => x.DateCreated).AsEnumerable().Last();

            if (!context.Tasks.Any())
            {
                context.AddTask();
            }
            var task = context.Tasks.OrderBy(x => x.DateCreated).AsEnumerable().Last();

            var taskStep = new TaskStep()
            {
                Employee = employee,
                Task = task,
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
            return FakeContext.Create();
#else
            return RealContextWrapper.Create();
#endif
        }
    }
}
