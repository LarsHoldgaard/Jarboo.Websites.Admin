using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.Includes;

namespace Jarboo.Admin.BL.Services
{
    public interface IEntityService<T>
    {
        T GetById(int id);
        List<T> GetAll();

        T GetByIdEx(int id, Include<T> include);
        List<T> GetAllEx(Include<T> include);
    }
}
