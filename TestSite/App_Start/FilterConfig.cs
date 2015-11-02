using System.Web;
using System.Web.Mvc;

namespace TestSite
{
    /// <summary>
    /// Filter
    /// </summary>
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
