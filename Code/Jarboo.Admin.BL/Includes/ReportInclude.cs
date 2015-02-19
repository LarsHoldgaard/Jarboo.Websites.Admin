using System.Data.Entity;
using System.Linq;

namespace Jarboo.Admin.BL.Includes
{
    public class ReportInclude : Include<DAL.Entities.Task>
    {
        private bool project;
        private bool customer;
        private bool taskSteps;
        private bool taskStepsWithEmployee;

        public ReportInclude Customer()
        {
            customer = true;
            return this;
        }
        public ReportInclude Project()
        {
            project = true;
            return this;
        }
        public ReportInclude TaskSteps(bool withEmployee = false)
        {
            taskSteps = true;
            taskStepsWithEmployee = withEmployee;
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

            return query;
        }
    }
}
