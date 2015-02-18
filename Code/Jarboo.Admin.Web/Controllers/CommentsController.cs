using System.Linq;
using System.Web.Mvc;
using Jarboo.Admin.BL;
using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Services.Interfaces;
using Jarboo.Admin.Web.Models.Comment;
using Ninject;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class CommentsController : BaseController
    {
        [Inject]
        public ICommentService CommentService { get; set; }

        public virtual ActionResult CommentList(int taskId)
        {
            var commentFilter = new CommentFilter().ByTask(taskId);
            var comments = CommentService.GetAll(Query.ForComment(commentFilter).Include(x => x.Employee()));

            var model = new CommentListViewModel { Comments = comments };
            return PartialView(MVC.Comments.Views._ListComment, model);
        }

        public virtual ActionResult Create(int taskId)
        {
            var questionCreate = new CommentViewModel { TaskId = taskId, EmployeeId = UserEmployeeId ?? 1 };

            return PartialView(MVC.Comments.Views._AddCommentForm, questionCreate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(CommentViewModel model)
        {

            return Handle(model, CommentService.Save, () => RedirectToAction(MVC.Comments.Create(model.TaskId)), new EmptyResult());
        }
    }
}