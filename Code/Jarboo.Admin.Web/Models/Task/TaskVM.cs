using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jarboo.Admin.Web.Models.Task
{
    public class TaskVM : DAL.Entities.Task
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
            return string.Format("{0} [{1}]", Title, DateModified.ToString());
        }
    }
}