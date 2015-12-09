using System;

namespace XMLSerializerValidatorUnitTest.XMLPOCOClasses
{
    [Serializable]
    public class DateTimeTest
    {
        [System.Xml.Serialization.XmlElement("DateTime")]
        public DateTime dateTime;
    }
}
