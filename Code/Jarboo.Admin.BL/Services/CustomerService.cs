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
            return query.FirstOrDefault(x => x.CustomerId == id);
        }

        protected override async System.Threading.Tasks.Task<Customer> FindAsync(int id, IQueryable<Customer> query)
        {
            return await query.FirstOrDefaultAsync(x => x.CustomerId == id);
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
        }
    }
}
