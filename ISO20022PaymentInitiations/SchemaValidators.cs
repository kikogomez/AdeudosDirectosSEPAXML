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
            return XMLValidator.ValidateXMLFileThroughXSDFile(xMLFilePath, @"XSDFiles\pain.002.001.03.xsd");
        }

        public static string ValidatePaymentStatusReportString(string xMLString)
        {
            return XMLValidator.ValidateXMLStringThroughXSDFile(xMLString, @"XSDFiles\pain.002.001.03.xsd");
        }
    }
}
