using Jarboo.Admin.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Jarboo.Admin.BL.Includes
{
    public class UserInclude : Include<User>
    {
        private bool employee;
        private bool customer;
        private bool roles;

        public UserInclude Customer()
        {
            customer = true;
            return this;
        }
        public UserInclude Employee()
        {
            employee = true;
            return this;
        }
        public UserInclude Roles()
        {
            roles = true;
            return this;
        }

        public override IQueryable<User> Execute(IQueryable<User> query)
        {
            if (employee)
            {
                query = query.Include(x => x.Employee);
            }

            if (customer)
            {
                query = query.Include(x => x.Customer);
            }

            if (roles)
            {
                query = query.Include(x => x.Roles);
            }

            return base.Execute(query);
        }
    }
}
