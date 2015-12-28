using System.Xml;
using System.Xml.Linq;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace DirectDebitElements.DirectDebitClasses
{
    public class PaymentStatusReport
    {
        string messageID;
        DateTime creationDateTime;
        int numberOfTransactions;
        decimal controlSum;

        //Ahora 1 solo grupo de <OrgnlGrpInfAndSts>. Lo ponemos como variables independientes?
        List<DirectDebitRemmitanceReject> paymentRemmitancesRejects; 

        public PaymentStatusReport() { }

        public PaymentStatusReport(string paymentStatusReportXMLMessage)
        {
        }

        public PaymentStatusReport(XDocument xmlDocument)
        {
        }
    }
}
