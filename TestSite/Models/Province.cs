using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestSite.Models
{
    /// <summary>
    /// Model of province
    /// </summary>
    [Serializable]
    public class Province
    {
        /// <summary>
        /// Id of province
        /// </summary>
        [JsonProperty]
        public int ProvinceId { get; set; }


        /// <summary>
        /// Name of province
        /// </summary>
        [JsonProperty]
        public string ProvinceName { get; set; }
    }
}