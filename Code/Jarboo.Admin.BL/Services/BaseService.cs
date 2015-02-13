using System;

using Jarboo.Admin.BL.Authorization;
using Jarboo.Admin.DAL;
using Jarboo.Admin.BL.Services.Interfaces;

namespace Jarboo.Admin.BL.Services
{
    public abstract class BaseService
    {
        protected BaseService(IUnitOfWork unitOfWork, IAuth auth, ICacheService cacheService)
        {
            UnitOfWork = unitOfWork;
            Auth = auth;
            CacheService = cacheService;
        }

        protected abstract string SecurityEntities { get; }
        protected bool Can(string action)
        {
            return Auth.Can(SecurityEntities, action);
        }
        protected bool Cannot(string action)
        {
            return !Can(action);
        }
        protected void OnAccessDenied()
        {
            throw new ApplicationException("Access denied");
        }

        protected IUnitOfWork UnitOfWork { get; set; }
        protected IAuth Auth { get; set; }
        protected ICacheService CacheService { get; set; }

        protected string UserId
        {
            get
            {
                return Auth.User.Id;
            }
        }
        protected int? UserCustomerId
        {
            get
            {
                return Auth.User.CustomerId;
            }
        }
        protected int? UserEmployeeId
        {
            get
            {
                return Auth.User.EmployeeId;
            }
        }
    }
}
