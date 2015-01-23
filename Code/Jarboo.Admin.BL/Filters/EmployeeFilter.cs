using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Filters
{
    public class EmployeeFilter : Filter<Employee>
    {
        public bool ShowDeleted { get; set; }

        public EmployeeFilter WithDeleted()
        {
            this.ShowDeleted = true;
            return this;
        }

        public override IQueryable<Employee> Execute(IQueryable<Employee> query)
        {
            if (!ShowDeleted)
            {
                query = query.Where(x => !x.DateDeleted.HasValue);
            }

            return base.Execute(query);
        }
    }
}
