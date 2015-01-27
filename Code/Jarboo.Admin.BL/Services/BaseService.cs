using Jarboo.Admin.DAL;

namespace Jarboo.Admin.BL.Services
{
    public abstract class BaseService
    {
        protected BaseService(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }

        protected IUnitOfWork UnitOfWork { get; set; }
    }
}
