using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TestSite
{
    public class Residence
    {
        public int Id{get;  set;}
        public string Name { get;  set; }
        public string StreetAddress { get;  set; }
        public int NoBathrooms { get;  set; }
        public int NoBedrooms { get;  set; }
        public int Area { get;  set; }
        public int CityId { get;  set; }
        public int ProvinceId { get;  set; }
        public string CityName { get;  set; }
        public string ProvinceName { get;  set; }
        public string PostalCode { get;  set; }
        public string Description { get;  set; }
        public float Longitude { get;  set; }
        public float Lattitude { get;  set; }
        public string ImagePreview { get;  set; }
        public float Price { get;  set; }
        public int EstateTypeId { get;  set; }
        public int ContractTypeId { get;  set; }
        public DateTime DateAdded { get;  set; }

        public Residence()
        {

        }

        public  Residence(int id)
        {
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Select * from reii422_estates_list where estate_id=@estate_id";
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
            //this.province = "FIX ME PLEASE";// reader.GetString("province");
            this.PostalCode = reader.GetString("postal_code");
            this.Description = reader.GetString("description");
            this.Lattitude = reader.GetFloat("lattitude");
            this.Longitude = reader.GetFloat("longitude");
            this.ImagePreview = reader.GetString("preview_image");
            this.Price = reader.GetFloat("price");
            this.EstateTypeId = reader.GetInt32("type_id");
            this.ContractTypeId = reader.GetInt32("contract_id");
            this.DateAdded = reader.GetDateTime("date_added");

            // to do
            // city name
            // province name

            reader.Close();
        }
    }
}