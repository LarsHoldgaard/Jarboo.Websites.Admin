using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jarboo.Admin.Web.Models
{
    public class SettingsViewModel
    {
        public bool UseGoogleDrive { get; set; }
        [Required]
        public string GoogleApiKey { get; set; }
        [Required]
        public string GoogleApiSecret { get; set; }
        [Required]
        public string GoogleRefreshToken { get; set; }
        [Required]
        public string GoogleDrivePath { get; set; }
        [Required]
        public string GoogleTemplatePath { get; set; }
    }
}