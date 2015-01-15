using Jarboo.Admin.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Jarboo.Admin.BL.Includes
{
    public class DocumentationInclude : Include<Documentation>
    {
        private bool project;
        private bool customer;

        public DocumentationInclude Customer()
        {
            customer = true;
            return this;
        }
        public DocumentationInclude Project()
        {
            project = true;
            return this;
        }

        public override IQueryable<Documentation> Execute(IQueryable<Documentation> query)
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
