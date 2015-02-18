using System.Data.Entity;
using System.Linq;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Includes
{
    public class CommentInclude : Include<Comment>
    {
        private bool employee;

        public CommentInclude Employee()
        {
            employee = true;
            return this;
        }

        public override IQueryable<Comment> Execute(IQueryable<Comment> query)
        {
            if (employee)
            {
                query = query.Include(x => x.Employee);
            }

            return query;
        }
    }
}
