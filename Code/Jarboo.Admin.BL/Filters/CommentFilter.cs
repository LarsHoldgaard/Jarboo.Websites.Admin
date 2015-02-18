using System.Linq;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Filters
{
    public class CommentFilter : Filter<Comment>
    {
        public int? TaskId { get; set; }

        public CommentFilter ByTask(int? taskId)
        {
            this.TaskId = taskId;
            return this;
        }

        public override IQueryable<Comment> Execute(IQueryable<Comment> query)
        {
            if (TaskId.HasValue)
            {
                query = query.Where(x => x.TaskId == TaskId.Value);
            }

            return base.Execute(query);
        }
    }
}
