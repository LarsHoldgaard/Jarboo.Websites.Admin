using System.ComponentModel.DataAnnotations;

namespace Jarboo.Admin.DAL.Entities
{
    public class Answer: BaseEntity
    {
        [Key]
        public int AnswerId { get; set; }
       
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
    
}
