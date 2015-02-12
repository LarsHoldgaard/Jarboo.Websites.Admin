using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

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
    public class UserService : BaseEntityService<string, User>, IUserService
    {
        public UserManager<User> UserManager { get; set; }

        public UserService(IUnitOfWork unitOfWork, IAuth auth, ICacheService cacheService, UserManager<User> userManager)
            : base(unitOfWork, auth, cacheService)
        {
            UserManager = userManager;
        }

        protected override System.Data.Entity.IDbSet<User> Table
        {
            get { return UnitOfWork.Users; }
        }
        protected override User Find(string id, IQueryable<User> query)
        {
            return query.FirstOrDefault(x => x.Id == id);
        }
        protected override async Task<User> FindAsync(string id, IQueryable<User> query)
        {
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }
        protected override string SecurityEntities
        {
            get { return Rights.Users.Name; }
        }
        protected override IQueryable<User> FilterCanView(IQueryable<User> query)
        {
            return query.Where(x => x.Id == UserId);
        }
        protected override bool HasAccessTo(User entity)
        {
            return entity.Id == UserId;
        }

        public void EditCustomer(UserCustomerEdit model, IBusinessErrorCollection errors)
        {
            Edit(model, errors,
                () =>
                {
                    var customer = UnitOfWork.Customers.ByUserIdMust(model.UserId);
                    customer.Name = model.Name;
                    customer.Country = model.Country;
                    customer.Creator = model.Creator;
                });
        }
        public void Edit(UserEdit model, IBusinessErrorCollection errors)
        {
            Edit(model, errors,
                () =>
                    {
                        var employee = UnitOfWork.Employees.ByUserId(model.UserId);
                        if (employee != null)
                        {
                            employee.FullName = model.Name;
                            employee.Email = model.Email;
                        }
                    });
        }
        private void Edit(UserEdit model, IBusinessErrorCollection errors, Action updateRelatedEntities)
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

            CheckCanEdit(user);

            if (UnitOfWork.Users.Any(x => x.DisplayName == model.Name && x.Id != user.Id))
            {
                errors.Add("Name", "Name already taken");
                return;
            }

            model.MapTo(user);
            var result = UserManager.UserValidator.ValidateAsync(user).GetAwaiter().GetResult();
            if (!result.Succeeded)
            {
                errors.AddErrorsFromResult(result);
                return;
            }

            using (var transaction = UnitOfWork.BeginTransaction())
            {
                try
                {
                    result = UserManager.Update(user);
                    if (!result.Succeeded)
                    {
                        errors.AddErrorsFromResult(result);
                        transaction.Rollback();
                        return;
                    }

                    if (updateRelatedEntities != null)
                    {
                        updateRelatedEntities();
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

            CheckCanEdit(user);

            var result = UserManager.ChangePassword(user.Id, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                errors.AddErrorsFromResult(result);
                return;
            }
        }

        private void CheckCanSetPassword(User entity)
        {
            if (Cannot(Rights.Users.SetPasswordAny))
            {
                OnAccessDenied();
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

            CheckCanSetPassword(user);

            var result = UserManager.PasswordValidator.ValidateAsync(model.Password).GetAwaiter().GetResult();
            if (!result.Succeeded)
            {
                errors.AddErrorsFromResult(result);
                return;
            }

            user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);

            result = UserManager.Update(user);
            if (!result.Succeeded)
            {
                errors.AddErrorsFromResult(result);
                return;
            }
        }
    }
}
