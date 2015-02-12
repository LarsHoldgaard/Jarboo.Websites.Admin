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

namespace Jarboo.Admin.BL.Services
{
    public class SpentTimeService : BaseEntityService<int, SpentTime>, ISpentTimeService
    {
        public SpentTimeService(IUnitOfWork unitOfWork, IAuth auth, ICacheService cacheService)
            : base(unitOfWork, auth, cacheService)
        {
            
        }

        protected override IDbSet<SpentTime> Table
        {
            get { return UnitOfWork.SpentTimes; }
        }

        protected override SpentTime Find(int id, IQueryable<SpentTime> query)
        {
            return query.ById(id);
        }

        protected override async Task<SpentTime> FindAsync(int id, IQueryable<SpentTime> query)
        {
            return await query.ByIdAsync(id);
        }

        protected override string SecurityEntities
        {
            get { return Rights.SpentTime.Name; }
        }

        protected override IQueryable<SpentTime> FilterCanView(IQueryable<SpentTime> query)
        {
            return query.Where(x => (x.ProjectId.HasValue && x.Project.CustomerId == UserCustomerId) || 
                (x.TaskId.HasValue && x.TaskStep.Task.Project.CustomerId == UserCustomerId) ||
                x.EmployeeId == UserEmployeeId);
        }

        protected override bool HasAccessTo(SpentTime entity)
        {
            if (entity.SpentTimeId != 0)
            {
                return UnitOfWork.SpentTimes.Any(x => x.SpentTimeId == entity.SpentTimeId && (
                    x.EmployeeId == UserEmployeeId ||
                    (x.ProjectId.HasValue && x.Project.CustomerId == UserCustomerId) ||
                    (x.TaskId.HasValue && x.TaskStep.Task.Project.CustomerId == UserCustomerId)));
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

        private void FillNewlyCreated(SpentTime entity, int employeeId)
        {
            var employee = UnitOfWork.Employees.AsNoTracking().ByIdMust(employeeId);
            entity.HourlyPrice = employee.HourlyPrice;

            if (this.CanAccept())
            {
                entity.Accepted = true;
                entity.DateVerified = DateTime.Now;
            }
        }
        public void SpentTimeOnTask(SpentTimeOnTask model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var entity = new SpentTime();
            FillNewlyCreated(entity, model.EmployeeId);

            Add(entity, model);
        }

        public void SpentTimeOnProject(SpentTimeOnProject model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var entity = new SpentTime();
            FillNewlyCreated(entity, model.EmployeeId);

            Add(entity, model);
        }

        public bool CanAccept()
        {
            return Can(Rights.SpentTime.AcceptAny);
        }

        public void CheckCanAccept()
        {
            if (!CanAccept())
            {
                this.OnAccessDenied();
            }
        }

        public void Accept(int id, IBusinessErrorCollection errors)
        {
            var entity = Table.ByIdMust(id);

            entity.DateModified = DateTime.Now;
            entity.DateVerified = DateTime.Now;
            entity.Accepted = true;

            CheckCanAccept();

            UnitOfWork.SaveChanges();
        }
        
        public void CheckCanDeny()
        {
            if (this.Cannot(Rights.SpentTime.DenyAny))
            {
                this.OnAccessDenied();
            }
        }

        public void Deny(int id, IBusinessErrorCollection errors)
        {
            var entity = Table.ByIdMust(id);

            entity.DateModified = DateTime.Now;
            entity.DateVerified = DateTime.Now;
            entity.Accepted = false;

            CheckCanDeny();

            UnitOfWork.SaveChanges();
        }
    }
}
