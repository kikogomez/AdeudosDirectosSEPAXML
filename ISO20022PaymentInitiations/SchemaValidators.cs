using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLSerializerValidator;

namespace ISO20022PaymentInitiations
{
    public static class SchemaValidators
    {
        public static string ValidatePaymentStatusReportFile(string xMLFilePath)
        {
            string applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string validationSchemaFileFullPath = applicationDirectory + @"\XSDFiles\pain.002.001.03.xsd";
            return XMLValidator.ValidateXMLFileThroughXSDFile(xMLFilePath, validationSchemaFileFullPath);
        }

        public static string ValidatePaymentStatusReportString(string xMLString)
        {
            string applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string validationSchemaFileFullPath = applicationDirectory + @"\XSDFiles\pain.002.001.03.xsd";
            return XMLValidator.ValidateXMLStringThroughXSDFile(xMLString, validationSchemaFileFullPath);
        }
    }
}
