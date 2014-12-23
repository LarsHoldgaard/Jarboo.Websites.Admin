using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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

        public TaskService(IUnitOfWork UnitOfWork, ITaskRegister taskRegister)
            : base(UnitOfWork)
        {
            TaskRegister = taskRegister;
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

            RegisterTask(customer.Name, taskFullTitle);

            try
            {
                var entity = Table.Add(new Task());
                Add(entity, model);
            }
            catch (Exception ex)
            {
                UnregisterTask(customer.Name, taskFullTitle);
                throw new ApplicationException("Could not save task in the database", ex);
            }
        }
        private void RegisterTask(string customerName, string taskTitle)
        {
            try
            {
                TaskRegister.Register(customerName, taskTitle);
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
            catch (Exception ex)
            {
                throw new ApplicationException("Could not unregister task in third party service. Task should be removed by hand.", ex);
            }
        }
    }
}
