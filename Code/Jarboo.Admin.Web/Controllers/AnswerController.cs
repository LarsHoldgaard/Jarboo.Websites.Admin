﻿using System.Net;
using System.Web.Mvc;
using Jarboo.Admin.BL;
using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Services.Interfaces;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Web.Models.Answer;
using Jarboo.Admin.Web.Models.Question;
using Links;
using Ninject;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class AnswerController : BaseController
    {
        [Inject]
        public IAnswerService AnswerService { get; set; }
        [Inject]
        public IQuestionService QuestionService { get; set; }

        public virtual ActionResult Index(int questionId)
        {

            var question = QuestionService.GetByIdEx(questionId, new QuestionInclude().Tasks());
            var model = question.MapTo<QuestionViewModel>();

            return View(model);
        }

        public virtual ActionResult AnswerList(int questionId)
        {
            var answerFilter = new AnswerFilter().ByQuestion(questionId);
            var answers = AnswerService.GetAll(Query.ForAnswer(answerFilter).Include(x => x.Employee()));
            var model = new AnswerListViewModel { Answers = answers };
            return PartialView("_ListAnswer", model);
        }

        public virtual ActionResult Create(int questionId, int taskId)
        {
            var answerCreate = new AnswerViewModel { QuestionId = questionId, TaskId = taskId, EmployeeId = UserEmployeeId ?? 1 };

            var questionUpdate = QuestionService.GetById(questionId);
            questionUpdate.IsRead = true;
            QuestionService.Edit(questionUpdate);
            return PartialView("_AnswerQuestionForm", answerCreate);
        }

        // POST: /Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(AnswerViewModel model)
        {
            return Handle(
                model, AnswerService.Save, RedirectToAction(MVC.Tasks.View(model.TaskId)),
                 RedirectToAction(MVC.Answer.Create()));
         
        }
    }
}