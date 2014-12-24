using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Google.Apis.Util.Store;

namespace Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration
{
    public class MemoryDataStore : IDataStore
    {
        private static readonly Lazy<MemoryDataStore> instance = new Lazy<MemoryDataStore>(() => new MemoryDataStore());
        public static MemoryDataStore Instance
        {
            get
            {
                return instance.Value;
            }
        }

        private MemoryDataStore()
        {
            
        }

        private static readonly ConcurrentDictionary<string, object> store = new ConcurrentDictionary<string, object>();

        public async System.Threading.Tasks.Task ClearAsync()
        {
            store.Clear();
        }

        public async System.Threading.Tasks.Task DeleteAsync<T>(string key)
        {
            object o;
            store.TryRemove(key, out o);
        }

        public async System.Threading.Tasks.Task<T> GetAsync<T>(string key)
        {
            if (store.ContainsKey(key))
            {
                return (T)store[key];
            }

            return default(T);
        }

        public async System.Threading.Tasks.Task StoreAsync<T>(string key, T value)
        {
            store[key] = value;
        }
    }
}