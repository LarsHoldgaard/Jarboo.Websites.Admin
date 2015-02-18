using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services.Interfaces
{
    public interface IQuestionService : IEntityService<int, Question>
    {
        void Save(Question model, IBusinessErrorCollection errors);
        void Edit(Question model);
    }
}
