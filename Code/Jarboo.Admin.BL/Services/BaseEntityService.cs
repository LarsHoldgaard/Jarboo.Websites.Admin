using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.Authorization;
using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Sorters;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services
{
    public abstract class BaseEntityService<TKey, T> : BaseService, IEntityService<TKey, T>
        where T : class, IBaseEntity, new()
    {
        public BaseEntityService(IUnitOfWork unitOfWork, IAuth auth)
            : base(unitOfWork, auth)
        { }

        protected abstract IDbSet<T> Table { get; }
        protected virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return Table.AsNoTracking();
            }
        }

        protected virtual IQueryable<T> FilterCanView(IQueryable<T> query)
        {
            return query.Where(x => false);
        }

        public T GetById(TKey id)
        {
            return this.GetByIdEx(id, Include<T>.None);
        }
        public T GetByIdEx(TKey id, Include<T> include)
        {
            if (Cannot(Rights.ViewAll) && Cannot(Rights.ViewSpecial))
            {
                return null;
            }

            var query = TableNoTracking.Include(include);
            if (Cannot(Rights.ViewAll))
            {
                query = this.FilterCanView(query);
            }

            return Find(id, query);
        }
        protected abstract T Find(TKey id, IQueryable<T> query);

        public PagedData<T> GetAll(IQuery<T, Include<T>, Filter<T>, Sorter<T>> query)
        {
            if (Cannot(Rights.ViewAll) && Cannot(Rights.ViewSpecial))
            {
                return PagedData.AllOnOnePage(Enumerable.Empty<T>().AsQueryable());
            }

            Func<IQueryable<T>, IQueryable<T>> securityFilter = null;
            if (Cannot(Rights.ViewAll))
            {
                securityFilter = this.FilterCanView;
            }

            return query.ApplyTo(TableNoTracking, securityFilter);
        }

        protected virtual bool HasAccessTo(T entity)
        {
            return false;
        }
        protected virtual bool CanAddSpecial(T entity)
        {
            return HasAccessTo(entity);
        }
        protected virtual bool CanEditSpecial(T entity)
        {
            return HasAccessTo(entity);
        }
        protected virtual bool CanDeleteSpecial(T entity)
        {
            return HasAccessTo(entity);
        }
        protected virtual bool CanDisableSpecial(T entity)
        {
            return HasAccessTo(entity);
        }

        protected void CheckCan(string permAny, string permSpecial, Func<T, bool> specialCan, T entity)
        {
            if (Cannot(permAny) && Cannot(permSpecial))
            {
                OnAccessDenied();
            }

            if (Cannot(permAny))
            {
                if (!specialCan(entity))
                {
                    OnAccessDenied();
                }
            }  
        }
        protected virtual void CheckCanAdd(T entity)
        {
            CheckCan(Rights.AddAny, Rights.AddSpecial, CanAddSpecial, entity);
        }
        protected virtual void CheckCanEdit(T entity)
        {
            CheckCan(Rights.EditAny, Rights.EditSpecial, CanEditSpecial, entity);
        }
        protected virtual void CheckCanDelete(T entity)
        {
            CheckCan(Rights.DeleteAny, Rights.DeleteSpecial, CanDeleteSpecial, entity);
        }
        protected virtual void CheckCanDisable(T entity)
        {
            CheckCan(Rights.DisableAny, Rights.DisableSpecial, CanDisableSpecial, entity);
        }

        protected void Add<TM>(T entity, TM model)
            where TM : class, new()
        {
            model.MapTo(entity);

            CheckCanAdd(entity);

            Table.Add(entity);
            Save(entity, model);
        }
        protected void Edit<TM>(T entity, TM model)
            where TM : class, new()
        {
            model.MapTo(entity);

            CheckCanEdit(entity);

            Table.Attach(entity);
            entity.DateModified = DateTime.Now;
            
            Save(entity, model);
        }
        protected virtual void Save<TM>(T entity, TM model)
            where TM : class, new()
        {
            UnitOfWork.SaveChanges();
            entity.MapTo(model);
        }

        protected void Delete(T entity)
        {
            CheckCanDelete(entity);

            Table.Attach(entity);
            Table.Remove(entity);

            UnitOfWork.SaveChanges();
        }
    }
}
