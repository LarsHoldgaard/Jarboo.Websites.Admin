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
    }
}