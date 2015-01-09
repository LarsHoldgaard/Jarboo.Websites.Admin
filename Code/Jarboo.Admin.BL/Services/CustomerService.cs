using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Jarboo.Admin.BL.External;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services
{
    public interface ICustomerService : IEntityService<Customer>
    {
        void Create(CustomerCreate model, IBusinessErrorCollection errors);
    }

    public class CustomerService : BaseEntityService<Customer>, ICustomerService
    {
        public CustomerService(IUnitOfWork UnitOfWork)
            : base(UnitOfWork)
        { }

        protected override System.Data.Entity.IDbSet<Customer> Table
        {
            get { return UnitOfWork.Customers; }
        }
        protected override Customer Find(int id, IQueryable<Customer> query)
        {
            return query.FirstOrDefault(x => x.CustomerId == id);
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
