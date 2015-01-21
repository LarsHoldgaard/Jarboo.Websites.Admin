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
        public string String { get; set; }

        public int? ProjectId { get; set; }
        public int? EmployeeId { get; set; }
        public bool IncludeTasksWithDoneStepsForEmployee { get; set; }

        public DateTime? DateModifiedFrom { get; set; }
        public DateTime? DateModifiedTo { get; set; }

        public bool ShowDeleted { get; set; }
        public bool ShowDone { get; set; }

        public TaskFilter ByString(string s)
        {
            String = s;
            return this;
        }

        public TaskFilter ByProjectId(int? projectId)
        {
            this.ProjectId = projectId;
            return this;
        }
        public TaskFilter ByEmployeeId(int? employeeId, bool includeTasksWithDoneSteps = false)
        {
            this.EmployeeId = employeeId;
            this.IncludeTasksWithDoneStepsForEmployee = includeTasksWithDoneSteps;
            return this;
        }

        public TaskFilter WithDone()
        {
            this.ShowDone = true;
            return this;
        }
        public TaskFilter WithDeleted()
        {
            this.ShowDeleted = true;
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
                if (IncludeTasksWithDoneStepsForEmployee)
                {
                    query = query.Where(x => x.Steps.Any(y => y.EmployeeId == EmployeeId.Value));
                }
                else
                {
                    query = query.Where(x => x.Steps.Any(y => !y.DateEnd.HasValue && y.EmployeeId == EmployeeId.Value));
                }
            }

            if (!ShowDeleted)
            {
                query = query.Where(x => !x.DateDeleted.HasValue);
            }

            if (!ShowDone)
            {
                query = query.Where(x => !x.Done);
            }

            if (!string.IsNullOrEmpty(String))
            {
                var values = String.Split(new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var value in values)
                {
                    query = query.Where(x => x.Title.IndexOf(value) != -1);
                }
            }

            return base.Execute(query);
        }
    }
}
