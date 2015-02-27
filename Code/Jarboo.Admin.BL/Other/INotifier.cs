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
        void NewTask(NewTaskData data);
        void NewEmployee(NewEmployeeData data);
    }

    public struct TaskResponsibleChangedData
    {
        public TaskResponsibleChangedData(Task task, Employee employee)
        {
            TaskTitle = task.Title;
            EmployeeEmail = employee.Email;
            Role = task.Steps.Last().Step.ToString();
        }

        public string TaskTitle;
        public string Role;
        public string EmployeeEmail;
    }

    public struct NewTaskData
    {
        public NewTaskData(Customer customer, Project project, Task task)
        {
            CustomerName = customer.Name;
            ProjectName = project.Name;
            TaskTitle = task.Title;
            TaskDescription = task.Description;
            TaskId = task.TaskId;
        }

        public string CustomerName;
        public string TaskTitle;
        public string ProjectName;
        public string TaskDescription;
        public int TaskId;
    }

    public struct NewEmployeeData
    {
        public NewEmployeeData(Employee employee)
        {
            FullName = employee.FullName;
            Email = employee.Email;
        }

        public string FullName;
        public string Email;
    }
}
