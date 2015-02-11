using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using DataTables.Mvc;
using Jarboo.Admin.BL;
using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.BL.Sorters;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Web.Infrastructure;
using Jarboo.Admin.Web.Models.DataTable;
using Jarboo.Admin.Web.Models.Report;
using Newtonsoft.Json;
using Ninject;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class ReportingController : BaseController
    {
        [Inject]
        public ITaskService TaskService { get; set; }
        [Inject]
        public IProjectService ProjectService { get; set; }
        [Inject]
        public ICustomerService CustomerService { get; set; }

        // GET: Reporting
        public virtual ActionResult Index()
        {
            return View(MVC.Reporting.Views.Index);
        }

        public virtual ActionResult List(ReportsListViewModel model)
        {
            model.ReportFilter = model.ReportFilter ?? new ReportFilter();

            if (UserCustomerId.HasValue)
            {
                model.ReportFilter.CustomerId = UserCustomerId.Value;

                var projects = ProjectService.GetAll(Query.ForProject().Include(x => x.Customer()).Filter(x => x.ByCustomerId(UserCustomerId)));
                var projectsList = projects
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.ProjectId.ToString()
                    })
                    .ToList();

                ViewBag.ProjectsList = projectsList;
            }
            else
            {
                ViewBag.ProjectsList = new SelectList(ProjectService.GetAll(Query.ForProject().Include(x => x.Customer())), "ProjectId", "Name", "Customer.Name");
            }
            ViewBag.CustomersList = new SelectList(CustomerService.GetAll(Query.ForCustomer()), "CustomerId", "Name");

            return View(model);
        }

        public enum ReportListColumns
        {
            ProjectName,
            Title,
            Step,
            Price
        }

        private static ReportListColumns[] columnsWithClientSorting = new ReportListColumns[] { ReportListColumns.ProjectName, ReportListColumns.Title, ReportListColumns.Step, ReportListColumns.Price };
        private static List<Column<ReportViewModel>> columns = new List<Column<ReportViewModel>>()
        {
             new Column<ReportViewModel>()
                {
                    Title = "Project",
                    Type = DataTableConfig.Column.ColumnSpecialType.ProjectLink,
                    Orderable = true,
                    Getter = (x) => new object[] {x.ProjectId, x.Project.Name}
                },
            new Column<ReportViewModel>()
                {
                    Title = "Task",
                    Orderable = true,
                    Getter = (x) => new object[] { x.Title}
                },
            new Column<ReportViewModel>()
                {
                    Title = "Task state",
                    Orderable = true,
                    Getter = (x) => x.Step().ToString()
                },
            new Column<ReportViewModel>()
                {
                    Title = "Price",
                    Orderable = true,
                    Getter = (x) => new object[] {x.Hours()}
                }
        };

        public virtual ActionResult ListConfig(ReportSorting sorting = ReportSorting.Title)
        {
            var config = new DataTableConfig();
            config.Searching = true;
            config.SetupServerDataSource(Url.Action(MVC.Reporting.ListData()), FormMethod.Post);
            config.Columns = new List<DataTableConfig.Column>(columns);

            switch (sorting)
            {
                case ReportSorting.Title:
                    {
                        config.AddOrder((int)ReportListColumns.Title, DataTables.Mvc.Column.OrderDirection.Ascendant);
                        break;
                    }
                case ReportSorting.Project:
                    {
                        config.AddOrder((int)ReportListColumns.ProjectName, DataTables.Mvc.Column.OrderDirection.Descendant);
                        break;
                    }
                case ReportSorting.Step:
                    {
                        config.AddOrder((int)ReportListColumns.Step, DataTables.Mvc.Column.OrderDirection.Descendant);
                        break;
                    }

            }
            var json = JsonConvert.SerializeObject(config);

            return Content(json, "application/json");
        }

        public virtual ActionResult ListData([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest request, ReportFilter reportFilter = null)
        {
            var tasks = this.GetTasks(request, reportFilter);

            var rows = columns.ExtractRows(tasks.DecorateReport());

            return Json(new DataTablesResponse(request.Draw, rows, tasks.TotalItems, tasks.TotalItems));
        }

        private PagedData<Task> GetTasks(IDataTablesRequest request, ReportFilter reportFilter)
        {
            var filter = reportFilter ?? new ReportFilter();

            var query = Query.ForReport(filter).Include(x => x.Project().TaskSteps());

            var pageSize = request.Length;
            var pageNumber = request.Start / request.Length;

            PagedData<Task> tasks = null;
            var sorting = request.Sortings<ReportListColumns>().FirstOrDefault();
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

        private PagedData<Task> GetSortedOnClientTasks(IQuery<Task, ReportInclude, ReportFilter, ReportSorter> query, int pageSize, int pageNumber, ReportListColumns column, SortDirection direction)
        {
            var allData = TaskService.GetAll(query);
            var sortedData = allData.Data.AsEnumerable();

            switch (column)
            {
                case ReportListColumns.Title:
                    {
                        sortedData = sortedData.SortBy(direction, x => x.Title);
                        break;
                    }
                case ReportListColumns.Step:
                    {
                        sortedData = sortedData.SortBy(direction, x => x.Type);
                        break;
                    }
                case ReportListColumns.ProjectName:
                    {
                        sortedData = sortedData.SortBy(direction, x => x.Project.Name);
                        break;
                    }
            }

            return new PagedData<Task>(
                sortedData.Skip(pageNumber * pageSize).Take(pageSize).ToList(),
                pageSize,
                pageNumber,
                allData.TotalItems);
        }

        private PagedData<Task> GetSortedOnServerTasks(IQuery<Task, ReportInclude, ReportFilter, ReportSorter> query, int pageSize, int pageNumber, ReportListColumns column, SortDirection direction)
        {
            query.WithPaging(pageSize, pageNumber);

            switch (column)
            {
                case ReportListColumns.Title:
                    {
                        query.Sort(x => x.ByTitle(direction));
                        break;
                    }

                case ReportListColumns.ProjectName:
                    {
                        query.Sort(x => x.ByProject(direction));
                        break;
                    }
                case ReportListColumns.Step:
                    {
                        query.Sort(x => x.ByType(direction));
                        break;
                    }

            }
            return TaskService.GetAll(query);
        }

        private PagedData<Task> GetTasksWithoutSorting(IQuery<Task, ReportInclude, ReportFilter, ReportSorter> query, int pageSize, int pageNumber)
        {
            return TaskService.GetAll(query.WithPaging(pageSize, pageNumber));
        }

    }
}