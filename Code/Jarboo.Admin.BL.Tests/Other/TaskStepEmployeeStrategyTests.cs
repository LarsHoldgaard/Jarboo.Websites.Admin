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
                var strategy = Factory.CreateStrategy(context);


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
                var strategy = Factory.CreateStrategy(context);


                var employee = strategy.SelectEmployee(step, projectId);


                Assert.AreEqual(expEmployeeId, employee.EmployeeId);
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
                var fitEmployeeId = context.AddEmployeePosition(x => { x.Position = step.GetPosition(); }).EmployeeId;
                context.AddEmployee();
                context.AddEmployeePosition(x => { x.Position = wrongPosition; });
                var strategy = Factory.CreateStrategy(context);


                var employee = strategy.SelectEmployee(step, projectId);


                Assert.AreEqual(fitEmployeeId, employee.EmployeeId);
            }
        }

        [Test]
        public void SelectEmployee_WhenDeletedEmplyee_IgnoresHim()
        {
            using (var context = ContextHelper.Create())
            {
                var stepWithWrongPosition = this.GetStepWithWrongPosition();
                var step = stepWithWrongPosition.Item1;
                var wrongPosition = stepWithWrongPosition.Item2;

                var projectId = context.AddProject().ProjectId;
                context.AddEmployee();
                var unfitEmployeeId = context.AddEmployeePosition(x => { x.Position = wrongPosition; }).EmployeeId;
                context.AddEmployee(x => x.DateDeleted = DateTime.Now);
                context.AddEmployeePosition(x => { x.Position = step.GetPosition(); });
                var strategy = Factory.CreateStrategy(context);


                var employee = strategy.SelectEmployee(step, projectId);


                Assert.AreEqual(unfitEmployeeId, employee.EmployeeId);
            }
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
