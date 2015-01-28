using Jarboo.Admin.BL.Authorization;
using Jarboo.Admin.DAL;

namespace Jarboo.Admin.BL.Services
{
    public abstract class BaseService
    {
        protected BaseService(IUnitOfWork unitOfWork, IAuth auth)
        {
            UnitOfWork = unitOfWork;
            Auth = auth;
        }

        protected IUnitOfWork UnitOfWork { get; set; }
        protected IAuth Auth { get; set; }

        protected int? UserCustomerId
        {
            get
            {
                return Auth.User.Customer == null ? null : (int?)Auth.User.Customer.CustomerId;
            }
        }
    }
}
