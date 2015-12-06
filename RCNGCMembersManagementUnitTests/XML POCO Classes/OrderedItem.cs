using System.Xml.Serialization;

namespace RCNGCMembersManagementUnitTests.XMLPOCOClasses
{
    public class OrderedItem
    {
        [XmlElement]
        public string ItemName;
        [XmlElement]
        public string Description;
        [XmlElement]
        public decimal UnitPrice;
        [XmlElement]
        public int Quantity;
        [XmlElement]
        public decimal LineTotal;

        public void Calculate()
        {
            LineTotal = UnitPrice * Quantity;
        }
    }
}