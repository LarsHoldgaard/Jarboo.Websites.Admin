using System.Linq;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Filters
{
    public class QuestionFilter : Filter<Question>
    {
        public int? TaskId { get; set; }

        public QuestionFilter ByTask(int? taskId)
        {
            this.TaskId = taskId;
            return this;
        }

        public override IQueryable<Question> Execute(IQueryable<Question> query)
        {
            if (TaskId.HasValue)
            {
                query = query.Where(x => x.TaskId == TaskId.Value);
            }

            return base.Execute(query);
        }
    }
}
