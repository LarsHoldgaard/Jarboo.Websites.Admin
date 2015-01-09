using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.Includes;
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
        protected virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return Table.AsNoTracking();
            }
        }

        public T GetById(int id)
        {
            return this.GetByIdEx(id, Include.None<T>());
        }
        public T GetByIdEx(int id, Include<T> include)
        {
            return Find(id, TableNoTracking.Include(include));
        }
        protected abstract T Find(int id, IQueryable<T> query);

        public List<T> GetAll()
        {
            return this.GetAllEx(Include.None<T>());
        }
        public List<T> GetAllEx(Include<T> include)
        {
            return TableNoTracking.Include(include)
                .AsEnumerable()
                .ToList();
        }

        protected void Add<TM>(T entity, TM model)
            where TM : class, new()
        {
            Table.Add(entity);
            Save(entity, model);
        }
        protected void Edit<TM>(T entity, TM model, Action beforeSave = null)
            where TM : class, new()
        {
            Table.Attach(entity);
            entity.DateModified = DateTime.Now;

            Save(entity, model, beforeSave);
        }
        protected virtual void Save<TM>(T entity, TM model, Action beforeSave = null)
            where TM : class, new()
        {
            model.MapTo(entity);
            if (beforeSave != null)
            {
                beforeSave();
            }
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
