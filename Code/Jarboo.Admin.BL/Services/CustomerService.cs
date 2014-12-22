using System;
using System.Collections.Generic;
using System.Linq;

using Jarboo.Admin.BL.Models;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services
{
    public interface ICustomerService
    {
        Customer GetById(int id);
        List<Customer> GetAll();

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

        public List<Customer> GetAll()
        {
            return Table
                .AsEnumerable()
                .ToList();
        }

        public void Create(CustomerCreate model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var entity = Table.Add(new Customer());
            Add(entity, model);
        }
    }
}
