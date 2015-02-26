using System.Linq;
using System.Web.Mvc;

using Jarboo.Admin.BL;
using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.Web.Models.Time;
using Ninject;
using Jarboo.Admin.BL.Services.Interfaces;
using System;
using Newtonsoft.Json;
using Jarboo.Admin.Web.Models.Morris;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class SpentTimeController : BaseController
    {
        [Inject]
        public ISpentTimeService SpentTimeService { get; set; }

        [Inject]
        public IEmployeeService EmployeeService { get; set; }

        [Inject]
        public ITaskService TaskService { get; set; }

        public virtual ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public virtual ActionResult GroupedList(SpentTimeFilter spentTimeFilter = null)
        {
            spentTimeFilter = (spentTimeFilter ?? new SpentTimeFilter()).ByAccepted(true);
            var items = SpentTimeService.GetAll(Query.ForSpentTime(spentTimeFilter));

            ViewBag.EmployeesList = new SelectList(EmployeeService.GetAll(Query.ForEmployee().Include(x => x.Positions())), "EmployeeId", "FullName");
            return this.View(items);
        }

        [ChildActionOnly]
        public virtual ActionResult List(SpentTimeFilter spentTimeFilter = null)
        {
            spentTimeFilter = spentTimeFilter ?? new SpentTimeFilter();
            var items = SpentTimeService.GetAll(Query.ForSpentTime(spentTimeFilter).Include(x => x.Task().Project().Customer().Employee()));
            return this.View(items);
        }

        [HttpPost]
        public virtual ActionResult Accept(int id, string returnUrl)
        {
            var result = this.RedirectToLocalUrl(returnUrl, MVC.SpentTime.Index());
            return Handle(id, SpentTimeService.Accept, result, result, "Accepted");
        }

        [HttpPost]
        public virtual ActionResult Deny(int id, string returnUrl)
        {
            var result = this.RedirectToLocalUrl(returnUrl, MVC.SpentTime.Index());
            return Handle(id, SpentTimeService.Deny, result, result, "Denied");
        }

        #region Times

        // GET:  Times
        public virtual ActionResult TimeList(int taskId)
        {
            var timeFilter = new SpentTimeFilter().ByTask(taskId);
            var times = SpentTimeService.GetAll(Query.ForSpentTime(timeFilter).Include(x => x.Employee())).ToList();

            var model = new TimeListViewModel()
            {
                Times = times,
                ProjectId = TaskService.GetById(taskId).ProjectId,
                TaskId = taskId,
                TotalHours = times.Sum(x => x.Hours)
            };

            return PartialView(MVC.SpentTime.Views._ListTime, model);
        }

        // GET: /Times/Create
        public virtual ActionResult Create(int taskId, int projectId)
        {
            var timeCreate = new TimeViewModel
           {
               TaskId = taskId,
               EmployeeId = UserEmployeeId ?? 1,
               ProjectId = projectId
           };

            return PartialView(MVC.SpentTime.Views._AddTimeForm, timeCreate);
        }

        // POST: /Times/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(TimeViewModel model)
        {
            var entity = model.MapTo<SpentTimeOnTask>();

            return Handle(entity, SpentTimeService.SpentTimeOnTask, () => RedirectToAction(MVC.SpentTime.TimeList(model.TaskId.Value)), new EmptyResult());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(int id, int taskId)
        {
            return Handle(id, SpentTimeService.Delete, () => RedirectToAction(MVC.SpentTime.TimeList(taskId)), new EmptyResult(), "Time successfully deleted");
        }
        #endregion

        public virtual ActionResult HoursPerDayChartData()
        {
            var spentTimes = SpentTimeService.GetAll(Query.ForSpentTime()
                .Filter(x => x.ByAccepted(true).ByFromDate(DateTime.Now.AddMonths(-1))))
                .Data
                .GroupBy(x => x.Date.Date).OrderBy(x => x.Key)
                .Select(x => new { date = x.Key, hours = (int)x.Aggregate(0m, (h, y) => h + y.Hours) });

            var config = new MorrisConfig()
                {
                    Data = spentTimes,
                    XKey = "date",  
                    YKeys = new [] {"hours"},
                    Labels = new [] {"Hours"}
                };
            
            var json = JsonConvert.SerializeObject(config);
            return Content(json, "application/json");
        }
    }
}