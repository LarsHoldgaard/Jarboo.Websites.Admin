using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

using Jarboo.Admin.DAL.Entities;

using Microsoft.AspNet.Identity.EntityFramework;

namespace Jarboo.Admin.DAL
{
    public class Context : IdentityDbContext<User>, IUnitOfWork
    {
        public Context()
            : this("name=Jarboo.Admin.DAL.Context")
        { }
        public Context(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            if (entityEntry.State != EntityState.Modified)
            {
                return base.ValidateEntity(entityEntry, items);
            }

            var errors = new List<DbValidationError>();
            foreach (var propertyName in entityEntry.OriginalValues.PropertyNames)
            {
                var property = entityEntry.Property(propertyName);
                if (property == null || !property.IsModified)
                {
                    continue;
                }

                errors.AddRange(property.GetValidationErrors());
            }

            return new DbEntityValidationResult(entityEntry, errors);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOptional(x => x.Customer)
                .WithOptionalDependent(x => x.User)
                .Map(x => x.MapKey("CustomerId"));

            modelBuilder.Entity<User>()
                .HasOptional(x => x.Employee)
                .WithOptionalDependent(x => x.User)
                .Map(x => x.MapKey("EmployeeId"));

            base.OnModelCreating(modelBuilder);
        }

        public IDbSet<Customer> Customers { get; set; }
        public IDbSet<Project> Projects { get; set; }
        public IDbSet<Documentation> Documentations { get; set; }
        public IDbSet<Task> Tasks { get; set; }
        public IDbSet<TaskStep> TaskSteps { get; set; }
        public IDbSet<Employee> Employees { get; set; }
        public IDbSet<EmployeePosition> EmployeePositions { get; set; }
        public IDbSet<SpentTime> SpentTimes { get; set; }
        public IDbSet<Comment> Comments { get; set; }
        public IDbSet<Question> Questions { get; set; }
        public IDbSet<Answer> Answers { get; set; }

        public IDbSet<Setting> Settings { get; set; }

        public IDbSet<Quiz> Quizzes { get; set; }

        public DbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }
    }
}
