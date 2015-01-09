using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.External;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.ThirdParty;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;
using Task = Jarboo.Admin.DAL.Entities.Task;

namespace Jarboo.Admin.BL.Services
{
    public interface ITaskService : IEntityService<Task>
    {
        void Create(TaskCreate model, IBusinessErrorCollection errors);

        void NextStep(TaskNextStep model, IBusinessErrorCollection errors);
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

        protected override System.Data.Entity.IDbSet<Task> Table
        {
            get { return UnitOfWork.Tasks; }
        }
        protected override Task Find(int id, IQueryable<Task> query)
        {
            return query.FirstOrDefault(x => x.TaskId == id);
        }

        public void Create(TaskCreate model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var customer = UnitOfWork.Customers.AsNoTracking().FirstOrDefault(x => x.Projects.Any(y => y.ProjectId == model.ProjectId));
            if (customer == null)
            {
                throw new Exception("Couldn't find customer for project " + model.ProjectId);
            }

            if (!model.EmployeeId.HasValue)
            {
                model.EmployeeId = TaskStepEmployeeStrategy.SelectEmployee(TaskStep.First(), model.ProjectId);
            }
            var employee = UnitOfWork.Employees.AsNoTracking().FirstOrDefault(x => x.EmployeeId == model.EmployeeId.Value);
            if (employee == null)
            {
                throw new Exception("Couldn't find employee " + model.EmployeeId.Value);
            }

            var taskFullTitle = Task.TaskFullTitle(model.Title, model.Type);
            string taskLink = null;
            string folderLink = null;

            try
            {
                folderLink = CreateFolder(customer.Name, taskFullTitle);

                taskLink = RegisterTask(customer.Name, taskFullTitle, folderLink);
                ChangeResponsible(customer.Name, taskFullTitle, taskLink, employee.TrelloId);

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
                this.Cleanup(customer, taskFullTitle, taskLink, folderLink);
                throw;
            }
            catch (Exception ex)
            {
                this.Cleanup(customer, taskFullTitle, taskLink, folderLink);
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

            var entity = Table.Include(x => x.Steps).FirstOrDefault(x => x.TaskId == model.TaskId);
            if (entity == null)
            {
                throw new NotFoundException();
            }
            entity.DateModified = now;

            var customer = UnitOfWork.Customers.FirstOrDefault(x => x.Projects.Any(y => y.ProjectId == entity.ProjectId));
            if (customer == null)
            {
                throw new Exception("Couldn't find customer for project " + entity.ProjectId);
            }

            var lastStep = entity.Steps.Last();
            lastStep.DateModified = now;
            lastStep.DateEnd = now;

            var nextStep = TaskStep.Next(lastStep.Step);
            if (nextStep.HasValue)
            {
                if (!model.EmployeeId.HasValue)
                {
                    model.EmployeeId = TaskStepEmployeeStrategy.SelectEmployee(TaskStep.First(), entity.ProjectId);
                }
                var employee = UnitOfWork.Employees.AsNoTracking().First(x => x.EmployeeId == model.EmployeeId.Value);

                ChangeResponsible(customer.Name, entity.FullTitle(), entity.CardLink, employee.TrelloId);

                entity.Steps.Add(new TaskStep() { EmployeeId = model.EmployeeId.Value, Step = nextStep.Value});
            }
            else
            {
                ChangeResponsible(customer.Name, entity.FullTitle(), entity.CardLink, null);

                entity.Done = true;
            }
            UnitOfWork.SaveChanges();
        }
    }
}
