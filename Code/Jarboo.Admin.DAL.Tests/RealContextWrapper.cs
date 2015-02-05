using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;

namespace Jarboo.Admin.DAL.Tests
{
    public interface IUnitOfWorkEx : IUnitOfWork
    {
        Context GetContext();
    }

    public class RealContextWrapper
    {
        public Context Context { get; set; }
        public IUnitOfWorkEx UnitOfWork { get; set; }

        public RealContextWrapper()
        {
            Context = CreateRalContext();
            UnitOfWork = A.Fake<IUnitOfWorkEx>();
            A.CallTo(() => UnitOfWork.GetContext()).Returns(Context);
            A.CallTo(() => UnitOfWork.SaveChanges()).ReturnsLazily(() => Context.SaveChanges());
            A.CallTo(() => UnitOfWork.Dispose()).Invokes(
                () =>
                    {
                        Context.Database.ExecuteSqlCommand("EXEC sp_msforeachtable \"ALTER TABLE ? NOCHECK CONSTRAINT all\"");
                        Context.Database.ExecuteSqlCommand("EXEC sp_MSForEachTable \"DELETE FROM ?\"");
                        Context.Database.ExecuteSqlCommand("exec sp_msforeachtable \"ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all\"");
                        Context.Dispose();
                    });

            ContextHelper.IterateDbSets(
                UnitOfWork,
                this.GetType().GetMethod("FakeSet", new Type[] { typeof(MemberExpression) }),
                this);
        }

        public void FakeSet<T>(MemberExpression exp)
            where T : class
        {
            var property = typeof(IUnitOfWork).GetProperties()
                .Where(x => x.PropertyType.IsGenericType)
                .First(x => x.PropertyType.GetGenericArguments()[0] == typeof(T));

            var getter = Expression.Lambda<Func<IDbSet<T>>>(exp);
            A.CallTo(getter).Returns((IDbSet<T>)property.GetValue(Context));
        }

        public static IUnitOfWork Create()
        {
            OneTimeInitialization();
            return new RealContextWrapper().UnitOfWork;
        }

        private static Context CreateRalContext()
        {
            return new Context();
        }
        /*private static DbConnection CreateDbConnection()
        {
            var sqlConnStringBuilder = new SqlConnectionStringBuilder();
            var sqlConnFact = new SqlConnectionFactory(sqlConnStringBuilder.ConnectionString);

            var sqlConnFact = new LocalDbConnectionFactory("v11.0");
            var sqlConn = sqlConnFact.CreateConnection("");
            return sqlConn;
        }*/

        private static bool inited = false;
        private static void OneTimeInitialization()
        {
            if (inited)
            {
                return;
            }
            inited = true;

            try
            {
                Database.SetInitializer(new DropCreateDatabaseAlways<Context>());
                using (var context = CreateRalContext())
                {
                    context.Database.Initialize(true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
