using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Jarboo.Admin.BL;
using Jarboo.Admin.BL.Authorization;
using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.BL.Services.Interfaces;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Web.Infrastructure;
using Jarboo.Admin.Web.Models.Documentation;
using Jarboo.Admin.Web.Models.Question;
using Ninject;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class QuestionsController : BaseController
    {
        [Inject]
        public IQuestionService QuestionService { get; set; }

        // GET: Questions
        public virtual ActionResult QuestionList(QuestionFilter questionFilter = null, string taskName = null)
        {
            var questionList = new QuestionListViewModel
            {
                TaskId = questionFilter.TaskId.Value,
                TaskName = taskName
            };
           
            var questions = QuestionService.GetAll(Query.ForQuestion(questionFilter).Include(x=>x.Answers())).Decorate<Question, QuestionViewModel>();
            questionList.AddRange(questions);
            //foreach (var que in questions)
            //{
            //    questionList.Add(new QuestionViewModel(que));
            //}
            return PartialView("_QuestionList", questionList);
        }

        // GET: /Employees/Create
        public virtual ActionResult Create(int? taskId, string taskName)
        {
            if (taskId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var questionCreate = new QuestionViewModel { TaskId = taskId.Value, Task = new Task { Title = taskName } };

            return View("AskQuestion", questionCreate);
        }

        // POST: /Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(QuestionViewModel model)
        {
            return Handle(
                model, QuestionService.Save,
                () => RedirectToAction(MVC.Tasks.View(model.TaskId)),
                RedirectToAction(MVC.Questions.Create()));
        }

    }
}