using System;
using System.Collections.Generic;
using System.Data;
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

using Microsoft.AspNet.Identity;

namespace Jarboo.Admin.BL.Services
{
    public interface IAccountService
    {
        void Register(UserCreate model, IBusinessErrorCollection errors);
    }

    public class AccountService : BaseService, IAccountService
    {
        public UserManager<User> UserManager { get; set; }

        public AccountService(IUnitOfWork unitOfWork, IAuth auth, UserManager<User> userManager)
            : base(unitOfWork, auth)
        {
            UserManager = userManager;
        }

        protected override string SecurityEntities
        {
            get { return Rights.Accounts.Name; }
        }

        public void Register(UserCreate model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var user = model.MapTo<User>();

            using (var transaction = UnitOfWork.BeginTransaction())
            {
                try
                {
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
    }
}
