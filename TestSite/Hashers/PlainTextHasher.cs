
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstateCompanyWebSite.Hashers
{
    /// <summary>
    /// Simple password hasher that returns plain text
    /// </summary>
    public class PlainTextHasher : IPasswordHasher
    {
        /// <summary>
        /// Returns password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string HashPassword(string password)
        {
            return password;
        }

        /// <summary>
        /// Returns whether the two password match
        /// </summary>
        /// <param name="hashedPassword"></param>
        /// <param name="providedPassword"></param>
        /// <returns></returns>
        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (hashedPassword.Equals(providedPassword))
                return PasswordVerificationResult.Success;
            else return PasswordVerificationResult.Failed;
        }
    }
}