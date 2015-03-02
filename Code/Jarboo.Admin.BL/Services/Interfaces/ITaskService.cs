using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services.Interfaces
{
    public interface ITaskService : IEntityService<int, Task>
    {
        void Save(TaskEdit model, IBusinessErrorCollection errors);

        void NextStep(TaskNextStep model, IBusinessErrorCollection errors);

        void Delete(int taskId, IBusinessErrorCollection errors);

        void UpdateTaskStep(TaskNextStep model, IBusinessErrorCollection errors);
    }
}
