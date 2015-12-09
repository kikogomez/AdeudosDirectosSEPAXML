using System;

namespace ISO20022PaymentInitiationsUnitTests.XMLPOCOClasses
{
    [Serializable]
    public class DateTimeTest
    {
        [System.Xml.Serialization.XmlElement("DaateTime")]
        public DateTime dateTime;
    }
}
