using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestSite.Models
{

    /// <summary>
    /// Model of city
    /// </summary>
    [Serializable]
    public class City
    {
        /// <summary>
        /// Id of city
        /// </summary>
        [JsonProperty]
        public int CityId { get; set; }
        

        /// <summary>
        /// Name of city
        /// </summary>
        [JsonProperty]
        public string CityName { get; set; }

        /// <summary>
        /// Longitude of city
        /// </summary>
        [JsonProperty]
        public float Longitude { get; set; }

        /// <summary>
        /// Latitude of city
        /// </summary>
        [JsonProperty]
        public float Lattitude{get; set;}
    }
}