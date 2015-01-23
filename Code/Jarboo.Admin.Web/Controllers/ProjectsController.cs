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

using Ninject;

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

        // GET: /Projects/
        public virtual ActionResult Index()
        {
            return View(ProjectService.GetAll(Query.ForProject().Include(x => x.Customer())));
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
            var projectEdit = new ProjectEdit();
            if (customerId.HasValue)
            {
                projectEdit.CustomerId = customerId.Value;
            }

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
            ViewBag.BoardNames = new SelectList(TaskRegister.BoardNames(), model.BoardName);
            ViewBag.CustomersList = new SelectList(CustomerService.GetAll(Query.ForCustomer()), "CustomerId", "Name", model.CustomerId);
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
                    RedirectToAction(MVC.Projects.Create()) :
                    RedirectToAction(MVC.Projects.Edit(model.ProjectId)));
        }
    }
}
