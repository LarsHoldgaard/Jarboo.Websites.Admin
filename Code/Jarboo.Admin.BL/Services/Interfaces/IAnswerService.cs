using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services.Interfaces
{
    public interface IAnswerService : IEntityService<int, Answer>
    {
        void Save(Answer model, IBusinessErrorCollection errors);
    }
}
