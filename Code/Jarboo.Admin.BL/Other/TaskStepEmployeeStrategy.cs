using System;
using System.Linq;

using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Other
{
    public interface ITaskStepEmployeeStrategy
    {
        int SelectEmployee(TaskStepEnum step, int projectId);
    }

    public class TaskStepEmployeeStrategy : ITaskStepEmployeeStrategy
    {
        protected IEmployeeService EmployeeService { get; set; }

        public TaskStepEmployeeStrategy(IEmployeeService employeeService)
        {
            EmployeeService = employeeService;
        }

        public int SelectEmployee(TaskStepEnum step, int projectId)
        {
            var employees = EmployeeService.GetAllEx(Include.ForEmployee().Positions());
            if (employees.Count == 0)
            {
                throw new ApplicationException("At least one employee should be created first");
            }

            var requiredPosition = step.GetPosition();
            if (employees.Any(x => x.Positions.Any(y => y.Position == requiredPosition)))
            {
                employees = employees.Where(x => x.Positions.Any(y => y.Position == requiredPosition)).ToList();
            }

            return employees[new Random().Next(employees.Count)].EmployeeId;
        }
    }
}
