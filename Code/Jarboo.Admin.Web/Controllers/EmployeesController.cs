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
            return View(EmployeeService.GetAll(Query.ForEmployee().Include(x => x.Positions())));
        }

        // GET: /Employees/View/5
        public virtual ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Employee employee = EmployeeService.GetByIdEx(id.Value, new EmployeeInclude().Positions());
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: /Employees/Create
        public virtual ActionResult Create()
        {
            var employee = new EmployeeCreate();
            return View(employee);
        }

        // POST: /Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(EmployeeCreate model)
        {
            return Handle(
                model, EmployeeService.Create,
                () => RedirectToAction(MVC.Employees.View(model.EmployeeId)),
                RedirectToAction(MVC.Employees.Create()));
        }
        
        // GET: /Employees/Edit/5
        public virtual ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employee = EmployeeService.GetByIdEx(id.Value, new EmployeeInclude().Positions());
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
                model, EmployeeService.Edit,
                RedirectToAction(MVC.Employees.View(model.EmployeeId)),
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

        public virtual ActionResult ChooseForTasks()
        {
            return this.View(EmployeeService.GetAll(Query.ForEmployee()).OrderBy(x => x.FullName));
        }
    }
}
