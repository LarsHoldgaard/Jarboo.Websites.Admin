using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.Services;
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
            var context = new FakeContext().AddProject();
            var strategy = CreateStrategy(context.UnitOfWork);


            Assert.Throws<ApplicationException>(() => strategy.SelectEmployee(TaskStepEnum.Specification, FakeContext.DefaultId));
        }

        [Test]
        public void SelectEmployee_WhenOneUnappropriateEmployee_SelectHim()
        {
            var step = TaskStepEnum.Specification;
            var position = Position.Tester;
            if (step.GetPosition() == position)
            {
                throw new Exception("Position must not fit step");
            }

            var context = new FakeContext().AddProject().AddEmployee().AddPosition(x => { x.Position = position; });
            var strategy = CreateStrategy(context.UnitOfWork);


            var employeeId = strategy.SelectEmployee(step, FakeContext.DefaultId);


            Assert.AreEqual(FakeContext.DefaultId, employeeId);
        }

        [Test]
        public void SelectEmployee_WhenAppropriateEmployeeExists_SelectHim()
        {
            var goodEmployeeId = 2;

            var step = TaskStepEnum.Specification;
            var wrongPosition = Position.Tester;
            if (step.GetPosition() == wrongPosition)
            {
                throw new Exception("Position must not fit step");
            }

            var context = new FakeContext().AddProject()
                .AddEmployee(x => { x.EmployeeId = 1; })
                .AddPosition(x =>
                    {
                        x.EmployeeId = 1;
                        x.Position = wrongPosition;
                    })
                .AddEmployee(x => { x.EmployeeId = goodEmployeeId; })
                .AddPosition(x =>
                    {
                        x.EmployeeId = goodEmployeeId;
                        x.Position = step.GetPosition();
                    })
                .AddEmployee(x => { x.EmployeeId = 3; })
                .AddPosition(x =>
                    {
                        x.EmployeeId = 3;
                        x.Position = wrongPosition;
                    })
                ;
            var strategy = CreateStrategy(context.UnitOfWork);


            var employeeId = strategy.SelectEmployee(step, FakeContext.DefaultId);


            Assert.AreEqual(goodEmployeeId, employeeId);
        }


        private TaskStepEmployeeStrategy CreateStrategy(IUnitOfWork unitOfWork)
        {
            var employeeService = new EmployeeService(unitOfWork);
            return new TaskStepEmployeeStrategy(employeeService);
        }
    }
}
