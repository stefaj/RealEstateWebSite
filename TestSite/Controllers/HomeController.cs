using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Text;
using RealEstateCompanyWebSite.Models;

namespace RealEstateCompanyWebSite.Controllers
{
    /// <summary>
    /// Controls output for home page
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Returns the index view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
         
            return View();
        }

        /// <summary>
        /// Returns the about view
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Message = "REII 422";

            return View();
        }

        /// <summary>
        /// Returns the contact view and lists all agents
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Contact us";

            List<Agent> agents = new List<Agent>();

            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Select Agent_Id, Agent_Name, Agent_Surname, Agent_Phone, Agent_Email, Agent_Description from Agent";

            MySqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                Agent agent = new Agent()
                {
                    Agent_Email = reader.GetString("Agent_Email"),
                    Agent_Id = reader.GetInt32("Agent_Id"),
                    Agent_Name = reader.GetString("Agent_Name"),
                    Agent_Phone = reader.GetString("Agent_Phone"),
                    Agent_Surname = reader.GetString("Agent_Surname"),
                    Agent_Description = reader.GetString("Agent_Description")
                };

                agents.Add(agent);
            }

            ViewBag.Agents = agents.ToArray();
            

            
            return View();
        
        }

    }
}