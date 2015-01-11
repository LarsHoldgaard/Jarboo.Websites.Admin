using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.DAL.Entities;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Jarboo.Admin.BL;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class DocumentationsController : BaseController
    {
        [Inject]
        public IDocumentationService DocumentationService { get; set; }
        [Inject]
        public IProjectService ProjectService { get; set; }

        // GET: /Documentations/
        public virtual ActionResult Index()
        {
            return View(DocumentationService.GetAllEx(Include.ForDocumentation()));
        }

        // GET: /Documentations/View/5
        public virtual ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Documentation documentation = DocumentationService.GetByIdEx(id.Value, Include.ForDocumentation().Project().Customer());
            if (documentation == null)
            {
                return HttpNotFound();
            }
            return View(documentation);
        }

        // GET: /Documentations/Create
        public virtual ActionResult Create()
        {
            var documentation = new DocumentationEdit();

            ViewBag.ProjectsList = new SelectList(ProjectService.GetAll(), "ProjectId", "Name", "Customer.Name", documentation.ProjectId);
            return View(documentation);
        }

        // GET: /Documentations/Edit/5
        public virtual ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var documentation = DocumentationService.GetById(id.Value);
            if (documentation == null)
            {
                return HttpNotFound();
            }

            var documentationEdit = documentation.MapTo<DocumentationEdit>();

            ViewBag.ProjectsList = new SelectList(ProjectService.GetAll(), "ProjectId", "Name", "Customer.Name", documentation.ProjectId);
            return View(documentationEdit);
        }

        // POST: /Documentations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(DocumentationEdit model)
        {
            return Handle(
                model, DocumentationService.Save,
                () => RedirectToAction(MVC.Documentations.View(model.DocumentationId)),
                () => model.DocumentationId == 0 ?
                    RedirectToAction(MVC.Documentations.Create()) :
                    RedirectToAction(MVC.Documentations.Edit(model.DocumentationId)));
        }
    }
}