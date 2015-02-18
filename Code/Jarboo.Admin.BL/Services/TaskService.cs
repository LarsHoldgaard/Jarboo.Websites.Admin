﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Jarboo.Admin.BL.Authorization;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.DAL.Extensions;

using Task = Jarboo.Admin.DAL.Entities.Task;
using Jarboo.Admin.BL.Services.Interfaces;
using System.Reflection;

namespace Jarboo.Admin.BL.Services
{
    public class TaskService : BaseEntityService<int, Task>, ITaskService
    {
        protected ITaskRegister TaskRegister { get; set; }
        protected IFolderCreator FolderCreator { get; set; }
        protected ITaskStepEmployeeStrategy TaskStepEmployeeStrategy { get; set; }
        protected INotifier Notifier { get; set; }

        public TaskService(IUnitOfWork unitOfWork, IAuth auth, ICacheService cacheService, ITaskRegister taskRegister, IFolderCreator folderCreator, ITaskStepEmployeeStrategy taskStepEmployeeStrategy, INotifier notifier)
            : base(unitOfWork, auth, cacheService)
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
            Type type = typeof(Task);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Task)this.CacheService.GetById(cacheKey);

            var task = query.ById(id);
            this.CacheService.Create(cacheKey, task);
            return task;
        }
        protected override async System.Threading.Tasks.Task<Task> FindAsync(int id, IQueryable<Task> query)
        {
            Type type = typeof(Task);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Task)this.CacheService.GetById(cacheKey);

            var task = await query.ByIdAsync(id);
            this.CacheService.Create(cacheKey, task);
            return task;
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

        public void Save(TaskEdit model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            if (model.TaskId == 0)
            {
                this.Create(model, errors);
            }
            else
            {
                var entity = new Task { TaskId = model.TaskId };
                Edit(entity, model);
            }

            ClearCache();
        }
        private void Create(TaskEdit model, IBusinessErrorCollection errors)
        {
            var project = UnitOfWork.Projects.AsNoTracking().Include(x => x.Customer).ByIdMust(model.ProjectId);
            var customer = project.Customer;

            var employee = !model.EmployeeId.HasValue ? 
                this.TaskStepEmployeeStrategy.SelectEmployee(TaskStep.First(), model.ProjectId) : 
                this.UnitOfWork.Employees.AsNoTracking().ByIdMust(model.EmployeeId.Value);

            var taskIdentifier = model.Identifier();
            string folderLink = null;

            var entity = new Task();

            try
            {
                folderLink = CreateFolder(customer.Name, taskIdentifier);

                ChangeResponsible(project.Name, taskIdentifier, employee.EmployeeId.ToString());

                entity.FolderLink = folderLink;
                entity.Steps.Add(new TaskStep()
                {
                    EmployeeId = employee.EmployeeId,
                    Step = TaskStep.First()
                });

                Add(entity, model);
            }
            catch (ApplicationException ex)
            {
                this.Cleanup(customer.Name, project.Name, taskIdentifier, folderLink);
                throw ex;
            }
            catch (Exception ex)
            {
                this.Cleanup(customer.Name, project.Name, taskIdentifier, folderLink);
                throw new ApplicationException("Couldn't create task", ex);
            }

            try
            {
                Notifier.NewTask(new NewTaskData(customer, project, entity));
                Notifier.TaskResponsibleChanged(new TaskResponsibleChangedData(entity, employee));
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
        private void ChangeResponsible(string boardName, string tasktaskIdentifierTitle, string responsibleUserId)
        {
            try
            {
                TaskRegister.ChangeResponsible(boardName, tasktaskIdentifierTitle, responsibleUserId);
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
        private void UnregisterTask(string boardName, string taskIdentifier)
        {
            try
            {
                TaskRegister.Unregister(boardName, taskIdentifier);
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
        private void Cleanup(string customerName, string projectName, string taskIdentifier, string folderLink)
        {
            this.UnregisterTask(projectName, taskIdentifier);
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

                ChangeResponsible(task.Project.Name, task.Identifier(), employee.EmployeeId.ToString());

                task.Steps.Add(new TaskStep() { EmployeeId = employee.EmployeeId, Step = nextStep.Value });
            }
            else
            {
                ChangeResponsible(task.Project.Name, task.Identifier(), null);

                task.Done = true;
            }

            try
            {
                UnitOfWork.SaveChanges();
                ClearCache();
            }
            catch (Exception)
            {
                ChangeResponsible(task.Project.Name, task.Identifier(), lastStep.Employee.EmployeeId.ToString());
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
            this.UnregisterTask(entity.Project.Name, entity.Identifier());

            ClearCache();
        }

        private void ClearCache()
        {
            try
            {
                Type type = typeof(Task);
                this.CacheService.DeleteByContaining(type.Name);
            }
            catch (Exception)
            {
            }
        }
    }
}
