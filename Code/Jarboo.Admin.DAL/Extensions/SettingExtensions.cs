using System;
using System.Linq;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.DAL.Extensions
{
    public static class SettingExtensions
    {
        public static Setting ByIdMust(this IQueryable<Setting> query, int id)
        {
            var setting = query.ById(id);
            if (setting == null)
            {
                throw new Exception("Couldn't find setting " + id);
            }
            return setting;
        }
        public static Setting ById(this IQueryable<Setting> query, int id)
        {
            return query.FirstOrDefault(x => x.Id == id);
        }
    }
}
