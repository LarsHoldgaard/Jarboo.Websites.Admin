using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services
{
    public interface IEntityService<T>
        where T : BaseEntity
    {
        T GetById(int id);
        List<T> GetAll();

        T GetByIdEx(int id, Include<T> include);
        PagedData<T> GetAllEx(Include<T> include, Filter<T> filter);
    }
}
