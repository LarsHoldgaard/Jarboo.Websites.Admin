using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jarboo.Admin.Web.Models.Morris
{
    public class MorrisConfig
    {
        [JsonProperty("data")]
        public IEnumerable Data { get; set; }
        [JsonProperty("xkey")]
        public string XKey { get; set; }
        [JsonProperty("ykeys")]
        public string[] YKeys { get; set; }
        [JsonProperty("labels")]
        public string[] Labels { get; set; }
    }
}