using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace Security.Claim
{
    /// <summary>
    /// Class PasswordManage
    /// </summary>
    public static class PasswordManage
    {
        /// <summary>
        /// The salt value size
        /// </summary>
        private const int _saltValueSize = 8;

        /// <summary>
        /// The password length
        /// </summary>
        private const int _passwordLength = 8;

        /// <summary>
        /// Hashes the password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>Password.</returns>
        public static Password HashPassword(string password, string salt = null)
        {
            if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            // If the salt string is null or the length is invalid then
            // create a new valid salt value.
            if (salt == null)
            {
                // Generate a salt string.
                salt = GenerateSaltValue();
            }
            // merge password and salt together
            string sHashWithSalt = password + salt;
            // convert this merged value to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(sHashWithSalt);
            // use hash algorithm to compute the hash
            HashAlgorithm algorithm = new SHA256Managed();
            // convert merged bytes to a hash as byte array
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);

            // Return the hashed password as 64 encoded string.
            return new Password
            {
                HashedPassword = Convert.ToBase64String(hash),
                Salt = salt
            };
        }

        /// <summary>
        /// Generates the salt value.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GenerateSaltValue()
        {
            var utf16 = new UnicodeEncoding();
            // Create a random number object seeded from the value
            // of the last random seed value. This is done
            // interlocked because it is a static value and we want
            // it to roll forward safely.

            var random = new Random(unchecked((int)DateTime.Now.Ticks));
            // Create an array of random values.
            var saltValue = new byte[_saltValueSize];

            random.NextBytes(saltValue);

            // Convert the salt value to a string. Note that the resulting string
            // will still be an array of binary values and not a printable string. 
            // Also it does not convert each byte to a double byte.
            var saltValueString = utf16.GetString(saltValue);

            // Return the salt value as a string.
            return saltValueString;
        }

        /// <summary>
        /// Compares the password.
        /// </summary>
        /// <param name="inputedPassword">The inputed password.</param>
        /// <param name="userPassword">The user password.</param>
        /// <param name="salt">The salt.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static bool ComparePassword(string inputedPassword, string userPassword, string salt)
        {
            //encrypt inputed password with salt.
            string encryptedPassword = HashPassword(inputedPassword, salt).HashedPassword;
            //compare.
            return encryptedPassword.Equals(userPassword);
        }

        /// <summary>
        /// Generates the password.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GeneratePassword()
        {
            var password = Regex.Replace(Membership.GeneratePassword(_passwordLength, 1), @"[!~{}|\:'<\[\]>,./]", "");
            return Regex.Replace(password, "[\"]", "");
        }
    }
}