using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace TestSite.Controllers
{
    /// <summary>
    /// Controls output for home page
    /// </summary>
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        
        }

        public ActionResult Search()
        {
            TableBuilder builder = new TableBuilder();

            builder.SetHeaderTitle("name", "location", "bathrooms", "bedrooms", "area");

            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Select * from EstatesList";

            MySqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                string name = reader.GetString(1);
                string location = reader.GetString(2);
                int no_bathrooms = reader.GetInt32(3);
                int no_bedrooms = reader.GetInt32(4);
                int area = reader.GetInt32(5);

                builder.AddRow(name, location, no_bathrooms.ToString(), no_bedrooms.ToString(), area.ToString());
            }

            ViewBag.Message = builder.ToHTML();          

            return View();
        }
    }
}