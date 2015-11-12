using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstateCompanyWebSite.Models
{
    public class Preferences
    {
        /// <summary>
        /// ID of this preference
        /// </summary>
        public int PreferenceID{get; set;}

        /// <summary>
        /// ID of client
        /// </summary>
        public int ClientID { get; set; }

        /// <summary>
        /// Minimum Bedrooms
        /// </summary>
        public int MinBedrooms { get; set; }

        /// <summary>
        /// Maximum Bedrooms
        /// </summary>
        public int MaxBedrooms { get; set; }

        /// <summary>
        /// Minimum Bathrooms
        /// </summary>
        public int MinBathrooms { get; set; }

        /// <summary>
        /// Maximum Bathrooms
        /// </summary>
        public int MaxBathrooms { get; set; }

        /// <summary>
        /// Minimum Garages
        /// </summary>
        public int MinGarages { get; set; }

        /// <summary>
        /// Maximum Garages
        /// </summary>
        public int MaxGarages { get; set; }

        /// <summary>
        /// Is there a pool? null is any
        /// </summary>
        public bool? HasPool { get; set; }

        /// <summary>
        /// Minimum Plot Size
        /// </summary>
        public int MinPlotSize { get; set; }

        /// <summary>
        /// Maximum Plot Size
        /// </summary>
        public int MaxPlotSize { get; set; }

        /// <summary>
        /// Minimum House Size
        /// </summary>
        public int MinHouseSize { get; set; }

        /// <summary>
        /// Maximum House Size
        /// </summary>
        public int MaxHouseSize { get; set; }

        /// <summary>
        /// Minimum Price
        /// </summary>
        public int MinPrice { get; set; }

        /// <summary>
        /// Maximum Price
        /// </summary>
        public int MaxPrice { get; set; }

        /// <summary>
        /// ID of area
        /// </summary>
        public int Area_ID { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Preferences()
        {
            PreferenceID = -1;
            ClientID = -1;
            MinBathrooms = -1;
            MinBedrooms = -1;
            MinGarages = -1;
            MinHouseSize = -1;
            MinPlotSize = -1;
            MinPrice = -1;
            MaxBathrooms = 2000;
            MaxBedrooms = 2000;
            MaxGarages = 2000;
            MaxHouseSize = 200000000;
            MaxPlotSize = 200000000;
            MaxPrice = 200000000;
            HasPool = null;
        }

        /// <summary>
        /// Returns whether some fields were intitialized
        /// </summary>
        /// <returns></returns>
        public bool IsSomethingSet()
        {
            if (HasPool != null)
                return true;
            if (ClientID != -1 ||
                Area_ID != -1 || MinBathrooms > -1 || MinBedrooms > -1 || MinGarages > -1 || MinHouseSize > -1 || MinPlotSize > -1 || MinPrice > -1)
                return true;
            return false;
        }


        /// <summary>
        /// Inserts the preferences into the database
        /// </summary>
        /// <param name="connectionString"></param>
        public void Insert(string connectionString)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();

            command.CommandText = @"Insert into Preference(Preference_Client_ID, Preference_Min_Bedrooms, Preference_Max_Bedrooms, Preference_Min_Bathrooms,  
Preference_Max_Bathrooms, Preference_Min_Garages, Preference_Max_Garages, Preference_hasPool, Preference_Min_Plot_Size, Preference_Max_Plot_Size, 
Preference_Min_House_Size, Preference_Max_House_Size, Preference_Min_Price, Preference_Max_Price) values(@id, @minbed, @maxbed, @minbath, @maxbath, @mingg,
@maxgg, @pool, @minplot, @maxplot, @minhouse, @maxhouse, @minprice, @maxprice)";

            command.Parameters.AddWithValue("@id", ClientID);
            command.Parameters.AddWithValue("@minbed", MinBedrooms);
            command.Parameters.AddWithValue("@maxbed", MaxBedrooms);
            command.Parameters.AddWithValue("@minbath", MinBathrooms);
            command.Parameters.AddWithValue("@maxbath", MaxBathrooms);
            command.Parameters.AddWithValue("@mingg", MinGarages);
            command.Parameters.AddWithValue("@maxgg", MaxGarages);
            command.Parameters.AddWithValue("@pool", HasPool);
            command.Parameters.AddWithValue("@minplot", MinPlotSize);
            command.Parameters.AddWithValue("@maxplot", MaxPlotSize);
            command.Parameters.AddWithValue("@minhouse", MinHouseSize);
            command.Parameters.AddWithValue("@maxhouse", MaxHouseSize);
            command.Parameters.AddWithValue("@minprice", MinPrice);
            command.Parameters.AddWithValue("@maxprice", MaxPrice);

            int pref_id = -1;
            try
            {
                command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            pref_id = (int)command.LastInsertedId;
            command = connection.CreateCommand();
            command.CommandText = @"Insert into Preference_Area(Preference_ID, Area_ID) values(@pref_id, @area_id)";
            command.Parameters.AddWithValue("@area_id", Area_ID);
            command.Parameters.AddWithValue("@pref_id", pref_id);


            if (pref_id > -1 && Area_ID > -1)
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            connection.Close();
        }
    }
}