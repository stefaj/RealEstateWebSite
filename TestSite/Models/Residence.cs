using AspNet.Identity.MySQL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TestSite.Models
{
    /// <summary>
    /// Model of residence in DB
    /// </summary>
    public class Residence
    {
        /// <summary>
        /// Id of property
        /// </summary>
        public int Id{get;  set;}

        /// <summary>
        /// Name of property
        /// </summary>
        public string Name { get;  set; }

        /// <summary>
        /// Street address of property
        /// </summary>
        public string StreetAddress { get;  set; }

        /// <summary>
        /// Number of bathrooms
        /// </summary>
        public int NoBathrooms { get;  set; }

        /// <summary>
        /// Number of bedrooms
        /// </summary>
        public int NoBedrooms { get;  set; }

        /// <summary>
        /// Plot area of property
        /// </summary>
        public int Area { get;  set; }

        /// <summary>
        /// Id of city in which property resides
        /// </summary>
        public int CityId { get;  set; }

        /// <summary>
        /// Id province in which property resides
        /// </summary>
        public int ProvinceId { get;  set; }

        /// <summary>
        /// Name of city in which property resides
        /// </summary>
        public string CityName { get;  set; }

        /// <summary>
        /// Name of property in which property resides
        /// </summary>
        public string ProvinceName { get;  set; }

        /// <summary>
        /// Postal code of property
        /// </summary>
        public string PostalCode { get;  set; }

        /// <summary>
        /// Short description of property
        /// </summary>
        public string Description { get;  set; }

        /// <summary>
        /// Longitude of property
        /// </summary>
        public float Longitude { get;  set; }

        /// <summary>
        /// Lattitude of property
        /// </summary>
        public float Lattitude { get;  set; }

        /// <summary>
        /// Main image of property
        /// </summary>
        public string ImagePreview { get;  set; }

        /// <summary>
        /// Price of property
        /// </summary>
        public float Price { get;  set; }

        /// <summary>
        /// Constructor for empty residence
        /// </summary>
        public Residence()
        {

        }

        /// <summary>
        /// Build residence from DB from given id
        /// </summary>
        /// <param name="id"></param>
        public  Residence(int id)
        {

            

            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Select * from reii422_estates_list, reii422_provinces, reii422_cities where reii422_estates_list.city_id = reii422_cities.city_id and reii422_provinces.province_id = reii422_cities.province_id and estate_id=@estate_id";
            command.Parameters.Add("@estate_id", id);

            MySqlDataReader reader = command.ExecuteReader();

            reader.Read();

            this.Id = id;
            this.Name = reader.GetString("name");
            this.StreetAddress = reader.GetString("street_address");
            this.NoBathrooms = reader.GetInt32("no_bathrooms");
            this.NoBedrooms = reader.GetInt32("no_bedrooms");
            this.Area = reader.GetInt32("area");
            this.CityId = reader.GetInt32("city_id");
          //  this.province = reader.GetString("province_name");// reader.GetString("province");
            this.PostalCode = reader.GetString("postal_code");
            this.Description = reader.GetString("description");
            this.Lattitude = reader.GetFloat("lattitude");
            this.Longitude = reader.GetFloat("longitude");
            this.ImagePreview = reader.GetString("preview_image");
            this.Price = reader.GetFloat("price");

            // to do
            // city name
            // province name

            reader.Close();
        }
    }
}