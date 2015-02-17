using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jarboo.Admin.BL.Services.Interfaces
{
    public interface ITaskService : IEntityService<int, Task>
    {
        void Save(TaskEdit model, IBusinessErrorCollection errors);

        void NextStep(TaskNextStep model, IBusinessErrorCollection errors);

        void Delete(int taskId, IBusinessErrorCollection errors);
    }
}
