using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestSite.Models;

namespace TestSite.Controllers
{
    public class DataController : Controller
    {
        //
        // GET: /Data/
        public ActionResult Cities()
        {
            List<City> cities = new List<City>();

            var post = Request.Form;
            var get = Request.QueryString;

            string province_id = post["province_id"];

            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Select * from reii422_cities where province_id=@province_id";
            command.Parameters.AddWithValue("@province_id", province_id);

            MySqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                int city_id = reader.GetInt32("city_id");
                string city_name = reader.GetString("city_name");
                float longitude = reader.GetFloat("longitude");
                float lattitude = reader.GetFloat("lattitude");

                cities.Add(new City() { Longitude = longitude, CityId = city_id, CityName = city_name, Lattitude = lattitude });


            }

            string json = JsonConvert.SerializeObject(cities.ToArray());

            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(json);

            Response.End();

            return View();
        }
	}
}