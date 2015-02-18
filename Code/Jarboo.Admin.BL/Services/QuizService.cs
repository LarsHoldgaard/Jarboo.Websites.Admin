using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.BL.Authorization;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.BL.Services.Interfaces;
using Jarboo.Admin.DAL;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Services
{
    public class QuizService : BaseEntityService<int, Quiz>, IQuizService
    {
        public QuizService(
            IUnitOfWork unitOfWork,
            IAuth auth,
            ICacheService cacheService)
            : base(unitOfWork, auth, cacheService)
        { }

        protected override System.Data.Entity.IDbSet<Quiz> Table
        {
            get { return UnitOfWork.Quizzes; }
        }

        protected override async Task<Quiz> FindAsync(int id, IQueryable<Quiz> query)
        {
            Type type = typeof(Quiz);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Quiz)this.CacheService.GetById(cacheKey);

            var customer = await query.FirstOrDefaultAsync(x => x.QuizId == id);
            this.CacheService.Create(cacheKey, customer);
            return customer;
        }

        protected override Quiz Find(int id, IQueryable<Quiz> query)
        {
            Type type = typeof(Quiz);
            var cacheKey = this.CacheService.GetCacheKey(type.Name + MethodBase.GetCurrentMethod().Name, id.ToString());
            if (this.CacheService.ContainsKey(cacheKey)) return (Quiz)this.CacheService.GetById(cacheKey);

            var customer = query.FirstOrDefault(x => x.QuizId == id);
            this.CacheService.Create(cacheKey, customer);
            return customer;
        }

        protected override string SecurityEntities
        {
            get { return Rights.Quizzes.Name; }
        }

        protected override IQueryable<Quiz> FilterCanView(IQueryable<Quiz> query)
        {
            return query.Where(x => x.Project.Tasks.Any(z => z.Steps.Any(a => a.EmployeeId == UserEmployeeId)));
        }

        public void Save(QuizEdit model, IBusinessErrorCollection errors)
        {
            if (!model.Validate(errors))
            {
                return;
            }

            if (model.QuizId == 0)
            {
                var entity = new Quiz();
                Add(entity, model);
            }
            else
            {
                var entity = new Quiz { QuizId = model.QuizId };
                Edit(entity, model);
            }

            ClearCache();
        }
        public void Delete(int id, IBusinessErrorCollection errors)
        {
            if (id == 0)
            {
                throw new Exception("Incorrect entity id");
            }

            Delete(new Quiz()
            {
                QuizId = id
            });

            ClearCache();
        }

        private void ClearCache()
        {
            try
            {
                Type type = typeof(Quiz);
                this.CacheService.DeleteByContaining(type.Name);
            }
            catch (Exception)
            {
            }
        }
    }
}
