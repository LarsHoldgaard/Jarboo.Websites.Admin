using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Jarboo.Admin.BL.Models;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Other
{
    public interface INotifier
    {
        void TaskResponsibleChanged(TaskResponsibleChangedData data);
    }

    public struct TaskResponsibleChangedData
    {
        public TaskResponsibleChangedData(Task task, Employee employee)
        {
            TaskTitle = task.Title;
            EmployeeEmail = employee.Email;
        }

        public TaskResponsibleChangedData(TaskEdit taskCreate, Employee employee)
        {
            TaskTitle = taskCreate.Title;
            EmployeeEmail = employee.Email;
        }

        public string TaskTitle;
        public string EmployeeEmail;
    }
}
