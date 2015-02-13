using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.Authorization;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.DAL.Extensions;
using Jarboo.Admin.BL.Services.Interfaces;
using System.Reflection;

namespace Jarboo.Admin.BL.Services
{
    public class ProjectService : BaseEntityService<int, Project>, IProjectService
    {
        protected ITaskRegister TaskRegister { get; set; }

        public ProjectService(IUnitOfWork unitOfWork, IAuth auth, ICacheService cacheService, ITaskRegister taskRegister)
            : base(unitOfWork, auth, cacheService)
        {
            TaskRegister = taskRegister;
        }

        protected override System.Data.Entity.IDbSet<Project> Table
        {
            get { return UnitOfWork.Projects; }
        }

        protected override Project Find(int id, IQueryable<Project> query)
        {
            Type type = typeof(Project);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Project)this.CacheService.GetById(cacheKey);

            var project = query.ByIdMust(id);
            this.CacheService.Create(cacheKey, project);
            return project;
        }

        protected override async Task<Project> FindAsync(int id, IQueryable<Project> query)
        {
            Type type = typeof(Project);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Project)this.CacheService.GetById(cacheKey);

            var project = await query.ByIdAsync(id);
            this.CacheService.Create(cacheKey, project);
            return project;
        }

        protected override string SecurityEntities
        {
            get { return Rights.Projects.Name; }
        }

        protected override IQueryable<Project> FilterCanView(IQueryable<Project> query)
        {
            return query.Where(x => x.CustomerId == UserCustomerId || x.Tasks.Any(y => y.Steps.Any(z => z.EmployeeId == UserEmployeeId)));
        }

        protected override bool HasAccessTo(Project entity)
        {
            if (entity.CustomerId != 0)
            {
                return entity.CustomerId == UserCustomerId;
            }
            else if (entity.ProjectId != 0)
            {
                return UnitOfWork.Projects.Any(x => x.ProjectId == entity.ProjectId && (
                    x.CustomerId == UserCustomerId ||
                    x.Tasks.Any(y => y.Steps.Any(z => z.EmployeeId == UserEmployeeId))));
            }
            else
            {
                return false;
            }
        }

        public void Save(ProjectEdit model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            if (string.IsNullOrEmpty(model.BoardName))
            {
                var customer = UnitOfWork.Customers.AsNoTracking().ByIdMust(model.CustomerId);
                model.BoardName = TaskRegister.DefaultProjectName(customer.Name);
            }

            if (model.ProjectId == 0)
            {
                var entity = new Project();
                Add(entity, model);
            }
            else
            {
                var entity = new Project { ProjectId = model.ProjectId };
                Edit(entity, model);
            }

            ClearCache();
        }

        private void ClearCache()
        {
            try
            {
                Type type = typeof(Project);
                this.CacheService.DeleteByContaining(type.Name);
            }
            catch (Exception)
            {
            }
        }
    }
}
