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
        public static FakeContext AddCustomer(this FakeContext context)
        {
            return context.Add(new Customer()
            {
                CustomerId = 1,
                Name = "Customer",
            });
        }
        public static FakeContext AddProject(this FakeContext context)
        {
            return context.Add(new Project()
            {
                ProjectId = 1,
                Name = "Project",
                CustomerId = 1,
            });
        }
        public static FakeContext AddEmployee(this FakeContext context, Action<Employee> edit = null)
        {
            var employee = new Employee()
                               {
                                   EmployeeId = 1,
                                   FullName = "Employee",
                                   TrelloId = "TrelloId",
                                   Email = "Email",
                                   Country = "Country",
                               };

            if (edit != null)
            {
                edit(employee);
            }

            return context.Add(employee);
        }
        public static FakeContext AddPosition(this FakeContext context, Action<EmployeePosition> edit = null)
        {
            var position = new EmployeePosition()
                               {
                                   EmployeeId = 1,
                                   Position = Position.Architecture,
                               };
            if (edit != null)
            {
                edit(position);
            }

            return context.Add(position);
        }
    }
}
