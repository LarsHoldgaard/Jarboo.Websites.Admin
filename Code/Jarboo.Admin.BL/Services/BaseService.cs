using Jarboo.Admin.DAL;

namespace Jarboo.Admin.BL.Services
{
    public abstract class BaseService
    {
        public IUnitOfWork UnitOfWork { get; set; }
    }
}
