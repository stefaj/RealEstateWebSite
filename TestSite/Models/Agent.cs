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

        [JsonProperty]
        public string Agent_Description { get; set; }

        
        [JsonProperty]
        public string Image_URL { get; set; }

        [JsonProperty]
        public string Agent_FullName
        {
            get
            {
                return Agent_Name + " " + Agent_Surname;
            }
        }

        [JsonProperty]
        public string Agent_PhoneString
        {
            get
            {
                string phone = Agent_Phone;
                if (phone.Length < 10)
                    return phone;
                string newPhone = phone[0] + phone[1] + phone[2] + " " + phone[3] + phone[4] + phone[5] + " " + phone[6] + phone[7] + phone[8] + phone[9];
                return newPhone;

            }
        }
    }
}