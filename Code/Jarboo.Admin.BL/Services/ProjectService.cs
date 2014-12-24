using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.Models;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services
{
    public interface IProjectService
    {
        Project GetById(int id);
        List<Project> GetAll();

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

        public override Project GetById(int id)
        {
            return Table.Include(x => x.Customer).Include(x => x.Tasks).FirstOrDefault(x => x.ProjectId == id);
        }

        public List<Project> GetAll()
        {
            return Table.Include(x => x.Customer)
                .AsEnumerable()
                .ToList();
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
