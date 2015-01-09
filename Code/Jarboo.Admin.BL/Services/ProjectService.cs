using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.External;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services
{
    public interface IProjectService : IEntityService<Project>
    {
        void Create(ProjectCreate model, IBusinessErrorCollection errors);
    }

    public class ProjectService : BaseEntityService<Project>, IProjectService
    {
        public ProjectService(IUnitOfWork UnitOfWork)
            : base(UnitOfWork)
        { }

        protected override System.Data.Entity.IDbSet<Project> Table
        {
            get { return UnitOfWork.Projects; }
        }
        protected override Project Find(int id, IQueryable<Project> query)
        {
            return query.FirstOrDefault(x => x.ProjectId == id);
        }

        public void Create(ProjectCreate model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var entity = new Project();
            Add(entity, model);
        }
    }
}
