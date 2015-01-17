﻿using System;
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
        public void Delete_WhenHasUndoneTaskStep_ChangesTaskResponsible()
        {
            using (var context = ContextHelper.Create())
            {
                var employee = context.AddEmployee(x => x.TrelloId = "id1");
                var employeeToDelete = context.AddEmployee();
                var taskStep = context.AddTaskStep(x => x.Employee = employeeToDelete);

                var mockTaskRegister = A.Fake<ITaskRegister>();
                var taskStepEmployeeStrategy = A.Fake<ITaskStepEmployeeStrategy>();
                A.CallTo(() => taskStepEmployeeStrategy.SelectEmployee(taskStep.Step, taskStep.Task.ProjectId)).Returns(employee);
                var service = Factory.CreateEmployeeService(context, taskRegister: mockTaskRegister, taskStepEmployeeStrategy: taskStepEmployeeStrategy);


                Helper.Suppress(() => service.Delete(employeeToDelete.EmployeeId, null));


                A.CallTo(() => mockTaskRegister.ChangeResponsible(
                        taskStep.Task.Project.Customer.Name,
                        taskStep.Task.Identifier(),
                        taskStep.Task.CardLink,
                        employee.TrelloId)).MustHaveHappened();
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
