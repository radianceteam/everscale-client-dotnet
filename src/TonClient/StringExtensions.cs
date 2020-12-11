using System;
using System.Security.Cryptography;
using System.Text;

namespace TonSdk
{
    internal static class StringExtensions
    {
        public static string ToBase64String(this string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }

        public static string HexToString(this string input)
        {
            return Encoding.UTF8.GetString(input.FromHexString());
        }

        public static string HexToBase64String(this string input)
        {
            return Convert.ToBase64String(input.FromHexString());
        }

        public static string FromBase64String(this string input)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(input));
        }

        public static string Sha256(this string input)
        {
            using (var hasher = SHA256.Create())
            {
                var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
                var builder = new StringBuilder();
                foreach (var t in hash)
                {
                    builder.Append(t.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static byte[] FromHexString(this string hex)
        {
            if (hex.Length % 2 == 1)
            {
                throw new ArgumentException("The binary key cannot have an odd number of digits");
            }
            var arr = new byte[hex.Length >> 1];
            for (var i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + GetHexVal(hex[(i << 1) + 1]));
            }
            return arr;
        }

        public static string ToHexString(this string input)
        {
            var ba = Encoding.UTF8.GetBytes(input);
            var hexString = BitConverter.ToString(ba);
            return hexString.Replace("-", "").ToLower();
        }

        public static int GetHexVal(int val)
        {
            return val - (val < 58 ? 48 : val < 97 ? 55 : 87);
        }
    }
}
