using System.ComponentModel.DataAnnotations;

namespace Jarboo.Admin.DAL.Entities
{
    public class Comment : BaseEntity
    {
        [Key]
        public int CommentId { get; set; }
      
        [Required]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        public int TaskId { get; set; }
        public virtual Task Task { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
