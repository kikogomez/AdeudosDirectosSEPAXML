using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferencesAndTools
{
    public static class EncodingConverter
    {
        public static string ConvertStringEncoding(Encoding sourceEncoding, Encoding destinationEncoding, string sourceString)
        {
            return destinationEncoding.GetString(Encoding.Convert(sourceEncoding, destinationEncoding, sourceEncoding.GetBytes(sourceString)));
        }
    }
}
