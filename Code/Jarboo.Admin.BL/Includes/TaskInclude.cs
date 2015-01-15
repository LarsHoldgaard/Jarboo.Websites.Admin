using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Includes
{
    public class TaskInclude : Include<DAL.Entities.Task>
    {
        private bool project;
        private bool customer;
        private bool taskSteps;
        private bool employee;

        public TaskInclude Customer()
        {
            customer = true;
            return this;
        }
        public TaskInclude Project()
        {
            project = true;
            return this;
        }
        public TaskInclude TaskSteps()
        {
            taskSteps = true;
            return this;
        }
        public TaskInclude Employee()
        {
            employee = true;
            return this;
        }

        public override IQueryable<DAL.Entities.Task> Execute(IQueryable<DAL.Entities.Task> query)
        {
            if (project)
            {
                query = query.Include(x => x.Project);
            }

            if (customer)
            {
                query = query.Include(x => x.Project.Customer);
            }

            if (taskSteps)
            {
                query = query.Include(x => x.Steps);
            }

            if (employee)
            {
                query = query.Include(x => x.Steps.Select(y => y.Employee));
            }

            return query;
        }
    }
}
