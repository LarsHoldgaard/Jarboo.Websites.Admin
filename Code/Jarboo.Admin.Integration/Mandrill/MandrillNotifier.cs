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
        public MandrillNotifierEmailer(IMandrillConfiguration configuration)
        {
            this.Configuration = configuration;
            EnsureService();
        }

        private IMandrillConfiguration Configuration { get; set; }
        private MandrillApi api = null;

        private void EnsureService()
        {
            api = new MandrillApi(Configuration.MandrillApiKey);
        }

        public void TaskResponsibleChanged(TaskResponsibleChangedData data)
        {
            var result = api.SendMessage(
                new EmailAddress[] { new EmailAddress(data.EmployeeEmail) },
                Configuration.TaskResponsibleChangedNotificationSubject,
                new EmailAddress(Configuration.MandrillFrom),
                Configuration.MandrillTaskResponsibleNotificationTemplate,
                Enumerable.Empty<TemplateContent>()
                );
        }

        public void SendPasswordRecoveryEmail(string email, string link)
        {
            var result = api.SendMessage(
                new EmailAddress[] { new EmailAddress(email) },
                "Jarbo password recovery",
                "You can reset your password <a href='" +link + "'>here</a>",
                new EmailAddress(Configuration.MandrillFrom)
                );
        }
    }
}
