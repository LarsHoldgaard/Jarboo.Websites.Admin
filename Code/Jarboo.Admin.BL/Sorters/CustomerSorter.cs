using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Sorters
{
    public class CustomerSorter : Sorter<Customer>
    {
        public CustomerSorter()
        {
            Name = SortDirection.Ascendant;
        }

        public SortDirection? Name { get; set; }

        public CustomerSorter ByName(SortDirection? name = SortDirection.Ascendant)
        {
            Name = name;
            return this;
        }

        public override IQueryable<Customer> Sort(IQueryable<Customer> query)
        {
            query = base.Sort(query);

            if (Name.HasValue)
            {
                query = query.SortBy(Name.Value, x => x.Name);
            }

            return query;
        }
    }
}
