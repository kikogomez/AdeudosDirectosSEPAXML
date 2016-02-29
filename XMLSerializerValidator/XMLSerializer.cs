using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace XMLSerializerValidator
{
    public static class XMLSerializer
    {
        public static Encoding defaultEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");

        public static string XMLSerializeToString<ObjectType>(ObjectType objetToSerialize, string rootElementName, string defaultNamespace)
        {
            //// Serialize to UTF-8 using UTF8StringWriter() (no BOM in resulting string)
            //TextWriter stringWriter = new Utf8StringWriter();
            //XMLSerialize<ObjectType>(objetToSerialize, rootElementName, defaultNamespace, stringWriter);
            //return stringWriter.ToString();

            ////Serialize to UTF8 using a MemoryStream (the BOM is included in resulting string!)
            //var memoryStream = new MemoryStream();
            //var encoding = Encoding.UTF8;
            //TextWriter streamWriter = new StreamWriter(memoryStream, encoding);
            //XMLSerialize<ObjectType>(objetToSerialize, rootElementName, defaultNamespace, streamWriter);
            //return encoding.GetString(memoryStream.ToArray());

            /////Serialize to ISO-8859-1 using a MemoryStream (no BOM in ISO-8859-1)
            //var memoryStream = new MemoryStream();
            //var encoding = Encoding.GetEncoding("ISO-8859-1");
            //TextWriter streamWriter = new StreamWriter(memoryStream, encoding);
            //XMLSerialize<ObjectType>(objetToSerialize, rootElementName, defaultNamespace, streamWriter);
            //return encoding.GetString(memoryStream.ToArray());

            // Serialize to string using a derived stringbuilder that allows define encoding
            TextWriter stringWriter = new EncodingDefinedStringWriter(defaultEncoding);
            XMLSerialize<ObjectType>(objetToSerialize, rootElementName, defaultNamespace, stringWriter);
            return stringWriter.ToString();
        }
        public static string XMLSerializeToString<ObjectType>(ObjectType objetToSerialize, string rootElementName, string defaultNamespace, Encoding encoding)
        {
            // Serialize using a derived class that allows to define encoding
            TextWriter stringWriter = new EncodingDefinedStringWriter(encoding);
            XMLSerialize<ObjectType>(objetToSerialize, rootElementName, defaultNamespace, stringWriter);
            return stringWriter.ToString();
        }

        public static void XMLSerializeToFile<ObjectType>(ObjectType objetToSerialize, string rootElementName, string defaultNamespace, string targetXMLFileName)
        {
            TextWriter fileWriter = new StreamWriter(targetXMLFileName, true, defaultEncoding);
            XMLSerialize<ObjectType>(objetToSerialize, rootElementName, defaultNamespace, fileWriter);
            fileWriter.Close();
        }

        public static void XMLSerializeToFile<ObjectType>(ObjectType objetToSerialize, string rootElementName, string defaultNamespace, string targetXMLFileName, Encoding encoding)
        {
            TextWriter fileWriter = new StreamWriter(targetXMLFileName, true, encoding);
            XMLSerialize<ObjectType>(objetToSerialize, rootElementName, defaultNamespace, fileWriter);
            fileWriter.Close();
        }

        private static void XMLSerialize<ObjectType>(ObjectType objetToSerialize, string rootElementName, string defaultNamespace, TextWriter xmlWriter)
        {
            XmlSerializer serializer = InitializeSerializer<ObjectType>(rootElementName, defaultNamespace);
            serializer.Serialize(xmlWriter, objetToSerialize);
        }

        public static ObjectType XMLDeserializeFromString<ObjectType>(string xmlString, string rootElementName, string defaultNamespace)
        {
            ObjectType deserializatedObject;
            TextReader stringReader = new StringReader(xmlString);
            deserializatedObject = XMLDeserialize<ObjectType>(stringReader, rootElementName, defaultNamespace);
            stringReader.Close();
            return deserializatedObject;
        }

        public static ObjectType XMLDeserializeFromFile<ObjectType>(string xmlFilePath, string rootElementName, string defaultNamespace)
        {
            ObjectType deserializatedObject;
            TextReader fileReader = new StreamReader(xmlFilePath);
            deserializatedObject = XMLDeserialize<ObjectType>(fileReader, rootElementName, defaultNamespace);
            fileReader.Close();
            return deserializatedObject;
        }

        private static ObjectType XMLDeserialize<ObjectType>(TextReader xmlReader, string rootElementName, string defaultNamespace)
        {
            ObjectType deserializatedObject;
            XmlSerializer serializer = InitializeSerializer<ObjectType>(rootElementName, defaultNamespace);
            deserializatedObject = (ObjectType)serializer.Deserialize(xmlReader);
            return deserializatedObject;
        }

        private static XmlSerializer InitializeSerializer<ObjectType>(string rootElementName, string defaultNamespace)
        {
            XmlSerializer serializer;
            if (rootElementName == null)
            {
                serializer =
                    defaultNamespace == null ?
                    new XmlSerializer(typeof(ObjectType)) :
                    new XmlSerializer(typeof(ObjectType), defaultNamespace);
            }
            else
            {
                serializer =
                    defaultNamespace == null ?
                    new XmlSerializer(typeof(ObjectType), new XmlRootAttribute { ElementName = rootElementName }) :
                    new XmlSerializer(typeof(ObjectType), null, null, new XmlRootAttribute { ElementName = rootElementName }, defaultNamespace);

            }
            return serializer;
        }
    }
}
