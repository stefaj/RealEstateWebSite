
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestSite.Hashers
{
    public class PlainTextHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return password;
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (hashedPassword.Equals(providedPassword))
                return PasswordVerificationResult.Success;
            else return PasswordVerificationResult.Failed;
        }
    }
}