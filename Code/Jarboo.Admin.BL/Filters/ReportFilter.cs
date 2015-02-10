using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Filters
{
    public class ReportFilter : Filter<Task>, IValidatableObject
    {
        public int? CustomerId { get; set; }
        public int? ProjectId { get; set; }
        
        public DateTime? DateModifiedFrom { get; set; }
        public DateTime? DateModifiedTo { get; set; }

        public ReportFilter ByCustomerId(int? customerId)
        {
            this.CustomerId = customerId;
            return this;
        }

        public ReportFilter ByProjectId(int? projectId)
        {
            this.ProjectId = projectId;
            return this;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateModifiedFrom.HasValue && DateModifiedTo.HasValue && DateModifiedTo.Value < DateModifiedFrom.Value)
            {
                yield return new ValidationResult("DateModifiedFrom goes after DateModifiedTo", new[] { "DateModifiedTo", "DateModifiedFrom" });
            }
        }

        public override IQueryable<Task> Execute(IQueryable<Task> query)
        {
            if (DateModifiedFrom.HasValue)
            {
                var date = DateModifiedFrom.Value.StartOfDay();
                query = query.Where(x => x.DateModified >= date);
            }

            if (DateModifiedTo.HasValue)
            {
                var date = DateModifiedTo.Value.EndOfDay();
                query = query.Where(x => x.DateModified <= date);
            }

            if (CustomerId.HasValue)
            {
                query = query.Where(x => x.Project.CustomerId == CustomerId.Value);
            }

            if (ProjectId.HasValue)
            {
                query = query.Where(x => x.ProjectId == ProjectId.Value);
            }

            return base.Execute(query);
        }
    }
}
