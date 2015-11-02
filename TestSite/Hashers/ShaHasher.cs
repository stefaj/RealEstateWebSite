using System;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNet.Identity;

namespace RealEstateCompanyWebSite.Hashers
{
    /// <summary>
    /// Generates hashes using PBKDF2 Sha1
    /// </summary>
    public class PBKDF2Sha1Hasher : IPasswordHasher
    {
        /// <summary>
        /// Number of bytes for salt
        /// </summary>
        public const int SALT_BYTE_SIZE = 24;

        /// <summary>
        /// Number of bytes for hash
        /// </summary>
        public const int HASH_BYTE_SIZE = 24;

        /// <summary>
        /// Number of iterations for PBKDF2
        /// </summary>
        public const int PBKDF2_ITERATIONS = 1000;

        /// <summary>
        /// Index of iteration
        /// </summary>
        public const int ITERATION_INDEX = 0;

        /// <summary>
        /// Location of salt
        /// </summary>
        public const int SALT_INDEX = 1;

        /// <summary>
        /// Location of hash
        /// </summary>
        public const int PBKDF2_INDEX = 2;

        /// <summary>
        /// Creates a hash for the given password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string CreateHash(string password)
        {
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_BYTE_SIZE];
            csprng.GetBytes(salt);

            byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);
            return PBKDF2_ITERATIONS + ":" +
                Convert.ToBase64String(salt) + ":" +
                Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Returns true if the passwords match
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <param name="correctHash">Hashed password</param>
        /// <returns></returns>
        public static bool ValidatePassword(string password, string correctHash)
        {

            char[] delimiter = { ':' };
            string[] split = correctHash.Split(delimiter);
            int iterations = Int32.Parse(split[ITERATION_INDEX]);
            byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
            byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);

            byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }


        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }

        /// <summary>
        /// Interface function for hashing password. Returns hashed password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string HashPassword(string password)
        {
            return PBKDF2Sha1Hasher.CreateHash(password);
        }

        /// <summary>
        /// Interface function for hashing password. Returns whether the password match
        /// </summary>
        /// <param name="hashedPassword">Hashed password</param>
        /// <param name="providedPassword">Plaintext password</param>
        /// <returns>PasswordVerificationResult.Success if passwords match else PasswordVerificationResult.Failed </returns>
        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (PBKDF2Sha1Hasher.ValidatePassword(providedPassword, hashedPassword))
                return PasswordVerificationResult.Success;
            else
                return PasswordVerificationResult.Failed;
            
        }
    }
}