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
        public static FakeContext AddEmployee(this FakeContext context)
        {
            return context.Add(new Employee()
            {
                EmployeeId = 1,
                FullName = "Employee",
                TrelloId = "TrelloId",
                Email = "Email",
                Country = "Country",
            });
        }
    }
}
