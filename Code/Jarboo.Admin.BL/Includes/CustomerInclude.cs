using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Includes
{
    public class CustomerInclude : Include<Customer>
    {
        private bool projects;

        public CustomerInclude Projects()
        {
            projects = true;
            return this;
        }

        public override IQueryable<Customer> Execute(IQueryable<Customer> query)
        {
            if (projects)
            {
                query = query.Include(x => x.Projects);
            }

            return query;
        }
    }
}
