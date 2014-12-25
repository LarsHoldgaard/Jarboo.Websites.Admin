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

        public override Task GetById(int id)
        {
            return Table.Include(x => x.Project.Customer).FirstOrDefault(x => x.TaskId == id);
        }

        public List<Task> GetAll()
        {
            return Table.Include(x => x.Project)
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
            var taskRegistered = false;
            var folderCreated = false;

            try
            {
                RegisterTask(customer.Name, taskFullTitle);
                taskRegistered = true;

                CreateFolder(customer.Name, taskFullTitle);
                folderCreated = true;

                var entity = new Task();
                if (!model.EmployeeId.HasValue)
                {
                    model.EmployeeId = TaskStepEmployeeStrategy.SelectEmployee(TaskStep.First(), model.ProjectId);
                }

                entity.Steps.Add(new TaskStep()
                {
                    EmployeeId = model.EmployeeId.Value,
                    Step = TaskStep.First()
                });

                Add(entity, model);
            }
            catch (ApplicationException ex)
            {
                this.Cleanup(customer, taskFullTitle, taskRegistered, folderCreated);
                throw;
            }
            catch (Exception ex)
            {
                this.Cleanup(customer, taskFullTitle, taskRegistered, folderCreated);
                throw new ApplicationException("Couldn't create task", ex);
            }
        }

        private void RegisterTask(string customerName, string taskTitle)
        {
            try
            {
                TaskRegister.Register(customerName, taskTitle);
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
        private void CreateFolder(string customerName, string taskTitle)
        {
            try
            {
                FolderCreator.Create(customerName, taskTitle);
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
        private void Cleanup(Customer customer, string taskFullTitle, bool taskRegistered, bool folderCreated)
        {
            if (taskRegistered)
            {
                this.UnregisterTask(customer.Name, taskFullTitle);
            }
            if (folderCreated)
            {
                this.DeleteFolder(customer.Name, taskFullTitle);
            }
        }
    }
}
