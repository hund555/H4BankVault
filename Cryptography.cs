using System;
using System.Security.Cryptography;

namespace H4BankVault
{
    public static class Cryptography
    {
        /// <summary>
        /// Hash password using a bytearray of the input, the salt and iterate 5000 times through the process.
        /// </summary>
        /// <param name="inputToHash">Input parameter to be hashed</param>
        /// <param name="salt">Salt for the hashing</param>
        /// <returns>Hashed password</returns>

        public static string CreateHashedPassword(byte[] inputToHash, byte[] salt)
        {
            var byteResult = new Rfc2898DeriveBytes(inputToHash, salt, 5000);
            return Convert.ToBase64String(byteResult.GetBytes(24));
        }

        /// <summary>
        /// Create a byte array and fill it with random data using the randomizer called RNGCryptoServiceProvider.
        /// </summary>
        /// <returns>Salt</returns>
        public static string GenerateSalt()
        {
            var saltArray = new byte[128 / 8];
            var rng = new RNGCryptoServiceProvider();

            rng.GetBytes(saltArray);
            return Convert.ToBase64String(saltArray);
        }
    }
}
