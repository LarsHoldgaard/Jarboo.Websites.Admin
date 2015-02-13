using Jarboo.Admin.BL.Authorization;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.BL.Services.Interfaces;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Services
{
    public class DocumentationService : BaseEntityService<int, Documentation>, IDocumentationService
    {
        public DocumentationService(IUnitOfWork unitOfWork, IAuth auth, ICacheService cacheService)
            : base(unitOfWork, auth, cacheService)
        { }

        protected override IDbSet<Documentation> Table
        {
            get { return UnitOfWork.Documentations; }
        }
        protected override Documentation Find(int id, IQueryable<Documentation> query)
        {
            Type type = typeof(Documentation);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Documentation)this.CacheService.GetById(cacheKey);

            var doc = query.FirstOrDefault(x => x.DocumentationId == id);
            this.CacheService.Create(cacheKey, doc);
            return doc;
        }

        protected override async Task<Documentation> FindAsync(int id, IQueryable<Documentation> query)
        {
            Type type = typeof(Documentation);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Documentation)this.CacheService.GetById(cacheKey);

            var doc = await query.FirstOrDefaultAsync(x => x.DocumentationId == id);
            this.CacheService.Create(cacheKey, doc);
            return doc;
        }

        protected override string SecurityEntities
        {
            get { return Rights.Documentations.Name; }
        }
        protected override IQueryable<Documentation> FilterCanView(IQueryable<Documentation> query)
        {
            return query.Where(x => x.Project.CustomerId == UserCustomerId || x.Project.Tasks.Any(y => y.Steps.Any(z => z.EmployeeId == UserEmployeeId)));
        }
        protected override bool HasAccessTo(Documentation entity)
        {
            if (entity.ProjectId != 0)
            {
                return UnitOfWork.Projects.Any(x => x.ProjectId == entity.ProjectId && x.CustomerId == UserCustomerId);
            }
            else if (entity.DocumentationId != 0)
            {
                return UnitOfWork.Documentations.Any(x => x.DocumentationId == entity.DocumentationId && x.Project.CustomerId == UserCustomerId);
            }
            else
            {
                return false;
            }
        }

        public void Save(DocumentationEdit model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            if (model.DocumentationId == 0)
            {
                var entity = new Documentation();
                Add(entity, model);
            }
            else
            {
                var entity = new Documentation { DocumentationId = model.DocumentationId };
                Edit(entity, model);
            }

            ClearCache();
        }

        public void Delete(int documentationId, IBusinessErrorCollection errors)
        {
            if (documentationId == 0)
            {
                throw new Exception("Incorrect entity id");
            }

            Delete(new Documentation()
            {
                DocumentationId = documentationId
            });

            ClearCache();
        }

        private void ClearCache()
        {
            try
            {
                Type type = typeof(Documentation);
                this.CacheService.DeleteByContaining(type.Name);
            }
            catch (Exception)
            {
            }
        }
    }
}
