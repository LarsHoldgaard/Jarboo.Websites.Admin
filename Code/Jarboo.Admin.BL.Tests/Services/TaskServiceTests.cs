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
        public void Create_Always_CallFolderCreate()
        {
            using (var context = ContextHelper.Create())
            {
                var customer = context.AddCustomer();
                var model = ValidTaskCreate(context);

                var mockFolderCreator = A.Fake<IFolderCreator>();
                var service = Factory.CreateTaskService(context, folderCreator: mockFolderCreator);


                Helper.Suppress(() =>service.Create(model, null));


                A.CallTo(() => mockFolderCreator.Create(customer.Name, model.Identifier())).MustHaveHappened();
            }
        }
        [Test]
        public void Create_WhenFailsAfterFolderCreate_DeleteFolder()
        {
            using (var context = ContextHelper.Create())
            {
                var customer = context.AddCustomer();
                var model = ValidTaskCreate(context);
                A.CallTo(() => context.SaveChanges()).Throws<Exception>();

                var mockFolderCreator = A.Fake<IFolderCreator>();
                A.CallTo(() => mockFolderCreator.Create(A<string>._, A<string>._)).Returns("link");
                var service = Factory.CreateTaskService(context, folderCreator: mockFolderCreator);


                Helper.Suppress(() => service.Create(model, null));


                A.CallTo(() => mockFolderCreator.Delete(customer.Name, model.Identifier())).MustHaveHappened();
            }
        }

        [Test]
        public void Create_Always_CallTaskRegister()
        {
            using (var context = ContextHelper.Create())
            {
                const string folderLink= "link";

                var project = context.AddProject();
                var model = ValidTaskCreate(context);

                var folderCreator = A.Fake<IFolderCreator>();
                A.CallTo(() => folderCreator.Create(A<string>._, A<string>._)).Returns(folderLink);
                var mockTaskRegister = A.Fake<ITaskRegister>();
                var service = Factory.CreateTaskService(context, taskRegister: mockTaskRegister, folderCreator: folderCreator);


                Helper.Suppress(() => service.Create(model, null));


                A.CallTo(() => mockTaskRegister.Register(project.BoardName, model.Identifier(), folderLink)).MustHaveHappened();
            }
        }
        [Test]
        public void Create_WhenFailsAfterTaskRegister_UnregisterTask()
        {
            using (var context = ContextHelper.Create())
            {
                const string taskLink = "link";

                var project = context.AddProject();
                var model = ValidTaskCreate(context);
                A.CallTo(() => context.SaveChanges()).Throws<Exception>();

                var mockTaskRegister = A.Fake<ITaskRegister>();
                A.CallTo(() => mockTaskRegister.Register(A<string>._, A<string>._, A<string>._)).Returns(taskLink);
                var service = Factory.CreateTaskService(context, taskRegister: mockTaskRegister);


                Helper.Suppress(() => service.Create(model, null));


                A.CallTo(() => mockTaskRegister.Unregister(project.BoardName, model.Identifier(), taskLink)).MustHaveHappened();
            }
        }

        [Test]
        public void Create_WhenEmployeeIdNotPassed_CallsTaskStepEmployeeStrategy()
        {
            using (var context = ContextHelper.Create())
            {
                var model = this.ValidTaskCreate(context);

                var mockTaskStepEmployeeStrategy = A.Fake<ITaskStepEmployeeStrategy>();
                var service = Factory.CreateTaskService(context, taskStepEmployeeStrategy: mockTaskStepEmployeeStrategy);

                Helper.Suppress(() => service.Create(model, null));

                A.CallTo(() => mockTaskStepEmployeeStrategy.SelectEmployee(TaskStep.First(), model.ProjectId)).MustHaveHappened();
            }
        }


        [Test]
        public void Delete_Always_DeleteTasksFolder()
        {
            using (var context = ContextHelper.Create())
            {
                var customer = context.AddCustomer();
                var task = context.AddTask();

                var mockFolderCreator = A.Fake<IFolderCreator>();
                var service = Factory.CreateTaskService(context, folderCreator: mockFolderCreator);


                service.Delete(task.TaskId, null);


                A.CallTo(() => mockFolderCreator.Delete(customer.Name, task.Identifier())).MustHaveHappened();
            }
        }
        [Test]
        public void Delete_Always_DeleteTasksCard()
        {
            using (var context = ContextHelper.Create())
            {
                const string cardLink = "card_link";

                var project = context.AddProject();
                var task = context.AddTask(x => { x.CardLink = cardLink; });

                var mockTaskRegister = A.Fake<ITaskRegister>();
                var service = Factory.CreateTaskService(context, taskRegister: mockTaskRegister);


                Helper.Suppress(() => service.Delete(task.TaskId, null));


                A.CallTo(() => mockTaskRegister.Unregister(project.BoardName, task.Identifier(), cardLink)).MustHaveHappened();
            }
        }


        [Test]
        public void NextStep_WhenLastStep_ChangeResponsibleToNone()
        {
            using (var context = ContextHelper.Create())
            {
                var project = context.AddProject();
                var task = context.AddTask();
                context.AddTaskStep(x => x.Step = Helper.LastStep());

                var model = this.ValidNextStep(context);
                model.EmployeeId = context.AddEmployee().EmployeeId;

                var mockTaskRegister = A.Fake<ITaskRegister>();
                var service = Factory.CreateTaskService(context, taskRegister: mockTaskRegister);

                Helper.Suppress(() => service.NextStep(model, null));

                A.CallTo(() => mockTaskRegister.ChangeResponsible(project.BoardName, task.Identifier(), task.CardLink, null)).MustHaveHappened();
            }
        }
        [Test]
        public void NextStep_WhenNotLastStep_ChangeResponsibleToPassed()
        {
            using (var context = ContextHelper.Create())
            {
                var project = context.AddProject();
                var employee = context.AddEmployee();
                var task = context.AddTask();
                context.AddTaskStep();

                var model = this.ValidNextStep(context);
                model.EmployeeId = employee.EmployeeId;

                var mockTaskRegister = A.Fake<ITaskRegister>();
                var service = Factory.CreateTaskService(context, taskRegister: mockTaskRegister);

                Helper.Suppress(() => service.NextStep(model, null));

                A.CallTo(() => mockTaskRegister.ChangeResponsible(project.BoardName, task.Identifier(), task.CardLink, employee.TrelloId)).MustHaveHappened();
            }
        }
        [Test]
        public void NextStep_WhenNotLastStepAndEmployeeIdNotPassed_CallsTaskStepEmployeeStrategy()
        {
            using (var context = ContextHelper.Create())
            {
                var project = context.AddProject();
                var taskStep = context.AddTaskStep();

                var model = this.ValidNextStep(context);
                var nextStep = TaskStep.Next(taskStep.Step);
                if (!nextStep.HasValue)
                {
                    throw new Exception("step can't be last");
                }

                var mockTaskStepEmployeeStrategy = A.Fake<ITaskStepEmployeeStrategy>();
                var service = Factory.CreateTaskService(context, taskStepEmployeeStrategy: mockTaskStepEmployeeStrategy);

                Helper.Suppress(() => service.NextStep(model, null));

                A.CallTo(() => mockTaskStepEmployeeStrategy.SelectEmployee(nextStep.Value, project.ProjectId)).MustHaveHappened();
            }
        }

        public TaskCreate ValidTaskCreate(IUnitOfWork context)
        {
            return new TaskCreate()
            {
                Title = "Title",
                Type = TaskType.Bug,
                ProjectId = context.EnsureProject().ProjectId,
            };
        }

        public TaskNextStep ValidNextStep(IUnitOfWork context)
        {
            return new TaskNextStep()
                       {
                           TaskId = context.EnsureTask().TaskId,
                       };
        }
    }
}
