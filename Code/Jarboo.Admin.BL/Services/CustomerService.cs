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

namespace Jarboo.Admin.BL.Services
{
    public interface ICustomerService : IEntityService<int, Customer>
    {
        void Create(CustomerCreate model, IBusinessErrorCollection errors);
    }

    public class CustomerService : BaseEntityService<int, Customer>, ICustomerService
    {
        public CustomerService(IUnitOfWork unitOfWork, IAuth auth)
            : base(unitOfWork, auth)
        { }

        protected override System.Data.Entity.IDbSet<Customer> Table
        {
            get { return UnitOfWork.Customers; }
        }
        protected override Customer Find(int id, IQueryable<Customer> query)
        {
            return query.FirstOrDefault(x => x.CustomerId == id);
        }

        protected override string SecurityEntities
        {
            get { return Rights.Customers.Name; }
        }
        protected override IQueryable<Customer> FilterCanView(IQueryable<Customer> query)
        {
            return query.Where(x => x.CustomerId == UserCustomerId);
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
