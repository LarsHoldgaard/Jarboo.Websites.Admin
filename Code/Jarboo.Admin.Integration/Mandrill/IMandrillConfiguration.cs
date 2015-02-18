using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.Integration.Mandrill
{
    public interface IMandrillConfiguration
    {
        string TaskResponsibleChangedNotificationSubject { get; }

        string MandrillApiKey { get; }
        string MandrillTaskResponsibleNotificationTemplate { get; }
        string MandrillFrom { get; }
        string MandrillPasswordRecoveryTemplate { get; }
    }
}
