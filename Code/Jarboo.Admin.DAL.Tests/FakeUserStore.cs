using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

using Microsoft.AspNet.Identity;

namespace Jarboo.Admin.DAL.Tests
{
    public class FakeUserStore : IUserStore<User, string>
    {
        public IUnitOfWork Context { get; set; }

        public FakeUserStore(IUnitOfWork context)
        {
            Context = context;
        }

        public System.Threading.Tasks.Task CreateAsync(User user)
        {
            return System.Threading.Tasks.Task.Run(
                () =>
                    {
                        Context.Users.Add(user);
                        Context.SaveChanges();
                    });
        }

        public System.Threading.Tasks.Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(string userId)
        {
            var user = Context.Users.FirstOrDefault(x => x.Id == userId);
            return System.Threading.Tasks.Task.FromResult(user);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            var user = Context.Users.FirstOrDefault(x => x.UserName == userName);
            return System.Threading.Tasks.Task.FromResult(user);
        }

        public System.Threading.Tasks.Task UpdateAsync(User user)
        {
            return System.Threading.Tasks.Task.Delay(0);
        }

        public void Dispose()
        { }
    }
}
