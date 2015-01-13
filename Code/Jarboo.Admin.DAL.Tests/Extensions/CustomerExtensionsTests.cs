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
            var context = FakeContext.Create();


            Assert.Null(context.Customers.ByProject(1));
        }
        [Test]
        public void ByProject_WhenExists_ReturnsIt()
        {
            var projectId = 0;
            var customerId = 0;
            var context = FakeContext.Create().AddProject(afterSave: x =>
                {
                    projectId = x.ProjectId;
                    customerId = x.CustomerId;
                });


            var customer = context.Customers.ByProject(projectId);


            Assert.NotNull(customer);
            Assert.AreEqual(customerId, customer.CustomerId);
        }


        [Test]
        public void ByProjectMust_WhenNotExists_Throws()
        {
            var context = FakeContext.Create();


            Assert.Throws<Exception>(() => context.Customers.ByProjectMust(1));
        }
    }
}
