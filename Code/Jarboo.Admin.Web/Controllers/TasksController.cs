using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using DataTables.Mvc;

using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Web.Infrastructure;
using Jarboo.Admin.Web.Models;
using Jarboo.Admin.Web.Models.DataTable;
using Jarboo.Admin.Web.Models.Task;

using Newtonsoft.Json;

using Ninject;

using RestSharp;

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

            return View(task.Decorate());
        }

        // GET: /Tasks/Create
        public virtual ActionResult Create(int? projectId)
        {
            var task = new TaskCreate();
            if (projectId.HasValue)
            {
                task.ProjectId = projectId.Value;
            }

            ViewBag.EmployeesList = new SelectList(EmployeeService.GetAll(Include.ForEmployee(), Filter.ForEmployee()), "EmployeeId", "FullName");
            ViewBag.ProjectsList = new SelectList(ProjectService.GetAll(Include.ForProject().Customer(), Filter.ForProject()), "ProjectId", "Name", "Customer.Name", task.ProjectId);
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

            ViewBag.EmployeesList = new SelectList(EmployeeService.GetAll(new EmployeeInclude(), new EmployeeFilter()), "EmployeeId", "FullName");
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

        #region ListColumns

        private static List<Column<TaskVM>> columns = new List<Column<TaskVM>>()
        {
            new Column<TaskVM>()
                {
                    Title = "Title",
                    Type = DataTableConfig.Column.ColumnSpecialType.TaskLink,
                    Getter = (x) => new object[] {x.TaskId, x.Title}
                },
            new Column<TaskVM>()
                {
                    Title = "Project name",
                    Type = DataTableConfig.Column.ColumnSpecialType.ProjectLink,
                    Getter = (x) => new object[] {x.ProjectId, x.Project.Name}
                },
            new Column<TaskVM>()
                {
                    Title = "Priority",
                    Getter = (x) => x.Priority.ToString()
                },
            new Column<TaskVM>()
                {
                    Title = "Type",
                    Getter = (x) => x.Type.ToString()
                },
            new Column<TaskVM>()
                {
                    Title = "Size",
                    Getter = (x) => x.Size.ToString()
                },
            new Column<TaskVM>()
                {
                    Title = "Urgency",
                    Getter = (x) => x.Urgency.ToString()
                },
            new Column<TaskVM>()
                {
                    Title = "Folder",
                    Type = DataTableConfig.Column.ColumnSpecialType.ExternalLink,
                    Getter = (x) => x.FolderLink
                },
            new Column<TaskVM>()
                {
                    Title = "Card",
                    Type = DataTableConfig.Column.ColumnSpecialType.ExternalLink,
                    Getter = (x) => x.CardLink
                },
            new Column<TaskVM>()
                {
                    Title = "Step",
                    Type = DataTableConfig.Column.ColumnSpecialType.TaskStepLink,
                    Getter = (x) => new object[] {x.TaskId, x.Step()}
                },
            new Column<TaskVM>()
                {
                    Title = "",
                    Type = DataTableConfig.Column.ColumnSpecialType.DeleteBtn,
                    Getter = (x) => new object[] {x.TaskId, new UrlHelper(Helper.GetRequestContext()).Action(MVC.Tasks.Delete())}
                },
        }; 

        #endregion

        public virtual ActionResult ListConfig(bool showProject = false, TaskSorting sorting = TaskSorting.Title)
        {
            var config = new DataTableConfig();
            config.SetupServerDataSource(Url.Action(MVC.Tasks.ListData()), FormMethod.Post);
            config.Columns = new List<DataTableConfig.Column>(columns);
            config.Columns[1].Visible = showProject;

            switch (sorting)
            {
                    case TaskSorting.Title:
                    {
                        config.AddOrder(0, DataTables.Mvc.Column.OrderDirection.Ascendant);
                        break;
                    }
                    case TaskSorting.Pricing:
                    {
                        config.AddOrder(2, DataTables.Mvc.Column.OrderDirection.Descendant);
                        break;
                    }
            }

            var json = JsonConvert.SerializeObject(config);
            return Content(json, "application/json");
        }
        public virtual ActionResult ListData([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest request)
        {
            var taskFilter = Filter.ForTask().WithPaging(request.Length, request.Start / request.Length);
            var tasks = TaskService.GetAll(Include.ForTask().Project().TaskSteps(), taskFilter);

            var rows = columns.ExtractRows(tasks.Decorate());

            //return null;
            return Json(new DataTablesResponse(request.Draw, rows, tasks.TotalItems, tasks.TotalItems));
        }
    }
}