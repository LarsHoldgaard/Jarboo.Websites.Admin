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

            var employee = !model.EmployeeId.HasValue ? 
                this.TaskStepEmployeeStrategy.SelectEmployee(TaskStep.First(), model.ProjectId) : 
                this.UnitOfWork.Employees.AsNoTracking().ByIdMust(model.EmployeeId.Value);

            var taskIdentifier = Task.TaskIdentifier(model.Title, model.Type);
            string taskLink = null;
            string folderLink = null;

            try
            {
                folderLink = CreateFolder(customer.Name, taskIdentifier);

                taskLink = RegisterTask(customer.Name, taskIdentifier, folderLink);
                ChangeResponsible(customer.Name, taskIdentifier, taskLink, employee.TrelloId);

                var entity = new Task()
                {
                    FolderLink = folderLink,
                    CardLink = taskLink
                };

                entity.Steps.Add(new TaskStep()
                {
                    EmployeeId = employee.EmployeeId,
                    Step = TaskStep.First()
                });

                Add(entity, model);
            }
            catch (ApplicationException ex)
            {
                this.Cleanup(customer, taskIdentifier, taskLink, folderLink);
                throw;
            }
            catch (Exception ex)
            {
                this.Cleanup(customer, taskIdentifier, taskLink, folderLink);
                throw new ApplicationException("Couldn't create task", ex);
            }
        }
        private string RegisterTask(string customerName, string taskIdentifier, string folderLink)
        {
            try
            {
                return TaskRegister.Register(customerName, taskIdentifier, folderLink);
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
        private void ChangeResponsible(string customerName, string tasktaskIdentifierTitle, string url, string responsibleUserId)
        {
            try
            {
                TaskRegister.ChangeResponsible(customerName, tasktaskIdentifierTitle, url, responsibleUserId);
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
        private void UnregisterTask(string customerName, string taskIdentifier, string url)
        {
            try
            {
                TaskRegister.Unregister(customerName, taskIdentifier, url);
            }
            catch
            { }
        }
        private string CreateFolder(string customerName, string taskIdentifier)
        {
            try
            {
                return FolderCreator.Create(customerName, taskIdentifier);
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
        private void DeleteFolder(string customerName, string taskIdentifier)
        {
            try
            {
                FolderCreator.Delete(customerName, taskIdentifier);
            }
            catch
            { }
        }
        private void Cleanup(Customer customer, string taskIdentifier, string taskLink, string folderLink)
        {
            if (!string.IsNullOrEmpty(taskLink))
            {
                this.UnregisterTask(customer.Name, taskIdentifier, taskLink);
            }
            if (!string.IsNullOrEmpty(folderLink))
            {
                this.DeleteFolder(customer.Name, taskIdentifier);
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
                Employee employee = !model.EmployeeId.HasValue ? 
                    this.TaskStepEmployeeStrategy.SelectEmployee(nextStep.Value, entity.ProjectId) : 
                    this.UnitOfWork.Employees.AsNoTracking().ByIdMust(model.EmployeeId.Value);

                ChangeResponsible(customer.Name, entity.Identifier(), entity.CardLink, employee.TrelloId);

                entity.Steps.Add(new TaskStep() { EmployeeId = employee.EmployeeId, Step = nextStep.Value });
            }
            else
            {
                ChangeResponsible(customer.Name, entity.Identifier(), entity.CardLink, null);

                entity.Done = true;
            }
            UnitOfWork.SaveChanges();
        }

        public void Delete(int taskId, IBusinessErrorCollection errors)
        {
            var entity = Table.ByIdMust(taskId);
            var customer = UnitOfWork.Customers.ByProjectMust(entity.ProjectId);

            entity.DateModified = DateTime.Now;
            entity.DateDeleted = DateTime.Now;

            UnitOfWork.SaveChanges();

            this.DeleteFolder(customer.Name, entity.Identifier());
            this.UnregisterTask(customer.Name, entity.Identifier(), entity.CardLink);
        }
    }
}
