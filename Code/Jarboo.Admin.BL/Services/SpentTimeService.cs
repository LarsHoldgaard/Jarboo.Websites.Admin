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

namespace Jarboo.Admin.BL.Services
{
    public interface ISpentTimeService : IEntityService<int, SpentTime>
    {
        void SpentTimeOnTask(SpentTimeOnTask model, IBusinessErrorCollection errors);
        void SpentTimeOnProject(SpentTimeOnProject model, IBusinessErrorCollection errors);
    }

    public class SpentTimeService : BaseEntityService<int, SpentTime>, ISpentTimeService
    {
        public SpentTimeService(IUnitOfWork unitOfWork, IAuth auth)
            : base(unitOfWork, auth)
        {
            
        }

        protected override IDbSet<SpentTime> Table
        {
            get { return UnitOfWork.SpentTimes; }
        }
        protected override SpentTime Find(int id, IQueryable<SpentTime> query)
        {
            return query.FirstOrDefault(x => x.SpentTimeId == id);
        }

        protected override string SecurityEntities
        {
            get { return Rights.SpentTime.Name; }
        }
        protected override IQueryable<SpentTime> FilterCanView(IQueryable<SpentTime> query)
        {
            return query.Where(x => x.Project.CustomerId == UserCustomerId || 
                x.TaskStep.Task.Project.CustomerId == UserCustomerId ||
                x.EmployeeId == UserEmployeeId);
        }
        protected override bool HasAccessTo(SpentTime entity)
        {
            if (entity.SpentTimeId != 0)
            {
                return UnitOfWork.SpentTimes.Any(x => x.SpentTimeId == entity.SpentTimeId && (
                    x.EmployeeId == UserEmployeeId ||
                    x.Project.CustomerId == UserCustomerId ||
                    x.TaskStep.Task.Project.CustomerId == UserCustomerId));
            }
            else if (entity.EmployeeId != 0)
            {
                return entity.EmployeeId == UserEmployeeId;
            }
            else if(entity.ProjectId.HasValue && entity.ProjectId != 0)
            {
                return UnitOfWork.Projects.Any(x => x.ProjectId == entity.ProjectId && x.CustomerId == UserCustomerId);
            }
            else if (entity.TaskId.HasValue && entity.TaskId != 0)
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

        public void SpentTimeOnTask(SpentTimeOnTask model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var entity = new SpentTime();
            Add(entity, model);
        }
        public void SpentTimeOnProject(SpentTimeOnProject model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var entity = new SpentTime();
            Add(entity, model);
        }
    }
}
