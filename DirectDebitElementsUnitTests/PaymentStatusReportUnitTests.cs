using System;
using System.Collections.Generic;
using System.Linq;
using DirectDebitElements;
using DirectDebitElements.DirectDebitClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class PaymentStatusReportUnitTests
    {

        [TestMethod]
        public void ADirectDebitTransactionRejectIsCorrectlyCreated()
        {
            string originalTransactionIdentification = "0123456789";
            string originalEndtoEndTransactionIdentification = "2015120100124";
            DateTime requestedCollectionDate = DateTime.Parse("2015-12-01");
            decimal amount = 10;
            string mandateID = "000001102564";
            BankAccount debtorAccount = new BankAccount(new InternationalAccountBankNumberIBAN("ES6812345678061234567890"));
            string rejectReason = "MS02";

            DirectDebitTransactionReject directDebitTransactionReject = new DirectDebitTransactionReject(
                originalTransactionIdentification,
                originalEndtoEndTransactionIdentification,
                requestedCollectionDate,
                amount,
                mandateID,
                debtorAccount,
                rejectReason);

            Assert.AreEqual(originalTransactionIdentification, directDebitTransactionReject.OriginalTransactionIdentification);
            Assert.AreEqual(originalEndtoEndTransactionIdentification, directDebitTransactionReject.OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual(requestedCollectionDate, directDebitTransactionReject.RequestedCollectionDate);
            Assert.AreEqual(amount, directDebitTransactionReject.Amount);
            Assert.AreEqual(mandateID, directDebitTransactionReject.MandateID);
            Assert.AreEqual(debtorAccount, directDebitTransactionReject.DebtorAccount);
            Assert.AreEqual(rejectReason, directDebitTransactionReject.RejectReason);
        }

        [TestMethod]
        public void ADirectDebitRemmitanceRejectIsCorrectlyCreated()
        {
            string originalDirectDebitRemmitanceMessageID = "PRE201207010001";
            int numberOfTransactions = 2;
            decimal controlSum = 150;

            DirectDebitTransactionReject paymentTransactionReject1 = new DirectDebitTransactionReject(
                "0123456788",
                "2015120100124",
                 DateTime.Parse("2015-12-01"),
                80,
                "000001102564",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES6812345678061234567890")),
                "MS02");

            DirectDebitTransactionReject paymentTransactionReject2 = new DirectDebitTransactionReject(
                "0123456789",
                "2015120100312",
                DateTime.Parse("2015-12-01"),
                70,
                "00000110421",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES3011112222003333333333")),
                "MS01");

            List<DirectDebitTransactionReject> directDebitTransactionRejects = 
                new List<DirectDebitTransactionReject>()
                { paymentTransactionReject1, paymentTransactionReject2 };

            DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitanceMessageID, numberOfTransactions, controlSum, directDebitTransactionRejects);

            Assert.AreEqual(originalDirectDebitRemmitanceMessageID, directDebitRemmitanceReject.OriginalDirectDebitRemmitanceMessageID);
            Assert.AreEqual(numberOfTransactions, directDebitRemmitanceReject.NumberOfTransactions);
            Assert.AreEqual(controlSum, directDebitRemmitanceReject.ControlSum);
            Assert.AreEqual(2, directDebitRemmitanceReject.DirectDebitTransactionRejects.Count);
            Assert.AreEqual(2, directDebitRemmitanceReject.DirectDebitTransactionRejects.Count);
            Assert.AreEqual("2015120100124", directDebitRemmitanceReject.DirectDebitTransactionRejects[0].OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual("2015120100312", directDebitRemmitanceReject.DirectDebitTransactionRejects[1].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        public void AnEmptyDirectDebitRemmitanceRejectIsCorrectlyCreated()
        {
            string originalDirectDebitRemmitanceMessageID = "PRE201207010001";
            int numberOfTransactions = 0;
            decimal controlSum = 0;
            List<DirectDebitTransactionReject> directDebitTransactionRejects = new List<DirectDebitTransactionReject>();

            DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitanceMessageID, numberOfTransactions, controlSum, directDebitTransactionRejects);

            Assert.AreEqual(originalDirectDebitRemmitanceMessageID, directDebitRemmitanceReject.OriginalDirectDebitRemmitanceMessageID);
            Assert.AreEqual(0, directDebitRemmitanceReject.NumberOfTransactions);
            Assert.AreEqual(0, directDebitRemmitanceReject.ControlSum);
            Assert.AreEqual(0, directDebitRemmitanceReject.DirectDebitTransactionRejects.Count);
        }

        [TestMethod]
        public void ADirectDebitTransactionRejectIsCorrectlyAddedToADirctDebitRemmitanceReject()
        {
            string originalDirectDebitRemmitanceMessageID = "PRE201207010001";
            int numberOfTransactions = 0;
            decimal controlSum = 0;
            List<DirectDebitTransactionReject> directDebitTransactionRejects = new List<DirectDebitTransactionReject>();

            DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitanceMessageID, numberOfTransactions, controlSum, directDebitTransactionRejects);

            DirectDebitTransactionReject directDebitTransactionReject = new DirectDebitTransactionReject(
                "0123456788",
                "2015120100124",
                DateTime.Parse("2015-12-01"),
                80,
                "000001102564",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES6812345678061234567890")),
                "MS02");
            directDebitRemmitanceReject.AddDirectDebitTransactionReject(directDebitTransactionReject);

            Assert.AreEqual(1, directDebitRemmitanceReject.NumberOfTransactions);
            Assert.AreEqual(80, directDebitRemmitanceReject.ControlSum);
            Assert.AreEqual(1, directDebitRemmitanceReject.DirectDebitTransactionRejects.Count);
            Assert.AreEqual("2015120100124", directDebitRemmitanceReject.DirectDebitTransactionRejects[0].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        public void ICanGetaListofAllTheOriginalEndtoEndTransactionIdentificationInADirectDebitTransactionRemmitanceReject()
        {
            string originalDirectDebitRemmitanceMessageID = "PRE201207010001";
            int numberOfTransactions = 2;
            decimal controlSum = 150;

            DirectDebitTransactionReject paymentTransactionReject1 = new DirectDebitTransactionReject(
                "0123456788",
                "2015120100124",
                 DateTime.Parse("2015-12-01"),
                80,
                "000001102564",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES6812345678061234567890")),
                "MS02");

            DirectDebitTransactionReject paymentTransactionReject2 = new DirectDebitTransactionReject(
                "0123456789",
                "2015120100312",
                DateTime.Parse("2015-12-01"),
                70,
                "00000110421",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES3011112222003333333333")),
                "MS01");

            List<DirectDebitTransactionReject> directDebitTransactionRejects =
                new List<DirectDebitTransactionReject>()
                { paymentTransactionReject1, paymentTransactionReject2 };

            DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitanceMessageID, numberOfTransactions, controlSum, directDebitTransactionRejects);

            List<string> expectedOriginalEndtoEndTransactionIdentificationList = new List<string>()
            { "2015120100124", "2015120100312"};

            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList, directDebitRemmitanceReject.OriginalEndtoEndTransactionIdentificationList);
        }

        [TestMethod]
        public void APaymentStatusReportIsCorrectlyCreated()
        {
            ///Direct Debit Remmitance Reject 1

            string originalDirectDebitRemmitance1MessageID = "PRE201512010001";

            DirectDebitTransactionReject paymentTransactionReject1 = new DirectDebitTransactionReject(
                "0123456788",
                "2015120100124",
                 DateTime.Parse("2015-12-01"),
                80,
                "000001102564",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES6812345678061234567890")),
                "MS02");

            DirectDebitTransactionReject paymentTransactionReject2 = new DirectDebitTransactionReject(
                "0123456789",
                "2015120100312",
                DateTime.Parse("2015-12-01"),
                70,
                "00000110421",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES3011112222003333333333")),
                "MS01");

            List<DirectDebitTransactionReject> directDebitTransactionRejectsList1 = new List<DirectDebitTransactionReject>()
            { paymentTransactionReject1, paymentTransactionReject2 };

            DirectDebitRemmitanceReject directDebitRemmitanceReject1 = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1.Count,
                directDebitTransactionRejectsList1.Select(ddTransactionReject => ddTransactionReject.Amount).Sum(),
                directDebitTransactionRejectsList1);

            ///Direct Debit Remmitance Reject 2

            string originalDirectDebitRemmitance2MessageID = "PRE201511150001";

            DirectDebitTransactionReject paymentTransactionReject3 = new DirectDebitTransactionReject(
                "0123456788",
                "2015150100124",
                 DateTime.Parse("2015-11-15"),
                80,
                "000001102564",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES6812345678061234567890")),
                "MS02");

            List<DirectDebitTransactionReject> directDebitTransactionRejectsList2 = new List<DirectDebitTransactionReject>()
            { paymentTransactionReject3 };

            DirectDebitRemmitanceReject directDebitRemmitanceReject2 = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitance2MessageID,
                directDebitTransactionRejectsList2.Count,
                directDebitTransactionRejectsList2.Select(ddTransactionReject => ddTransactionReject.Amount).Sum(),
                directDebitTransactionRejectsList2);

            //Payment Status Report
            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = directDebitRemmitanceReject1.NumberOfTransactions + directDebitRemmitanceReject2.NumberOfTransactions;
            decimal controlSum = directDebitRemmitanceReject1.ControlSum + directDebitRemmitanceReject2.ControlSum;
            List<DirectDebitRemmitanceReject> directDebitRemmitanceRejectsList = new List<DirectDebitRemmitanceReject>()
            { directDebitRemmitanceReject1, directDebitRemmitanceReject2 };

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                numberOfTransactions,
                controlSum,
                directDebitRemmitanceRejectsList);

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(numberOfTransactions, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(controlSum, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(directDebitRemmitanceRejectsList, paymentStatusReport.DirectDebitRemmitanceRejects);
        }

    }
}
