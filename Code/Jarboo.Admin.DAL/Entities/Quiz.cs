using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.DAL.Entities
{
    public class Quiz : BaseEntity
    {
        public int QuizId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Link { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
