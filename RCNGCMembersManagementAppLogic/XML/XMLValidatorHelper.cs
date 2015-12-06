using System.Xml;
using System.Xml.Schema;

namespace RCNGCMembersManagementAppLogic.XML
{
    public static class XMLValidatorHelper
    {
        public static void AddElementToSchema(XmlSchema xmlSchema, string elementName, string elementType, string xmlNamespace)
        {
            XmlSchemaElement testNode = new XmlSchemaElement();
            testNode.Name = elementName;
            testNode.Namespaces.Add("", xmlNamespace);
            testNode.SchemaTypeName = new XmlQualifiedName(elementType, xmlNamespace);
            xmlSchema.Items.Add(testNode);
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add(xmlSchema);
            schemaSet.Compile();
        }
    }
}
