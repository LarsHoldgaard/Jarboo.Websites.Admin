using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Jarboo.Admin.Web.Models.DataTable
{
    public class DataTableConfig
    {
        public class AjaxConfig
        {
            [JsonProperty("url")]
            public string Url { get; set; }
            [JsonProperty("type")]
            [JsonConverter(typeof(StringEnumConverter))]
            public FormMethod Method { get; set; }
        }

        public class Column
        {
            public Column()
            {
                Visible = true;
            }

            public enum ColumnSpecialType
            {
                TaskLink,
                TaskStepLink,
                ProjectLink,
                ExternalLink,
                DeleteBtn
            }

            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("visible")]
            public bool Visible { get; set; }
            [JsonProperty("type")]
            [JsonConverter(typeof(StringEnumConverter))]
            public ColumnSpecialType? Type { get; set; }
        }

        public DataTableConfig()
        {
            Order = new ArrayList();
            Searching = true;
        }

        [JsonProperty("serverSide")]
        public bool ServerSide { get; set; }
        [JsonProperty("ajax")]
        public AjaxConfig Ajax { get; set; }
        [JsonProperty("order")]
        public ArrayList Order { get; set; }
        [JsonProperty("columns")]
        public List<Column> Columns { get; set; }
        [JsonProperty("searching")]
        public bool Searching { get; set; }

        public void AddOrder(int column, DataTables.Mvc.Column.OrderDirection direction)
        {
            this.Order.Add(column);
            this.Order.Add(direction == DataTables.Mvc.Column.OrderDirection.Ascendant ? "asc" : "desc");
        }

        public void SetupServerDataSource(string url, FormMethod method)
        {
            this.ServerSide = true;
            this.Ajax = new AjaxConfig()
                       {
                           Url = url,
                           Method = method
                       };
        }
    }
}