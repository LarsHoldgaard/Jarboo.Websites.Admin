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
            using (var context = ContextHelper.Create())
            {
                var projectId = context.AddProject().ProjectId;
                var strategy = CreateStrategy(context);


                Assert.Throws<ApplicationException>(
                    () => strategy.SelectEmployee(TaskStepEnum.Specification, projectId));
            }
        }

        [Test]
        public void SelectEmployee_WhenOneUnappropriateEmployee_SelectHim()
        {
            using (var context = ContextHelper.Create())
            {
                var stepWithWrongPosition = this.GetStepWithWrongPosition();
                var step = stepWithWrongPosition.Item1;
                var wrongPosition = stepWithWrongPosition.Item2;

                var projectId = context.AddProject().ProjectId;
                var expEmployeeId = context.AddEmployeePosition(x => { x.Position = wrongPosition; }).EmployeeId;
                var strategy = CreateStrategy(context);


                var employeeId = strategy.SelectEmployee(step, projectId);


                Assert.AreEqual(expEmployeeId, employeeId);
            }
        }

        [Test]
        public void SelectEmployee_WhenAppropriateEmployeeExists_SelectHim()
        {
            using (var context = ContextHelper.Create())
            {
                var stepWithWrongPosition = this.GetStepWithWrongPosition();
                var step = stepWithWrongPosition.Item1;
                var wrongPosition = stepWithWrongPosition.Item2;

                var projectId = context.AddProject().ProjectId;
                context.AddEmployee();
                context.AddEmployeePosition(x => { x.Position = wrongPosition; });
                context.AddEmployee();
                var goodEmployeeId = context.AddEmployeePosition(x => { x.Position = step.GetPosition(); }).EmployeeId;
                context.AddEmployee();
                context.AddEmployeePosition(x => { x.Position = wrongPosition; });
                var strategy = CreateStrategy(context);


                var employeeId = strategy.SelectEmployee(step, projectId);


                Assert.AreEqual(goodEmployeeId, employeeId);
            }
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
