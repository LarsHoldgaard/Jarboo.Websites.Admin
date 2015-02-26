using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jarboo.Admin.DAL.Entities
{
    public enum Position
    {
        [Display(Name = "Project Leader")]
        ProjectLeader,
        Architecture,
        Developer,
        [Display(Name = "Code Reviewer")]
        CodeReviewer,
        Tester,
    }

    public class EmployeePosition
    {
        [Key]
        [Column(Order = 0)]
        public int EmployeeId { get; set; }
        [Key]
        [Column(Order = 1)]
        public Position Position { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
