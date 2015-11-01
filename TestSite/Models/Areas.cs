using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestSite.Models
{
    [Serializable]
    public class Area
    {
        [JsonProperty]
        public int AreaId { get; set; }
        
        [JsonProperty]
        public string AreaName { get; set; }

        [JsonProperty]
        public float Longitude { get; set; }

        [JsonProperty]
        public float Lattitude{get; set;}
    }
}