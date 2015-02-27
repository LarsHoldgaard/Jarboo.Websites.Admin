using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Models
{
    public class ProjectEdit
    {
        public int ProjectId { get; set; }
        [Required]
        public string Name { get; set; }
        public int CustomerId { get; set; }
    }
}
