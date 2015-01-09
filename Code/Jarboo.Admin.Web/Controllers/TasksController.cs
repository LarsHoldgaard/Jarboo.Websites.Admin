using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.DAL.Entities;

using Ninject;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class TasksController : BaseController
    {
        [Inject]
        public ITaskService TaskService { get; set; }
        [Inject]
        public IProjectService ProjectService { get; set; }
        [Inject]
        public IEmployeeService EmployeeService { get; set; }

        // GET: /Tasks/
        public virtual ActionResult Index()
        {
            return View(TaskService.GetAllEx(Include.ForTask().Project()));
        }

        // GET: /Tasks/View/5
        public virtual ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = TaskService.GetByIdEx(id.Value, Include.ForTask().Project().Customer());
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: /Tasks/Create
        public virtual ActionResult Create(int? projectId)
        {
            var task = new TaskCreate();
            if (projectId.HasValue)
            {
                task.ProjectId = projectId.Value;
            }

            ViewBag.EmployeesList = new SelectList(EmployeeService.GetAll(), "EmployeeId", "FullName");
            ViewBag.ProjectsList = new SelectList(ProjectService.GetAll(), "ProjectId", "Name", "Customer.Name", task.ProjectId);
            return View(task);
        }

        // POST: /Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(TaskCreate model)
        {
            return Handle(
                model,
                TaskService.Create,
                () => RedirectToAction(MVC.Tasks.View(model.TaskId)),
                RedirectToAction(MVC.Tasks.Create()));
        }

        // GET: /Tasks/Steps/5
        public virtual ActionResult Steps(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = TaskService.GetByIdEx(id.Value, Include.ForTask().Project().Customer().TaskSteps().Employee());
            if (task == null)
            {
                return HttpNotFound();
            }

            ViewBag.EmployeesList = new SelectList(EmployeeService.GetAll(), "EmployeeId", "FullName");
            return View(task);
        }

        // POST: /Tasks/NextStep
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult NextStep(TaskNextStep model)
        {
            return Handle(
                model,
                TaskService.NextStep,
                () => RedirectToAction(MVC.Tasks.Steps(model.TaskId)),
                RedirectToAction(MVC.Tasks.Steps(model.TaskId)));
        }
    }
}