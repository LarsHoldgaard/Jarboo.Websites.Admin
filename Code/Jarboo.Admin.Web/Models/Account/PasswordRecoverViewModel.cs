using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jarboo.Admin.Web.Models.Account
{
    public class PasswordRecoverViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}