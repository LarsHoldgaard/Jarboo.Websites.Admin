using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jarboo.Admin.Web.Models.Account
{
    public class PasswordRecoverVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}