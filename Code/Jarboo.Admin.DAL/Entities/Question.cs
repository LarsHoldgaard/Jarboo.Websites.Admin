using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Jarboo.Admin.DAL.Entities
{
    public enum Status
    {
        Unanswered,
        Answered
    }
    public class Question : BaseEntity
    {
        public Question()
        {
            Answers = new List<Answer>();
        }

        [Key]
        public int QuestionId { get; set; }
        [DisplayName("Headline of question")]
        public string Headline { get; set; }

        [DisplayName("Describe your question")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public Status Status { get; set; }
        public DateTime? LastUpdate { get; set; }

        public int TaskId { get; set; }
        public virtual Task Task { get; set; }

        public List<Answer> Answers { get; set; }
    }
}
