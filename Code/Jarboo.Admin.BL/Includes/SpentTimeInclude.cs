using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Includes
{
    public class SpentTimeInclude : Include<SpentTime>
    {
        private bool project;
        private bool customer;
        private bool task;
        private bool taskStep;
        private bool employee;

        public SpentTimeInclude Customer()
        {
            customer = true;
            return this;
        }
        public SpentTimeInclude Project()
        {
            project = true;
            return this;
        }
        public SpentTimeInclude Task()
        {
            task = true;
            return this;
        }
        public SpentTimeInclude TaskStep()
        {
            taskStep = true;
            return this;
        }
        public SpentTimeInclude Employee()
        {
            employee = true;
            return this;
        }

        public override IQueryable<SpentTime> Execute(IQueryable<SpentTime> query)
        {
            if (project)
            {
                query = query.Include(x => x.TaskStep.Task.Project).Include(x => x.Project);
            }

            if (customer)
            {
                query = query.Include(x => x.TaskStep.Task.Project.Customer).Include(x => x.Project.Customer);
            }

            if (task)
            {
                query = query.Include(x => x.TaskStep.Task);
            }

            if (taskStep)
            {
                query = query.Include(x => x.TaskStep);
            }

            if (employee)
            {
                query = query.Include(x => x.Employee);
            }

            return base.Execute(query);
        }
    }
}
