using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.Authorization;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.DAL.Extensions;

using Task = Jarboo.Admin.DAL.Entities.Task;

namespace Jarboo.Admin.BL.Services
{
    public interface ITaskService : IEntityService<int, Task>
    {
        void Create(TaskCreate model, IBusinessErrorCollection errors);

        void NextStep(TaskNextStep model, IBusinessErrorCollection errors);

        void Delete(int taskId, IBusinessErrorCollection errors);
    }

    public class TaskService : BaseEntityService<int, Task>, ITaskService
    {
        protected ITaskRegister TaskRegister { get; set; }
        protected IFolderCreator FolderCreator { get; set; }
        protected ITaskStepEmployeeStrategy TaskStepEmployeeStrategy { get; set; }
        protected INotifier Notifier { get; set; }

        public TaskService(IUnitOfWork unitOfWork, IAuth auth, ITaskRegister taskRegister, IFolderCreator folderCreator, ITaskStepEmployeeStrategy taskStepEmployeeStrategy, INotifier notifier)
            : base(unitOfWork, auth)
        {
            TaskRegister = taskRegister;
            FolderCreator = folderCreator;
            TaskStepEmployeeStrategy = taskStepEmployeeStrategy;
            Notifier = notifier;
        }

        protected override IDbSet<Task> Table
        {
            get { return UnitOfWork.Tasks; }
        }
        protected override Task Find(int id, IQueryable<Task> query)
        {
            return query.ById(id);
        }

        protected override string SecurityEntities
        {
            get { return Rights.Tasks.Name; }
        }
        protected override IQueryable<Task> FilterCanView(IQueryable<Task> query)
        {
            return query.Where(x => x.Project.CustomerId == UserCustomerId || x.Steps.Any(y => y.EmployeeId == UserEmployeeId));
        }
        protected override bool HasAccessTo(Task entity)
        {
            if (entity.ProjectId != 0)
            {
                return UnitOfWork.Projects.Any(x => x.ProjectId == entity.ProjectId && x.CustomerId == UserCustomerId);
            }
            else if (entity.TaskId != 0)
            {
                return UnitOfWork.Tasks.Any(x => x.TaskId == entity.TaskId && (
                    x.Project.CustomerId == UserCustomerId ||
                    x.Steps.Any(y => y.EmployeeId == UserEmployeeId)));
            }
            else
            {
                return false;
            }
        }

        public void Create(TaskCreate model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var project = UnitOfWork.Projects.AsNoTracking().Include(x => x.Customer).ByIdMust(model.ProjectId);
            var customer = project.Customer;

            var employee = !model.EmployeeId.HasValue ? 
                this.TaskStepEmployeeStrategy.SelectEmployee(TaskStep.First(), model.ProjectId) : 
                this.UnitOfWork.Employees.AsNoTracking().ByIdMust(model.EmployeeId.Value);

            var taskIdentifier = model.Identifier();
            string taskLink = null;
            string folderLink = null;

            try
            {
                folderLink = CreateFolder(customer.Name, taskIdentifier);

                taskLink = RegisterTask(project.BoardName, taskIdentifier, folderLink);
                ChangeResponsible(project.BoardName, taskIdentifier, taskLink, employee.TrelloId);

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
                this.Cleanup(customer.Name, project.BoardName, taskIdentifier, taskLink, folderLink);
                throw;
            }
            catch (Exception ex)
            {
                this.Cleanup(customer.Name, project.BoardName, taskIdentifier, taskLink, folderLink);
                throw new ApplicationException("Couldn't create task", ex);
            }

            try
            {
                Notifier.TaskResponsibleChanged(new TaskResponsibleChangedData(model, employee));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    "Task was successfully created, but the error was raised during email sending", ex);
            }
        }
        private string RegisterTask(string boardName, string taskIdentifier, string folderLink)
        {
            try
            {
                return TaskRegister.Register(boardName, taskIdentifier, folderLink);
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
        private void ChangeResponsible(string boardName, string tasktaskIdentifierTitle, string url, string responsibleUserId)
        {
            try
            {
                TaskRegister.ChangeResponsible(boardName, tasktaskIdentifierTitle, url, responsibleUserId);
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
        private void UnregisterTask(string boardName, string taskIdentifier, string url)
        {
            try
            {
                TaskRegister.Unregister(boardName, taskIdentifier, url);
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
        private void Cleanup(string customerName, string boardName, string taskIdentifier, string taskLink, string folderLink)
        {
            if (!string.IsNullOrEmpty(taskLink))
            {
                this.UnregisterTask(boardName, taskIdentifier, taskLink);
            }
            if (!string.IsNullOrEmpty(folderLink))
            {
                this.DeleteFolder(customerName, taskIdentifier);
            }
        }

        private void CheckCanNextStep(Task entity)
        {
            CheckCan(Rights.Tasks.NextStepAny, Rights.Tasks.NextStepSpecial, HasAccessTo, entity);
        }
        public void NextStep(TaskNextStep model, IBusinessErrorCollection errors)
        {
            var now = DateTime.Now;

            var task = Table.Include(x => x.Steps.Select(y => y.Employee)).Include(x => x.Project).ByIdMust(model.TaskId);
            task.DateModified = now;

            CheckCanNextStep(task);

            var lastStep = task.Steps.Last();
            lastStep.DateModified = now;
            lastStep.DateEnd = now;

            Employee employee = null;
            var nextStep = TaskStep.Next(lastStep.Step);
            if (nextStep.HasValue)
            {
                employee = !model.EmployeeId.HasValue ? 
                    this.TaskStepEmployeeStrategy.SelectEmployee(nextStep.Value, task.ProjectId) : 
                    this.UnitOfWork.Employees.AsNoTracking().ByIdMust(model.EmployeeId.Value);

                ChangeResponsible(task.Project.BoardName, task.Identifier(), task.CardLink, employee.TrelloId);

                task.Steps.Add(new TaskStep() { EmployeeId = employee.EmployeeId, Step = nextStep.Value });
            }
            else
            {
                ChangeResponsible(task.Project.BoardName, task.Identifier(), task.CardLink, null);

                task.Done = true;
            }

            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                ChangeResponsible(task.Project.BoardName, task.Identifier(), task.CardLink, lastStep.Employee.TrelloId);
                throw;
            }

            if (nextStep.HasValue)
            {
                try
                {
                    Notifier.TaskResponsibleChanged(new TaskResponsibleChangedData(task, employee));
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(
                        "Task was successfully moved to the next step, but the error was raised during email sending", ex);
                }
            }
        }

        public void Delete(int taskId, IBusinessErrorCollection errors)
        {
            var entity = Table.Include(x => x.Project).ByIdMust(taskId);
            var customer = UnitOfWork.Customers.ByProjectMust(entity.ProjectId);

            entity.DateModified = DateTime.Now;
            entity.DateDeleted = DateTime.Now;

            CheckCanDisable(entity);

            UnitOfWork.SaveChanges();

            this.DeleteFolder(customer.Name, entity.Identifier());
            this.UnregisterTask(entity.Project.BoardName, entity.Identifier(), entity.CardLink);
        }
    }
}
