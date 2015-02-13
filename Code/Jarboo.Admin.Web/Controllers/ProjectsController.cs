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
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.DAL;
using Jarboo.Admin.Web.Models.Documentation;
using Jarboo.Admin.Web.Models.Project;

using Ninject;
using Jarboo.Admin.BL.Services.Interfaces;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class ProjectsController : BaseController
    {
        [Inject]
        public IProjectService ProjectService { get; set; }
        [Inject]
        public ICustomerService CustomerService { get; set; }
        [Inject]
        public ITaskRegister TaskRegister { get; set; }
        [Inject]
        public ISpentTimeService SpentTimeService { get; set; }
        [Inject]
        public IEmployeeService EmployeeService { get; set; }

        // GET: /Projects/
        public virtual ActionResult Index()
        {
            return View(MVC.Projects.Views.Index);
        }

        // GET: /Projects/View/5
        public virtual ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = ProjectService.GetByIdEx(id.Value, new ProjectInclude().Customer());
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: /Projects/Create
        public virtual ActionResult Create(int? customerId)
        {
            if (customerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var projectEdit = new ProjectEdit();
            projectEdit.CustomerId = customerId.Value;

            return this.CreateEditView(projectEdit);
        }

        // GET: /Projects/Edit/5
        public virtual ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = ProjectService.GetById(id.Value);
            if (project == null)
            {
                return HttpNotFound();
            }

            var projectEdit = project.MapTo<ProjectEdit>();
            return this.CreateEditView(projectEdit);
        }

        private ActionResult CreateEditView(ProjectEdit model)
        {
            ViewBag.BoardNames = new SelectList(TaskRegister.ProjectNames(), model.BoardName);
            ViewBag.Customer = CustomerService.GetById(model.CustomerId);
            return View(model);
        }

        // POST: /Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(ProjectEdit model)
        {
            return Handle(model, ProjectService.Save,
                () => RedirectToAction(MVC.Projects.View(model.ProjectId)),
                () => model.ProjectId == 0 ?
                    RedirectToAction(MVC.Projects.Create(model.CustomerId)) :
                    RedirectToAction(MVC.Projects.Edit(model.ProjectId)));
        }

        public virtual ActionResult ProductListByCustomerForSelectJson(int? value)
        {
            var projects = ProjectService.GetAll(Query.ForProject().Include(x => x.Customer()).Filter(x => x.ByCustomerId(value)));
            var projectsList = projects
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.ProjectId.ToString()
                })
                .ToList();
            projectsList.Insert(0, new SelectListItem()
                                       {
                                           Text = "Any project"
                                       });
            return Json(projectsList, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public virtual ActionResult List(bool showCustomer = false, ProjectFilter projectFilter = null)
        {
            projectFilter = projectFilter ?? new ProjectFilter();

            var model = new ProjectsListViewModel()
            {
                ShowCustomer = showCustomer,
                Projects = ProjectService.GetAll(Query.ForProject(projectFilter).Include(x => x.Customer()))
            };

            return View(model);
        }

        public virtual ActionResult AddHours(int? projectId)
        {
            if (projectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new SpentTimeOnProject();
            model.ProjectId = projectId.Value;

            ViewBag.EmployeesList = new SelectList(EmployeeService.GetAll(Query.ForEmployee()), "EmployeeId", "FullName");
            ViewBag.Project = ProjectService.GetByIdEx(model.ProjectId, new ProjectInclude().Customer());
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult AddHours(SpentTimeOnProject model)
        {
            return Handle(model, SpentTimeService.SpentTimeOnProject,
                RedirectToAction(MVC.Projects.View(model.ProjectId)),
                RedirectToAction(MVC.Projects.AddHours(model.ProjectId)));
        }
    }
}
