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
        private bool taskStepsWithEmployee;
        private bool spentTimes;
        private bool spentTimesWithEmployee;

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
        public TaskInclude TaskSteps(bool withEmployee = false)
        {
            taskSteps = true;
            taskStepsWithEmployee = withEmployee;
            return this;
        }
        public TaskInclude SpentTimes(bool withEmployee = false)
        {
            spentTimes = true;
            spentTimesWithEmployee = withEmployee;
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
                if (taskStepsWithEmployee)
                {
                    query = query.Include(x => x.Steps.Select(y => y.Employee));
                }
                else
                {
                    query = query.Include(x => x.Steps);                    
                }
            }

            if (spentTimes)
            {
                if (spentTimesWithEmployee)
                {
                    query = query.Include(x => x.Steps.Select(y => y.SpentTimes.Select(z => z.Employee)));
                }
                else
                {
                    query = query.Include(x => x.Steps.Select(y => y.SpentTimes));   
                }
            }

            return query;
        }
    }
}
