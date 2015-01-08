using System;
using System.Data.Entity;
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

        //DbContextTransaction BeginTransaction();

        int SaveChanges();
    }
}
