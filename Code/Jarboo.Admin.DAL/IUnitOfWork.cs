using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.DAL
{
    using System.Data.Entity;
    using Jarboo.Admin.DAL.Entities;

    public interface IUnitOfWork : IDisposable
    {
        IDbSet<Customer> Customers { get; set; }
        IDbSet<Project> Projects { get; set; }
        IDbSet<Documentation> Documentations { get; set; }
        IDbSet<Task> Tasks { get; set; }

        int SaveChanges();
    }
}
