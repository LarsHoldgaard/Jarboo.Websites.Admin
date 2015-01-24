using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Jarboo.Admin.Web.Infrastructure
{
    public static class Configuration
    {
        public static bool IsDebug()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        private static string[] predefinedCustomers;
        public static string[] PredefinedCustomers
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

        private static bool? useTrello;
        public static bool UseTrello
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

        private static string trelloApiKey;
        public static string TrelloApiKey
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

        private static string trelloToken;
        public static string TrelloToken
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

        private static bool? useGoogleDrive;
        public static bool UseGoogleDrive
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

        private static string googleDriveTemplatePath;
        public static string GoogleDriveTemplatePath
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

        private static string googleDrivePath;
        public static string GoogleDrivePath
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

        private static string googleClientId;
        public static string GoogleClientId
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

        private static string googleClientSecret;
        public static string GoogleClientSecret
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

        private static string googleRefreshToken;
        public static string GoogleRefreshToken
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


        private static CustomErrorsMode? redirectOnError;
        public static CustomErrorsMode ErrorMode
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