using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DI.Shared.Managers
{
    public class SecurityManager
    {
        #region HASHING

        static int saltLengthLimit = 32;

        public static string CreateHash(string value, string salt)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            string result = GenerateSHA256Hash(value, salt);

            return result;
        }

        public static string CreateSalt()
        {
            var salt = new byte[saltLengthLimit];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        static string GenerateSHA256Hash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);

            SHA256Managed sha256hashstring = new SHA256Managed();
            byte[] hash = sha256hashstring.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

        #endregion

        #region SECURITY KEYS

        public static string BuildSecurityKey(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(value);
            var base64 = Convert.ToBase64String(bytes);

            return base64;
        }

        #endregion
    }
}
