using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.DAL.Entities
{
    public interface IBaseEntity
    {
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }
    }

    public class BaseEntity : IBaseEntity
    {
        public BaseEntity()
        {
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
