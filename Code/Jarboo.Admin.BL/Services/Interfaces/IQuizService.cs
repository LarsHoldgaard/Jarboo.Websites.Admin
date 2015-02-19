using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services.Interfaces
{
    public interface IQuizService : IEntityService<int, Quiz>
    {
        void Save(QuizEdit model, IBusinessErrorCollection errors);
        void Delete(int id, IBusinessErrorCollection errors);
    }
}
