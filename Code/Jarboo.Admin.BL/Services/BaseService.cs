using Jarboo.Admin.DAL;

namespace Jarboo.Admin.BL.Services
{
    public abstract class BaseService
    {
        public BaseService(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }

        protected IUnitOfWork UnitOfWork { get; set; }
    }
}
