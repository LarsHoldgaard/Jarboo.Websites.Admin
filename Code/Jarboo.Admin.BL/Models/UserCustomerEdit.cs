using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Models
{
    public class UserCustomerEdit : UserEdit
    {
        [Required]
        public string Country { get; set; }
        [Required]
        public string Creator { get; set; }
    }
}
