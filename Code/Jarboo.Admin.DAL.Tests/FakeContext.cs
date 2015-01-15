using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Collections;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.DAL.Tests
{
    public class FakeContext
    {
        private const string KeyId = "Id";

        public IUnitOfWork UnitOfWork { get; set; }
        private Dictionary<Type, IList> lists = new Dictionary<Type, IList>();

        private FakeContext()
        {
            UnitOfWork = A.Fake<IUnitOfWork>();

            ContextHelper.IterateDbSets(
                UnitOfWork,
                this.GetType().GetMethod("FakeSet", new Type[] { typeof(MemberExpression) }),
                this);
        }

        public void FakeSet<T>(MemberExpression exp)
            where T : class
        {
            var getter = Expression.Lambda<Func<IDbSet<T>>>(exp);
            FakeSet(getter);
        }
        public void FakeSet<T>(Expression<Func<IDbSet<T>>> getter)
            where T : class
        {
            var list = (List<T>)typeof(List<>)
              .MakeGenericType(typeof(T))
              .GetConstructor(Type.EmptyTypes)
              .Invoke(null);

            lists.Add(typeof(T), list);

            FakeSet(getter, list);
        }
        private void FakeSet<T>(Expression<Func<IDbSet<T>>> getter, List<T> data)
            where T: class
        {
            var queryableData = data.AsQueryable();

            var dbSet = A.Fake<IDbSet<T>>();
            A.CallTo(() => dbSet.Provider).Returns(queryableData.Provider);
            A.CallTo(() => dbSet.Expression).Returns(queryableData.Expression);
            A.CallTo(() => dbSet.ElementType).Returns(queryableData.ElementType);
            A.CallTo(() => dbSet.GetEnumerator()).ReturnsLazily(x => queryableData.GetEnumerator());
            A.CallTo(() => dbSet.Add(A<T>._)).ReturnsLazily(
                x =>
                    {
                        var entity = (T)x.Arguments[0];

                        data.Add(entity);
                        SetEntityKey(entity);
                        AddToRefEntities(entity, data.GetType());

                        return entity;
                    });

            A.CallTo(getter).Returns(dbSet);
        }

        private List<T> GetList<T>()
        {
            return (List<T>)GetList(typeof(T));
        }
        private IList GetList(Type type)
        {
            if (!lists.ContainsKey(type))
            {
                throw new Exception("Unknown entity - " + type.Name + ". All dbsets must be registered in the constructor");
            }
            var list = lists[type];
            return (IList)list;
        }

        private void SetEntityKey<T>(T entity)
        {
            var keyPropertyName = typeof(T).Name + KeyId;
            var keyProperty = typeof(T).GetProperty(keyPropertyName);
            if (keyProperty == null)
            {
                return;
            }

            var lastKey = 0;
            foreach (var ent in GetList<T>())
            {
                var keyValue = keyProperty.GetValue(ent);
                var intKeyValue = Convert.ToInt32(keyValue);
                if (lastKey < intKeyValue)
                {
                    lastKey = intKeyValue;
                }
            }

            keyProperty.SetValue(entity, lastKey + 1);
        }

        private void AddToRefEntities<T>(T entity, Type listType)
        {
            // Called with (Project, List<Project>)

            // Searching for peoperties with other entity's id references
            //   int(?) CustomerId {get; set;}
            var keyProperties = typeof(T).GetProperties()
                .Where(x => x.Name.EndsWith(KeyId))
                .Where(x => x.PropertyType == typeof(int) || x.PropertyType == typeof(int?));

            // Back reference from other entity - ProjectId
            //var backRefPropertyName = typeof(T).Name + KeyId;

            foreach (var keyProperty in keyProperties)
            {
                // Found Property CustomerId

                // Property that would reference other entity - Customer
                var refPropertyName = keyProperty.Name.Substring(0, keyProperty.Name.Length - KeyId.Length);
                var refProperty = typeof(T).GetProperty(refPropertyName);
                if (refProperty == null)
                {
                    continue;
                }

                // Customer
                var refEntity = refProperty.GetValue(entity);
                if (refEntity == null)
                {
                    continue;
                }

                // typeof(Customer)
                var refEntityType = refProperty.PropertyType;

                // Customer's CustomerId property
                var refEntityKeyProperty = refEntityType.GetProperty(keyProperty.Name);
                if (refEntityKeyProperty == null)
                {
                    continue;
                }

                // Set Project's CustomerId = Customer's Customer
                keyProperty.SetValue(entity, refEntityKeyProperty.GetValue(refEntity));

                // Customer's "List<Project> Projects" property
                // One to many relationship
                var refEntityListProperty = refEntityType.GetProperties().FirstOrDefault(x => x.PropertyType == listType);
                if (refEntityListProperty == null)
                {
                    continue;
                }

                // Add Project to Customer's Projects
                var refEntityList = (IList)refEntityListProperty.GetValue(refEntity, null);
                refEntityList.Add(entity);
            }
        }

        public static IUnitOfWork Create()
        {
            return new FakeContext().UnitOfWork;
        }
    }
}
