using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.DAL
{
    public class Context : DbContext, IUnitOfWork
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

        public IDbSet<Customer> Customers { get; set; }
        public IDbSet<Project> Projects { get; set; }
        public IDbSet<Documentation> Documentations { get; set; }
        public IDbSet<Task> Tasks { get; set; }
        public IDbSet<TaskStep> TaskSteps { get; set; }
        public IDbSet<Employee> Employees { get; set; }
        public IDbSet<EmployeePosition> EmployeePositions { get; set; }

        public DbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }
    }
}
