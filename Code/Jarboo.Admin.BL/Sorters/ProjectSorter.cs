using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Sorters
{
    public class ProjectSorter : Sorter<Project>
    {
        public ProjectSorter()
        {
            Name = SortDirection.Ascendant;
        }

        public SortDirection? Name { get; set; }

        public ProjectSorter ByName(SortDirection? name = SortDirection.Ascendant)
        {
            Name = name;
            return this;
        }

        public override IQueryable<Project> Sort(IQueryable<Project> query)
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
