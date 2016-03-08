using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReferencesAndTools;

namespace ReferencesAndToolsUnitTests
{
    [TestClass]
    public class EncodingTests
    {
        [TestMethod]
        public void SpecialCharactersEncodedAsISOLatingAreWellConvertedIntoUTF8_UsingByteArray()
        {
            string specialCaracters = "áéíóúñª";

            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;

            byte[] isoBytes = iso.GetBytes(specialCaracters);
            byte[] utfBytes = Encoding.Convert(iso, utf8, isoBytes);
            string specialCharactersEncodedUTF8 = utf8.GetString(utfBytes);

            Assert.AreEqual(specialCaracters, specialCharactersEncodedUTF8);
        }

        [TestMethod]
        public void SpecialCharactersEncodedAsISOLatingAreWellConvertedIntoUTF8_DirectConversion()
        {
            string specialCaracters = "áéíóúñª";

            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;

            string specialCharactersEncodedUTF8 = utf8.GetString(Encoding.Convert(iso, utf8, iso.GetBytes(specialCaracters)));

            Assert.AreEqual(specialCaracters, specialCharactersEncodedUTF8);
        }

        [TestMethod]
        public void SpecialCharactersEncodedAsISOLatingAreWellConvertedIntoUTF8_DirectConversionUsingEncodingConverter()
        {
            string specialCaracters = "áéíóúñª";

            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;

            string specialCharactersEncodedUTF8 = EncodingConverter.ConvertStringEncoding(iso, utf8, specialCaracters); 

            Assert.AreEqual(specialCaracters, specialCharactersEncodedUTF8);
        }

        [TestMethod]
        public void SpecialCharactersEncodedAsUTFThenConvertedToStringThenEncodedToISOAndConvertedAsString()
        {
            string specialCaracters = "áéíóúñª";

            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;

            string specialCharactersEncodedUTF8 = utf8.GetString(utf8.GetBytes(specialCaracters));

            Assert.AreEqual(specialCaracters, specialCharactersEncodedUTF8);

            string specialCharacteresEncodedISO = iso.GetString(iso.GetBytes(specialCharactersEncodedUTF8));

            Assert.AreEqual(specialCaracters, specialCharacteresEncodedISO);
        }


    }
}
