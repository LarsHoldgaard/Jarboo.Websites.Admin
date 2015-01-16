using System;
using System.Data.Entity;
using System.Linq;

using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Other
{
    public interface ITaskStepEmployeeStrategy
    {
        Employee SelectEmployee(TaskStepEnum step, int projectId);
    }

    public class TaskStepEmployeeStrategy : ITaskStepEmployeeStrategy
    {
        protected IUnitOfWork UnitOfWork { get; set; }

        public TaskStepEmployeeStrategy(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public Employee SelectEmployee(TaskStepEnum step, int projectId)
        {

            var employees = UnitOfWork.Employees.AsNoTracking().Include(Include.ForEmployee().Positions()).Filter(Filter.ForEmployee()).Data;
            if (employees.Count == 0)
            {
                throw new ApplicationException("At least one employee should be created first");
            }

            var requiredPosition = step.GetPosition();
            if (employees.Any(x => x.Positions.Any(y => y.Position == requiredPosition)))
            {
                employees = employees.Where(x => x.Positions.Any(y => y.Position == requiredPosition)).ToList();
            }

            return employees[new Random().Next(employees.Count)];
        }
    }
}
