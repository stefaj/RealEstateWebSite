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
            var estateTypes = GetEstateTypes();
            var contracts = GetContracts();

            SearchQuery searchQuery = new SearchQuery(ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString);

            // Getting ze post data ja

            System.Collections.Specialized.NameValueCollection postedValues = Request.Form;

            if (postedValues["SearchStr"] != null)
            {
                searchQuery.SetKeywords(postedValues["SearchStr"]);
                ViewBag.SearchStr = postedValues["SearchStr"];

            }
            if(postedValues["Province"] != null && postedValues["Province"] != "")
            {
                searchQuery.SetProvince(int.Parse(postedValues["Province"]));
            }
            if (postedValues["City"] != null && postedValues["City"] != "")
                searchQuery.SetCity(int.Parse(postedValues["City"]));

            StringBuilder contentBuilder = new StringBuilder();

            string baseStr = @"<div class='col-lg-3 col-md-3 col-sm-6'>
                <div class='propertyItem'>
                    <div class='propertyContent'>
                        <a class='propertyType' href='{0}'>{1}</a>
                        <a class='propertyImgLink' href='{0}'><img class='propertyImg' src='{2}' ></a>
                     
                        <p>{3}</p>
                        <p>{9}</p>
                        <div class='divider thin'></div>
                        <p class='forSale'>{4}</p>
                        <p class='price'>{5}</p>
                    </div>
                    <table border='1' class='propertyDetails'>
                        <tbody>
                            <tr>
                                <td><img src='/Assets/Images/icon-area.png' style='margin-right:7px;'>{6}</td>
                                <td><img src='/Assets/Images/icon-bed.png' style='margin-right:7px;'>{7}</td>
                                <td><img src='/Assets/Images/icon-drop.png' style='margin-right:7px;'>{8}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>";

        
            var res = searchQuery.GenerateResults();

            MySqlDataReader reader = res;

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

                string content = string.Format(baseStr, link, type, imagePath, locationStr, contract, priceStr, area, no_bedrooms, no_bathrooms, location);

                contentBuilder.Append(content);


            }

            ViewBag.EstatesListHtml = contentBuilder.ToString();
            ViewBag.Provinces = GetProvincesArr();
            ViewBag.Estates = GetEstateTypesArr();

            return View();
        }

        /// <summary>
        /// Returns all column entries for a specified table
        /// </summary>
        /// <param name="tableName">Name of table</param>
        /// <param name="columnName">Name of table to return</param>
        /// <returns>Returns columnName from tableName</returns>
        public Dictionary<int,string> EnumerateTable(string id_column, string columnName, string tableName)
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

        public Dictionary<int, string> GetEstateTypes()
        {
            return EnumerateTable("type_id", "type_name", "reii422_estates_type");
        }

        public Dictionary<int, string> GetContracts()
        {
            return EnumerateTable("contract_id", "contract_name", "reii422_contracts");
        }

        public Dictionary<int, string> GetProvinces()
        {
            return EnumerateTable("province_id", "province_name", "reii422_provinces");
        }
        
        /// <summary>
        /// Returns a list of provinces from the provinces table
        /// </summary>
        /// <returns></returns>
        public string[] GetEstateTypesArr()
        {
            return EnumerateTable("type_id", "type_name", "reii422_estates_type").Values.ToArray();
        }

        public Province[] GetProvincesArr()
        {
            List<Province> provinces = new List<Province>();
            var dic = EnumerateTable("province_id","province_name","reii422_provinces");
            foreach(var key in dic.Keys)
            {
               provinces.Add(new Province(){ ProvinceId=key, ProvinceName=dic[key]});
            }
            return provinces.ToArray();
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
            command.CommandText = "Select * from reii422_estates_list where estate_id=@estate_id";
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
            ViewBag.province = "FIX ME PLEASE";// reader.GetString("province");
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
