using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Includes
{
    public class QuizInclude : Include<Quiz>
    {
        private bool project;
        private bool customer;

        public QuizInclude Customer()
        {
            customer = true;
            return this;
        }
        public QuizInclude Project()
        {
            project = true;
            return this;
        }

        public override IQueryable<Quiz> Execute(IQueryable<Quiz> query)
        {
            if (project)
            {
                query = query.Include(x => x.Project);
            }

            if (customer)
            {
                query = query.Include(x => x.Project.Customer);
            }

            return query;
        }
    }
}
