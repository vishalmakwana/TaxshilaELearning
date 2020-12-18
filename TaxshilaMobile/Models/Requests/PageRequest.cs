using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.Models
{
    public class PageRequest
    {
        [JsonProperty("Pagenumber")]
        public string PageNumber { get; set; } = "1";

        [JsonProperty("Pageindex")]
        public string PageIndex { get; set; } = "1";
        [JsonProperty("Pagesize")]
        public string PageSize { get; set; }
    }
}
