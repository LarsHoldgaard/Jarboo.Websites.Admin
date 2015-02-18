using System.Data.Entity;
using System.Linq;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Includes
{
    public class AnswerInclude : Include<Answer>
    {
        private bool question;
        private bool employee;

        public AnswerInclude Question()
        {
            question = true;
            return this;
        }

        public AnswerInclude Employee()
        {
            employee = true;
            return this;
        }

        public override IQueryable<Answer> Execute(IQueryable<Answer> query)
        {
            if (question)
            {
                query = query.Include(x => x.Question);
            }

            if (employee)
            {
                query = query.Include(x => x.Employee);
            }

            return query;
        }
    }
}
