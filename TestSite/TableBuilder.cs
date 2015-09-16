using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TestSite
{
    /// <summary>
    /// Helper class to generate HTML for table
    /// </summary>
    /// <example>How to use:
    /// <code>
    /// TableBuilder builder = new TableBuilder();
    /// builder.SetHeaderTitle("name", "surname", "age");
    /// builder.AddRow("Johny", "Walker", 34);  
    /// ViewBag.Message = builder.ToHTML();      
    /// </code></example>
    public class TableBuilder
    {
        StringBuilder table;
        bool isClosed;
        /// <summary>
        /// Constructor
        /// </summary>
        public TableBuilder()
        {
            Clear();
        }

        private void Clear()
        {
            this.table = new StringBuilder();
            this.table.Append("<div class='table-responsive'><table class='table'>");
            isClosed = false;
        }

        /// <summary>
        /// Sets the header title for the table
        /// </summary>
        /// <param name="names">List of header titles</param>
        public void SetHeaderTitle(params string[] names)
        {
            Clear();

            table.Append("<tr>");
            foreach(var s in names)
            {
                table.Append("<th>");
                table.Append(s);
                table.Append("</th>");
            }
            table.Append("</tr>");
        }

        private void Close()
        {
            table.Append("</table></div>");
            isClosed = true;
        }

        /// <summary>
        /// Adds a row to the table
        /// </summary>
        /// <param name="dataItems">List of items in row</param>
        public void AddRow(params string[] dataItems)
        {
            table.Append("<tr>");
            foreach (var s in dataItems)
            {
                table.Append("<td>");
                table.Append(s);
                table.Append("</td>");
            }
            table.Append("</tr>");
        }

        /// <summary>
        /// Outputs the HTML for the table
        /// </summary>
        /// <returns></returns>
        public string ToHTML()
        {
            if (!isClosed)
                Close();
            return table.ToString();
        }
    }
}