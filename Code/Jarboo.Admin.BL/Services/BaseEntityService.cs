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
    public abstract class BaseEntityService<T> : BaseService
        where T : BaseEntity, new()
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

        protected abstract string SecurityEntities { get; }
        protected bool Can(string action)
        {
            return Auth.Can(SecurityEntities, action);
        }
        protected bool Cannot(string action)
        {
            return !Can(action);
        }

        protected virtual IQueryable<T> FilterCanView(IQueryable<T> query)
        {
            return query;
        }

        public T GetById(int id)
        {
            return this.GetByIdEx(id, Include<T>.None);
        }
        public T GetByIdEx(int id, Include<T> include)
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
        protected abstract T Find(int id, IQueryable<T> query);

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

        protected virtual bool CanAEDSpecial(T entity)
        {
            return true;
        }
        protected virtual bool CanAddSpecial(T entity)
        {
            return CanAEDSpecial(entity);
        }
        protected virtual bool CanEditSpecial(T entity)
        {
            return CanAEDSpecial(entity);
        }
        protected virtual bool CanDeleteSpecial(T entity)
        {
            return CanAEDSpecial(entity);
        }
        protected virtual void CheckCanAdd(T entity)
        {
            if (Cannot(Rights.AddAny) && Cannot(Rights.AddSpecial))
            {
                throw new ApplicationException("Access denied");
            }

            if (Cannot(Rights.AddAny))
            {
                if (!CanAddSpecial(entity))
                {
                    throw new ApplicationException("Access denied");
                }
            }
        }
        protected virtual void CheckCanEdit(T entity)
        {
            if (Cannot(Rights.EditAny) && Cannot(Rights.EditSpecial))
            {
                throw new ApplicationException("Access denied");
            }

            if (Cannot(Rights.EditAny))
            {
                if (!CanEditSpecial(entity))
                {
                    throw new ApplicationException("Access denied");
                }
            }
        }
        protected virtual void CheckCanDelete(T entity)
        {
            if (Cannot(Rights.DeleteAny) && Cannot(Rights.DeleteSpecial))
            {
                throw new ApplicationException("Access denied");
            }

            if (Cannot(Rights.DeleteAny))
            {
                if (!CanDeleteSpecial(entity))
                {
                    throw new ApplicationException("Access denied");
                }
            }
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
