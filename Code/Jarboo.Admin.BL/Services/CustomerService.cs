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

using Microsoft.AspNet.Identity;

namespace Jarboo.Admin.BL.Services
{
    public class CustomerService : BaseEntityService<int, Customer>, ICustomerService
    {
        public UserManager<User> UserManager { get; set; }

        public CustomerService(
            IUnitOfWork unitOfWork,
            IAuth auth,
            ICacheService cacheService,
            UserManager<User> userManager)
            : base(unitOfWork, auth, cacheService)
        {
            UserManager = userManager;
        }

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

            if (UnitOfWork.Users.Any(x => x.DisplayName == model.Name) || UnitOfWork.Customers.Any(x => x.Name == model.Name))
            {
                errors.Add("Name", "Name already taken");
                return;
            }

            using (var transaction = UnitOfWork.BeginTransaction())
            {
                try
                {
                    User user = null;

                    if (!string.IsNullOrEmpty(model.Email))
                    {
                        user = model.MapTo<User>();

                        var result = UserManager.Create(user, model.Password);
                        if (!result.Succeeded)
                        {
                            errors.AddErrorsFromResult(result);
                            transaction.Rollback();
                            return;
                        }

                        result = UserManager.AddToRole(user.Id, UserRoles.Customer.ToString());
                        if (!result.Succeeded)
                        {
                            errors.AddErrorsFromResult(result);
                            transaction.Rollback();
                            return;
                        }
                    }

                    var customer = model.MapTo<Customer>();
                    customer.User = user;

                    UnitOfWork.Customers.Add(customer);
                    UnitOfWork.SaveChanges();

                    transaction.Commit();

                    customer.MapTo(model);

                    ClearCache();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
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
