using AspNet.Identity.MySQL;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using TestSite.Email;
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

            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
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


        // Functionality removed due to new database design
        /*public static Dictionary<int, string> GetEstateTypes()
        {
            return EnumerateTable("type_id", "type_name", "reii422_estates_type");
        }

        public static Dictionary<int, string> GetContracts()
        {
            return EnumerateTable("contract_id", "contract_name", "reii422_contracts");
        }
        
        public static string[] GetEstateTypesArr()
        {
            return EnumerateTable("type_id", "type_name", "reii422_estates_type").Values.ToArray();
        }
         */

        /// <summary>
        /// Returns a dictionary of provinces
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetProvinces()
        {
            return EnumerateTable("province_id", "province_name", "Province");
        }



        /// <summary>
        /// Returns a list of provinces from the provinces table
        /// </summary>
        /// <returns></returns>
        public static Province[] GetProvincesArr()
        {
            List<Province> provinces = new List<Province>();
            var dic = EnumerateTable("province_id", "province_name", "Province");
            foreach (var key in dic.Keys)
            {
                provinces.Add(new Province() { ProvinceId = key, ProvinceName = dic[key] });
            }
            return provinces.ToArray();
        }

        #endregion

        /// <summary>
        /// Returns all cities for a given provinces as a JSON array
        /// </summary>
        /// <returns></returns>
        public ActionResult Cities()
        {
            List<City> cities = new List<City>();

            var post = Request.Form;
            var get = Request.QueryString;

            string province_id = post["province_id"];

            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Select * from City, Area where City_Province_Id=@province_id and Area_City_ID=City_ID order by city_name asc";
            command.Parameters.AddWithValue("@province_id", province_id);

            MySqlDataReader reader = null;

            try
            {
                reader = command.ExecuteReader();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            while(reader.Read())
            {
                int city_id = reader.GetInt32("city_id");
                string city_name = reader.GetString("city_name");
                float longitude = reader.GetFloat("City_Longitude");
                float lattitude = reader.GetFloat("City_Latitude");
              //  float longitude = 0;
              //  float lattitude = 0;

                cities.Add(new City() { Longitude = longitude, CityId = city_id, CityName = city_name, Lattitude = lattitude });


            }

            string json = JsonConvert.SerializeObject(cities.ToArray());

            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(json);

            Response.End();

            return View();
        }



       /// <summary>
       /// Returns all areas for a given city as a JSON array
       /// </summary>
       /// <returns></returns>
        public ActionResult Areas()
        {
            List<Area> areas = new List<Area>();

            var post = Request.Form;
            var get = Request.QueryString;

            string city_id = post["city_id"];

            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Select * from Area where Area_City_ID=@city_id order by Area_Name asc";
            command.Parameters.AddWithValue("@city_id", city_id);

            MySqlDataReader reader = null;

            try
            {
                reader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            while (reader.Read())
            {
                int area_id = reader.GetInt32("Area_ID");
                string area_name = reader.GetString("Area_Name");
                // float longitude = reader.GetFloat("longitude");
                // float lattitude = reader.GetFloat("lattitude");
                  float longitude = 0;
                  float lattitude = 0;

                  areas.Add(new Area() { Longitude = longitude, AreaId = area_id, AreaName = area_name, Lattitude = lattitude });


            }

            string json = JsonConvert.SerializeObject(areas.ToArray());

            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(json);

            Response.End();

            return View();
        }


        /// <summary>
        /// Calling this will send an email for the interested buyer to an agent with the parameters specified in post
        /// </summary>
        /// <returns></returns>
        public ActionResult SendEmail()
        {
            var post = Request.Form;

            int agent_id = int.Parse(post["agent_id"]);

            string client_msg = post["body"]; // todo escape string en check security

            //int property_id = int.Parse(post["property_id"]);
            int listing_id = int.Parse(post["listing_id"]);

            string client_name = post["client_name"];
            string client_phone = post["client_phone"];
            string client_email = post["client_email"];

            string link = Url.Action("Residence", "Estates") + "?id=" + listing_id;
            string subject = string.Format("Interest in " + listing_id);
            
            string body = string.Format("<html>Interest in <a href=\"{0}\">listing {1}</a><br><h3>Interested buyer</h3>", link, listing_id);
            body += "<h2>" + client_name + "</h2>";


            body += string.Format("<a href=\"{0}\" mailto>email: {0} </p>", client_email);
            body += "</br><p>phone number: " + client_phone + "</p>";
            body += "</br><h3>Client Message: </h3>";
            body += "</br><p>" + client_msg + "</p></html>";


            // Read agent email
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Select Agent_Name, Agent_Surname, Agent_Phone, Agent_Email from Agent where Agent_Id=@agent_id";
            command.Parameters.AddWithValue("@agent_id", agent_id);

            MySqlDataReader reader = command.ExecuteReader();
            
            reader.Read();
            if(!reader.HasRows)
            {
                return null;
            }
            
            string agent_email = reader.GetString("Agent_Email");

            string from = ConfigurationManager.AppSettings["webEmail"];

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage(from, agent_email);
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;
                   
            StandardSMTPEmailer emailer = new StandardSMTPEmailer();
            emailer.Send(msg);

            return Redirect(Request.UrlReferrer.ToString());

        }

        /// <summary>
        /// Calling this will send an email to a specified agent for a general query
        /// </summary>
        /// <returns></returns>
        public ActionResult SendEmailContact()
        {
            var post = Request.Form;

            int agent_id = int.Parse(post["agent_id"]);

            string client_msg = post["body"]; // todo escape string en check security

            string client_name = post["client_name"];
            string client_phone = post["client_phone"];
            string client_email = post["client_email"];

            string subject = "General query ";

            
            string body = "<html><h3>Curious Client</h3>";
            body += "<h2>" + client_name + "</h2>";
            body += "<p>email: " + client_email + "</p>";
            body += "</br><p>phone number: " + client_phone + "</p>";
            body += "</br><h3>Client Message: </h3>";
            body += "</br><p>" + client_msg + "</p></html>";


            // Read agent email
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connection.Open();


            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Select Agent_Name, Agent_Surname, Agent_Phone, Agent_Email from Agent where Agent_Id=@agent_id";
            command.Parameters.AddWithValue("@agent_id", agent_id);

            MySqlDataReader reader = command.ExecuteReader();

            reader.Read();

            string agent_email = reader.GetString("Agent_Email");

            string from = ConfigurationManager.AppSettings["webEmail"];

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage(from, agent_email);
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;

            StandardSMTPEmailer emailer = new StandardSMTPEmailer();
            emailer.Send(msg);

            return Redirect(Request.UrlReferrer.ToString());

        }

        /// <summary>
        /// Searches for properties based on features, location and keywords that are given via post parameters.
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {
         //   var estateTypes = GetEstateTypes();
          //  var contracts = GetContracts();

            SearchQuery searchQuery = new SearchQuery(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            // Getting ze post data ja

            System.Collections.Specialized.NameValueCollection postedValues = Request.Form;

            if (postedValues["SearchStr"] != null && postedValues["SearchStr"].Length > 0)
            {
                searchQuery.SetKeywords(postedValues["searchStr"]);
                ViewBag.SearchStr = postedValues["searchStr"];

            }
            if (postedValues["province"] != null && postedValues["province"] != "" && postedValues["province"] != "Any")
            {
                searchQuery.ProvinceId = (int.Parse(postedValues["province"]));
            }
            if (postedValues["city"] != null && postedValues["city"] != "" && postedValues["city"] != "Any")
                searchQuery.City_Id = (int.Parse(postedValues["city"]));
            if (postedValues["area"] != null && postedValues["area"] != "" && postedValues["area"] != "Any")
                searchQuery.Area_Id = (int.Parse(postedValues["area"]));
            if (postedValues["area"] != null && postedValues["area"] != "" && postedValues["area"] != "Any")
                searchQuery.Area_Id = (int.Parse(postedValues["area"]));
            if(postedValues["noBedrooms"] != null)
            {
                searchQuery.BedroomsMin = int.Parse(postedValues["noBedrooms"]);
            }
            if (postedValues["noBathrooms"] != null)
            {
                searchQuery.BathroomsMin = int.Parse(postedValues["noBathrooms"]);
            }
            if (postedValues["noGarages"] != null)
            {
                searchQuery.GaragesMin = int.Parse(postedValues["noGarages"]);
            }
            

            var res = searchQuery.GenerateResults();

            MySqlDataReader reader = res;

            List<Dictionary<string, string>> props = new List<Dictionary<string, string>>();
            while (reader.Read())
            {
                int id = reader.GetInt32("List_ID");
              //  string name = reader.GetString(1);
                string streetname = reader.GetString("Address_Streetname");
                int streetno = reader.GetInt32("Address_Streetno");
                int no_bathrooms = reader.GetInt32("Property_Bathroom_Count");
                int no_bedrooms = reader.GetInt32("Property_Bedroom_Count");
                int area = reader.GetInt32("Property_Plot_Size");
               // int city_id = reader.GetInt32("city_id");
                string area_name = reader.GetString("Area_Name");
                string city = reader.GetString("City_Name");
                string province = reader.GetString("Province_Name");// reader.GetString("province");
                //int estateTypeId = reader.GetInt32("type_id");
               // int contractId = reader.GetInt32("contract_id");
                string image = reader.GetString("Image_URL");
                float price = reader.GetFloat("List_Price");


                // composite fields
              //  string type = estateTypes[estateTypeId];
                string link = Url.Action("Residence", "Estates") + "?id=" + id;
                //string imagePath = "/Assets/Images/Homes/" + image;
                string imagePath = image;
                string locationStr = string.Format("{0}, {1}", city, province);
              //  string contract = contracts[contractId];

                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                nfi.NumberGroupSeparator = " ";
                string formatted = price.ToString("#,#", nfi); // "1 234 897.11"

                string priceStr = string.Format("R {0}", formatted);
                string location = streetname + " " + streetno.ToString();

                Dictionary<string, string> propDic = new Dictionary<string, string>();

                propDic["image"] = imagePath;
                propDic["link"] = link;
                propDic["street"] = location;
                propDic["city"] = locationStr;
                propDic["price"] = priceStr;
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


        /// <summary>
        /// Returns the logged in user
        /// </summary>
        /// <returns></returns>
        public static IdentityUser GetCurrentUser()
        {
            string user_id = null;
            var claimsIdentity = System.Web.HttpContext.Current.GetOwinContext().Authentication.User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.Claims
       .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    user_id = userIdClaim.Value;
                }
            }
            if (user_id != null)
            {
                MySQLDatabase db = new MySQLDatabase("DefaultConnection");
                UserTable<IdentityUser> table = new UserTable<IdentityUser>(db);
                var user = table.GetUserById(user_id);
                return user;
            }
            return null;
        }
	}
}