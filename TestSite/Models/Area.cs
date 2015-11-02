using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstateCompanyWebSite.Models
{
    /// <summary>
    /// Model of area
    /// </summary>
    [Serializable]
    public class Area
    {
        /// <summary>
        /// Id of area
        /// </summary>
        [JsonProperty]
        public int AreaId { get; set; }
        
        /// <summary>
        /// Name of area
        /// </summary>
        [JsonProperty]
        public string AreaName { get; set; }

        /// <summary>
        /// Longitude of area
        /// </summary>
        [JsonProperty]
        public float Longitude { get; set; }

        /// <summary>
        /// Latitude of area
        /// </summary>
        [JsonProperty]
        public float Lattitude{get; set;}
    }
}