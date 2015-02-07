using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Sorters;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services
{
    public interface IEntityService<TKey, T>
        where T : IBaseEntity
    {
        T GetById(TKey id);
        T GetByIdEx(TKey id, Include<T> include);

        PagedData<T> GetAll(IQuery<T, Include<T>, Filter<T>, Sorter<T>> query);
    }
}
