using Jarboo.Admin.BL.Other;

namespace Jarboo.Admin.Integration.Noop
{
    public class NoopNotifier : INotifier
    {
        public void TaskResponsibleChanged(TaskResponsibleChangedData data)
        { }
        public void NewTask(NewTaskData data)
        { }
        public void NewEmployee(NewEmployeeData data)
        { }
        public void EndTask(EndTaskData data)
        { }
    }
}
