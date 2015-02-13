using Jarboo.Admin.BL.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Jarboo.Admin.BL.Services
{
    public class HttpCacheService : ICacheService
    {
        private static HttpCacheService instance;
        private static object syncObject = new object();
        public static HttpCacheService Instance
        {
            get
            {
                lock (syncObject)
                {
                    if (instance == null)
                    {
                        instance = new HttpCacheService();
                    }
                }

                return instance;
            }
        }

        public object GetById(string cacheKey)
        {
            if (ContainsKey(cacheKey))
            {
                return HttpContext.Current.Cache[cacheKey];
            }

            return null;
        }

        public object Create(string cacheKey, object obj)
        {
            return Create(cacheKey, obj, DateTime.UtcNow.AddHours(5));
        }

        public object Create(string cacheKey, object obj, DateTime expire)
        {
            if (!ContainsKey(cacheKey))
            {
                HttpContext.Current.Cache.Insert(cacheKey,
                    obj,
                    null,
                    expire,
                    Cache.NoSlidingExpiration);
            }
            return GetById(cacheKey);
        }

        public void Delete(string cacheKey)
        {
            HttpContext.Current.Cache.Remove(cacheKey);
        }

        public void DeleteByContaining(string containing)
        {
            if (string.IsNullOrEmpty(containing)) return;

            List<string> deleteList = new List<string>();
            HttpContext oc = HttpContext.Current;

            // find all cache keys in the system... maybe insane? I don't know lol
            IDictionaryEnumerator en = oc.Cache.GetEnumerator();
            while (en.MoveNext())
            {
                var k = en.Key.ToString();
                if (k.Contains(containing))
                {
                    deleteList.Add(k);
                }
            }

            foreach (var del in deleteList)
            {
                Delete(del);
            }
        }

        public bool ContainsKey(string cacheKey)
        {
            bool useCache = bool.Parse(ConfigurationManager.AppSettings["UseCache"]);
            if (!useCache) return false;

            return HttpContext.Current.Cache[cacheKey] != null;
        }

        public string GetCacheKey(string methodName, string value)
        {
            return string.Format("{0}_{1}", methodName, value);
        }
    
    }
}
