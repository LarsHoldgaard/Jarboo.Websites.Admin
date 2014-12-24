using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services
{
    public abstract class BaseEntityService<T> : BaseService
        where T : BaseEntity, new()
    {
        public BaseEntityService(IUnitOfWork UnitOfWork)
            : base(UnitOfWork)
        { }

        protected abstract IDbSet<T> Table { get; }
        //protected abstract IQueryable<T> All { get; }

        protected virtual T GetByIdInternal(int id)
        {
            return Table.Find(id);
        }
        public virtual T GetById(int id)
        {
            var entity = GetByIdInternal(id);
            if (entity == null)
            {
                throw new NotFoundException();
            }

            return entity;
        }

        protected void Add<TM>(T entity, TM model)
            where TM : class, new()
        {
            entity.DateCreated = DateTime.Now;
            entity.DateModified = entity.DateCreated;

            Table.Add(entity);
            Save(entity, model);
        }
        protected void Edit<TM>(T entity, TM model)
            where TM : class, new()
        {
            entity.DateModified = DateTime.Now;

            Table.Attach(entity);
            Save(entity, model);
        }
        protected virtual void Save<TM>(T entity, TM model)
            where TM : class, new()
        {
            model.MapTo(entity);
            UnitOfWork.SaveChanges();
            entity.MapTo(model);
        }

        /*protected void Delete(T entity)
        {
            Table.Attach(entity);
            Table.Remove(entity);

            UnitOfWork.SaveChanges();
        }*/
    }
}
