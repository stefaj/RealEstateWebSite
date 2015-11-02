using RealEstateCompanyWebSite.Hashers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            PlainTextHasher phash = new PlainTextHasher();
            PBKDF2Sha1Hasher sha = new PBKDF2Sha1Hasher();

            string password = "123456";
            var hash = phash.HashPassword(password);
            var success = phash.VerifyHashedPassword(hash, password);

            var hash2 = sha.HashPassword(password);
            var success2 = sha.VerifyHashedPassword(hash2, password);

            Console.ReadLine();
        }
    }
}
