using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace RCNGCMembersManagementAppLogic.XML
{
    public static class XMLValidator
    {
        static string validatorErrors;

        public static string ValidateXMLFileThroughXSDFile(string xmlFilePath, string xsdFilePath)
        {

            XmlSchema xmlSchema = XmlSchema.Read(new StreamReader(xsdFilePath), XMLValidationEventHandler);
            XDocument fileToTest = XDocument.Load(new StreamReader(xmlFilePath));
            return ValidateXMLThroughSchema(fileToTest, xmlSchema);
        }

        public static string ValidateXMLStringThroughXSDFile(string xmlString, string xsdFilePath)
        {
            XmlSchema xmlSchema = XmlSchema.Read(new StreamReader(xsdFilePath), XMLValidationEventHandler);
            XDocument stringToTest = XDocument.Load(new StringReader(xmlString));
            return ValidateXMLThroughSchema(stringToTest, xmlSchema);
        }

        public static string ValidateXMLNodeThroughModifiedXSD(
            string elementName,
            string elementType,
            string xmlNamespace,
            string stringXML,
            string xsdFilePath)
        {
            validatorErrors = "";
            XmlSchema xmlSchema = XmlSchema.Read(new StreamReader(xsdFilePath), XMLValidationEventHandler);
            XMLValidatorHelper.AddElementToSchema(xmlSchema, elementName, elementType, xmlNamespace);
            XDocument stringToTest;
            try
            {
                stringToTest = XDocument.Load(new StringReader(stringXML));
            }
            catch (XmlException e)
            {
                throw e;
            }
            return ValidateXMLThroughSchema(stringToTest, xmlSchema);
        }

        static string ValidateXMLThroughSchema(XDocument xmlDocument, XmlSchema xmlSchema)
        {
            validatorErrors = "";

            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(xmlSchema);
            xmlDocument.Validate(schemas, XMLValidationEventHandler);
            return validatorErrors;
        }

        static void XMLValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                validatorErrors += ("WARNING: " + Environment.NewLine);
                validatorErrors += (e.Message + Environment.NewLine);
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                validatorErrors += ("ERROR: " + Environment.NewLine);
                validatorErrors += (e.Message + Environment.NewLine);
            }
        }
    }
}
