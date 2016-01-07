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
            Assert.AreEqual((decimal)220.30, paymentStatusReport.ControlSum);
            Assert.AreEqual(2, paymentStatusReport.DirectDebitRemmitanceRejects[0].NumberOfTransactions);
            Assert.AreEqual((decimal)130.30, paymentStatusReport.DirectDebitRemmitanceRejects[0].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList1, paymentStatusReport.DirectDebitRemmitanceRejects[0].OriginalEndtoEndTransactionIdentificationList);
            Assert.AreEqual(1, paymentStatusReport.DirectDebitRemmitanceRejects[1].NumberOfTransactions);
            Assert.AreEqual((decimal)90, paymentStatusReport.DirectDebitRemmitanceRejects[1].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList2, paymentStatusReport.DirectDebitRemmitanceRejects[1].OriginalEndtoEndTransactionIdentificationList);
        }

        [TestMethod]
        public void AnotherPaymentStatusReportIsCorrectlyInitializedFromAXMLStringMessage()
        {
            string xMLFilePath = @"XML Test Files\pain.002.001.03\LaCaixa_pain00200103_Example2.xml";
            StreamReader fileReader = new StreamReader(xMLFilePath);
            string paymentStatusReportXMLStringMessage = fileReader.ReadToEnd();

            SEPAMessagesManager sEPAMessagesManager = new SEPAMessagesManager();
            PaymentStatusReport paymentStatusReport = sEPAMessagesManager.ReadISO20022PaymentStatusReportMessage(paymentStatusReportXMLStringMessage);

            //General info from file
            DateTime expectedMessageCreationDate = DateTime.Parse("2015-12-08T06:00:01");
            DateTime expectedRejectAccountChargeDateTime = DateTime.Parse("2015-12-08");
            Assert.AreEqual("DAG35008770033201512080511490000677", paymentStatusReport.MessageID);
            Assert.AreEqual(expectedMessageCreationDate, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(expectedRejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(4, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual((decimal)1006.65, paymentStatusReport.ControlSum);
            Assert.AreEqual(3, paymentStatusReport.DirectDebitRemmitanceRejects.Count);

            //Info from first DirectDectDebitRemmitance
            List<string> expectedOriginalEndtoEndTransactionIdentificationList1 = new List<string>()
            {"15M/025450120151203", "15M/025720120151203"};
            Assert.AreEqual("2015-12-0112205515Rem.150 Ord.1", paymentStatusReport.DirectDebitRemmitanceRejects[0].OriginalDirectDebitRemmitanceMessageID);
            Assert.AreEqual(2, paymentStatusReport.DirectDebitRemmitanceRejects[0].NumberOfTransactions);
            Assert.AreEqual((decimal)657.73, paymentStatusReport.DirectDebitRemmitanceRejects[0].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList1, paymentStatusReport.DirectDebitRemmitanceRejects[0].OriginalEndtoEndTransactionIdentificationList);

            //Info from second DirectDectDebitRemmitance
            List<string> expectedOriginalEndtoEndTransactionIdentificationList2 = new List<string>()
            {"15M/022581120151204"};
            Assert.AreEqual("2015-12-0113442815Rem.151 Ord.1", paymentStatusReport.DirectDebitRemmitanceRejects[1].OriginalDirectDebitRemmitanceMessageID);
            Assert.AreEqual(1, paymentStatusReport.DirectDebitRemmitanceRejects[1].NumberOfTransactions);
            Assert.AreEqual((decimal)277.45, paymentStatusReport.DirectDebitRemmitanceRejects[1].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList2, paymentStatusReport.DirectDebitRemmitanceRejects[1].OriginalEndtoEndTransactionIdentificationList);

            //Info from third DirectDectDebitRemmitance
            List<string> expectedOriginalEndtoEndTransactionIdentificationList3 = new List<string>()
            {"15M/026530120151204"};
            Assert.AreEqual("2015-12-0115153115Rem.152 Ord.1", paymentStatusReport.DirectDebitRemmitanceRejects[2].OriginalDirectDebitRemmitanceMessageID);
            Assert.AreEqual(1, paymentStatusReport.DirectDebitRemmitanceRejects[1].NumberOfTransactions);
            Assert.AreEqual((decimal)71.47, paymentStatusReport.DirectDebitRemmitanceRejects[2].ControlSum);
            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList2, paymentStatusReport.DirectDebitRemmitanceRejects[1].OriginalEndtoEndTransactionIdentificationList);
        }
    }
}
