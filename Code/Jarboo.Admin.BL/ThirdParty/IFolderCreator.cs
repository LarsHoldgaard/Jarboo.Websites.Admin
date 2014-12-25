using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.ThirdParty
{
    public interface IFolderCreator
    {
        void Create(string customerName, string taskTitle);
        void Delete(string customerName, string taskTitle);
    }
}
