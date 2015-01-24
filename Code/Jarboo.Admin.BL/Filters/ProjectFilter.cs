using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Filters
{
    public class ProjectFilter : Filter<Project>
    {
        public int? CustomerId { get; set; }

        public ProjectFilter ByCustomerId(int? customerId)
        {
            this.CustomerId = customerId;
            return this;
        }

        public override IQueryable<Project> Execute(IQueryable<Project> query)
        {
            if (CustomerId.HasValue)
            {
                query = query.Where(x => x.CustomerId == CustomerId.Value);
            }

            return base.Execute(query);
        }
    }
}
