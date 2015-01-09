using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Collections;

namespace Jarboo.Admin.DAL.Tests
{
    public class FakeContext
    {
        private const string KeyId = "Id";
        public const int DefaultId = 1;

        public IUnitOfWork UnitOfWork { get; set; }
        private Dictionary<Type, IList> lists = new Dictionary<Type, IList>();

        public FakeContext()
        {
            UnitOfWork = A.Fake<IUnitOfWork>();
            FakeSet(() => UnitOfWork.Customers);
            FakeSet(() => UnitOfWork.Projects);
            FakeSet(() => UnitOfWork.Documentations);
            FakeSet(() => UnitOfWork.Tasks);
            FakeSet(() => UnitOfWork.TaskSteps);
            FakeSet(() => UnitOfWork.Employees);
            FakeSet(() => UnitOfWork.EmployeePositions);
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

            A.CallTo(getter).Returns(dbSet);
        }

        private List<T> GetList<T>()
        {
            if(!lists.ContainsKey(typeof(T)))
            {
                throw new Exception("Unknown entity - " + typeof(T).Name + ". All dbsets must be registered in the constructor");
            }
            var list = lists[typeof(T)];
            return (List<T>)list;
        }

        public FakeContext Add<T>(T entity)
        {
            var getListsMethod = typeof(FakeContext).GetMethod("GetList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var list = GetList<T>();
            list.Add(entity);

            var keyProperties = typeof(T).GetProperties()
                .Where(x => x.Name.EndsWith(KeyId))
                .Where(x => x.PropertyType == typeof(int) || x.PropertyType == typeof(int?));

            foreach (var keyProperty in keyProperties)
            {
                var refPropertyName = keyProperty.Name.Substring(0, keyProperty.Name.Length - KeyId.Length);
                var refProperty = typeof(T).GetProperty(refPropertyName);
                if (refProperty == null)
                {
                    continue;
                }

                var keyValue = keyProperty.GetValue(entity);
                if (keyValue == null)
                {
                    continue;
                }

                var refPropertyType = refProperty.PropertyType;
                var refEntityKeyProperty = refPropertyType.GetProperty(keyProperty.Name);
                if (refEntityKeyProperty == null)
                {
                    continue;
                }

                var refEntityListProperty = refPropertyType.GetProperties().FirstOrDefault(x => x.PropertyType == list.GetType());
                if (refEntityListProperty == null)
                {
                    continue;
                }

                var refEntities = (IList)getListsMethod.MakeGenericMethod(refPropertyType).Invoke(this, null);
                foreach (var refEntity in refEntities)
                {
                    var refEntityKeyValue = refEntityKeyProperty.GetValue(refEntity);
                    if (refEntityKeyValue == null || !refEntityKeyValue.Equals(keyValue))
                    {
                        continue;
                    }

                    var refEntityList = (IList)refEntityListProperty.GetValue(refEntity, null);
                    refEntityList.Add(entity);

                    refProperty.SetValue(entity, refEntity);
                }
            }

            return this;
        }
    }
}
