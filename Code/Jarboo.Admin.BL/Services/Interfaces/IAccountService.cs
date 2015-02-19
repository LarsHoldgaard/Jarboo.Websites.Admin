using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services.Interfaces
{
    public interface IAccountService
    {
        User Login(UserLogin model, IBusinessErrorCollection errors);
        void RecoverPassword(PasswordRecover model, IBusinessErrorCollection errors);
        void ResetPassword(ResetPassword model, IBusinessErrorCollection errors);
    }
}
