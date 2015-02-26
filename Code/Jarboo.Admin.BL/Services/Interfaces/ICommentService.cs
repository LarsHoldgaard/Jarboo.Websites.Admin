using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services.Interfaces
{
    public interface ICommentService : IEntityService<int, Comment>
    {
        void Save(Comment model, IBusinessErrorCollection errors);
    }
}
