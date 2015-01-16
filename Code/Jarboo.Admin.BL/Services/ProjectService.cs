using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.DAL.Extensions;

namespace Jarboo.Admin.BL.Services
{
    public interface IProjectService : IEntityService<Project>
    {
        void Save(ProjectEdit model, IBusinessErrorCollection errors);
    }

    public class ProjectService : BaseEntityService<Project>, IProjectService
    {
        protected ITaskRegister TaskRegister { get; set; }

        public ProjectService(IUnitOfWork unitOfWork, ITaskRegister taskRegister)
            : base(unitOfWork)
        {
            TaskRegister = taskRegister;
        }

        protected override System.Data.Entity.IDbSet<Project> Table
        {
            get { return UnitOfWork.Projects; }
        }
        protected override Project Find(int id, IQueryable<Project> query)
        {
            return query.ByIdMust(id);
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
                model.BoardName = TaskRegister.DefaultBoardName(customer.Name);
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
        }
    }
}
