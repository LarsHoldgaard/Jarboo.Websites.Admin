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
    public class AnswerService : BaseEntityService<int, Answer>, IAnswerService
    {
        private IQuestionService questionService;
        public AnswerService(IUnitOfWork unitOfWork, IAuth auth, ICacheService cacheService, IQuestionService questionService)
            : base(unitOfWork, auth, cacheService)
        {
            this.questionService = questionService;
        }

        protected override IDbSet<Answer> Table
        {
            get { return UnitOfWork.Answers; }
        }
        protected override Answer Find(int id, IQueryable<Answer> query)
        {
            Type type = typeof(Answer);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Answer)this.CacheService.GetById(cacheKey);

            var doc = query.FirstOrDefault(x => x.AnswerId == id);
            this.CacheService.Create(cacheKey, doc);
            return doc;
        }

        protected override async Task<Answer> FindAsync(int id, IQueryable<Answer> query)
        {
            Type type = typeof(Answer);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Answer)this.CacheService.GetById(cacheKey);

            var doc = await query.FirstOrDefaultAsync(x => x.AnswerId == id);
            this.CacheService.Create(cacheKey, doc);
            return doc;
        }

        protected override string SecurityEntities
        {
            get { return Rights.Answer.Name; }
        }

        protected override IQueryable<Answer> FilterCanView(IQueryable<Answer> query)
        {
            return query;
        }

        public void Save(Answer model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            if (model.AnswerId == 0)
            {
                var entity = new Answer();

                Add(entity, model);
                var modelQuestion = questionService.GetById(model.QuestionId);
                modelQuestion.LastUpdate = model.DateCreated;
                modelQuestion.Status = Status.Answered;
                questionService.Edit(modelQuestion);

            }
            else
            {
                var entity = new Answer { AnswerId = model.AnswerId };
                Edit(entity, model);
            }

            ClearCache();
        }

        public void Delete(int AnswerId, IBusinessErrorCollection errors)
        {
            if (AnswerId == 0)
            {
                throw new Exception("Incorrect entity id");
            }

            Delete(new Answer()
            {
                AnswerId = AnswerId
            });

            ClearCache();
        }

        private void ClearCache()
        {
            try
            {
                Type type = typeof(Answer);
                this.CacheService.DeleteByContaining(type.Name);
            }
            catch (Exception)
            {
            }
        }
    }
}
