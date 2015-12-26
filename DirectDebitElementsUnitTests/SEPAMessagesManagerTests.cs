using System.IO;
using System.Xml.Linq;
using DirectDebitElements;
using DirectDebitElements.DirectDebitClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class SEPAMessagesManagerTests
    {

        [TestMethod]
        public void APaymentStatusReportIsCorrectlyInitializedFromAXMLStringMessage()
        {
            string xMLFilePath = @"XML Test Files\pain.002.001.03\LaCaixa_pain00200103_Example1.xml";
            StreamReader fileReader = new StreamReader(xMLFilePath);
            string paymentStatusReportXMLStringMessage = fileReader.ReadToEnd();

            SEPAMessagesManager sEPAMessagesManager = new SEPAMessagesManager();

            //PaymentStatusReport paymentStatusReport = sepaMessagesManager.

            Assert.Inconclusive();
        }

        [TestMethod]
        public void APaymentStatusReportIsCorrectlyInitializedFromA()
        {
            string xMLFilePath = "";
            Assert.Inconclusive();
        }
    }
}
