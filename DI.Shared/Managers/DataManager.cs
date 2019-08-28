using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Managers
{
    public static class DataManager
    {
        public static string GenerateShareCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[6];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string code = new String(stringChars);

            return "dct-" + code;
        }

        public static string GeneratePassword()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[9];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string code = new String(stringChars);

            return code;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();

            return dtDateTime;
        }

        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                   new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        }

        public static int DateTimeToUnixTimestampInt(DateTime dateTime)
        {
            return Convert.ToInt32(Math.Round((dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds, 0));
        }

        public static int GenerateRandomNumber(int min, int max)
        {
            var random = new Random();

            return random.Next(min, max);
        }

        public static string GetFileNameFromAmazonUrl(string value)
        {
            string result = string.Empty;

            string prefix = "-file-";
            int start = value.IndexOf(prefix) + prefix.Length;
            int end = value.IndexOf("?");

            result = value.Substring(start, end - start);

            return result;
        }

        public static bool IsASCII(string value)
        {
            // ASCII encoding replaces non-ascii with question marks, so we use UTF8 to see if multi-byte sequences are there
            return Encoding.UTF8.GetByteCount(value) == value.Length;
        }
    }
}
