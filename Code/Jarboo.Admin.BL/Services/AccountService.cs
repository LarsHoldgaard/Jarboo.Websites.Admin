﻿using System;
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
using Jarboo.Admin.BL.Services.Interfaces;

namespace Jarboo.Admin.BL.Services
{
    public class AccountService : BaseService, IAccountService
    {
        public UserManager<User> UserManager { get; set; }
        public IEmailer Emailer { get; set; }


        public AccountService(IUnitOfWork unitOfWork, IAuth auth, ICacheService cacheService, UserManager<User> userManager, IEmailer emailer)
            : base(unitOfWork, auth, cacheService)
        {
            UserManager = userManager;
            Emailer = emailer;
        }

        protected override string SecurityEntities
        {
            get { return Rights.Accounts.Name; }
        }

        public User Login(UserLogin model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return null;
            }

            var user = UserManager.Find(model.Email, model.Password);
            if (user == null)
            {
                errors.Add("", "Invalid email or password");
                return null;
            }

            user.DateLastLogin = DateTime.Now;
            UnitOfWork.SaveChanges();

            return user;
        }

        public void Register(UserCreate model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            if (UnitOfWork.Users.Any(x => x.DisplayName == model.Name))
            {
                errors.Add("Name", "Name already taken");
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
                                                     User = user,
                                                     Country = model.Country,
                                                     Creator = model.Creator
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

        public void RecoverPassword(PasswordRecover model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var user = UserManager.FindByEmail(model.Email);
            if (user == null)
            {
                errors.Add("", "Invalid email");
                return;
            }

            var code = UserManager.GeneratePasswordResetToken(user.Id);

            var recoverUrl = string.Format(model.LinkTemplate,
                Uri.EscapeDataString(user.Id),
                Uri.EscapeDataString(code));

            Emailer.SendPasswordRecoveryEmail(user.Email, recoverUrl);
        }

        public void ResetPassword(ResetPassword model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            var result = UserManager.PasswordValidator.ValidateAsync(model.Password).GetAwaiter().GetResult();
            if (!result.Succeeded)
            {
                errors.AddErrorsFromResult(result);
                return;
            }

            result = UserManager.ResetPassword(model.UserId, model.Code, model.Password);
            if (!result.Succeeded)
            {
                errors.AddErrorsFromResult(result);
                return;
            }
        }
    }
}
