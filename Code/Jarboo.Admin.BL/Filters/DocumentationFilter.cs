using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Filters
{
    public class DocumentationFilter : Filter<Documentation>
    {
        public int? ProjectId { get; set; }
        public int? CustomerId { get; set; }

        public DocumentationFilter ByProjectId(int? projectId)
        {
            this.ProjectId = projectId;
            return this;
        }
        public DocumentationFilter ByCustomerId(int? customerId)
        {
            this.CustomerId = customerId;
            return this;
        }

        public override IQueryable<Documentation> Execute(IQueryable<Documentation> query)
        {
            if (CustomerId.HasValue)
            {
                query = query.Where(x => x.Project.CustomerId == CustomerId.Value);
            }

            if (ProjectId.HasValue)
            {
                query = query.Where(x => x.ProjectId == ProjectId.Value);
            }

            return base.Execute(query);
        }
    }
}
