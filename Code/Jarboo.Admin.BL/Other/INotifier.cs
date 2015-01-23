using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Other
{
    public interface INotifier
    {
        void TaskResponsibleChanged(Task task, Employee employee);
    }
}
