using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

            SearchQuery searchQuery = new SearchQuery(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString);

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


            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Select * from Property, Province, City, Area where Property.Address_ID = Address.Address_ID and Address.Area_ID = Area.AreaID"
            + " and Area.Area_ID = City.City_ID and Province.Province_ID = City.Province_ID and Property.Property_ID=@estate_id";
            command.Parameters.Add("@estate_id", id);

            MySqlDataReader reader = command.ExecuteReader();

            reader.Read();

            ViewBag.id = id;
            ViewBag.name = reader.GetString("name");
            ViewBag.street_address = reader.GetString("street_address");
            ViewBag.no_bathrooms = reader.GetInt32("no_bathrooms");
            ViewBag.no_bedrooms = reader.GetInt32("no_bedrooms");
            ViewBag.area = reader.GetInt32("area");
            ViewBag.city_id = reader.GetInt32("city_id");
            ViewBag.city = reader.GetString("city_name");
            ViewBag.province = reader.GetString("province_name");// reader.GetString("province");
            ViewBag.postal_code = reader.GetString("postal_code");
            ViewBag.description = reader.GetString("description");
            ViewBag.lattitude = reader.GetFloat("lattitude");
            ViewBag.longitude = reader.GetFloat("longitude");
            ViewBag.ImagePreview = "/Assets/Images/Homes/" + reader.GetString("preview_image");
            ViewBag.price = reader.GetFloat("price");
            ViewBag.estate_type_id = reader.GetInt32("type_id");
            ViewBag.contract_id = reader.GetInt32("contract_id");
            ViewBag.date_added = reader.GetDateTime("date_added");

            ViewBag.contract = "none";
            ViewBag.type = "none";

            ViewBag.StreetView = string.Format("https://www.google.com/maps/embed/v1/streetview?location={0}%2C{1}&key={2}", ViewBag.lattitude, ViewBag.longitude, "AIzaSyDlnvQdhrcHR6dv2UyyVmzWT_pzaPcmwTo");


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
