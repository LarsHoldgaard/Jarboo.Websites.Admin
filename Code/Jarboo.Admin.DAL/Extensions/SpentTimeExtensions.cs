using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.DAL.Extensions
{
    public static class SpentTimeExtensions
    {
        public static SpentTime ByIdMust(this IQueryable<SpentTime> query, int id)
        {
            var customer = query.ById(id);
            if (customer == null)
            {
                throw new Exception("Couldn't find SpentTime " + id);
            }
            return customer;
        }

        public static SpentTime ById(this IQueryable<SpentTime> query, int id)
        {
            return query.FirstOrDefault(x => x.SpentTimeId == id);
        }

        public static async Task<SpentTime> ByIdAsync(this IQueryable<SpentTime> query, int id)
        {
            return await query.FirstOrDefaultAsync(x => x.SpentTimeId == id);
        }
    }
}
