using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Jarboo.Admin.BL.Authorization;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.BL.Services.Interfaces;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services
{
    public class CommentService : BaseEntityService<int, Comment>, ICommentService
    {
        public CommentService(IUnitOfWork unitOfWork, IAuth auth, ICacheService cacheService)
            : base(unitOfWork, auth, cacheService)
        {
        }

        protected override IDbSet<Comment> Table
        {
            get { return UnitOfWork.Comments; }
        }
        protected override Comment Find(int id, IQueryable<Comment> query)
        {
            Type type = typeof(Comment);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Comment)this.CacheService.GetById(cacheKey);

            var doc = query.FirstOrDefault(x => x.CommentId == id);
            this.CacheService.Create(cacheKey, doc);
            return doc;
        }

        protected override async Task<Comment> FindAsync(int id, IQueryable<Comment> query)
        {
            Type type = typeof(Comment);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Comment)this.CacheService.GetById(cacheKey);

            var doc = await query.FirstOrDefaultAsync(x => x.CommentId == id);
            this.CacheService.Create(cacheKey, doc);
            return doc;
        }

        protected override string SecurityEntities
        {
            get { return Rights.Comment.Name; }
        }

        protected override IQueryable<Comment> FilterCanView(IQueryable<Comment> query)
        {
            return query;
        }

        public void Save(Comment model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            if (model.CommentId == 0)
            {
                var entity = new Comment();
                Add(entity, model);
            }
            else
            {
                var entity = new Comment { CommentId = model.CommentId };
                Edit(entity, model);
            }

            ClearCache();
        }

        public void Delete(int commentId, IBusinessErrorCollection errors)
        {
            if (commentId == 0)
            {
                throw new Exception("Incorrect entity id");
            }

            Delete(new Comment()
            {
                CommentId = commentId
            });

            ClearCache();
        }

        private void ClearCache()
        {
            try
            {
                Type type = typeof(Comment);
                this.CacheService.DeleteByContaining(type.Name);
            }
            catch (Exception)
            {
            }
        }
    }
}
