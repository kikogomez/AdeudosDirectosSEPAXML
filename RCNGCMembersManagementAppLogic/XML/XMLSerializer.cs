using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace RCNGCMembersManagementAppLogic.XML
{
    public static class XMLSerializer
    {
        public static string XMLSerializeToString<ObjectType>(ObjectType objetToSerialize, string rootElementName, string defaultNamespace)
        {
            TextWriter stringWriter = new StringWriter();
            XMLSerialize<ObjectType>(objetToSerialize, rootElementName, defaultNamespace, stringWriter);
            return stringWriter.ToString();
        }

        public static void XMLSerializeToFile<ObjectType>(ObjectType objetToSerialize, string rootElementName, string defaultNamespace, string targetXMLFileName)
        {
            TextWriter fileWriter = new StreamWriter(targetXMLFileName, true, Encoding.UTF8);
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
