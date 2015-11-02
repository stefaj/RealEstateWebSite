using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSite.Queries
{
    /// <summary>
    /// Basic interface for advanced queries
    /// </summary>
    interface IQuery
    {
        /// <summary>
        /// This function returns the results of the query
        /// </summary>
        /// <returns></returns>
        MySqlDataReader GenerateResults();
    }
}
