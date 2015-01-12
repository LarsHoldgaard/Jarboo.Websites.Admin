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
        public int? EmployeeId { get; set; }

        public DateTime? DateModifiedFrom { get; set; }
        public DateTime? DateModifiedTo { get; set; }

        public TaskFilter WithProjectId(int? projectId)
        {
            this.ProjectId = projectId;
            return this;
        }
        public TaskFilter WithEmployeeId(int? employeeId)
        {
            this.EmployeeId = employeeId;
            return this;
        }

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
                var date = DateModifiedFrom.Value.StartOfDay();
                query = query.Where(x => x.DateModified >= date);
            }

            if (DateModifiedTo.HasValue)
            {
                var date = DateModifiedTo.Value.EndOfDay();
                query = query.Where(x => x.DateModified <= date);
            }

            if (ProjectId.HasValue)
            {
                query = query.Where(x => x.ProjectId == ProjectId.Value);
            }

            if (EmployeeId.HasValue)
            {
                query = query.Where(x => x.Steps.Any(y => y.EmployeeId == EmployeeId.Value));
            }

            return base.Execute(query);
        }
    }
}
