using Jarboo.Admin.BL.Other;
using Jarboo.Admin.BL.Services.Interfaces;
using Jarboo.Admin.DAL.Entities;
using Mandrill;

namespace Jarboo.Admin.Integration.Mandrill
{
    public class MandrillNotifierEmailer : INotifier, IEmailer
    {
        private readonly Setting setting;
        private readonly IUrlConstructor urlConstructor;
        private readonly MandrillApi api;

        public MandrillNotifierEmailer(IUrlConstructor urlConstructor, ISettingService settingService)
        {
            setting = settingService.GetCurrentSetting();
            this.urlConstructor = urlConstructor;

            if (setting.UseMandrill)
            {
                api = new MandrillApi(setting.MandrillApiKey);
            }
        }

        public void TaskResponsibleChanged(TaskResponsibleChangedData data)
        {
            if (!setting.UseMandrill)
            {
                return;
            }

            var message = new EmailMessage
            {
                to = new[] { new EmailAddress(data.EmployeeEmail) },
                from_email = setting.MandrillFrom,
                subject = setting.MandrillTaskResponsibleChangedNotificationSubject
            };

            message.AddGlobalVariable("TASKNAME", data.TaskTitle);
            message.AddGlobalVariable("TASKROLE", data.Role);

            api.SendMessage(message, setting.MandrillTaskResponsibleNotificationTemplate, null);
        }

        public void SendPasswordRecoveryEmail(string email, string link)
        {
            if (!setting.UseMandrill)
            {
                return;
            }

            var message = new EmailMessage
            {
                to = new[] { new EmailAddress(email) },
                from_email = setting.MandrillFrom,
                subject = "Jarbo password recovery"
            };

            message.AddGlobalVariable("QUESTIONLINK", link);

            api.SendMessage(message, setting.MandrillPasswordRecoveryTemplate, null);
        }

        public void NewTask(NewTaskData data)
        {
            if (!setting.UseMandrill)
            {
                return;
            }

            var message = new EmailMessage
            {
                to = new[] { new EmailAddress(setting.JarbooInfoEmail) },
                from_email = setting.MandrillFrom,
                subject = "New task"
            };

            message.AddGlobalVariable("CUSTOMERNAME", data.CustomerName);
            message.AddGlobalVariable("TASKNAME", data.TaskTitle);
            message.AddGlobalVariable("PROJECTNAME", data.ProjectName);
            message.AddGlobalVariable("TASKDESCRIPTION", data.TaskDescription);
            message.AddGlobalVariable("TASKLINK", urlConstructor.TaskView(data.TaskId));

            api.SendMessage(message, setting.MandrillNewTaskTemplate, null);
        }

        public void NewEmployee(NewEmployeeData data)
        {
            var message = new EmailMessage
            {
                to = new EmailAddress[] { new EmailAddress(data.Email) },
                from_email = setting.MandrillFrom,
                subject = "Welcome to Jarboo",
            };

            message.AddGlobalVariable("FULLNAME", data.Email);

            api.SendMessage(message, setting.MandrillNewEmployeeTemplate, null);
        }
    }
}
