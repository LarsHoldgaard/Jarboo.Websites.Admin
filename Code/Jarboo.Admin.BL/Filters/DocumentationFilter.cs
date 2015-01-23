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

        public DocumentationFilter ByProjectId(int? projectId)
        {
            this.ProjectId = projectId;
            return this;
        }

        public override IQueryable<Documentation> Execute(IQueryable<Documentation> query)
        {
            if (ProjectId.HasValue)
            {
                query = query.Where(x => x.ProjectId == ProjectId.Value);
            }

            return base.Execute(query);
        }
    }
}
