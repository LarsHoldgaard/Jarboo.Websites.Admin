using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Extensions;

using NUnit.Framework;

namespace Jarboo.Admin.DAL.Tests.Extensions
{
    [TestFixture]
    public class CustomerExtensionsTests
    {
        [Test]
        public void ByProject_WhenNotExists_ReturnsNull()
        {
            using (var context = ContextHelper.Create())
            {
                var customer = context.Customers.ByProject(1);


                Assert.Null(customer);
            }
        }
        [Test]
        public void ByProject_WhenExists_ReturnsIt()
        {
            using (var context = ContextHelper.Create())
            {
                var project = context.AddProject();


                var customer = context.Customers.ByProject(project.ProjectId);


                Assert.NotNull(customer);
                Assert.AreEqual(project.CustomerId, customer.CustomerId);
            }
        }


        [Test]
        public void ByProjectMust_WhenNotExists_Throws()
        {
            using (var context = ContextHelper.Create())
            {
                Assert.Throws<Exception>(() => context.Customers.ByProjectMust(1));
            }
        }
    }
}
