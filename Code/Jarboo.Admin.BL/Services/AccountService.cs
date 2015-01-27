using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;

using Microsoft.AspNet.Identity;

namespace Jarboo.Admin.BL.Services
{
    public interface IAccountService
    {
        void Register(Register model, IBusinessErrorCollection errors);
    }

    public class AccountService : BaseService, IAccountService
    {
        public UserManager<User> UserManager { get; set; }

        public AccountService(IUnitOfWork UnitOfWork, UserManager<User> userManager)
            :base(UnitOfWork)
        {
            UserManager = userManager;
        }

        public void Register(Register model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var user = new User()
                           {
                               Email = model.Email,
                               UserName = model.Email,
                               DisplayName = model.Name,
                           };
            /*using (var scope = new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
            }*/

            using (var transaction = UnitOfWork.BeginTransaction())
            {
                try
                {
                    var result = UserManager.Create(user, model.Password);
                    if (!result.Succeeded)
                    {
                        AddErrorsFromResult(result, errors);
                        transaction.Rollback();
                        return;
                    }

                    UnitOfWork.Customers.Add(new Customer()
                                                 {
                                                     Name = model.Name,
                                                     User = user
                                                 });
                    UnitOfWork.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private void AddErrorsFromResult(IdentityResult result, IBusinessErrorCollection errors)
        {
            foreach (var error in result.Errors)
            {
                errors.Add("", error);
            }
        }
    }
}
