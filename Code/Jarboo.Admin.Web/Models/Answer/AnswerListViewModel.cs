using System.Collections.Generic;

namespace Jarboo.Admin.Web.Models.Answer
{
    public class AnswerListViewModel
    {
        public IEnumerable<DAL.Entities.Answer> Answers { get; set; }
    }
}