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
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string location = reader.GetString(2);
                int no_bathrooms = reader.GetInt32(3);
                int no_bedrooms = reader.GetInt32(4);
                int area = reader.GetInt32(5);

                string namehtml = string.Format("<a href=\"{0}\">{1}</a>", Url.Action("Residence", "Home") + "?id=" + id, name);

                builder.AddRow(namehtml, location, no_bathrooms.ToString(), no_bedrooms.ToString(), area.ToString());
            }

            ViewBag.Message = builder.ToHTML();          

            return View();
        }

        public ActionResult Residence()
        {
            string id = Request.QueryString["id"];
            if (id != null)
            {

            }
            else
                return View();


            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Select name,street_address,no_bathrooms,no_bedrooms,area,city,province,postal_code,description,lattitude,longitude from EstatesList where estate_id=" + id;

            MySqlDataReader reader = command.ExecuteReader();

            reader.Read();
            
            ViewBag.name = reader.GetString(0);
            ViewBag.street_address = reader.GetString(1);
            ViewBag.no_bathrooms = reader.GetInt32(2);
            ViewBag.no_bedrooms = reader.GetInt32(3);
            ViewBag.area = reader.GetInt32(4);
            ViewBag.city = reader.GetString(5);
            ViewBag.province = reader.GetString(6);
            ViewBag.postal_code = reader.GetString(7);
            ViewBag.description = reader.GetString(8);
            ViewBag.lattitude = reader.GetFloat(9);
            ViewBag.longitude = reader.GetFloat(10);

            ViewBag.Message = "Your contact page.";


            ViewBag.StreetView = string.Format("https://www.google.com/maps/embed/v1/streetview?location={0}%2C{1}&key={2}", ViewBag.lattitude, ViewBag.longitude, "AIzaSyDlnvQdhrcHR6dv2UyyVmzWT_pzaPcmwTo");


            return View();
        }
    }
}