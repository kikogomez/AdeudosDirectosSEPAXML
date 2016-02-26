using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XMLSerializerValidator;
using XMLSerializerValidatorUnitTest.XMLPOCOClasses;
using System.IO;
using System.Xml;
using ExtensionMethods;

namespace RCNGCMembersManagementUnitTests
{
    [TestClass]
    public class XMLSerializationUnitTests
    {
        [TestMethod]
        public void SerializationWithMemoryStreamRequiresRemoveBOMIfUTF()
        {
            OrderedItem orderedItem = new OrderedItem();
            orderedItem.ItemName = "Widget";
            orderedItem.Description = "Regular Widget";
            orderedItem.Quantity = 10;
            orderedItem.UnitPrice = (decimal)2.30;
            orderedItem.Calculate();

            var memoryStream = new MemoryStream();
            //var encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            System.Text.Encoding encoding = new System.Text.UTF8Encoding(false);
            TextWriter stringWriter = new StreamWriter(memoryStream, encoding);
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(OrderedItem));
            serializer.Serialize(stringWriter, orderedItem);
            string xMLString = encoding.GetString(memoryStream.ToArray());
            Assert.AreEqual("<", xMLString.Substring(0,1));
        }

        [TestMethod]
        public void SerializationToStringUTF8WorksOK()
        {
            string xMLExpectedString = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<OrderedItem xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <ItemName>Widget</ItemName>\r\n  <Description>Regular Widget</Description>\r\n  <UnitPrice>2.3</UnitPrice>\r\n  <Quantity>10</Quantity>\r\n  <LineTotal>23.0</LineTotal>\r\n</OrderedItem>";

            OrderedItem orderedItem = new OrderedItem();
            orderedItem.ItemName = "Widget";
            orderedItem.Description = "Regular Widget";
            orderedItem.Quantity = 10;
            orderedItem.UnitPrice = (decimal)2.30;
            orderedItem.Calculate();
            string xMLString = XMLSerializer.XMLSerializeToString<OrderedItem>(orderedItem, null, null, System.Text.Encoding.GetEncoding("utf-16"));
            Assert.AreEqual(xMLExpectedString, xMLString);
        }

        [TestMethod]
        public void SerializationToStringISO88591WorksOK()
        {
            string xMLExpectedString = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>\r\n<OrderedItem xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <ItemName>Widget</ItemName>\r\n  <Description>Regular Widget</Description>\r\n  <UnitPrice>2.3</UnitPrice>\r\n  <Quantity>10</Quantity>\r\n  <LineTotal>23.0</LineTotal>\r\n</OrderedItem>";

            OrderedItem orderedItem = new OrderedItem();
            orderedItem.ItemName = "Widget";
            orderedItem.Description = "Regular Widget";
            orderedItem.Quantity = 10;
            orderedItem.UnitPrice = (decimal)2.30;
            orderedItem.Calculate();
            string xMLString = XMLSerializer.XMLSerializeToString<OrderedItem>(orderedItem, null, null, System.Text.Encoding.GetEncoding("ISO-8859-1"));
            Assert.AreEqual(xMLExpectedString, xMLString);
        }

        [TestMethod]
        public void DefaultEncodingIsISO85591()
        {
            string xMLExpectedString = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>\r\n<OrderedItem xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <ItemName>Widget</ItemName>\r\n  <Description>Regular Widget</Description>\r\n  <UnitPrice>2.3</UnitPrice>\r\n  <Quantity>10</Quantity>\r\n  <LineTotal>23.0</LineTotal>\r\n</OrderedItem>";

            OrderedItem orderedItem = new OrderedItem();
            orderedItem.ItemName = "Widget";
            orderedItem.Description = "Regular Widget";
            orderedItem.Quantity = 10;
            orderedItem.UnitPrice = (decimal)2.30;
            orderedItem.Calculate();
            string xMLString = XMLSerializer.XMLSerializeToString<OrderedItem>(orderedItem, null, null);
            Assert.AreEqual(xMLExpectedString, xMLString);
        }

        [TestMethod]
        public void DeserializationFromStringWorksOK()
        {
            string xMLStringToDeserialize = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<OrderedItem xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <ItemName>Widget</ItemName>\r\n  <Description>Regular Widget</Description>\r\n  <UnitPrice>2.3</UnitPrice>\r\n  <Quantity>10</Quantity>\r\n  <LineTotal>23.0</LineTotal>\r\n</OrderedItem>";
            OrderedItem orderedItem = XMLSerializer.XMLDeserializeFromString<OrderedItem>(xMLStringToDeserialize, null, null);
            Assert.AreEqual((decimal)23, orderedItem.LineTotal);
        }

        [TestMethod]
        public void DeserializationFromFileWorksOk()
        {
            string XMLFilePath = @"XML Test Files\OrderedItem.xml";
            OrderedItem orderedItem = XMLSerializer.XMLDeserializeFromFile<OrderedItem>(XMLFilePath, null, null);
            Assert.AreEqual((decimal)23, orderedItem.LineTotal);
        }

        [TestMethod] //Not unit test, but integration test
        public void SerializationDeserializationThroughAFileWorksOK()
        {
            string targetSerializedFilePath = "SerializeTest.xml";

            OrderedItem orderedItem = new OrderedItem();
            orderedItem.ItemName = "Widget";
            orderedItem.Description = "Regular Widget";
            orderedItem.Quantity = 10;
            orderedItem.UnitPrice = (decimal)2.30;
            orderedItem.Calculate();
            XMLSerializer.XMLSerializeToFile<OrderedItem>(orderedItem, null, null, targetSerializedFilePath);
            OrderedItem deserializedOrderedItem = XMLSerializer.XMLDeserializeFromFile<OrderedItem>(targetSerializedFilePath, null, null);
            Assert.AreEqual(orderedItem.ItemName, deserializedOrderedItem.ItemName);
            Assert.AreEqual(orderedItem.Description, deserializedOrderedItem.Description);
            Assert.AreEqual(orderedItem.Quantity, deserializedOrderedItem.Quantity);
            Assert.AreEqual(orderedItem.UnitPrice, deserializedOrderedItem.UnitPrice);
            File.Delete("SerializeTest.xml");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void AnErroneousDeserializationThrowsInvalidOperationException()
        {
            string XMLFilePath = @"XML Test Files\OrderedItem(wrong).xml";
            try
            {
                OrderedItem orderedItem = XMLSerializer.XMLDeserializeFromFile<OrderedItem>(XMLFilePath, null, null);
            }
            catch(InvalidOperationException)
            {
                Assert.IsTrue(true);    //Just asserting code reaches here
                throw;
            }          
        }

        [TestMethod]
        public void UnespecifiedDateTimeKindDateTimeSerializesToCorrectFormat()
        {
            DateTime localDateTime =
            DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified).Truncate(TimeSpan.FromSeconds(1)); ;
            DateTimeTest dateTimeTest = new DateTimeTest();
            dateTimeTest.dateTime = localDateTime;
            string dateSerialization = XMLSerializer.XMLSerializeToString<DateTimeTest>(dateTimeTest, null, null);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(dateSerialization);
            DateTime recoveredDateTime;
            string myDate = xmlDoc.DocumentElement.ChildNodes[0].InnerText;
            bool success = DateTime.TryParseExact(myDate,
                                      "yyyy-MM-ddTHH:mm:ss",
                                      System.Globalization.CultureInfo.InvariantCulture,
                                      System.Globalization.DateTimeStyles.None,
                                      out recoveredDateTime);
            Assert.IsTrue(success);
        }
    }
}
