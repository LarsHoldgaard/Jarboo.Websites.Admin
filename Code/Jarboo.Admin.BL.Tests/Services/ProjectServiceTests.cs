using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FakeItEasy;

using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL.Tests;

using NUnit.Framework;

namespace Jarboo.Admin.BL.Tests.Services
{
    [TestFixture]
    public class ProjectServiceTests
    {
        [Test]
        public void Create_WhenBoardNameMissing_CallsDefaultBoardName()
        {
            using (var context = ContextHelper.Create())
            {
                var customer = context.AddCustomer();
                var model = ValidProjectEdit(context);
                model.BoardName = null;

                var mockTaskRegister = A.Fake<ITaskRegister>();
                var service = Factory.CreateProjectService(context, taskRegister: mockTaskRegister);


                service.Save(model, null);


                A.CallTo(() => mockTaskRegister.DefaultBoardName(customer.Name)).MustHaveHappened();
            }
        }

        private ProjectEdit ValidProjectEdit(DAL.IUnitOfWork context)
        {
            return new ProjectEdit()
                       {
                           Name = "Project",
                           BoardName = "BoardName",
                           CustomerId = context.EnsureCustomer().CustomerId
                       };
        }
    }
}
