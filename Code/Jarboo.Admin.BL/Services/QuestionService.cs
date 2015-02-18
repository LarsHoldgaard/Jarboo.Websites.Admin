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
    public class QuestionService : BaseEntityService<int, Question>, IQuestionService
    {
        public QuestionService(IUnitOfWork unitOfWork, IAuth auth, ICacheService cacheService)
            : base(unitOfWork, auth, cacheService)
        {
        }

        protected override IDbSet<Question> Table
        {
            get { return UnitOfWork.Questions; }
        }
        protected override Question Find(int id, IQueryable<Question> query)
        {
            Type type = typeof(Question);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Question)this.CacheService.GetById(cacheKey);

            var doc = query.FirstOrDefault(x => x.QuestionId == id);
            this.CacheService.Create(cacheKey, doc);
            return doc;
        }

        protected override async Task<Question> FindAsync(int id, IQueryable<Question> query)
        {
            Type type = typeof(Question);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Question)this.CacheService.GetById(cacheKey);

            var doc = await query.FirstOrDefaultAsync(x => x.QuestionId == id);
            this.CacheService.Create(cacheKey, doc);
            return doc;
        }

        protected override string SecurityEntities
        {
            get { return Rights.Question.Name; }
        }

        protected override IQueryable<Question> FilterCanView(IQueryable<Question> query)
        {
            return query;
        }

        public void Save(Question model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            if (model.QuestionId == 0)
            {
                var entity = new Question();
                Add(entity, model);
            }
            else
            {
                var entity = new Question { QuestionId = model.QuestionId };
                Edit(entity, model);
            }

            ClearCache();
        }

        public void Edit(Question model)
        {
            var entity = new Question { QuestionId = model.QuestionId };
            Edit(entity, model);
        }

        public void Delete(int questionId, IBusinessErrorCollection errors)
        {
            if (questionId == 0)
            {
                throw new Exception("Incorrect entity id");
            }

            Delete(new Question()
            {
                QuestionId = questionId
            });

            ClearCache();
        }

        private void ClearCache()
        {
            try
            {
                Type type = typeof(Question);
                this.CacheService.DeleteByContaining(type.Name);
            }
            catch (Exception)
            {
            }
        }
    }
}
