using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jarboo.Admin.Web.Models.Quiz
{
    public class QuizzesListViewModel
    {
        public IEnumerable<DAL.Entities.Quiz> Quizzes { get; set; }
        public bool ShowProject { get; set; }
    }
}