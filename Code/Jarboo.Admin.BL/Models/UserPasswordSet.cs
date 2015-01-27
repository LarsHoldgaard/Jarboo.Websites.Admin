using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Models
{
    public class UserPasswordSet
    {
        public string UserId { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
