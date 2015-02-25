using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using DataTables.Mvc;

using Jarboo.Admin.BL;
using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Sorters;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Web.Infrastructure;
using Jarboo.Admin.Web.Models.DataTable;
using Jarboo.Admin.Web.Models.Task;

using Newtonsoft.Json;

using Ninject;
using Jarboo.Admin.BL.Services.Interfaces;

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
        [Inject]
        public ICustomerService CustomerService { get; set; }
        [Inject]
        public ISpentTimeService SpentTimeService { get; set; }

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

            Task task = TaskService.GetByIdEx(id.Value, new TaskInclude().Project().Customer().TaskSteps());
            if (task == null)
            {
                return HttpNotFound();
            }

            return View(task.Decorate());
        }

        // GET: /Tasks/Create
        public virtual ActionResult Create(int? projectId)
        {
            if (projectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new TaskEdit();
            model.ProjectId = projectId.Value;

            return CreateEditView(model);
        }

        public virtual ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var task = TaskService.GetById(id.Value);
            if (task == null)
            {
                return HttpNotFound();
            }

            var model = task.MapTo<TaskEdit>();
            return this.CreateEditView(model);
        }

        // POST: /Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(TaskEdit model)
        {
            return Handle(model, TaskService.Save,
                () => RedirectToAction(MVC.Tasks.View(model.TaskId)),
                () => model.ProjectId == 0 ?
                    RedirectToAction(MVC.Tasks.Create(model.ProjectId)) :
                    RedirectToAction(MVC.Tasks.Edit(model.TaskId)));
        }

        private ActionResult CreateEditView(TaskEdit model)
        {
            ViewBag.EmployeesList = new SelectList(EmployeeService.GetAll(Query.ForEmployee()), "EmployeeId", "FullName");
            ViewBag.Project = ProjectService.GetByIdEx(model.ProjectId, new ProjectInclude().Customer());
            return View(model);
        }

        // GET: /Tasks/Steps/5
        public virtual ActionResult Steps(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = TaskService.GetByIdEx(id.Value, new TaskInclude().Project().Customer().TaskSteps(true).SpentTimes());
            if (task == null)
            {
                return HttpNotFound();
            }

            if (CurrentUser.EmployeeId.HasValue)
            {
                ViewBag.CurrentEmployee = EmployeeService.GetByIdEx(CurrentUser.EmployeeId.Value, new EmployeeInclude().Positions());
            }
            ViewBag.EmployeesList = new SelectList(EmployeeService.GetAll(Query.ForEmployee().Include(x => x.Positions())), "EmployeeId", "FullName");
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
            ActionResult result = this.RedirectToLocalUrl(returnUrl, MVC.Tasks.Index());
            return Handle(id, TaskService.Delete, result, result, "Task successfully deleted");
        }

        public virtual ActionResult List(TasksListViewModel model)
        {
            model.TaskFilter = model.TaskFilter ?? new TaskFilter();
            ViewBag.CustomersList = new SelectList(CustomerService.GetAll(Query.ForCustomer()), "CustomerId", "Name");
            ViewBag.ProjectsList = new SelectList(ProjectService.GetAll(Query.ForProject().Include(x => x.Customer())), "ProjectId", "Name", "Customer.Name");
            return View(model);
        }

        #region ListColumns

        public enum TaskListColumns
        {
            Title = 0,
            Date,
            ProjectName,
            Priority,
            Type,
            Size,
            Urgency,
            Folder,
            Step,
            Hours,
            EstimatedPrice,
            Deadline,
            Delete
        }
        private static TaskListColumns[] columnsWithClientSorting = new TaskListColumns[] { TaskListColumns.Priority, TaskListColumns.Hours };
        private static List<Column<TaskViewModel>> columns = new List<Column<TaskViewModel>>()
        {
            new Column<TaskViewModel>()
                {
                    Title = "Title",
                    Type = DataTableConfig.Column.ColumnSpecialType.TaskLink,
                    Orderable = true,
                    Getter = (x) => new object[] {x.TaskId, x.Title}
                },
            new Column<TaskViewModel>()
                {
                    Title = "Date",
                    Orderable = true,
                    Getter = (x) => x.Date()
                },
            new Column<TaskViewModel>()
                {
                    Title = "Project name",
                    Type = DataTableConfig.Column.ColumnSpecialType.ProjectLink,
                    Getter = (x) => new object[] {x.ProjectId, x.Project.Name}
                },
            new Column<TaskViewModel>()
                {
                    Title = "Priority",
                    Orderable = true,
                    Getter = (x) => x.Priority.ToString()
                },
            new Column<TaskViewModel>()
                {
                    Title = "Type",
                    Orderable = true,
                    Getter = (x) => x.Type.ToString()
                },
            new Column<TaskViewModel>()
                {
                    Title = "Size",
                    Orderable = true,
                    Getter = (x) => x.Size.ToString()
                },
            new Column<TaskViewModel>()
                {
                    Title = "Urgency",
                    Orderable = true,
                    Getter = (x) => x.Urgency.ToString()
                },
            new Column<TaskViewModel>()
                {
                    Title = "Folder",
                    Type = DataTableConfig.Column.ColumnSpecialType.ExternalLink,
                    Getter = (x) => x.FolderLink
                },
            new Column<TaskViewModel>()
                {
                    Title = "Step",
                    Type = DataTableConfig.Column.ColumnSpecialType.TaskStepLink,
                    Getter = (x) => new object[] {x.TaskId, x.Step()}
                },
            new Column<TaskViewModel>()
                {
                    Title = "Hours",
                    Orderable = true,
                    Getter = (x) => x.Hours().ToString()
                },
            new Column<TaskViewModel>()
                {
                    Title = "Estimated price",
                    Orderable = true,
                    Getter = (x) => x.EstimatedPrice.ToString()
                },
            new Column<TaskViewModel>()
                {
                    Title = "Deadline",
                    Orderable = true,
                    Getter = (x) => x.DeadlineStr()
                },
            new Column<TaskViewModel>()
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
            config.Searching = true;
            config.Paging = true;
            config.PageLength = 25;
            config.SetupServerDataSource(Url.Action(MVC.Tasks.ListData()), FormMethod.Post);
            config.Columns = new List<DataTableConfig.Column>(columns);
            config.Columns[(int)TaskListColumns.ProjectName].Visible = showProject;
            config.Columns[(int)TaskListColumns.Delete].Visible = this.Can(MVC.Tasks.Delete());

            switch (sorting)
            {
                case TaskSorting.Title:
                    {
                        config.AddOrder((int)TaskListColumns.Title, DataTables.Mvc.Column.OrderDirection.Ascendant);
                        break;
                    }
                case TaskSorting.Priority:
                    {
                        config.AddOrder((int)TaskListColumns.Priority, DataTables.Mvc.Column.OrderDirection.Descendant);
                        break;
                    }
            }

            var json = JsonConvert.SerializeObject(config);
            return Content(json, "application/json");
        }
        public virtual ActionResult ListData([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest request, TaskFilter taskFilter = null)
        {
            var tasks = this.GetTasks(request, taskFilter);

            var rows = columns.ExtractRows(tasks.Decorate());

            //return null;
            return Json(new DataTablesResponse(request.Draw, rows, tasks.TotalItems, tasks.TotalItems));
        }

        private PagedData<Task> GetTasks(IDataTablesRequest request, TaskFilter taskFilter)
        {
            var filter = taskFilter ?? new TaskFilter();
                //.WithPaging(request.Length, request.Start / request.Length);
            var query = Query.ForTask(filter).Include(x => x.Project().TaskSteps().SpentTimes());
            
            var pageSize = request.Length;
            var pageNumber = request.Start / request.Length;

            PagedData<Task> tasks = null;
            var sorting = request.Sortings<TaskListColumns>().FirstOrDefault();
            if (sorting != null)
            {
                var column = sorting.Item1;
                var direction = sorting.Item2;

                if (columnsWithClientSorting.Contains(column))
                {
                    tasks = this.GetSortedOnClientTasks(query, pageSize, pageNumber, column, direction);
                }
                else
                {
                    tasks = this.GetSortedOnServerTasks(query, pageSize, pageNumber, column, direction);
                }
            }
            else
            {
                tasks = this.GetTasksWithoutSorting(query, pageSize, pageNumber);
            }
            return tasks;
        }
        private PagedData<Task> GetSortedOnClientTasks(IQuery<Task, TaskInclude, TaskFilter, TaskSorter> query, int pageSize, int pageNumber, TaskListColumns column, SortDirection direction)
        {
            var allData = TaskService.GetAll(query);
            var sortedData = allData.Data.AsEnumerable();
            switch (column)
            {
                case TaskListColumns.Priority:
                    {
                        sortedData = sortedData.SortBy(direction, x => x.Priority);
                        break;
                    }
                case TaskListColumns.Hours:
                    {
                        sortedData = sortedData.SortBy(direction, x => x.Hours());
                        break;
                    }
            }

            return new PagedData<Task>(
                sortedData.Skip(pageNumber * pageSize).Take(pageSize).ToList(),
                pageSize,
                pageNumber,
                allData.TotalItems);
        }
        private PagedData<Task> GetSortedOnServerTasks(IQuery<Task, TaskInclude, TaskFilter, TaskSorter> query, int pageSize, int pageNumber, TaskListColumns column, SortDirection direction)
        {
            query.WithPaging(pageSize, pageNumber);

            switch (column)
            {
                case TaskListColumns.Title:
                    {
                        query.Sort(x => x.ByTitle(direction));
                        break;
                    }
                case TaskListColumns.Date:
                    {
                        query.Sort(x => x.ByDateModified(direction));
                        break;
                    }
                case TaskListColumns.Type:
                    {
                        query.Sort(x => x.ByType(direction));
                        break;
                    }
                case TaskListColumns.Size:
                    {
                        query.Sort(x => x.BySize(direction));
                        break;
                    }
                case TaskListColumns.Urgency:
                    {
                        query.Sort(x => x.ByUrgency(direction));
                        break;
                    }
                case TaskListColumns.EstimatedPrice:
                    {
                        query.Sort(x => x.ByEstimatedPrice(direction));
                        break;
                    }
                case TaskListColumns.Deadline:
                    {
                        query.Sort(x => x.ByDeadline(direction));
                        break;
                    }
            }

            return TaskService.GetAll(query);
        }
        private PagedData<Task> GetTasksWithoutSorting(IQuery<Task, TaskInclude, TaskFilter, TaskSorter> query, int pageSize, int pageNumber)
        {
            return TaskService.GetAll(query.WithPaging(pageSize, pageNumber));
        }

        // GET: /Tasks/NextTask/5
        public virtual ActionResult NextTask(int? id)
        {
            id = id ?? UserEmployeeId;

            if (id == null)
            {
                return RedirectToAction(MVC.Employees.ChooseForTasks());
            }

            Employee employee = EmployeeService.GetByIdEx(id.Value, new EmployeeInclude().Positions());
            if (employee == null)
            {
                return HttpNotFound();
            }

            var nextTask = TaskService.GetAll(Query.ForTask()
                    .Include(x => x.Project().Customer().TaskSteps())
                    .Filter(x => x.ByEmployeeId(id.Value)))
                .OrderByDescending(x => x.Priority)
                .FirstOrDefault();

            ViewBag.NextTask = nextTask;
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult AddHours(SpentTimeOnTask model)
        {
            return Handle(model, SpentTimeService.SpentTimeOnTask,
                RedirectToAction(MVC.Tasks.Steps(model.TaskId)),
                RedirectToAction(MVC.Tasks.Steps(model.TaskId)));
        }


        public virtual ActionResult PendingTask()
        {
            var projects = ProjectService.GetAll(Query.ForProject().Filter(x => x.ByCustomerId(UserCustomerId ?? 1)));

            if (projects == null)
            {
                return HttpNotFound();
            }

            var projectsList = projects.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.ProjectId.ToString()
            }).ToList();

            var model = new TaskEdit();
            ViewBag.Projects = projectsList;
            return View(MVC.Tasks.Views.PendingTaskCreate, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult PendingTask(TaskEdit model)
        {
            return Handle(model, TaskService.Save,
                () => RedirectToAction(MVC.Tasks.PendingTaskView()), RedirectToAction(MVC.Tasks.Views.PendingTaskCreate));
        }

        public virtual ActionResult PendingTaskList(TaskFilter taskFilter)
        {
            var tasks = TaskService.GetAll(Query.ForTask(taskFilter).Include(x => x.Project())).ToList();

            var model = new TasksListViewModel { Tasks = tasks };

            return PartialView(MVC.Tasks.Views.PendingTaskList, model);
        }

        public virtual ActionResult PendingTaskListEstimated(TaskFilter taskFilter)
        {
            var tasks = TaskService.GetAll(Query.ForTask(taskFilter).Include(x => x.Project())).ToList();

            var model = new TasksListViewModel { Tasks = tasks };

            return PartialView(MVC.Tasks.Views.PendingTaskListEstimated, model);
        }

        public virtual ActionResult PendingTaskView()
        {
            if (UserCustomerId != null) ViewBag.UserCustomerId = UserCustomerId.Value;
            return View(MVC.Tasks.Views.PendingTask);
        }

        public virtual ActionResult ApproveTask(int taskId)
        {
            var task = TaskService.GetById(taskId);
            if (task == null)
            {
                return HttpNotFound();
            }
            task.DateApproved = DateTime.Now;
            var model = task.MapTo<TaskEdit>();

            return Handle(model, TaskService.Save,
                RedirectToAction(MVC.Tasks.PendingTaskView()),
                RedirectToAction(MVC.Tasks.PendingTaskView()));
        }
    }
}
