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
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace Jarboo.Admin.DAL.Tests
{
    public static class ContextHelper
    {
        #region FillDb

        public static IUnitOfWork AddCustomer(this IUnitOfWork context, Action<Customer> edit = null, Action<Customer> afterSave = null)
        {
            var customer = new Customer()
                               {
                                   Name = "Customer",
                               };

            if (edit != null)
            {
                edit(customer);
            }

            context.Customers.Add(customer);
            context.SaveChanges();

            if (afterSave != null)
            {
                afterSave(customer);
            }

            return context;
        }
        public static IUnitOfWork AddProject(this IUnitOfWork context, Action<Project> edit = null, Action<Project> afterSave = null)
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

            if (edit != null)
            {
                edit(project);
            }

            context.Projects.Add(project);
            context.SaveChanges();

            if (afterSave != null)
            {
                afterSave(project);
            }

            return context;
        }
        public static IUnitOfWork AddEmployee(this IUnitOfWork context, Action<Employee> edit = null, Action<Employee> afterSave = null)
        {
            var employee = new Employee()
                               {
                                   FullName = "Employee",
                                   TrelloId = "TrelloId",
                                   Email = "email@email.com",
                                   Country = "Country",
                               };

            if (edit != null)
            {
                edit(employee);
            }

            context.Employees.Add(employee);
            context.SaveChanges();

            if (afterSave != null)
            {
                afterSave(employee);
            }

            return context;
        }
        public static IUnitOfWork AddEmployeePosition(this IUnitOfWork context, Action<EmployeePosition> edit = null, Action<EmployeePosition> afterSave = null)
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
            if (edit != null)
            {
                edit(position);
            }

            context.EmployeePositions.Add(position);
            context.SaveChanges();

            if (afterSave != null)
            {
                afterSave(position);
            }

            return context;
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
