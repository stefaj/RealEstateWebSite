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
    /// <summary>
    /// This class poses a JSON API to get data from the database
    /// </summary>
    public class DataController : Controller
    {

        #region Helpers
        /// <summary>
        /// Returns all column entries for a specified table
        /// </summary>
        /// <param name="tableName">Name of table</param>
        /// <param name="columnName">Name of table to return</param>
        /// <returns>Returns columnName from tableName</returns>
        public static Dictionary<int, string> EnumerateTable(string id_column, string columnName, string tableName)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();

            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = string.Format("Select {0}, {1} from {2}", id_column, columnName, tableName);
            //command.Parameters.AddWithValue("@columnName", columnName);
            //command.Parameters.AddWithValue("@tableName", tableName);

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dic[reader.GetInt32(0)] = reader.GetString(1);

            }
            return dic;
        }

        public static Dictionary<int, string> GetEstateTypes()
        {
            return EnumerateTable("type_id", "type_name", "reii422_estates_type");
        }

        public static Dictionary<int, string> GetContracts()
        {
            return EnumerateTable("contract_id", "contract_name", "reii422_contracts");
        }

        public static Dictionary<int, string> GetProvinces()
        {
            return EnumerateTable("province_id", "province_name", "reii422_provinces");
        }

        /// <summary>
        /// Returns a list of provinces from the provinces table
        /// </summary>
        /// <returns></returns>
        public static string[] GetEstateTypesArr()
        {
            return EnumerateTable("type_id", "type_name", "reii422_estates_type").Values.ToArray();
        }

        public static Province[] GetProvincesArr()
        {
            List<Province> provinces = new List<Province>();
            var dic = EnumerateTable("province_id", "province_name", "reii422_provinces");
            foreach (var key in dic.Keys)
            {
                provinces.Add(new Province() { ProvinceId = key, ProvinceName = dic[key] });
            }
            return provinces.ToArray();
        }

        #endregion

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
            command.CommandText = "Select * from reii422_cities where province_id=@province_id order by city_name asc";
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

        public ActionResult Search()
        {
            var estateTypes = GetEstateTypes();
            var contracts = GetContracts();

            SearchQuery searchQuery = new SearchQuery(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString);

            // Getting ze post data ja

            System.Collections.Specialized.NameValueCollection postedValues = Request.Form;

            if (postedValues["SearchStr"] != null)
            {
                searchQuery.SetKeywords(postedValues["searchStr"]);
                ViewBag.SearchStr = postedValues["searchStr"];

            }
            if (postedValues["province"] != null && postedValues["province"] != "" && postedValues["province"] != "Any")
            {
                searchQuery.SetProvince(int.Parse(postedValues["province"]));
            }
            if (postedValues["city"] != null && postedValues["city"] != "" && postedValues["city"] != "Any")
                searchQuery.SetCity(int.Parse(postedValues["city"]));

            

            var res = searchQuery.GenerateResults();

            MySqlDataReader reader = res;

            List<Dictionary<string, string>> props = new List<Dictionary<string, string>>();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string location = reader.GetString(2);
                int no_bathrooms = reader.GetInt32(3);
                int no_bedrooms = reader.GetInt32(4);
                int area = reader.GetInt32("area");
                int city_id = reader.GetInt32("city_id");
                string city = reader.GetString("city_name");
                string province = reader.GetString("province_name");// reader.GetString("province");
                int estateTypeId = reader.GetInt32("type_id");
                int contractId = reader.GetInt32("contract_id");
                string image = reader.GetString("preview_image");
                float price = reader.GetFloat("price");


                string type = estateTypes[estateTypeId];
                string link = Url.Action("Residence", "Estates") + "?id=" + id;
                string imagePath = "/Assets/Images/Homes/" + image;
                string locationStr = string.Format("{0}, {1}", city, province);
                string contract = contracts[contractId];
                string priceStr = string.Format("R {0}", price);

                Dictionary<string, string> propDic = new Dictionary<string, string>();

                propDic["image"] = imagePath;
                propDic["link"] = link;
                propDic["street"] = location;
                propDic["city"] = locationStr;
                propDic["price"] = price.ToString();
                propDic["area"] = area.ToString();
                propDic["bedrooms"] = no_bedrooms.ToString();
                propDic["bathrooms"] = no_bathrooms.ToString();

                props.Add(propDic);
            }

            string json = JsonConvert.SerializeObject(props.ToArray());

            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(json);

            Response.End();

            reader.Close();
            searchQuery.Close();
            return View();
        }
	}
}