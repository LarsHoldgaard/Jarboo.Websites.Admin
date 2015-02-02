using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.Other;

namespace Jarboo.Admin.Integration.Noop
{
    public class NoopEmailer : IEmailer
    {
        public void SendPasswordRecoveryEmail(string email, string link)
        { }
    }
}
