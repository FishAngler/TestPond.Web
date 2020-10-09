using System;
using System.Text;

namespace TestPond.BusinessLayer.Services
{
    public class EncodingUtilities
    {
        public static string DecodeStringFromBase64(string stringToDecode)
        {
            var converted = Convert.FromBase64String(stringToDecode);
            var encoded = Encoding.UTF8.GetString(converted);

            return encoded;
        }
    }
}
