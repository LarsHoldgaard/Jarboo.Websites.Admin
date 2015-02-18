using System.Linq;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Filters
{
    public class AnswerFilter : Filter<Answer>
    {
        public int? QuestionId { get; set; }
        public int? TaskId { get; set; }

        public AnswerFilter ByQuestion(int? questionId)
        {
            this.QuestionId = questionId;
            return this;
        }

        public AnswerFilter ByTaskId(int? taskId)
        {
            this.TaskId = taskId;
            return this;
        }

        public override IQueryable<Answer> Execute(IQueryable<Answer> query)
        {
            if (QuestionId.HasValue)
            {
                query = query.Where(x => x.QuestionId == QuestionId.Value);
            }

            if (TaskId.HasValue)
            {
                query = query.Where(x => x.EmployeeId == TaskId.Value);
            }

            return base.Execute(query);
        }
    }
}
