using FakeItEasy;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.ThirdParty;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.DAL.Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Tests.Services
{
    [TestFixture]
    public class TaskServiceTests
    {
        [Test]
        public void Create_WhenMissingProject_Throws()
        {
            var context = EmptyContext();

            var service = new TaskServiceBuilder()
                .UnitOfWork(context.UnitOfWork)
                .Build();
            var model = ValidTaskCreate();


            Assert.Throws<Exception>(() => service.Create(model, null));
        }
        [Test]
        public void Create_WhenMissingCustomer_Throws()
        {
            var context = EmptyContext().AddProject();

            var service = new TaskServiceBuilder()
                .UnitOfWork(context.UnitOfWork)
                .Build();
            var model = ValidTaskCreate();


            Assert.Throws<Exception>(() => service.Create(model, null));
        }
        [Test]
        public void Create_WhenMissingEmployee_Throws()
        {
            var context = EmptyContext().AddCustomer().AddProject();

            var service = new TaskServiceBuilder()
                .UnitOfWork(context.UnitOfWork)
                .Build();
            var model = ValidTaskCreate();


            Assert.Throws<Exception>(() => service.Create(model, null));
        }

        [Test]
        public void Create_WhenFailsAfterFolderCreate_DeleteFolder()
        {
            var context = EmptyContext().AddCustomer().AddProject().AddEmployee();
            A.CallTo(() => context.UnitOfWork.SaveChanges()).Throws<Exception>();

            var folderCreator = A.Fake<IFolderCreator>();
            A.CallTo(() => folderCreator.Create(A<string>._, A<string>._)).Returns("link");

            var service = new TaskServiceBuilder()
                .UnitOfWork(context.UnitOfWork)
                .FolderCreator(folderCreator)
                .Build();
            var model = ValidTaskCreate();


            Assert.Throws<ApplicationException>(() => service.Create(model, null));


            A.CallTo(() => folderCreator.Delete(A<string>._, A<string>._)).MustHaveHappened();
        }

        public FakeContext EmptyContext()
        {
            return new FakeContext();
        }
        public TaskCreate ValidTaskCreate()
        {
            return new TaskCreate()
            {
                Title = "Title",
                ProjectId = 1,
                EmployeeId = 1,
            };
        }
    }
}
