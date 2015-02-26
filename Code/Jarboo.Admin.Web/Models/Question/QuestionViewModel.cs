 
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.Web.Models.Question
{
    public class QuestionViewModel : DAL.Entities.Question
    {
        public QuestionViewModel()
        {
           
        }

        public QuestionViewModel(QuestionViewModel mapTo)
        {
            //if (mapTo.Answers.Count != 0)
            //{
            //    mapTo.Status = Status.Answered;
            //}
        }
    }
}