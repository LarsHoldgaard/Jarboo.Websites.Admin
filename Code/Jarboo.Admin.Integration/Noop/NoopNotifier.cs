using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.Integration.Noop
{
    public class NoopNotifier : INotifier
    {
        public void TaskResponsibleChanged(TaskResponsibleChangedData data)
        { }
        public void NewTask(NewTaskData data)
        { }
    }
}
