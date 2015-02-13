using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Sorters;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services.Interfaces
{
    public interface IEntityService<TKey, T> where T : IBaseEntity
    {
        T GetById(TKey id);
        System.Threading.Tasks.Task<T> GetByIdAsync(TKey id);
        T GetByIdEx(TKey id, Include<T> include);
        System.Threading.Tasks.Task<T> GetByIdExAsync(TKey id, Include<T> include);

        PagedData<T> GetAll(IQuery<T, Include<T>, Filter<T>, Sorter<T>> query);
        System.Threading.Tasks.Task<PagedData<T>> GetAllAsync(IQuery<T, Include<T>, Filter<T>, Sorter<T>> query);
    }
}
