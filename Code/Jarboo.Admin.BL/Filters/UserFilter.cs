using Jarboo.Admin.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Filters
{
    public class UserFilter : Filter<User>
    {
        public int? UserCustomerId { get; set; }

        public UserFilter ByUserCustomerId(int? userCustomerId)
        {
            this.UserCustomerId = userCustomerId;
            return this;
        }

        public override IQueryable<User> Execute(IQueryable<User> query)
        {
            if (UserCustomerId.HasValue)
            {
                query = query.Where(x => x.UserCustomerId == UserCustomerId.Value);
            }

            return base.Execute(query);
        }
    }
}
