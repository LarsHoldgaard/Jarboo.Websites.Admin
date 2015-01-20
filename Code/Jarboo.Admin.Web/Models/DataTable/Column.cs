using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace Jarboo.Admin.Web.Models.DataTable
{
    public class Column<T> : DataTableConfig.Column
    {
        [JsonIgnore]
        public Func<T, object> Getter { get; set; }
    }

    public static class ColumnExtensions
    {
        public static List<List<object>> ExtractRows<T>(this List<Column<T>> columns, IEnumerable<T> data)
        {
            return data.Select(t => columns.Select(x => x.Getter(t)).ToList()).ToList();
        }
    }
}