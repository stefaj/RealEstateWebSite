using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestSite.Models
{
    [Serializable]
    public class Province
    {
        [JsonProperty]
        public int ProvinceId { get; set; }

        [JsonProperty]
        public string ProvinceName { get; set; }
    }
}