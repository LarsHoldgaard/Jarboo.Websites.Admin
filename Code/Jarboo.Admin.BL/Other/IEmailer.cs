using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Other
{
    public interface IEmailer
    {
        void SendPasswordRecoveryEmail(string email, string link);
    }
}
