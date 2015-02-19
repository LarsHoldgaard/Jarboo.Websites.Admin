﻿using System.Net;
using System.Web.Mvc;
using Jarboo.Admin.BL;
using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Services.Interfaces;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Web.Infrastructure;
using Jarboo.Admin.Web.Models.Question;
using Ninject;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class QuestionsController : BaseController
    {
        [Inject]
        public IQuestionService QuestionService { get; set; }

        // GET: Questions
        [ChildActionOnly]
        public virtual ActionResult QuestionList(QuestionFilter questionFilter = null, string taskName = null)
        {
            var questionList = new QuestionListViewModel
            {
                TaskId = questionFilter.TaskId.Value,
                TaskName = taskName
            };

            var questions = QuestionService.GetAll(Query.ForQuestion(questionFilter).Include(x => x.Answers())).Decorate<Question, QuestionViewModel>();
            questionList.AddRange(questions);

            return PartialView(MVC.Questions.Views._QuestionList, questionList);
        }

        // GET: /Questions/Create
        public virtual ActionResult Create(int? taskId, string taskName)
        {
            if (taskId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var questionCreate = new QuestionViewModel { TaskId = taskId.Value, Task = new Task { Title = taskName } };

            return View(MVC.Questions.Views.AskQuestion, questionCreate);
        }

        // POST: /Questions/Create
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