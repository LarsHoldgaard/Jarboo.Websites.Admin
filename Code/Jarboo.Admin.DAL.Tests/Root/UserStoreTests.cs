using System;
using System.Linq;

using Jarboo.Admin.DAL.Entities;

using NUnit.Framework;

namespace Jarboo.Admin.DAL.Tests.Root
{
    [TestFixture]
    public class UserStoreTests
    {
        [Test]
        public void CreateAsync_Always_CreatesUser()
        {
            using (var context = ContextHelper.Create())
            {
                var userStore = context.CreateUserStore();

                var id = Guid.NewGuid().ToString();
                var user = new User() { Id = id, UserName = id, DisplayName = id };


                userStore.CreateAsync(user).GetAwaiter().GetResult();
                var userFromDb = context.Users.FirstOrDefault(x => x.Id == id);


                Assert.NotNull(userFromDb);
                Assert.AreEqual(id, userFromDb.Id);
            }
        }

        [Test]
        public void FindByIdAsync_Always_FindExisting()
        {
            using (var context = ContextHelper.Create())
            {
                var userStore = context.CreateUserStore();

                var id = Guid.NewGuid().ToString();
                var user = new User() { Id = id, UserName = id, DisplayName = id };

                userStore.CreateAsync(user).GetAwaiter().GetResult();


                var userFromDb = userStore.FindByIdAsync(id).GetAwaiter().GetResult();


                Assert.NotNull(userFromDb);
                Assert.AreEqual(id, userFromDb.Id);
            }
        }

        [Test]
        public void FindByNameAsync_Always_FindExisting()
        {
            using (var context = ContextHelper.Create())
            {
                var userStore = context.CreateUserStore();

                var id = Guid.NewGuid().ToString();
                var userName = "UserName";
                var user = new User() { Id = id, UserName = userName, DisplayName = id };

                userStore.CreateAsync(user).GetAwaiter().GetResult();


                var userFromDb = userStore.FindByNameAsync(userName).GetAwaiter().GetResult();


                Assert.NotNull(userFromDb);
                Assert.AreEqual(id, userFromDb.Id);
            }
        }

        [Test]
        public void UpdateAsync_Always_CreatesUser()
        {
            using (var context = ContextHelper.Create())
            {
                var userStore = context.CreateUserStore();

                var id = Guid.NewGuid().ToString();
                var user = new User() { Id = id, UserName = id, DisplayName = id };

                userStore.CreateAsync(user).GetAwaiter().GetResult();

                var userName = "UserName";
                user.UserName = userName;


                userStore.UpdateAsync(user).GetAwaiter().GetResult();
                var userFromDb = userStore.FindByIdAsync(id).GetAwaiter().GetResult();


                Assert.NotNull(userFromDb);
                Assert.AreEqual(userName, userFromDb.UserName);
            }
        }
    }
}
