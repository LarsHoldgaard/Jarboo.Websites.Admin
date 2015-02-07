using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Filters
{
    public class SpentTimeFilter : Filter<SpentTime>
    {
        public int? TaskId { get; set; }
        public TaskStepEnum? TaskStep { get; set; }
        public int? ProjectId { get; set; }
        public bool OnlyTopLevelProjectsHours { get; set; }
        public bool? Accepted { get; set; }

        public bool ShowDeleted { get; set; }

        public SpentTimeFilter ByTask(int? taskId)
        {
            this.TaskId = taskId;
            return this;
        }
        public SpentTimeFilter ByTaskStep(TaskStepEnum? taskStep)
        {
            this.TaskStep = taskStep;
            return this;
        }
        public SpentTimeFilter ByProject(int? projectId, bool topLevelOnly = false)
        {
            this.ProjectId = projectId;
            this.OnlyTopLevelProjectsHours = topLevelOnly;
            return this;
        }
        public SpentTimeFilter ByAccepted(bool? accepted)
        {
            this.Accepted = accepted;
            return this;
        }

        public SpentTimeFilter WithDeleted()
        {
            this.ShowDeleted = true;
            return this;
        }

        public override IQueryable<SpentTime> Execute(IQueryable<SpentTime> query)
        {
            if (TaskId.HasValue)
            {
                query = query.Where(x => x.TaskId == TaskId.Value);
            }
            if (TaskStep.HasValue)
            {
                query = query.Where(x => x.TaskStep.Step == TaskStep.Value);
            }

            if (ProjectId.HasValue)
            {
                if (OnlyTopLevelProjectsHours)
                {
                    query = query.Where(x => x.ProjectId == ProjectId.Value);
                }
                else
                {
                    query = query.Where(x => x.ProjectId == ProjectId.Value || x.TaskStep.Task.ProjectId == ProjectId.Value);
                }
            }

            if (Accepted.HasValue)
            {
                query = query.Where(x => x.Accepted == Accepted.Value);
            }

            if (!ShowDeleted)
            {
                query = query.Where(x => !x.DateDeleted.HasValue);
            }

            return base.Execute(query);
        }
    }
}
