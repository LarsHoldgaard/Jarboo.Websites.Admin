using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using Jarboo.Admin.BL.Models;

namespace Jarboo.Admin.Web.Models.Account
{
    public class LoginViewModel : UserLogin
    {
        [Required]
        public bool Persist { get; set; }
    }
}