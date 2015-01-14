using FakeItEasy;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
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
        public void Create_WhenFailsAfterFolderCreate_DeleteFolder()
        {
            using (var context = ContextHelper.Create())
            {
                var folderCreator = A.Fake<IFolderCreator>();
                A.CallTo(() => folderCreator.Create(A<string>._, A<string>._)).Returns("link");

                var service = ServicesFactory.CreateTaskService(context, folderCreator: folderCreator);
                var model = CreateValidTask(context);
                A.CallTo(() => context.SaveChanges()).Throws<Exception>();


                Assert.Throws<ApplicationException>(() => service.Create(model, null));


                A.CallTo(() => folderCreator.Delete(A<string>._, A<string>._)).MustHaveHappened();
            }
        }

        public TaskCreate CreateValidTask(IUnitOfWork context)
        {
            context.AddProject().AddEmployee();

            return new TaskCreate()
            {
                Title = "Title",
                ProjectId = context.Projects.First().ProjectId,
                EmployeeId = context.Employees.First().EmployeeId,
            };
        }
    }
}
