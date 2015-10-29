using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestSite.Models
{
    [Serializable]
    public class Agent
    {
        [JsonProperty]
        public int Agent_Id { get; set; }

        [JsonProperty]
        public string Agent_Name { get; set; }

        [JsonProperty]
        public string Agent_Surname { get; set; }

        [JsonProperty]
        public string Agent_Phone { get; set; }

        [JsonProperty]
        public string Agent_Email { get; set; }
    }
}