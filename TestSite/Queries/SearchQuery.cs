using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestSite.Queries;

namespace TestSite
{
    public class SearchQuery : IQuery
    {
        MySqlConnection connection;
        string[] keywords;

        int type_id = -1;
        float price_min = -1;
        float price_max = -1;
        int bedrooms_min = -1;
        int bathrooms_min = -1;
        int province_id = -1;
        int city_id = -1;
        int contract_id = -1;

        int limit = 10;

        public SearchQuery(string connectionString)
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        public void Close()
        {
            connection.Close();
        }

        public void SetKeywords(string keywords)
        {
            SetKeywords(keywords.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public void SetKeywords(string[] keywords)
        {
            this.keywords = keywords;
        }

        public void SetMinimumPrice(float minPrice)
        {
            this.price_min = minPrice;
        }

        public void SetMaximumPrice(float maxPrice)
        {
            this.price_max = maxPrice;
        }

        public void SetProvince(int id)
        {
            this.province_id = id;
        }

        public void SetCity(int id)
        {
            this.city_id = id;
        }

        public void SetBedroomsMinimum(int min)
        {
            this.bedrooms_min = min;
        }

        public void SetBathroomsMinimum(int min)
        {
            this.bathrooms_min = min;
        }

        public void SetEstateType(int id)
        {
            this.type_id = id;
        }

        public void SetContract(int id)
        {
            this.contract_id = id;
        }

        public void SetMaxResults(int limit)
        {
            this.limit = limit;
        }

        private MySqlDataReader BuildQuery()
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "Select List_ID, List_Price, Property_Bedroom_Count, Property_Bathroom_Count, Property_Garage_Count, Property_hasPool, Property_Plot_Size, Property_House_Size,"
            + " Property_Value, Address_Streetname, Address_Streetno, Area_Name, City_Name, Province_Name, distinct Image_URL"
            + " from Listing, Property, Address, Area, City, Province, Image where "
            + "Listing.Property_ID = Property.Property_ID and Property.Address_ID = Address.Address_ID and Address.Area_ID = Area.Area_Id and Area.Area_City_Id = City.City_Id"
            + " and Province.Province_ID= City.City_Province_ID and Image.Property_Id = Property.Property_Id";

            int k=0;
            if(keywords != null)
                foreach(var keyword in keywords)
                {
                   // string keywordNo = string.Format("@k{0}",k++);
                    command.CommandText += string.Format(" and (street_address like '%{0}%' or province_name like '%{0}%' or city_name like '%{0}%' or name like '%{0}%')",keyword);
                    if(k < keywords.Length - 1)
                        command.CommandText += " or ";
                    //command.Parameters.AddWithValue(keywordNo, keyword);
                    // TO DO SANITIZE!

                }
            if (price_min != -1)
            {
                command.CommandText += " and price >= @price_min";
                command.Parameters.AddWithValue("@price_min", price_min);
 
            }
            if (price_max != -1)
            {
                command.CommandText += " and price =< @price_max";
                command.Parameters.AddWithValue("@price_max", price_max);
            }
            if (bedrooms_min != -1)
            {
                command.CommandText += " and no_bedrooms >= @bedrooms_min";
                command.Parameters.AddWithValue("@bedrooms_min", bedrooms_min);
            }
            if (bathrooms_min != -1)
            {
                command.CommandText += " and no_bathrooms >= @bathrooms_min";
                command.Parameters.AddWithValue("@bathrooms_min", bathrooms_min);
            }
            if (type_id != -1)
            {
                command.CommandText += " and type_id = @type_id";
                command.Parameters.AddWithValue("@type_id", type_id);
            }
            if (province_id != -1)
            {
                command.CommandText += " and reii422_provinces.province_id = @province_id";
                command.Parameters.AddWithValue("@province_id", province_id);
            }
            if (city_id != -1)
            {
                command.CommandText += " and reii422_estates_list.city_id = @city_id";
                command.Parameters.AddWithValue("@city_id", city_id);
            }
            if (contract_id != -1)
            {
                command.CommandText += " and contract_id = @contract_id";
                command.Parameters.AddWithValue("@contract_id", city_id);
            }
            command.CommandText += " order by List_ID DESC";
            if (limit != -1)
                command.CommandText += " limit " + limit;

            try
            {
                return command.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        public MySqlDataReader GenerateResults()
        {
            return BuildQuery();
        }
    }
}