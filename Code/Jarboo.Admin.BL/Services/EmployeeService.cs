using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;

using Jarboo.Admin.BL.Authorization;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.DAL.Extensions;

namespace Jarboo.Admin.BL.Services
{
    public interface IEmployeeService : IEntityService<Employee>
    {
        void Save(EmployeeEdit model, IBusinessErrorCollection errors);

        void Delete(int employeeId, IBusinessErrorCollection errors);
    }

    public class EmployeeService : BaseEntityService<Employee>, IEmployeeService
    {
        protected ITaskRegister TaskRegister { get; set; }
        protected ITaskStepEmployeeStrategy TaskStepEmployeeStrategy { get; set; }

        public EmployeeService(IUnitOfWork unitOfWork, IAuth auth, ITaskRegister taskRegister, ITaskStepEmployeeStrategy taskStepEmployeeStrategy)
            : base(unitOfWork, auth)
        {
            TaskStepEmployeeStrategy = taskStepEmployeeStrategy;
            TaskRegister = taskRegister;
        }

        protected override IDbSet<Employee> Table
        {
            get { return UnitOfWork.Employees; }
        }
        protected override Employee Find(int id, IQueryable<Employee> query)
        {
            return query.ById(id);
        }

        protected override string SecurityEntities
        {
            get { return Rights.Employees.Name; }
        }

        public void Save(EmployeeEdit model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            if (model.EmployeeId == 0)
            {
                var entity = new Employee();
                Add(entity, model);
            }
            else
            {
                UnitOfWork.EmployeePositions.Where(x => x.EmployeeId == model.EmployeeId).Delete();

                var entity = new Employee { EmployeeId = model.EmployeeId };
                Edit(entity, model);
            }
        }

        public void Delete(int employeeId, IBusinessErrorCollection errors)
        {
            var entity = Table.ByIdMust(employeeId);
            entity.DateModified = DateTime.Now;
            entity.DateDeleted = DateTime.Now;

            CheckCanDisable(entity);

            UnitOfWork.SaveChanges();

            try
            {
                var steps = UnitOfWork.TaskSteps.Include(x => x.Task.Project.Customer).ForEmployee(employeeId).NotDone().ToList();
                foreach (var step in steps)
                {
                    var newEmployee = TaskStepEmployeeStrategy.SelectEmployee(step.Step, step.Task.ProjectId);
                    ChangeResponsible(step.Task.Project.Customer.Name, step.Task.Identifier(), step.Task.CardLink, newEmployee.TrelloId);
                    step.EmployeeId = newEmployee.EmployeeId;
                }
                UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error during tasks assigment. Some task may left assigned to deleted employee.", ex);
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
    }
}
