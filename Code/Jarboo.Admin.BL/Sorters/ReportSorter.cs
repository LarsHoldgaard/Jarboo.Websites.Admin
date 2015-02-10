using System.Linq;
using Task = Jarboo.Admin.DAL.Entities.Task;

namespace Jarboo.Admin.BL.Sorters
{
    public class ReportSorter : Sorter<DAL.Entities.Task>
    {
        public ReportSorter()
        {
            Title = SortDirection.Ascendant;
        }

        public SortDirection? Title { get; set; }
        public SortDirection? Step { get; set; }
        public SortDirection? Project { get; set; } 

        public ReportSorter ByTitle(SortDirection? title = SortDirection.Ascendant)
        {
            Title = title;
            return this;
        }

        public ReportSorter ByType(SortDirection? step = SortDirection.Ascendant)
        {
            Step = step;
            return this;
        }

        public ReportSorter ByProject(SortDirection? project = SortDirection.Ascendant)
        {
            Project = project;
            return this;
        }
        public override IQueryable<DAL.Entities.Task> Sort(IQueryable<Task> query)
        {
            query = base.Sort(query);

            if (Title.HasValue)
            {
                query = query.SortBy(Title.Value, x => x.Title);
            }

            if (Step.HasValue)
            {
                query = query.SortBy(Step.Value, x => x.Type);
            }

            if (Project.HasValue)
            {
                query = query.SortBy(Project.Value, x => x.Type);
            }

            return query;
        }
    }
}
