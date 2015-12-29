using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using DirectDebitElements;
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
            PaymentStatusReport paymentStatusReport = sEPAMessagesManager.ReadISO20022PaymentStatusReportMessage(paymentStatusReportXMLStringMessage);

            DateTime expectedMessageCreationDate = DateTime.Parse("2012-07-18T06:00:01");
            DateTime expectedRejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            List<string> expectedOriginalEndtoEndTransactionIdentificationList1 = new List<string>()
            {"201207010001/01002", "201207010001/02452"};
            List<string> expectedOriginalEndtoEndTransactionIdentificationList2 = new List<string>()
            {"201205270001/01650"};

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(expectedMessageCreationDate, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(expectedRejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(3, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(220.30, paymentStatusReport.ControlSum);
            Assert.AreEqual(2, paymentStatusReport.DirectDebitRemmitanceRejects[0].NumberOfTransactions);
            Assert.AreEqual(130.30, paymentStatusReport.DirectDebitRemmitanceRejects[0].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList1, paymentStatusReport.DirectDebitRemmitanceRejects[0].OriginalEndtoEndTransactionIdentificationList);
            Assert.AreEqual(1, paymentStatusReport.DirectDebitRemmitanceRejects[1].NumberOfTransactions);
            Assert.AreEqual(90, paymentStatusReport.DirectDebitRemmitanceRejects[1].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList2, paymentStatusReport.DirectDebitRemmitanceRejects[1].OriginalEndtoEndTransactionIdentificationList);
        }

        //[TestMethod]
        //public void APaymentStatusReportIsCorrectlyInitializedFromA()
        //{
        //    string xMLFilePath = "";
        //    Assert.Inconclusive();
        //}
    }
}
