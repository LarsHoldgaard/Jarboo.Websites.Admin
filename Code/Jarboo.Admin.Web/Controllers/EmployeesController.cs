using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.BL;
using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.DAL;

using Ninject;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class EmployeesController : BaseController
    {
        [Inject]
        public IEmployeeService EmployeeService { get; set; }
        [Inject]
        public ITaskService TaskService { get; set; }

        // GET: /Employees/
        public virtual ActionResult Index()
        {
            return View(EmployeeService.GetAll(Include.ForEmployee().Positions(), BL.Filters.Filter.ForEmployee()));
        }

        // GET: /Employees/View/5
        public virtual ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Employee employee = EmployeeService.GetByIdEx(id.Value, Include.ForEmployee().Positions());
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: /Employees/Create
        public virtual ActionResult Create()
        {
            var employee = new EmployeeEdit();
            return View(employee);
        }
        
        // GET: /Employees/Edit/5
        public virtual ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employee = EmployeeService.GetByIdEx(id.Value, Include.ForEmployee().Positions());
            if (employee == null)
            {
                return HttpNotFound();
            }

            var employeeEdit = employee.MapTo<EmployeeEdit>();
            return View(employeeEdit);
        }

        // POST: /Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(EmployeeEdit model)
        {
            return Handle(
                model, EmployeeService.Save,
                () => RedirectToAction(MVC.Employees.View(model.EmployeeId)),
                () => model.EmployeeId == 0 ? 
                    RedirectToAction(MVC.Employees.Create()) :
                    RedirectToAction(MVC.Employees.Edit(model.EmployeeId)));
        }

        // POST: /Employees/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(int id, string returnUrl)
        {
            ActionResult result;
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                result = RedirectToAction(MVC.Employees.Index());
            }
            else
            {
                result = this.Redirect(returnUrl);
            }

            return Handle(id, EmployeeService.Delete, result, result, "Employee successfully deleted");
        }

        // GET: /Employees/View/5
        public virtual ActionResult Tasks(int? id)
        {
            if (id == null)
            {
                return View(MVC.Employees.Views.ChooseForTasks, EmployeeService.GetAll(Include.ForEmployee(), BL.Filters.Filter.ForEmployee()).OrderBy(x => x.FullName));
            }

            Employee employee = EmployeeService.GetByIdEx(id.Value, Include.ForEmployee().Positions());
            if (employee == null)
            {
                return HttpNotFound();
            }

            var nextTask = TaskService.GetAll(
                    Include.ForTask().Project().Customer().TaskSteps(),
                    BL.Filters.Filter.ForTask().WithEmployeeId(id.Value))
                .OrderByDescending(x => x.Priority)
                .FirstOrDefault();

            ViewBag.NextTask = nextTask;
            return View(employee);
        }
    }
}
