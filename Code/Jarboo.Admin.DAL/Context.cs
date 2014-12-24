using System.Data.Entity;
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

        public IDbSet<Customer> Customers { get; set; }
        public IDbSet<Project> Projects { get; set; }
        public IDbSet<Documentation> Documentations { get; set; }
        public IDbSet<Task> Tasks { get; set; }
        public IDbSet<Employee> Employees { get; set; }
        public IDbSet<EmployeePosition> EmployeePositions { get; set; }


        public DbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }
    }
}
