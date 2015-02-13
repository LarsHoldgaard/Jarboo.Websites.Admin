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
    public interface IEmployeeService : IEntityService<int, Employee>
    {
        void Create(EmployeeCreate model, IBusinessErrorCollection errors);
        void Edit(EmployeeEdit model, IBusinessErrorCollection errors);

        void Delete(int employeeId, IBusinessErrorCollection errors);
    }
}
