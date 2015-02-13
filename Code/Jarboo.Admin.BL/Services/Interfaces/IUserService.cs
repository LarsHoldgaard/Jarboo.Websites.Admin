using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Services.Interfaces
{
    public interface IUserService : IEntityService<string, User>
    {
        void EditCustomer(UserCustomerEdit model, IBusinessErrorCollection errors);
        void Edit(UserEdit model, IBusinessErrorCollection errors);
        void ChangePassword(UserPasswordChange model, IBusinessErrorCollection errors);
        void SetPassword(UserPasswordSet model, IBusinessErrorCollection errors);
    }

}
