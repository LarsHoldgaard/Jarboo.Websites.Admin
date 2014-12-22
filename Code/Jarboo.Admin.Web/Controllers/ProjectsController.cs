using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.BL.Models;
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

        // GET: /Projects/
        public virtual ActionResult Index()
        {
            return View(ProjectService.GetAll());
        }

        // GET: /Projects/View/5
        public virtual ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = ProjectService.GetById(id.Value);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: /Project/Create
        public virtual ActionResult Create(int? customerId)
        {
            var project = new ProjectCreate();
            if (customerId.HasValue)
            {
                project.CustomerId = customerId.Value;
            }

            ViewBag.CustomerId = new SelectList(CustomerService.GetAll(), "CustomerId", "Name", project.CustomerId);
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(ProjectCreate model)
        {
            return Handle(model, ProjectService.Create,
                () => RedirectToAction(MVC.Projects.View(model.ProjectId)),
                RedirectToAction(MVC.Projects.Create()));
        }
    }
}
