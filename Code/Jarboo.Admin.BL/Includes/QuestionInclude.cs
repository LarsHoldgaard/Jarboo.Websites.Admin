using System.Data.Entity;
using System.Linq;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Includes
{
    public class QuestionInclude : Include<Question>
    {
        private bool answwer;
        private bool task;

        public QuestionInclude Answers()
        {
            answwer = true;
            return this;
        }
        public QuestionInclude Tasks()
        {
            task = true;
            return this;
        }
        public override IQueryable<Question> Execute(IQueryable<Question> query)
        {
            if (answwer)
            {
                query = query.Include(x => x.Answers);
            }

            if (task)
            {
                query = query.Include(x => x.Task);
            }
            return query;
        }
    }
}
