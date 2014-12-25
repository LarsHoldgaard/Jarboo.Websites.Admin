using Jarboo.Admin.BL.Services;
using Jarboo.Admin.BL.ThirdParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Ninject;

namespace Jarboo.Admin.Web.Infrastructure.BLExternals
{
    public class RandomTaskStepEmployeeStrategy : ITaskStepEmployeeStrategy
    {
        [Inject]
        public IEmployeeService EmployeeService { get; set; }

        public int SelectEmployee(DAL.Entities.TaskStepEnum step, int projectId)
        {
            var employees = EmployeeService.GetAll();
            if (employees.Count == 0)
            {
                throw new ApplicationException("At least one employee should be created first");
            }

            return employees[new Random().Next(employees.Count)].EmployeeId;
        }
    }
}