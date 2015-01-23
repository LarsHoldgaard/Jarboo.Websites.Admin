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
    public class MandrillNotifier : INotifier
    {
        public MandrillNotifier(IMandrillConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        private IMandrillConfiguration Configuration { get; set; }
        private MandrillApi api = null;

        private void EnsureService()
        {
            api = new MandrillApi(Configuration.MandrillApiKey);
        }

        public void TaskResponsibleChanged(Task task, Employee employee)
        {
            this.EnsureService();

            api.SendMessage(
                new EmailAddress[] { new EmailAddress(employee.Email) },
                "Task was assigned to you",
                new EmailAddress(Configuration.MandrillFrom),
                Configuration.MandrillTaskResponsibleNotificationTemplate,
                Enumerable.Empty<TemplateContent>()
                );
        }
    }
}
