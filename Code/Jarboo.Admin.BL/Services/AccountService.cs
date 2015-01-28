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
        void Edit(UserEdit model, IBusinessErrorCollection errors);
        void ChangePassword(UserPasswordChange model, IBusinessErrorCollection errors);
        void SetPassword(UserPasswordSet model, IBusinessErrorCollection errors);
    }

    public class AccountService : BaseService, IAccountService
    {
        public UserManager<User> UserManager { get; set; }

        public AccountService(IUnitOfWork unitOfWork, IAuth auth, UserManager<User> userManager)
            : base(unitOfWork, auth)
        {
            UserManager = userManager;
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
                        AddErrorsFromResult(result, errors);
                        transaction.Rollback();
                        return;
                    }

                    result = UserManager.AddToRole(user.Id, UserRoles.Customer.ToString());
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

        public void Edit(UserEdit model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var user = UserManager.FindById(model.UserId);
            if (user == null)
            {
                throw new ApplicationException("User not found");
            }

            model.MapTo(user);
            var result = UserManager.UserValidator.ValidateAsync(user).GetAwaiter().GetResult();
            if (!result.Succeeded)
            {
                this.AddErrorsFromResult(result, errors);
                return;
            }

            using (var transaction = UnitOfWork.BeginTransaction())
            {
                try
                {
                    result = UserManager.Update(user);
                    if (!result.Succeeded)
                    {
                        this.AddErrorsFromResult(result, errors);
                        transaction.Rollback();
                        return;
                    }

                    var customer = UnitOfWork.Customers.ByUserId(user.Id);
                    if (customer != null)
                    {
                        customer.Name = user.DisplayName;
                    }

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

        public void ChangePassword(UserPasswordChange model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var user = UserManager.FindById(model.UserId);
            if (user == null)
            {
                throw new ApplicationException("User not found");
            }

            var result = UserManager.ChangePassword(user.Id, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                this.AddErrorsFromResult(result, errors);
                return;
            }
        }

        public void SetPassword(UserPasswordSet model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var user = UserManager.FindById(model.UserId);
            if (user == null)
            {
                throw new ApplicationException("User not found");
            }

            var result = UserManager.PasswordValidator.ValidateAsync(model.Password).GetAwaiter().GetResult();
            if (!result.Succeeded)
            {
                this.AddErrorsFromResult(result, errors);
                return;
            }

            user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);

            result = UserManager.Update(user);
            if (!result.Succeeded)
            {
                this.AddErrorsFromResult(result, errors);
                return;
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
