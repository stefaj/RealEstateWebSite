using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstateCompanyWebSite.Models
{
    /// <summary>
    /// Model of agent
    /// </summary>
    [Serializable]
    public class Agent
    {
        /// <summary>
        /// Id of agent
        /// </summary>
        [JsonProperty]
        public int Agent_Id { get; set; }

        /// <summary>
        /// First name of agent
        /// </summary>
        [JsonProperty]
        public string Agent_Name { get; set; }

        /// <summary>
        /// Last name of agent
        /// </summary>
        [JsonProperty]
        public string Agent_Surname { get; set; }

        /// <summary>
        /// Main phone number of agent
        /// </summary>
        [JsonProperty]
        public string Agent_Phone { get; set; }

        /// <summary>
        /// Email of agent
        /// </summary>
        [JsonProperty]
        public string Agent_Email { get; set; }

        /// <summary>
        /// Short description of agent
        /// </summary>
        [JsonProperty]
        public string Agent_Description { get; set; }

        /// <summary>
        /// Profile image of agent
        /// </summary>
        [JsonProperty]
        public string Image_URL { get; set; }

        /// <summary>
        /// Full name of agent (includes first and last name)
        /// </summary>
        [JsonProperty]
        public string Agent_FullName
        {
            get
            {
                return Agent_Name + " " + Agent_Surname;
            }
        }

        /// <summary>
        /// Readable phone number of agent
        /// </summary>
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