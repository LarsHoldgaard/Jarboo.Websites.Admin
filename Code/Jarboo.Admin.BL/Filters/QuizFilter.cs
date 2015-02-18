using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Filters
{
    public class QuizFilter : Filter<Quiz>
    {
        public int? ProjectId { get; set; }
        public int? CustomerId { get; set; }

        public QuizFilter ByProjectId(int? projectId)
        {
            this.ProjectId = projectId;
            return this;
        }
        public QuizFilter ByCustomerId(int? customerId)
        {
            this.CustomerId = customerId;
            return this;
        }

        public override IQueryable<Quiz> Execute(IQueryable<Quiz> query)
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
