using RealEstateCompanyWebSite.SQL;
using System.Data.Entity;

namespace RealEstateCompanyWebSite.Models
{
    
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    /// <summary>
    /// Application user
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
    }

    /// <summary>
    /// Insert comments like this yah
    /// </summary>
    public class ApplicationDbContext : MySQLDatabase
    {
         public ApplicationDbContext(string connectionName)
            : base(connectionName)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext("DefaultConnection");
        }
    }
}