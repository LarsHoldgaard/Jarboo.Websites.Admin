using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
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
            Type type = typeof(SpentTime);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (SpentTime)this.CacheService.GetById(cacheKey);

            var spentTime = query.ById(id);
            this.CacheService.Create(cacheKey, spentTime);
            return spentTime;
        }

        protected override async Task<SpentTime> FindAsync(int id, IQueryable<SpentTime> query)
        {
            Type type = typeof(SpentTime);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (SpentTime)this.CacheService.GetById(cacheKey);

            var spentTime = await query.ByIdAsync(id);
            this.CacheService.Create(cacheKey, spentTime);
            return spentTime;
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
            else if (entity.ProjectId.HasValue && entity.ProjectId != 0)
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

        private void FillNewlyCreated(SpentTime entity, int employeeId, SpentTimeOnTask model = null)
        {
            var employee = UnitOfWork.Employees.AsNoTracking().ByIdMust(employeeId);

            var task = UnitOfWork.Tasks.Include(x => x.Project.Customer).ByIdMust(model.TaskId);

            entity.Price = model.Price.GetValueOrDefault() != 0 ? model.Price : employee.HourlyPrice;

            decimal? resultingCommission;

            if (task.Project.Commission.HasValue)
            {
                resultingCommission = task.Project.Commission.Value;
            }
            else if (task.Project.Customer.Commission.HasValue)
            {
                resultingCommission = task.Project.Customer.Commission.Value;
            }
            else
            {
                resultingCommission = decimal.Parse(ConfigurationManager.AppSettings["BaseCommission"]);
            }

            var total = model.Price.GetValueOrDefault() != 0 ? entity.Price * (1 + resultingCommission) : entity.Price * model.Hours * (1 + resultingCommission);

            if (total.HasValue) entity.Total = (decimal)total;

            entity.Commission = resultingCommission.Value;

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
            FillNewlyCreated(entity, model.EmployeeId, model);

            Add(entity, model);
            ClearCache();
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
            ClearCache();
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
            ClearCache();
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
            ClearCache();
        }

        public void Delete(int spentTimeId, IBusinessErrorCollection errors)
        {
            if (spentTimeId == 0)
            {
                throw new Exception("Incorrect entity id");
            }

            Delete(new SpentTime()
            {
                SpentTimeId = spentTimeId
            });

            ClearCache();
        }

        private void ClearCache()
        {
            try
            {
                Type type = typeof(SpentTime);
                this.CacheService.DeleteByContaining(type.Name);
            }
            catch (Exception)
            {
            }
        }
    }
}
