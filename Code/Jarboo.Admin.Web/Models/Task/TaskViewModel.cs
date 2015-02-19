using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jarboo.Admin.Web.Models.Task
{
    public class TaskViewModel : DAL.Entities.Task
    {
        public string Step()
        {
            if (Done)
            {
                return "Done";
            }

            if (Steps.Count == 0)
            {
                return "Unknown";
            }

            return Steps.Last().Step.ToString();
        }

        public string FullTitle()
        {
            return Title;
        }

        public string Date()
        {
            return DateModified.ToString();
        }

        public string DeadlineStr()
        {
            if (Deadline.HasValue)
            {
                return Deadline.Value.ToShortDateString();
            }
            else
            {
                return "No deadline";
            }
        }

        public string EstimatedPriceStr()
        {
            if (EstimatedPrice.HasValue)
            {
                return EstimatedPrice.Value.ToString();
            }
            else
            {
                return "None";
            }
        }
    }
}