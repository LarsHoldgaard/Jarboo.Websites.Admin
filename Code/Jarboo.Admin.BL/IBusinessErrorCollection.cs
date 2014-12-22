using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL
{
    public interface IBusinessErrorCollection
    {
        void Add(string property, string error);
        bool HasErrors();
    }
}
