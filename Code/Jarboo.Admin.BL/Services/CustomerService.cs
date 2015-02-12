using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Jarboo.Admin.BL.Authorization;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.BL.Services.Interfaces;
using System.Reflection;

namespace Jarboo.Admin.BL.Services
{
    public class CustomerService : BaseEntityService<int, Customer>, ICustomerService
    {
        public CustomerService(IUnitOfWork unitOfWork, IAuth auth, ICacheService cacheService)
            : base(unitOfWork, auth, cacheService)
        { }

        protected override System.Data.Entity.IDbSet<Customer> Table
        {
            get { return UnitOfWork.Customers; }
        }
        
        protected override Customer Find(int id, IQueryable<Customer> query)
        {
            Type type = typeof(Customer);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Customer)this.CacheService.GetById(cacheKey);

            var customer = query.FirstOrDefault(x => x.CustomerId == id);
            this.CacheService.Create(cacheKey, customer);
            return customer;
        }

        protected override async System.Threading.Tasks.Task<Customer> FindAsync(int id, IQueryable<Customer> query)
        {
            Type type = typeof(Customer);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Customer)this.CacheService.GetById(cacheKey);

            var customer = await query.FirstOrDefaultAsync(x => x.CustomerId == id);
            this.CacheService.Create(cacheKey, customer);
            return customer;
        }

        protected override string SecurityEntities
        {
            get { return Rights.Customers.Name; }
        }

        protected override IQueryable<Customer> FilterCanView(IQueryable<Customer> query)
        {
            return query.Where(x => x.CustomerId == UserCustomerId || x.Projects.Any(y => y.Tasks.Any(z => z.Steps.Any(a => a.EmployeeId == UserEmployeeId))));
        }

        public void Create(CustomerCreate model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var entity = new Customer();
            Add(entity, model);
            ClearCache();
        }

        private void ClearCache()
        {
            try
            {
                Type type = typeof(Customer);
                this.CacheService.DeleteByContaining(type.Name);
            }
            catch (Exception)
            {
            }
        }
    }
}
