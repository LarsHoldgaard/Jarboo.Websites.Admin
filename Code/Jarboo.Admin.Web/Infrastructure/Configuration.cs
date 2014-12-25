using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Jarboo.Admin.Web.Infrastructure
{
    public static class Configuration
    {
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
    }
}