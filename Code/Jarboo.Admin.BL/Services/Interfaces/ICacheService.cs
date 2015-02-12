using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Services.Interfaces
{
    public interface ICacheService
    {
        object GetById(string cacheKey);
        object Create(string cacheKey, object obj);
        object Create(string cacheKey, object obj, DateTime expire);
        void Delete(string cacheKey);
        void DeleteByContaining(string containing);
        bool ContainsKey(string cacheKey);
        string GetCacheKey(string methodName, string value);
    }
}
