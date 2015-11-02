using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RealEstateCompanyWebSite
{
    /// <summary>
    /// Helper class for db connections
    /// </summary>
    public class DatabaseConnection
    {
        MySqlConnection connection;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public DatabaseConnection(string connectionString)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        /// <summary>
        /// Opens the mysql connection
        /// </summary>
        public void Reopen()
        {
            connection.Open();
        }


        /// <summary>
        /// Closes the mysql connection
        /// </summary>
        public void Close()
        {
            connection.Close();
        }

        /// <summary>
        /// Creates a mysql command
        /// </summary>
        /// <returns></returns>
        public MySqlCommand PrepareCommand()
        {
            return connection.CreateCommand();
        }

        /// <summary>
        /// Runs a mysqlcommand
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public MySqlDataReader Query(MySqlCommand command)
        {
            MySqlDataReader reader = command.ExecuteReader();
            return reader;
        }

        /// <summary>
        /// Returns a reader for a sql query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public MySqlDataReader Query(string query)
        {
            string escaped = Escape(query);
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = escaped;
            return Query(command);
        }

        /// <summary>
        /// Lazy query
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>
        public IEnumerable<string[]> QueryYield(string qry)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = string.Format(qry);

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string[] row = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                    row[i] = reader.GetString(i);
                yield return row.ToArray();
            }
        }

        /*public static void Main(string[] args)
        {
            var expr = Query("select * from reii422_cities");
            foreach (var i in expr)
            {
                Console.WriteLine(i[0] + " " + i[1]);
            }

            Console.ReadLine();
        }*/

        /// <summary>
        /// Escape a string properly
        /// </summary>
        /// <param name="usString"></param>
        /// <returns></returns>
        static string Escape(string usString)
        {
            if (usString == null)
            {
                return null;
            }
            return Regex.Replace(usString, @"[\r\n\x00\x1a\\'""]", @"\$0");
        }
    }
}