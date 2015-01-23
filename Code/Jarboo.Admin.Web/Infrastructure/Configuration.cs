using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

using Jarboo.Admin.Integration.GoogleDrive;
using Jarboo.Admin.Integration.Mandrill;
using Jarboo.Admin.Integration.Trello;

namespace Jarboo.Admin.Web.Infrastructure
{
    public class Configuration : IGoogleDriveConfiguration, ITrelloConfiguration, IMandrillConfiguration
    {
        private Configuration()
        { }

        private static Lazy<Configuration> configuration = new Lazy<Configuration>(() => new Configuration());
        public static Configuration Instance
        {
            get
            {
                return configuration.Value;
            }
        }

        public bool IsDebug()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        private string[] predefinedCustomers;
        public string[] PredefinedCustomers
        {
            get
            {
                if (predefinedCustomers == null)
                {
                    predefinedCustomers = ConfigurationManager.AppSettings["PredefinedCustomers"].Split(';');
                }
                return predefinedCustomers;
            }
        }


        private bool? useTrello;
        public bool UseTrello
        {
            get
            {
                if (useTrello == null)
                {
                    useTrello = bool.Parse(ConfigurationManager.AppSettings["UseTrello"]);
                }
                return useTrello.Value;
            }
        }

        private string trelloApiKey;
        public string TrelloApiKey
        {
            get
            {
                if (trelloApiKey == null)
                {
                    trelloApiKey = ConfigurationManager.AppSettings["TrelloApiKey"];
                }
                return trelloApiKey;
            }
        }

        private string trelloToken;
        public string TrelloToken
        {
            get
            {
                if (trelloToken == null)
                {
                    trelloToken = ConfigurationManager.AppSettings["TrelloToken"];
                }
                return trelloToken;
            }
        }


        private bool? useGoogleDrive;
        public bool UseGoogleDrive
        {
            get
            {
                if (useGoogleDrive == null)
                {
                    useGoogleDrive = bool.Parse(ConfigurationManager.AppSettings["UseGoogleDrive"]);
                }
                return useGoogleDrive.Value;
            }
        }

        private string googleDriveTemplatePath;
        public string GoogleDriveTemplatePath
        {
            get
            {
                if (googleDriveTemplatePath == null)
                {
                    googleDriveTemplatePath = ConfigurationManager.AppSettings["GoogleDriveTemplatePath"];
                }
                return googleDriveTemplatePath;
            }
        }

        private string googleDrivePath;
        public string GoogleDrivePath
        {
            get
            {
                if (googleDrivePath == null)
                {
                    googleDrivePath = ConfigurationManager.AppSettings["GoogleDrivePath"];
                }
                return googleDrivePath;
            }
        }

        private string googleClientId;
        public string GoogleClientId
        {
            get
            {
                if (googleClientId == null)
                {
                    googleClientId = ConfigurationManager.AppSettings["GoogleClientId"];
                }
                return googleClientId;
            }
        }

        private string googleClientSecret;
        public string GoogleClientSecret
        {
            get
            {
                if (googleClientSecret == null)
                {
                    googleClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"];
                }
                return googleClientSecret;
            }
        }

        private string googleRefreshToken;
        public string GoogleRefreshToken
        {
            get
            {
                if (googleRefreshToken == null)
                {
                    googleRefreshToken = ConfigurationManager.AppSettings["GoogleRefreshToken"];
                }
                return googleRefreshToken;
            }
        }

        private string googleLocalUserId;
        public string GoogleLocalUserId
        {
            get
            {
                if (googleLocalUserId == null)
                {
                    googleLocalUserId = ConfigurationManager.AppSettings["GoogleLocalUserId"];
                }
                return googleLocalUserId;
            }
        }


        private bool? useNotifier;
        public bool UseNotifier
        {
            get
            {
                if (useNotifier == null)
                {
                    useNotifier = bool.Parse(ConfigurationManager.AppSettings["UseNotifier"]);
                }
                return useNotifier.Value;
            }
        }

        private string mandrillApiKey;
        public string MandrillApiKey
        {
            get
            {
                if (mandrillApiKey == null)
                {
                    mandrillApiKey = ConfigurationManager.AppSettings["MandrillApiKey"];
                }
                return mandrillApiKey;
            }
        }

        private string mandrillTaskResponsibleNotificationTemplate;
        public string MandrillTaskResponsibleNotificationTemplate
        {
            get
            {
                if (mandrillTaskResponsibleNotificationTemplate == null)
                {
                    mandrillTaskResponsibleNotificationTemplate = ConfigurationManager.AppSettings["MandrillTaskResponsibleNotificationTemplate"];
                }
                return mandrillTaskResponsibleNotificationTemplate;
            }
        }

        private string mandrillFrom;
        public string MandrillFrom
        {
            get
            {
                if (mandrillFrom == null)
                {
                    mandrillFrom = ConfigurationManager.AppSettings["MandrillFrom"];
                }
                return mandrillFrom;
            }
        }


        private CustomErrorsMode? redirectOnError;
        public CustomErrorsMode ErrorMode
        {
            get
            {
                if (redirectOnError == null)
                {
                    var configuration = WebConfigurationManager.OpenWebConfiguration("~");
                    var section = (CustomErrorsSection)configuration.GetSection("system.web/customErrors");
                    redirectOnError = section.Mode;
                }
                return redirectOnError.Value;
            }
        }
    }
}