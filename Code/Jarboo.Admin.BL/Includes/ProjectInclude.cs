using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Includes
{
    public class ProjectInclude : Include<Project>
    {
        private bool customer;
        private bool taskSteps;
        private bool tasks;

        public ProjectInclude Customer()
        {
            customer = true;
            return this;
        }
        public ProjectInclude TaskSteps()
        {
            taskSteps = true;
            return this;
        }
        public ProjectInclude Tasks()
        {
            tasks = true;
            return this;
        }

        public override IQueryable<Project> Execute(IQueryable<Project> query)
        {
            if (customer)
            {
                query = query.Include(x => x.Customer);
            }

            if (tasks)
            {
                query = query.Include(x => x.Tasks);
            }

            if (taskSteps)
            {
                query = query.Include(x => x.Tasks.Select(y => y.Steps));
            }

            return query;
        }
    }
}
