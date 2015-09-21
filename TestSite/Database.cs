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