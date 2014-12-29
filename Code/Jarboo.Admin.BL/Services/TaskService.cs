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
    public interface ITaskService
    {
        Task GetById(int id);
        List<Task> GetAll();

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

        public Task GetById(int id)
        {
            return TableNoTracking.Include(x => x.Project.Customer).Include(x => x.Steps.Select(y => y.Employee)).FirstOrDefault(x => x.TaskId == id);
        }

        public List<Task> GetAll()
        {
            return TableNoTracking.Include(x => x.Project)
                .Include(x => x.Steps)
                .AsEnumerable()
                .ToList();
        }

        public void Create(TaskCreate model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var customer = UnitOfWork.Customers.FirstOrDefault(x => x.Projects.Any(y => y.ProjectId == model.ProjectId));
            if (customer == null)
            {
                throw new Exception("Couldn't find customer for project " + model.ProjectId);
            }

            var taskFullTitle = Task.TaskFullTitle(model.Title, model.Type);
            string taskLink = null;
            string folderLink = null;

            try
            {
                taskLink = RegisterTask(customer.Name, taskFullTitle);
                folderLink = CreateFolder(customer.Name, taskFullTitle);

                if (!model.EmployeeId.HasValue)
                {
                    model.EmployeeId = TaskStepEmployeeStrategy.SelectEmployee(TaskStep.First(), model.ProjectId);
                }

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
        private string RegisterTask(string customerName, string taskTitle)
        {
            try
            {
                return TaskRegister.Register(customerName, taskTitle);
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
        private void UnregisterTask(string customerName, string taskTitle)
        {
            try
            {
                TaskRegister.Unregister(customerName, taskTitle);
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
                this.UnregisterTask(customer.Name, taskFullTitle);
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

            if (!model.EmployeeId.HasValue)
            {
                model.EmployeeId = TaskStepEmployeeStrategy.SelectEmployee(TaskStep.First(), entity.ProjectId);
            }

            var lastStep = entity.Steps.Last();
            lastStep.DateModified = now;
            lastStep.DateEnd = now;

            var nextStep = TaskStep.Next(lastStep.Step);
            if (nextStep.HasValue)
            {
                entity.Steps.Add(new TaskStep() { EmployeeId = model.EmployeeId.Value, Step = nextStep.Value});
            }
            else
            {
                entity.Done = true;
            }
            UnitOfWork.SaveChanges();
        }
    }
}
