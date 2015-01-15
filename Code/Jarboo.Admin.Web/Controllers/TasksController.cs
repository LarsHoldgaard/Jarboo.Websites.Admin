using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Web.Infrastructure;
using Jarboo.Admin.Web.Models.Task;

using Ninject;

using Filter = Jarboo.Admin.BL.Filters.Filter;

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
            return View(MVC.Tasks.Views.Index);
        }

        // GET: /Tasks/View/5
        public virtual ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = TaskService.GetByIdEx(id.Value, Include.ForTask().Project().Customer().TaskSteps());
            if (task == null)
            {
                return HttpNotFound();
            }

            //return MVC.Tasks.PartialView(MVC.Tasks.View(task.Decorate()));
            return PartialView("View", task.Decorate());
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
            ViewBag.ProjectsList = new SelectList(ProjectService.GetAllEx(Include.ForProject().Customer(), Filter<Project>.None), "ProjectId", "Name", "Customer.Name", task.ProjectId);
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

        public virtual ActionResult List(bool showProject = false, int? projectId = null, int? employeeId = null, TaskFilter taskFilter = null, bool showDone = false)
        {
            taskFilter = (taskFilter ?? Filter.ForTask()).WithProjectId(projectId).WithEmployeeId(employeeId);

            if (showDone)
            {
                taskFilter.WithDone();
            }
           
            var model = new TasksListViewModel()
                            {
                                ShowProject = showProject,
                                Tasks = TaskService.GetAllEx(Include.ForTask().Project().TaskSteps(), taskFilter).Decorate(),
                                TaskFilter = taskFilter
                            };

            return View(model); 
        }

        // POST: /Tasks/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(int id, string returnUrl)
        {
            ActionResult result;
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                result = RedirectToAction(MVC.Tasks.Index());
            }
            else
            {
                result = this.Redirect(returnUrl);
            }

            return Handle(id, TaskService.Delete, result, result, "Task successfully deleted");
        }
    }
}