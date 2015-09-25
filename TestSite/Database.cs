using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace TestSite
{
    public class DatabaseConnection
    {
        MySqlConnection connection;

        public DatabaseConnection(string connectionString)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        public void Reopen()
        {
            connection.Open();
        }

        public void Close()
        {
            connection.Close();
        }

        public MySqlCommand PrepareCommand()
        {
            return connection.CreateCommand();
        }

        public MySqlDataReader Query(MySqlCommand command)
        {
            MySqlDataReader reader = command.ExecuteReader();
            return reader;
        }

        public MySqlDataReader Query(string query)
        {
            string escaped = Escape(query);
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = escaped;
            return Query(command);
        }

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