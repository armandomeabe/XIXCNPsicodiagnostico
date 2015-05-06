using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AcreditacionesBackend.Utils
{
    public static class Base64String
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            var stageBBytes = System.Text.Encoding.UTF8.GetBytes(System.Convert.ToBase64String(plainTextBytes));
            return System.Convert.ToBase64String(stageBBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            var stageBBytes = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(stageBBytes));
        }
    }
}