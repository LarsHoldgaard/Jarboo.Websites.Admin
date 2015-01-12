using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Filters
{
    public class TaskFilter : Filter<Task>, IValidatableObject
    {
        public int? ProjectId { get; set; }

        public DateTime? DateModifiedFrom { get; set; }
        public DateTime? DateModifiedTo { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateModifiedFrom.HasValue && DateModifiedTo.HasValue && DateModifiedTo.Value < DateModifiedFrom.Value)
            {
                yield return new ValidationResult("DateModifiedFrom goes after DateModifiedTo", new [] { "DateModifiedTo", "DateModifiedFrom" });
            }
        }

        public override PagedData<Task> Execute(IQueryable<Task> query)
        {
            if (DateModifiedFrom.HasValue)
            {
                query = query.Where(x => x.DateModified >= DateModifiedFrom.Value);
            }

            if (DateModifiedTo.HasValue)
            {
                query = query.Where(x => x.DateModified <= DateModifiedTo.Value);
            }

            return base.Execute(query);
        }
    }
}
