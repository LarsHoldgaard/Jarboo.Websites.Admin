using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

using Jarboo.Admin.Integration.GoogleDrive;
using Jarboo.Admin.Integration.Mandrill;

namespace Jarboo.Admin.Web.Infrastructure
{
    public class Configuration
    {
        private Configuration()
        { }

        private static readonly Lazy<Configuration> configuration = new Lazy<Configuration>(() => new Configuration());
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

        private string[] _predefinedCustomers;
        public string[] PredefinedCustomers
        {
            get
            {
                if (_predefinedCustomers == null)
                {
                    _predefinedCustomers = ConfigurationManager.AppSettings["PredefinedCustomers"].Split(';');
                }
                return _predefinedCustomers;
            }
        }

        private CustomErrorsMode? _redirectOnError;
        public CustomErrorsMode ErrorMode
        {
            get
            {
                if (_redirectOnError == null)
                {
                    var conf = WebConfigurationManager.OpenWebConfiguration("~");
                    var section = (CustomErrorsSection)conf.GetSection("system.web/customErrors");
                    _redirectOnError = section.Mode;
                }
                return _redirectOnError.Value;
            }
        }
       
        private string _adminEmail;
        public string AdminEmail
        {
            get
            {
                if (_adminEmail == null)
                {
                    _adminEmail = ConfigurationManager.AppSettings["AdminEmail"];
                }
                return _adminEmail;
            }
        }

        private string _adminPassword;
        public string AdminPassword
        {
            get
            {
                if (_adminPassword == null)
                {
                    _adminPassword = ConfigurationManager.AppSettings["AdminPassword"];
                }
                return _adminPassword;
            }
        }
    }
}