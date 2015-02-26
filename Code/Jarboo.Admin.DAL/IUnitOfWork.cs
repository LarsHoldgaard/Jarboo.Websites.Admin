using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        IDbSet<Customer> Customers { get; set; }
        IDbSet<Project> Projects { get; set; }
        IDbSet<Documentation> Documentations { get; set; }
        IDbSet<Task> Tasks { get; set; }
        IDbSet<TaskStep> TaskSteps { get; set; }
        IDbSet<Employee> Employees { get; set; }
        IDbSet<EmployeePosition> EmployeePositions { get; set; }
        IDbSet<User> Users { get; set; }
        IDbSet<SpentTime> SpentTimes { get; set; }
        IDbSet<Quiz> Quizzes { get; set; }
        IDbSet<Comment> Comments { get; set; }
        IDbSet<Question> Questions { get; set; }
        IDbSet<Answer> Answers { get; set; }
        IDbSet<Setting> Settings { get; set; }
            
        DbContextTransaction BeginTransaction();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();
    }
}
