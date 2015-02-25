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
using Microsoft.AspNet.Identity;
using Jarboo.Admin.BL.Services.Interfaces;
using System.Reflection;

namespace Jarboo.Admin.BL.Services
{
    public class EmployeeService : BaseEntityService<int, Employee>, IEmployeeService
    {
        protected ITaskRegister TaskRegister { get; set; }
        protected ITaskStepEmployeeStrategy TaskStepEmployeeStrategy { get; set; }
        public UserManager<User> UserManager { get; set; }
        protected INotifier Notifier { get; set; }

        public EmployeeService(IUnitOfWork unitOfWork, IAuth auth, ICacheService cacheService, ITaskRegister taskRegister, ITaskStepEmployeeStrategy taskStepEmployeeStrategy, UserManager<User> userManager, INotifier notifier)
            : base(unitOfWork, auth, cacheService)
        {
            TaskStepEmployeeStrategy = taskStepEmployeeStrategy;
            TaskRegister = taskRegister;
            UserManager = userManager;
            Notifier = notifier;
        }

        protected override IDbSet<Employee> Table
        {
            get { return UnitOfWork.Employees; }
        }
        protected override Employee Find(int id, IQueryable<Employee> query)
        {
            Type type = typeof(Employee);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Employee)this.CacheService.GetById(cacheKey);

            var employee = query.ById(id);
            this.CacheService.Create(cacheKey, employee);
            return employee;
        }

        protected override async System.Threading.Tasks.Task<Employee> FindAsync(int id, IQueryable<Employee> query)
        {
            Type type = typeof(Employee);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Employee)this.CacheService.GetById(cacheKey);

            var employee = await query.ByIdAsync(id);
            this.CacheService.Create(cacheKey, employee);
            return employee;
        }

        protected override string SecurityEntities
        {
            get { return Rights.Employees.Name; }
        }

        public void Create(EmployeeCreate model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            if (UnitOfWork.Users.Any(x => x.DisplayName == model.FullName))
            {
                errors.Add("FullName", "Name already taken");
                return;
            }

            using (var transaction = UnitOfWork.BeginTransaction())
            {
                try
                {
                    var user = new User()
                    {
                        Email = model.Email,
                        UserName = model.Email,
                        DisplayName = model.FullName
                    };

                    var result = UserManager.Create(user, model.Password);
                    if (!result.Succeeded)
                    {
                        errors.AddErrorsFromResult(result);
                        transaction.Rollback();
                        return;
                    }

                    result = UserManager.AddToRole(user.Id, UserRoles.Employee.ToString());
                    if (!result.Succeeded)
                    {
                        errors.AddErrorsFromResult(result);
                        transaction.Rollback();
                        return;
                    }

                    var entity = new Employee()
                    {
                        User = user
                    };
                    Add(entity, model);

                    transaction.Commit();

                    Notifier.NewEmployee(new NewEmployeeData(entity));
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public void Edit(EmployeeEdit model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            UnitOfWork.EmployeePositions.Where(x => x.EmployeeId == model.EmployeeId).Delete();

            var entity = Table.ByIdMust(model.EmployeeId);

            model.MapTo(entity);

            Edit(entity, model);
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
                    ChangeResponsible(step.Task.Project.Customer.Name, step.Task.Identifier(), newEmployee.EmployeeId.ToString());
                    step.EmployeeId = newEmployee.EmployeeId;
                }
                UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error during tasks assigment. Some task may left assigned to deleted employee.", ex);
            }
        }
        private void ChangeResponsible(string customerName, string tasktaskIdentifierTitle, string responsibleUserId)
        {
            try
            {
                TaskRegister.ChangeResponsible(customerName, tasktaskIdentifierTitle, responsibleUserId);
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
    
        private void ClearCache()
        {
            try
            {
                Type type = typeof(Employee);
                this.CacheService.DeleteByContaining(type.Name);
            }
            catch (Exception)
            {
            }
        }
    }
}
