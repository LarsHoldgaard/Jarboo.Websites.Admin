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
        public bool? Hired { get; set; }
        public string Query { get; set; }

        public EmployeeFilter WithDeleted()
        {
            this.ShowDeleted = true;
            return this;
        }

        public EmployeeFilter IsHired(bool? isHired)
        {
            this.Hired = isHired;
            return this;
        }

        public EmployeeFilter FilterBy(string query)
        {
            this.Query = query;
            return this;
        }

        public override IQueryable<Employee> Execute(IQueryable<Employee> query)
        {
            if (!ShowDeleted)
            {
                query = query.Where(x => !x.DateDeleted.HasValue);
            }
            if (this.Hired.HasValue)
            {
                query = query.Where(x => x.IsHired == this.Hired);
            }
            if (!string.IsNullOrEmpty(this.Query))
            {
                query = query.Where(x => x.FullName.Contains(this.Query) || x.Email.Contains(this.Query));
            }
            return base.Execute(query);
        }
    }
}
