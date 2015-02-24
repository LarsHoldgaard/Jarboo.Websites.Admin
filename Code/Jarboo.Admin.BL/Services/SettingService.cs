using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jarboo.Admin.BL.Authorization;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.BL.Services.Interfaces;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.DAL.Extensions;

namespace Jarboo.Admin.BL.Services
{
    public class SettingService : BaseEntityService<int, Setting>, ISettingService
    {
        private readonly string _currentConfiguration = ConfigurationManager.AppSettings["SettingConfiguration"];
        public SettingService(IUnitOfWork unitOfWork, IAuth auth, ICacheService cacheService) : base(unitOfWork, auth, cacheService)
        {
        }

        protected override string SecurityEntities
        {
            get { return Rights.Settings.Name; }
        }

        protected override IDbSet<Setting> Table
        {
            get { return UnitOfWork.Settings; }
        }

        protected override async Task<Setting> FindAsync(int id, IQueryable<Setting> query)
        {
            return await query.FirstOrDefaultAsync(f => f.Id == id);
        }

        protected override Setting Find(int id, IQueryable<Setting> query)
        {
            return query.FirstOrDefault(f => f.Id == id);
        }

        protected override IQueryable<Setting> FilterCanView(IQueryable<Setting> query)
        {
            return query.Where(w => w.Configuration == _currentConfiguration);
        }

        public void Edit(SettingEdit model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var entity = Table.ByIdMust(model.Id);

            Edit(entity, model);
        }

        public Setting GetCurrentSetting()
        {
            return TableNoTracking.FirstOrDefault(f => f.Configuration == _currentConfiguration);
        }

        protected override bool HasAccessTo(Setting entity)
        {
            return false;
        }
    }
}
