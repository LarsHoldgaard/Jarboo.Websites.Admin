using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Models
{
    public class PasswordRecover
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string LinkTemplate { get; set; }
    }
}
