using System;
using System.Collections.Generic;
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
    public interface IUserService : IEntityService<string, User>
    {
        void Edit(UserEdit model, IBusinessErrorCollection errors);
        void ChangePassword(UserPasswordChange model, IBusinessErrorCollection errors);
        void SetPassword(UserPasswordSet model, IBusinessErrorCollection errors);
    }

    public class UserService : BaseEntityService<string, User>, IUserService
    {
        public UserManager<User> UserManager { get; set; }

        public UserService(IUnitOfWork unitOfWork, IAuth auth, UserManager<User> userManager)
            : base(unitOfWork, auth)
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

            CheckCanEdit(user);

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

                    var customer = UnitOfWork.Customers.ByUserId(user.Id);
                    if (customer != null)
                    {
                        customer.Name = user.DisplayName;
                    }

                    var employee = UnitOfWork.Employees.ByUserId(user.Id);
                    if (employee != null)
                    {
                        employee.FullName = user.DisplayName;
                        employee.Email = user.Email;
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
