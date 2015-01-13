using Jarboo.Admin.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.DAL.Tests
{
    public static class FakeContextExtensions
    {
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

            var project = new Project()
                              {
                                  Name = "Project",
                                  Customer = context.Customers.OrderBy(x => x.DateCreated).Last()
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
                                   Email = "Email",
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


            var position = new EmployeePosition()
                               {
                                   Employee = context.Employees.OrderBy(x => x.DateCreated).Last(),
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
    }
}
