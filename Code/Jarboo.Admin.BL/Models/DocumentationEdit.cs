using Jarboo.Admin.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Models
{
    public class DocumentationEdit
    {
        public int DocumentationId { get; set; }
        [Required]
        public string DocumentLink { get; set; }
        public DocumentationType Type { get; set; }

        public int ProjectId { get; set; }
    }
}
