using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL.Entities;

using Mandrill;

using Task = Jarboo.Admin.DAL.Entities.Task;

namespace Jarboo.Admin.Integration.Mandrill
{
    public class MandrillNotifierEmailer : INotifier, IEmailer
    {
        public MandrillNotifierEmailer(IMandrillConfiguration configuration, IUrlConstructor urlConstructor)
        {
            this.Configuration = configuration;
            UrlConstructor = urlConstructor;
            EnsureService();
        }

        private IMandrillConfiguration Configuration { get; set; }
        private IUrlConstructor UrlConstructor { get; set; }
        private MandrillApi api = null;

        private void EnsureService()
        {
            api = new MandrillApi(Configuration.MandrillApiKey);
        }

        public void TaskResponsibleChanged(TaskResponsibleChangedData data)
        {
            var message = new EmailMessage
            {
                to = new EmailAddress[] { new EmailAddress(data.EmployeeEmail) },
                from_email = Configuration.MandrillFrom,
                subject = Configuration.TaskResponsibleChangedNotificationSubject,
            };

            message.AddGlobalVariable("TASKNAME", data.TaskTitle);
            message.AddGlobalVariable("TASKROLE", data.Role);

            api.SendMessage(message, Configuration.MandrillTaskResponsibleNotificationTemplate, null);
        }

        public void SendPasswordRecoveryEmail(string email, string link)
        {
            var message = new EmailMessage
            {
                to = new EmailAddress[] { new EmailAddress(email) },
                from_email = Configuration.MandrillFrom,
                subject = "Jarbo password recovery",
            };

            message.AddGlobalVariable("QUESTIONLINK", link);

            api.SendMessage(message, Configuration.MandrillPasswordRecoveryTemplate, null);
        }

        public void NewTask(NewTaskData data)
        {
            var message = new EmailMessage
            {
                to = new EmailAddress[] { new EmailAddress(Configuration.JarbooInfoEmail) },
                from_email = Configuration.MandrillFrom,
                subject = "New task",
            };

            message.AddGlobalVariable("CUSTOMERNAME", data.CustomerName);
            message.AddGlobalVariable("TASKNAME", data.TaskTitle);
            message.AddGlobalVariable("PROJECTNAME", data.ProjectName);
            message.AddGlobalVariable("TASKDESCRIPTION", data.TaskDescription);
            message.AddGlobalVariable("TASKLINK", UrlConstructor.TaskView(data.TaskId));

            api.SendMessage(message, Configuration.MandrillNewTaskTemplate, null);
        }

        public void NewEmployee(NewEmployeeData data)
        {
            var message = new EmailMessage
            {
                to = new EmailAddress[] { new EmailAddress(data.Email) },
                from_email = Configuration.MandrillFrom,
                subject = "Welcome to Jarboo",
            };

            message.AddGlobalVariable("FULLNAME", data.Email);

            api.SendMessage(message, Configuration.MandrillNewEmployeeTemplate, null);
        }
    }
}
