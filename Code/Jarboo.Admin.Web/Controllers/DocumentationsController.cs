using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Web.Models.Documentation;

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
            return View(DocumentationService.GetAllEx(Include.ForDocumentation().Project(), Filter<Documentation>.None));
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
        public virtual ActionResult Create(int? projectId)
        {
            var documentationEdit = new DocumentationEdit();
            if (projectId.HasValue)
            {
                documentationEdit.ProjectId = projectId.Value;
            }

            return this.CreateEditView(documentationEdit);
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
            return this.CreateEditView(documentationEdit);
        }

        private ActionResult CreateEditView(DocumentationEdit model)
        {
            ViewBag.ProjectsList = new SelectList(ProjectService.GetAllEx(Include.ForProject().Customer(), Filter<Project>.None), "ProjectId", "Name", "Customer.Name", model.ProjectId);
            return View(model);
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

        // POST: /Documentations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(DocumentationDelete model)
        {
            var result = model.ProjectId.HasValue
                             ? RedirectToAction(MVC.Projects.View(model.ProjectId.Value))
                             : RedirectToAction(MVC.Documentations.Index());

            return Handle(model.DocumentationId, DocumentationService.Delete, result, result);
        }
    }
}