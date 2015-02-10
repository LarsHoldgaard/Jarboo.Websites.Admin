using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FakeItEasy;

using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.DAL.Tests;

using NUnit.Framework;

namespace Jarboo.Admin.BL.Tests.Services
{
    [TestFixture]
    public class EmployeeServiceTests
    {
        [Test]
        public void Delete_Always_DeleteEmployee()
        {
            using (var context = ContextHelper.Create())
            {
                var employeeId = context.AddEmployee().EmployeeId;
                var service = Factory.CreateEmployeeService(context);


                Helper.Suppress(() => service.Delete(employeeId, null));

                
                Assert.NotNull(context.Employees.First().DateDeleted);
            }
        }

        [Test]
        public void Delete_WhenHasDoneTaskStep_NotChangesResponsible()
        {
            using (var context = ContextHelper.Create())
            {
                var taskStep = context.AddTaskStep(x => x.DateEnd = DateTime.Now);

                var mockTaskStepEmployeeStrategy = A.Fake<ITaskStepEmployeeStrategy>();
                var service = Factory.CreateEmployeeService(context, taskStepEmployeeStrategy: mockTaskStepEmployeeStrategy);


                Helper.Suppress(() => service.Delete(taskStep.EmployeeId, null));


                A.CallTo(() => mockTaskStepEmployeeStrategy.SelectEmployee(A<TaskStepEnum>._, A<int>._)).MustNotHaveHappened();
            }
        }
    }
}
