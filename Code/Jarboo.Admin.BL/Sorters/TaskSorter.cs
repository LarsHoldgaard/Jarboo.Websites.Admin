using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Sorters
{
    public class TaskSorter : Sorter<Task>
    {
        public TaskSorter()
        {
            Title = SortDirection.Ascendant;
        }

        public SortDirection? Title { get; set; }
        public SortDirection? DateModified { get; set; }
        public SortDirection? Type { get; set; }
        public SortDirection? Size { get; set; }
        public SortDirection? Urgency { get; set; }

        public TaskSorter ByTitle(SortDirection? title = SortDirection.Ascendant)
        {
            Title = title;
            return this;
        }
        public TaskSorter ByDateModified(SortDirection? dateModified = SortDirection.Ascendant)
        {
            DateModified = dateModified;
            return this;
        }
        public TaskSorter ByType(SortDirection? type = SortDirection.Ascendant)
        {
            Type = type;
            return this;
        }
        public TaskSorter BySize(SortDirection? size = SortDirection.Ascendant)
        {
            Size = size;
            return this;
        }
        public TaskSorter ByUrgency(SortDirection? urgency = SortDirection.Ascendant)
        {
            Urgency = urgency;
            return this;
        }

        public override IQueryable<Task> Sort(IQueryable<Task> query)
        {
            query = base.Sort(query);
            
            if (Title.HasValue)
            {
                query = query.SortBy(Title.Value, x => x.Title);
            }

            if (DateModified.HasValue)
            {
                query = query.SortBy(DateModified.Value, x => x.DateModified);
            }

            if (Type.HasValue)
            {
                query = query.SortBy(Type.Value, x => x.Type);
            }

            if (Size.HasValue)
            {
                query = query.SortBy(Size.Value, x => x.Size);
            }

            if (Urgency.HasValue)
            {
                query = query.SortBy(Urgency.Value, x => x.Urgency);
            }

            return query;
        }
    }
}
