using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.DAL.Extensions;

using Task = Jarboo.Admin.DAL.Entities.Task;

namespace Jarboo.Admin.BL.Services
{
    public interface ITaskService : IEntityService<Task>
    {
        void Create(TaskCreate model, IBusinessErrorCollection errors);

        void NextStep(TaskNextStep model, IBusinessErrorCollection errors);

        void Delete(int taskId, IBusinessErrorCollection errors);
    }

    public class TaskService : BaseEntityService<Task>, ITaskService
    {
        protected ITaskRegister TaskRegister { get; set; }
        protected IFolderCreator FolderCreator { get; set; }
        protected ITaskStepEmployeeStrategy TaskStepEmployeeStrategy { get; set; }

        public TaskService(IUnitOfWork unitOfWork, ITaskRegister taskRegister, IFolderCreator folderCreator, ITaskStepEmployeeStrategy taskStepEmployeeStrategy)
            : base(unitOfWork)
        {
            TaskRegister = taskRegister;
            FolderCreator = folderCreator;
            TaskStepEmployeeStrategy = taskStepEmployeeStrategy;
        }

        protected override IDbSet<Task> Table
        {
            get { return UnitOfWork.Tasks; }
        }
        protected override Task Find(int id, IQueryable<Task> query)
        {
            return query.ById(id);
        }

        public void Create(TaskCreate model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var customer = UnitOfWork.Customers.AsNoTracking().ByProject(model.ProjectId);

            if (!model.EmployeeId.HasValue)
            {
                model.EmployeeId = TaskStepEmployeeStrategy.SelectEmployee(TaskStep.First(), model.ProjectId);
            }
            var employee = UnitOfWork.Employees.AsNoTracking().ByIdMust(model.EmployeeId.Value);

            var taskTitle = Task.TaskTitleWithType(model.Title, model.Type);
            string taskLink = null;
            string folderLink = null;

            try
            {
                folderLink = CreateFolder(customer.Name, taskTitle);

                taskLink = RegisterTask(customer.Name, taskTitle, folderLink);
                ChangeResponsible(customer.Name, taskTitle, taskLink, employee.TrelloId);

                var entity = new Task()
                {
                    FolderLink = folderLink,
                    CardLink = taskLink
                };

                entity.Steps.Add(new TaskStep()
                {
                    EmployeeId = model.EmployeeId.Value,
                    Step = TaskStep.First()
                });

                Add(entity, model);
            }
            catch (ApplicationException ex)
            {
                this.Cleanup(customer, taskTitle, taskLink, folderLink);
                throw;
            }
            catch (Exception ex)
            {
                this.Cleanup(customer, taskTitle, taskLink, folderLink);
                throw new ApplicationException("Couldn't create task", ex);
            }
        }
        private string RegisterTask(string customerName, string taskTitle, string folderLink)
        {
            try
            {
                return TaskRegister.Register(customerName, taskTitle, folderLink);
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could not register task in third party service", ex);
            }
        }
        private void ChangeResponsible(string customerName, string taskTitle, string url, string responsibleUserId)
        {
            try
            {
                TaskRegister.ChangeResponsible(customerName, taskTitle, url, responsibleUserId);
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could not set responsible for task", ex);
            }
        }
        private void UnregisterTask(string customerName, string taskTitle, string url)
        {
            try
            {
                TaskRegister.Unregister(customerName, taskTitle, url);
            }
            catch
            { }
        }
        private string CreateFolder(string customerName, string taskTitle)
        {
            try
            {
                return FolderCreator.Create(customerName, taskTitle);
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could not create folder in third party service", ex);
            }
        }
        private void DeleteFolder(string customerName, string taskTitle)
        {
            try
            {
                FolderCreator.Delete(customerName, taskTitle);
            }
            catch
            { }
        }
        private void Cleanup(Customer customer, string taskFullTitle, string taskLink, string folderLink)
        {
            if (!string.IsNullOrEmpty(taskLink))
            {
                this.UnregisterTask(customer.Name, taskFullTitle, taskLink);
            }
            if (!string.IsNullOrEmpty(folderLink))
            {
                this.DeleteFolder(customer.Name, taskFullTitle);
            }
        }

        public void NextStep(TaskNextStep model, IBusinessErrorCollection errors)
        {
            var now = DateTime.Now;

            var entity = Table.Include(x => x.Steps).ByIdMust(model.TaskId);
            entity.DateModified = now;

            var customer = UnitOfWork.Customers.ByProjectMust(entity.ProjectId);

            var lastStep = entity.Steps.Last();
            lastStep.DateModified = now;
            lastStep.DateEnd = now;

            var nextStep = TaskStep.Next(lastStep.Step);
            if (nextStep.HasValue)
            {
                if (!model.EmployeeId.HasValue)
                {
                    model.EmployeeId = TaskStepEmployeeStrategy.SelectEmployee(nextStep.Value, entity.ProjectId);
                }
                var employee = UnitOfWork.Employees.AsNoTracking().ByIdMust(model.EmployeeId.Value);

                ChangeResponsible(customer.Name, entity.TitleWithType(), entity.CardLink, employee.TrelloId);

                entity.Steps.Add(new TaskStep() { EmployeeId = model.EmployeeId.Value, Step = nextStep.Value});
            }
            else
            {
                ChangeResponsible(customer.Name, entity.TitleWithType(), entity.CardLink, null);

                entity.Done = true;
            }
            UnitOfWork.SaveChanges();
        }

        public void Delete(int taskId, IBusinessErrorCollection errors)
        {
            if (taskId == 0)
            {
                throw new Exception("Incorrect entity id");
            }

            var entity = new Task { TaskId = taskId };
            Table.Attach(entity);

            entity.DateModified = DateTime.Now;
            entity.DateDeleted = DateTime.Now;

            UnitOfWork.SaveChanges();
        }
    }
}
