using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestSite.Models
{
    [Serializable]
    public class City
    {
        [JsonProperty]
        public int CityId { get; set; }
        
        [JsonProperty]
        public string CityName { get; set; }

        [JsonProperty]
        public float Longitude { get; set; }

        [JsonProperty]
        public float Lattitude{get; set;}
    }
}