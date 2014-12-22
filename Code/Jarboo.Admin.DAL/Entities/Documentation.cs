using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.DAL.Entities
{
    using System.ComponentModel.DataAnnotations;

    public enum DocumentationType
    {
        Architecture,
        ProgrammingStyle,
        Setup 
    }

    public class Documentation : BaseEntity
    {
        public int DocumentationId { get; set; }
        [Required]
        public string DocumentLink { get; set; }
        public DocumentationType Type { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
