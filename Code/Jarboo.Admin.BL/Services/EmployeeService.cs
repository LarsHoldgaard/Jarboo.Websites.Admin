using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;

using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.DAL.Extensions;

namespace Jarboo.Admin.BL.Services
{
    public interface IEmployeeService : IEntityService<Employee>
    {
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
        protected override Employee Find(int id, IQueryable<Employee> query)
        {
            return query.ById(id);
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
                UnitOfWork.EmployeePositions.Where(x => x.EmployeeId == model.EmployeeId).Delete();

                var entity = new Employee { EmployeeId = model.EmployeeId };
                Edit(entity, model);
            }
        }
    }
}
