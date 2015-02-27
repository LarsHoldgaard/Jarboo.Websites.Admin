using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Includes
{
    public class EmployeeInclude : Include<Employee>
    {
        private bool positions;
        private bool taskSteps;
        private bool tasks;
        private bool user;

        public EmployeeInclude Positions()
        {
            positions = true;
            return this;
        }
        public EmployeeInclude TaskSteps()
        {
            taskSteps = true;
            return this;
        }
        public EmployeeInclude Tasks()
        {
            tasks = true;
            return this;
        }
        public EmployeeInclude User()
        {
            user = true;
            return this;
        }
        public override IQueryable<Employee> Execute(IQueryable<Employee> query)
        {
            if (positions)
            {
                query = query.Include(x => x.Positions);
            }

            if (taskSteps)
            {
                query = query.Include(x => x.TaskSteps);
            }

            if (tasks)
            {
                query = query.Include(x => x.TaskSteps.Select(y => y.Task));
            }

            if (user)
            {
                query = query.Include(x => x.User);
            }

            return query;
        }
    }
}
