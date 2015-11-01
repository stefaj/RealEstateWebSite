using AspNet.Identity.MySQL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TestSite.Models;

namespace TestSite.Controllers
{
    public class EstatesController : Controller
    {
        //
        // GET: /Search/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Search/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Search/Create
        public ActionResult Estates()
        {
            return View();
        }

        //
        // POST: /Search/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Search/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Search/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Search/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }


        public ActionResult Search()
        {
           // var estateTypes = DataController.GetEstateTypes();
          //  var contracts = DataController.GetContracts();

            SearchQuery searchQuery = new SearchQuery(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            ViewBag.Provinces = DataController.GetProvincesArr();
            //ViewBag.Estates = DataController.GetEstateTypesArr();

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


            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();

            command.CommandText = "Select List_ID, List_Price, Property_Description, Property_Bedroom_Count, Property_Bathroom_Count, Property_Garage_Count,"
            + " Property_Value, Address_Streetname, Address_Streetno, Area_Name, City_Name, Province_Name, List_Description, Property_hasPool, Property_Plot_Size,"
            + " Address_Longitude, Address_Latitude, Property.Property_ID, City_Id, City_Name, Property_House_Size, Agent_ID"
            + " from Listing, Property, Address, Area, City, Province where "
            + "Listing.Property_ID = Property.Property_ID and Property.Address_ID = Address.Address_ID and Address.Area_ID = Area.Area_Id and Area.Area_City_Id = City.City_Id"
            + " and Province.Province_ID= City.City_Province_ID and Listing.List_ID = @listing_id";

            command.Parameters.Add("@listing_id", id);

            MySqlDataReader reader = command.ExecuteReader();

            reader.Read();

            int property_id = reader.GetInt32("Property_ID");

            string street_address = reader.GetString("Address_Streetname") + " " + reader.GetString("Address_streetno");
            int agent_id = reader.GetInt32("Agent_ID");

            ViewBag.ListID = reader.GetInt32("List_ID");
            ViewBag.id = id;
            // ViewBag.name = reader.GetString("name");

            ViewBag.street_address = street_address;
            ViewBag.no_bathrooms = reader.GetInt32("Property_Bathroom_Count");
            ViewBag.no_bedrooms = reader.GetInt32("Property_Bedroom_Count");
            ViewBag.area = reader.GetInt32("Property_Plot_Size");
            ViewBag.city_id = reader.GetInt32("City_Id");
            ViewBag.city = reader.GetString("City_Name");
            ViewBag.province = reader.GetString("Province_Name");// reader.GetString("province");
            // ViewBag.postal_code = reader.GetString("postal_code");

            ViewBag.property_description = reader.GetString("Property_Description");
            ViewBag.listing_description = reader.GetString("List_Description");

            ViewBag.lattitude = reader.GetFloat("Address_Latitude");
            ViewBag.longitude = reader.GetFloat("Address_Longitude");

            //ViewBag.ImagePreview = reader.GetString("preview_image");

            ViewBag.price = reader.GetFloat("List_Price");
            ViewBag.StreetView = string.Format("https://www.google.com/maps/embed/v1/streetview?location={0}%2C{1}&key={2}", ViewBag.lattitude, ViewBag.longitude, "AIzaSyDlnvQdhrcHR6dv2UyyVmzWT_pzaPcmwTo");
            ViewBag.Map = string.Format("https://www.google.com/maps/embed/v1/place?&q={0}&key=AIzaSyDlnvQdhrcHR6dv2UyyVmzWT_pzaPcmwTo", street_address);
            ViewBag.name = ViewBag.street_address;


            connection.Close();
            List<string> images = new List<string>();



            connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = "Select Image_URL, Image_Caption from Image where Property_ID = @property_id";
            command.Parameters.Add("@property_id", property_id);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                string image_url = reader.GetString("Image_URL");
               // string image_caption = reader.GetString("Image_Caption");

                images.Add(image_url);
            }

            ViewBag.Images = images;



            connection.Close();
            connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connection.Open();

            command = connection.CreateCommand();
            command.CommandText = "Select Agent_Id, Agent_Name, Agent_Surname, Agent_Phone, Agent_Email, Agent_Image_URL, Agent_Description from Agent where Agent_ID = @agent_id";
            command.Parameters.AddWithValue("@agent_id", agent_id);
            reader = command.ExecuteReader();

            reader.Read();
            Agent agent = new Agent()
            {
                Agent_Email = reader.GetString("Agent_Email"),
                Agent_Id = reader.GetInt32("Agent_Id"),
                Agent_Name = reader.GetString("Agent_Name"),
                Agent_Phone = reader.GetString("Agent_Phone"),
                Agent_Surname = reader.GetString("Agent_Surname"),
                Agent_Description = reader.GetString("Agent_Description"),
                Image_URL = reader.GetString("Agent_Image_URL")
            };



            ViewBag.Agent = agent;

            connection.Close();

            ViewBag.User = DataController.GetCurrentUser();












            return View();
        }

        //
        // POST: /Search/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Cities()
        {
            return View();
        }
    }
}
