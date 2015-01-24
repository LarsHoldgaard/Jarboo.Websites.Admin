using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DataTables.Mvc;

using Jarboo.Admin.BL.Sorters;

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

        public static IEnumerable<Tuple<TEnum, SortDirection>> Sortings<TEnum>(this IDataTablesRequest request)
        {
            foreach (var sortedColumn in request.Columns.GetSortedColumns())
            {
                int columnIndex;
                if (!int.TryParse(sortedColumn.Data, out columnIndex))
                {
                    continue;
                }
                var column = (TEnum)Enum.ToObject(typeof(TEnum), columnIndex);

                var direction = sortedColumn.SortDirection == Column.OrderDirection.Ascendant ? SortDirection.Ascendant : SortDirection.Descendant;

                yield return Tuple.Create(column, direction);
            }
        }
    }
}