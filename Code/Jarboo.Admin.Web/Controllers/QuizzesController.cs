using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.BL;
using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Services.Interfaces;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Web.Models.Quiz;

using Ninject;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class QuizzesController : BaseController
    {
        [Inject]
        public IQuizService QuizService { get; set; }
        [Inject]
        public IProjectService ProjectService { get; set; }

        // GET: /Quizzes/
        public virtual ActionResult Index()
        {
            return View(MVC.Quizzes.Views.Index);
        }

        // GET: /Quizzes/View/5
        public virtual ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Quiz quiz = QuizService.GetByIdEx(id.Value, new QuizInclude().Project().Customer());
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // GET: /Quizzes/Create
        public virtual ActionResult Create(int? projectId)
        {
            if (projectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var quizEdit = new QuizEdit();
            quizEdit.ProjectId = projectId.Value;

            return this.CreateEditView(quizEdit);
        }

        // GET: /Quizzes/Edit/5
        public virtual ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var quiz = QuizService.GetById(id.Value);
            if (quiz == null)
            {
                return HttpNotFound();
            }

            var quizEdit = quiz.MapTo<QuizEdit>();
            return this.CreateEditView(quizEdit);
        }

        private ActionResult CreateEditView(QuizEdit model)
        {
            ViewBag.Project = ProjectService.GetByIdEx(model.ProjectId, new ProjectInclude().Customer());
            return View(model);
        }

        // POST: /Quizzes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(QuizEdit model)
        {
            return Handle(
                model, QuizService.Save,
                () => RedirectToAction(MVC.Quizzes.View(model.QuizId)),
                () => model.QuizId == 0 ?
                    RedirectToAction(MVC.Quizzes.Create(model.ProjectId)) :
                    RedirectToAction(MVC.Quizzes.Edit(model.QuizId)));
        }

        // POST: /Quizzes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(int id, string returnUrl)
        {
            ActionResult result;
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                result = RedirectToAction(MVC.Quizzes.Index());
            }
            else
            {
                result = this.Redirect(returnUrl);
            }

            return Handle(id, QuizService.Delete, result, result, "Quiz successfully deleted");
        }

        [ChildActionOnly]
        public virtual ActionResult List(bool showProject = false, QuizFilter quizFilter = null)
        {
            quizFilter = quizFilter ?? new QuizFilter();

            var model = new QuizzesListViewModel()
            {
                ShowProject = showProject,
                Quizzes = QuizService.GetAll(Query.ForQuiz(quizFilter).Include(x => x.Project()))
            };

            return View(model);
        }
	}
}