using Jarboo.Admin.BL.Other;
using Jarboo.Admin.BL.Services.Interfaces;
using Jarboo.Admin.DAL.Entities;
using Mandrill;

namespace Jarboo.Admin.Integration.Mandrill
{
    public class MandrillNotifierEmailer : INotifier, IEmailer
    {
        private readonly Setting _setting;
        private readonly IUrlConstructor _urlConstructor;
        private readonly MandrillApi _api;

        public MandrillNotifierEmailer(IUrlConstructor urlConstructor, ISettingService settingService)
        {
            _setting= settingService.GetCurrentSetting();
            _urlConstructor = urlConstructor;

            if (_setting.UseMandrill)
            {
                _api = new MandrillApi(_setting.MandrillApiKey);
            }
        }

        public void TaskResponsibleChanged(TaskResponsibleChangedData data)
        {
            if (!_setting.UseMandrill)
            {
                return;
            }

            var message = new EmailMessage
            {
                to = new[] { new EmailAddress(data.EmployeeEmail) },
                from_email = _setting.MandrillFrom,
                subject = _setting.MandrillTaskResponsibleChangedNotificationSubject
            };

            message.AddGlobalVariable("TASKNAME", data.TaskTitle);
            message.AddGlobalVariable("TASKROLE", data.Role);

            _api.SendMessage(message, _setting.MandrillTaskResponsibleNotificationTemplate, null);
        }

        public void SendPasswordRecoveryEmail(string email, string link)
        {
            if (!_setting.UseMandrill)
            {
                return;
            }

            var message = new EmailMessage
            {
                to = new[] { new EmailAddress(email) },
                from_email = _setting.MandrillFrom,
                subject = "Jarbo password recovery"
            };

            message.AddGlobalVariable("QUESTIONLINK", link);

            _api.SendMessage(message, _setting.MandrillPasswordRecoveryTemplate, null);
        }

        public void NewTask(NewTaskData data)
        {
            if (!_setting.UseMandrill)
            {
                return;
            }

            var message = new EmailMessage
            {
                to = new[] { new EmailAddress(_setting.JarbooInfoEmail) },
                from_email = _setting.MandrillFrom,
                subject = "New task"
            };

            message.AddGlobalVariable("CUSTOMERNAME", data.CustomerName);
            message.AddGlobalVariable("TASKNAME", data.TaskTitle);
            message.AddGlobalVariable("PROJECTNAME", data.ProjectName);
            message.AddGlobalVariable("TASKDESCRIPTION", data.TaskDescription);
            message.AddGlobalVariable("TASKLINK", _urlConstructor.TaskView(data.TaskId));

            _api.SendMessage(message, _setting.MandrillNewTaskTemplate, null);
        }
    }
}
