using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.External;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services
{
    public interface IEmployeeService
    {
        Employee GetById(int id);
        List<Employee> GetAll();

        void Save(EmployeeEdit model, IBusinessErrorCollection errors);
    }

    public class EmployeeService : BaseEntityService<Employee>, IEmployeeService
    {
        public EmployeeService(IUnitOfWork UnitOfWork)
            : base(UnitOfWork)
        { }

        protected override IDbSet<Employee> Table
        {
            get { return UnitOfWork.Employees; }
        }

        public Employee GetById(int id)
        {
            return TableNoTracking.Include(x => x.Positions).Include(x => x.TaskSteps.Select(y => y.Task)).FirstOrDefault(x => x.EmployeeId == id);
        }

        public List<Employee> GetAll()
        {
            return TableNoTracking.Include(x => x.Positions)
                .AsEnumerable()
                .ToList();
        }

        public void Save(EmployeeEdit model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            if (model.EmployeeId == 0)
            {
                var entity = new Employee();
                Add(entity, model);
            }
            else
            {
                foreach (var position in UnitOfWork.EmployeePositions.Where(x => x.EmployeeId == model.EmployeeId))
                {
                    UnitOfWork.EmployeePositions.Remove(position);
                }

                var entity = new Employee { EmployeeId = model.EmployeeId };
                Edit(entity, model);
            }
        }
    }
}
