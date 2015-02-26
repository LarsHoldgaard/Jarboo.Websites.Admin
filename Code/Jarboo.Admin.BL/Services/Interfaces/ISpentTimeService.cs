using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Services.Interfaces
{
    public interface ISpentTimeService : IEntityService<int, SpentTime>
    {
        void SpentTimeOnTask(SpentTimeOnTask model, IBusinessErrorCollection errors);
        void SpentTimeOnProject(SpentTimeOnProject model, IBusinessErrorCollection errors);

        void Accept(int id, IBusinessErrorCollection errors);
        void Deny(int id, IBusinessErrorCollection errors);

        void Delete(int timeId, IBusinessErrorCollection errors);
    }
}
