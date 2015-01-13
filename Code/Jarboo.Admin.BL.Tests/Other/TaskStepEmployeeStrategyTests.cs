using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.Services;
using Jarboo.Admin.BL.Tests.Services;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.DAL.Tests;

using NUnit.Framework;
using Jarboo.Admin.BL.Other;

namespace Jarboo.Admin.BL.Tests.Other
{
    [TestFixture]
    public class TaskStepEmployeeStrategyTests
    {
        [Test]
        public void SelectEmployee_WhenNoEmployee_Throws()
        {
            var projectId = 0;

            var context = FakeContext.Create().AddProject(afterSave: x => { projectId = x.ProjectId; });
            var strategy = CreateStrategy(context);


            Assert.Throws<ApplicationException>(() => strategy.SelectEmployee(TaskStepEnum.Specification, projectId));
        }

        [Test]
        public void SelectEmployee_WhenOneUnappropriateEmployee_SelectHim()
        {
            var stepWithWrongPosition = this.GetStepWithWrongPosition();
            var step = stepWithWrongPosition.Item1;
            var wrongPosition = stepWithWrongPosition.Item2;

            var projectId = 0;
            var expEmployeeId = 0;

            var context = FakeContext.Create()
                .AddProject(afterSave: x => { projectId = x.ProjectId; })
                .AddEmployeePosition(x => { x.Position = wrongPosition; }, 
                    x => { expEmployeeId = x.EmployeeId; });
            var strategy = CreateStrategy(context);


            var employeeId = strategy.SelectEmployee(step, projectId);


            Assert.AreEqual(expEmployeeId, employeeId);
        }

        [Test]
        public void SelectEmployee_WhenAppropriateEmployeeExists_SelectHim()
        {
            var stepWithWrongPosition = this.GetStepWithWrongPosition();
            var step = stepWithWrongPosition.Item1;
            var wrongPosition = stepWithWrongPosition.Item2;

            var projectId = 0;
            var goodEmployeeId = 0;

            var context = FakeContext.Create()
                .AddProject(afterSave: x => { projectId = x.ProjectId; })
                .AddEmployee()
                .AddEmployeePosition(x => { x.Position = wrongPosition; })
                .AddEmployee()
                .AddEmployeePosition(x => { x.Position = step.GetPosition(); },
                    x => { goodEmployeeId = x.EmployeeId; })
                .AddEmployee()
                .AddEmployeePosition(x => { x.Position = wrongPosition; });
            var strategy = CreateStrategy(context);


            var employeeId = strategy.SelectEmployee(step, projectId);


            Assert.AreEqual(goodEmployeeId, employeeId);
        }

        private TaskStepEmployeeStrategy CreateStrategy(IUnitOfWork unitOfWork)
        {
            var employeeService = ServicesFactory.CreateEmployeeService(unitOfWork);
            return new TaskStepEmployeeStrategy(employeeService);
        }

        private Tuple<TaskStepEnum, Position> GetStepWithWrongPosition()
        {
            var step = TaskStepEnum.Specification;
            var wrongPosition = Position.Tester;
            if (step.GetPosition() == wrongPosition)
            {
                throw new Exception("Position must not fit step");
            }

            return Tuple.Create(step, wrongPosition);
        }
    }
}
