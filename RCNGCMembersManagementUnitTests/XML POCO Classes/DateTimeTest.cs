using System;

namespace RCNGCMembersManagementUnitTests.XMLPOCOClasses
{
    [Serializable]
    public class DateTimeTest
    {
        [System.Xml.Serialization.XmlElement("DaateTime")]
        public DateTime dateTime;
    }
}
