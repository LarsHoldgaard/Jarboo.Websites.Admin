using System.Collections.Generic;

namespace Jarboo.Admin.Web.Models.Question
{
    public class QuestionListViewModel : List<QuestionViewModel>
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
     //   public IEnumerable<DAL.Entities.Documentation> Documentations { get; set; }
    }
}