using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Services
{
    public interface IDocumentationService : IEntityService<Documentation>
    {
        void Save(DocumentationEdit model, IBusinessErrorCollection errors);
        void Delete(int documentationId, IBusinessErrorCollection errors);
    }

    public class DocumentationService : BaseEntityService<Documentation>, IDocumentationService
    {
        public DocumentationService(IUnitOfWork UnitOfWork)
            : base(UnitOfWork)
        { }

        protected override IDbSet<Documentation> Table
        {
            get { return UnitOfWork.Documentations; }
        }
        protected override Documentation Find(int id, IQueryable<Documentation> query)
        {
            return query.FirstOrDefault(x => x.DocumentationId == id);
        }

        public void Save(DocumentationEdit model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            if (model.DocumentationId == 0)
            {
                var entity = new Documentation();
                Add(entity, model);
            }
            else
            {
                var entity = new Documentation { DocumentationId = model.DocumentationId };
                Edit(entity, model);
            }
        }

        public void Delete(int documentationId, IBusinessErrorCollection errors)
        {
            if (documentationId == 0)
            {
                throw new Exception("Incorrect entity id");
            }

            Delete(new Documentation()
            {
                DocumentationId = documentationId
            });
        }
    }
}
