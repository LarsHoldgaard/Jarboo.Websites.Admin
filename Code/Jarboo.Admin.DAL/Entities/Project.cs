using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jarboo.Admin.DAL.Entities
{
    public class Project : BaseEntity
    {
        public Project()
        {
            Documentations = new List<Documentation>();
            Tasks = new List<Task>();
            SpentTimes = new List<SpentTime>();
            Quizzes = new List<Quiz>();
        }

        public int ProjectId { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal? Commission { get; set; }
        public decimal? PriceOverride { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual List<Documentation> Documentations { get; set; }
        public virtual List<Task> Tasks { get; set; }
        public virtual List<SpentTime> SpentTimes { get; set; }
        public virtual List<Quiz> Quizzes { get; set; }
    }
}
