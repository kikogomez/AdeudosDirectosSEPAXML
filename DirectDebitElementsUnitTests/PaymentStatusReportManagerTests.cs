using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectDebitElements;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class PaymentStatusReportManagerTests
    {
        static DirectDebitTransactionReject directDebitTransactionReject1;
        static DirectDebitTransactionReject directDebitTransactionReject2;
        static DirectDebitTransactionReject directDebitTransactionReject3;
        static List<DirectDebitTransactionReject> directDebitTransactionRejectsList1;
        static List<DirectDebitTransactionReject> directDebitTransactionRejectsList2;
        static string originalDirectDebitRemmitance1MessageID;
        static string originalDirectDebitRemmitance2MessageID;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            directDebitTransactionReject1 = new DirectDebitTransactionReject(
                "0123456788",
                "2015120100124",
                DateTime.Parse("2015-12-01"),
                80,
                "000001102564",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES6812345678061234567890")),
                "MS02");

            directDebitTransactionReject2 = new DirectDebitTransactionReject(
                "0123456789",
                "2015120100312",
                DateTime.Parse("2015-12-01"),
                70,
                "00000110421",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES3011112222003333333333")),
                "MS01");

            directDebitTransactionReject3 = new DirectDebitTransactionReject(
                "0123456790",
                "2015150100124",
                DateTime.Parse("2015-11-15"),
                80,
                "000001102564",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES6812345678061234567890")),
                "MS02");

            directDebitTransactionRejectsList1 =
                new List<DirectDebitTransactionReject>()
                { directDebitTransactionReject1, directDebitTransactionReject2 };

            directDebitTransactionRejectsList2 =
                new List<DirectDebitTransactionReject>()
                { directDebitTransactionReject3};

            originalDirectDebitRemmitance1MessageID = "PRE201512010001";
            originalDirectDebitRemmitance2MessageID = "PRE201511150001";
        }

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

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            DirectDebitTransactionReject directDebitTransactionReject = paymentStatusReportManager.CreateDirectDebitTransactionReject(
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
            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            DirectDebitRemmitanceReject directDebitRemmitanceReject = paymentStatusReportManager.CreateDirectDebitRemmitanceReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);

            Assert.AreEqual(originalDirectDebitRemmitance1MessageID, directDebitRemmitanceReject.OriginalDirectDebitRemmitanceMessageID);
            Assert.AreEqual(2, directDebitRemmitanceReject.NumberOfTransactions);
            Assert.AreEqual(150, directDebitRemmitanceReject.ControlSum);
            Assert.AreEqual("2015120100124", directDebitRemmitanceReject.DirectDebitTransactionRejects[0].OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual("2015120100312", directDebitRemmitanceReject.DirectDebitTransactionRejects[1].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        public void ADirectDebitRemmitanceRejectIsCorrectlyCreatedGivenACorrectNumberOfTransactionsAndControlSum()
        {
            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            int numberofTransactions = 2;
            decimal controlSum = 150;

            DirectDebitRemmitanceRejectCreationResult directDebitRemmitanceRejectCreationResult = paymentStatusReportManager.CreateDirectDebitRemmitanceReject(
                originalDirectDebitRemmitance1MessageID,
                numberofTransactions,
                controlSum,
                directDebitTransactionRejectsList1);

            DirectDebitRemmitanceReject directDebitRemmitanceReject = directDebitRemmitanceRejectCreationResult.DirectDebitRemmitanceReject;
            List<string> errorMessages = directDebitRemmitanceRejectCreationResult.ErrorMessages;

            Assert.AreEqual(0, errorMessages.Count);
            Assert.AreEqual(originalDirectDebitRemmitance1MessageID, directDebitRemmitanceReject.OriginalDirectDebitRemmitanceMessageID);
            Assert.AreEqual(2, directDebitRemmitanceReject.NumberOfTransactions);
            Assert.AreEqual(150, directDebitRemmitanceReject.ControlSum);
            Assert.AreEqual("2015120100124", directDebitRemmitanceReject.DirectDebitTransactionRejects[0].OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual("2015120100312", directDebitRemmitanceReject.DirectDebitTransactionRejects[1].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        public void IfGivenIncorrectNumberOfTransactionsTheDirectDebirRemmitanceRejectIsCorrectlyCreatedButAnErrorMessageIsGenerated()
        {
            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            int numberofTransactions = 1;
            decimal controlSum = 150;

            DirectDebitRemmitanceRejectCreationResult directDebitRemmitanceRejectCreationResult = paymentStatusReportManager.CreateDirectDebitRemmitanceReject(
                originalDirectDebitRemmitance1MessageID,
                numberofTransactions,
                controlSum,
                directDebitTransactionRejectsList1);

            DirectDebitRemmitanceReject directDebitRemmitanceReject = directDebitRemmitanceRejectCreationResult.DirectDebitRemmitanceReject;
            string errorMessage = directDebitRemmitanceRejectCreationResult.ErrorMessages[0];
            string expectedErrorMessage = "The Number of Transactions is wrong. Provided: 1. Expected: 2. Initialized with expected value";

            Assert.AreEqual(expectedErrorMessage, errorMessage);
            Assert.AreEqual(originalDirectDebitRemmitance1MessageID, directDebitRemmitanceReject.OriginalDirectDebitRemmitanceMessageID);
            Assert.AreEqual(2, directDebitRemmitanceReject.NumberOfTransactions);
            Assert.AreEqual(150, directDebitRemmitanceReject.ControlSum);
            Assert.AreEqual("2015120100124", directDebitRemmitanceReject.DirectDebitTransactionRejects[0].OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual("2015120100312", directDebitRemmitanceReject.DirectDebitTransactionRejects[1].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        public void IfGivenIncorrectControlSumTheDirectDebirRemmitanceRejectIsCorrectlyCreatedButAnErrorMessageIsGenerated()
        {
            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            int numberofTransactions = 2;
            decimal controlSum = 100;

            DirectDebitRemmitanceRejectCreationResult directDebitRemmitanceRejectCreationResult = paymentStatusReportManager.CreateDirectDebitRemmitanceReject(
                originalDirectDebitRemmitance1MessageID,
                numberofTransactions,
                controlSum,
                directDebitTransactionRejectsList1);

            DirectDebitRemmitanceReject directDebitRemmitanceReject = directDebitRemmitanceRejectCreationResult.DirectDebitRemmitanceReject;
            string errorMessage = directDebitRemmitanceRejectCreationResult.ErrorMessages[0];
            string expectedErrorMessage = "The Control Sum is wrong. Provided: 100. Expected: 150. Initialized with expected value";

            Assert.AreEqual(expectedErrorMessage, errorMessage);
            Assert.AreEqual(originalDirectDebitRemmitance1MessageID, directDebitRemmitanceReject.OriginalDirectDebitRemmitanceMessageID);
            Assert.AreEqual(2, directDebitRemmitanceReject.NumberOfTransactions);
            Assert.AreEqual(150, directDebitRemmitanceReject.ControlSum);
            Assert.AreEqual("2015120100124", directDebitRemmitanceReject.DirectDebitTransactionRejects[0].OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual("2015120100312", directDebitRemmitanceReject.DirectDebitTransactionRejects[1].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        public void IfGivenBothIncorrectNumberOfTransactionsAndIncorrectControlSumTheDirectDebirRemmitanceRejectIsCorrectlyCreatedButAnErrorMessageIsGenerated()
        {
            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            int numberofTransactions = 1;
            decimal controlSum = 100;

            DirectDebitRemmitanceRejectCreationResult directDebitRemmitanceRejectCreationResult = paymentStatusReportManager.CreateDirectDebitRemmitanceReject(
                originalDirectDebitRemmitance1MessageID,
                numberofTransactions,
                controlSum,
                directDebitTransactionRejectsList1);

            DirectDebitRemmitanceReject directDebitRemmitanceReject = directDebitRemmitanceRejectCreationResult.DirectDebitRemmitanceReject;
            List<string> errorMessages = directDebitRemmitanceRejectCreationResult.ErrorMessages;
            List<string> expectedErrorMessages = new List<string>()
            {
                "The Number of Transactions is wrong. Provided: 1. Expected: 2. Initialized with expected value",
                "The Control Sum is wrong. Provided: 100. Expected: 150. Initialized with expected value"
            };

            CollectionAssert.AreEqual(expectedErrorMessages, errorMessages);
            Assert.AreEqual(originalDirectDebitRemmitance1MessageID, directDebitRemmitanceReject.OriginalDirectDebitRemmitanceMessageID);
            Assert.AreEqual(2, directDebitRemmitanceReject.NumberOfTransactions);
            Assert.AreEqual(150, directDebitRemmitanceReject.ControlSum);
            Assert.AreEqual("2015120100124", directDebitRemmitanceReject.DirectDebitTransactionRejects[0].OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual("2015120100312", directDebitRemmitanceReject.DirectDebitTransactionRejects[1].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        public void AnEmptyDirectDebitRemmitanceRejectIsCorrectlyCreated()
        {
            string originalDirectDebitRemmitanceMessageID = "PRE201207010001";

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            DirectDebitRemmitanceReject directDebitRemmitanceReject = paymentStatusReportManager.CreateAnEmptyDirectDebitRemmitanceReject(originalDirectDebitRemmitanceMessageID);

            Assert.AreEqual(originalDirectDebitRemmitanceMessageID, directDebitRemmitanceReject.OriginalDirectDebitRemmitanceMessageID);
            Assert.AreEqual(0, directDebitRemmitanceReject.NumberOfTransactions);
            Assert.AreEqual(0, directDebitRemmitanceReject.ControlSum);
            Assert.AreEqual(0, directDebitRemmitanceReject.DirectDebitTransactionRejects.Count);
        }

        [TestMethod]
        public void ADirectDebitTransactionRejectIsCorrectlyAddedToADirectDebitRemmitanceReject()
        {
            string originalDirectDebitRemmitanceMessageID = "PRE201207010001";
            List<DirectDebitTransactionReject> directDebitTransactionRejects = new List<DirectDebitTransactionReject>();

            DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(originalDirectDebitRemmitanceMessageID, directDebitTransactionRejects);

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            paymentStatusReportManager.AddRejectedTransactionToRemmitanceReject(directDebitRemmitanceReject, directDebitTransactionReject1);

            Assert.AreEqual(originalDirectDebitRemmitanceMessageID, directDebitRemmitanceReject.OriginalDirectDebitRemmitanceMessageID);
            Assert.AreEqual(1, directDebitRemmitanceReject.NumberOfTransactions);
            Assert.AreEqual(80, directDebitRemmitanceReject.ControlSum);
            Assert.AreEqual(1, directDebitRemmitanceReject.DirectDebitTransactionRejects.Count);
        }

        [TestMethod]
        public void APaymentStatusReportIsCorrectlyCreatedGivenACorrectNumberOfTransactionsAndControlSum()
        {
            DirectDebitRemmitanceReject directDebitRemmitanceReject1 = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);

            DirectDebitRemmitanceReject directDebitRemmitanceReject2 = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitance2MessageID,
                directDebitTransactionRejectsList2);

            List<DirectDebitRemmitanceReject> directDebitRemmitanceRejectsList = new List<DirectDebitRemmitanceReject>()
            { directDebitRemmitanceReject1, directDebitRemmitanceReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = directDebitRemmitanceReject1.NumberOfTransactions + directDebitRemmitanceReject2.NumberOfTransactions;
            decimal controlSum = directDebitRemmitanceReject1.ControlSum + directDebitRemmitanceReject2.ControlSum;

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            PaymentStatusReportCreationResult paymentStatusReportCreationResult = paymentStatusReportManager.CreatePaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                numberOfTransactions,
                controlSum,
                directDebitRemmitanceRejectsList);

            PaymentStatusReport paymentStatusReport = paymentStatusReportCreationResult.PaymentStatusreport;
            List<string> errorMessages = paymentStatusReportCreationResult.ErrorMessages;

            Assert.AreEqual(0, errorMessages.Count);
            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(numberOfTransactions, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(controlSum, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(directDebitRemmitanceRejectsList, paymentStatusReport.DirectDebitRemmitanceRejects);
        }

        [TestMethod]
        public void IfGivenIncorrectNumberOfTransactionsThePaymentStatusReportIsCorrectlyCreatedButAnErrorMessageIsGenerated()
        {
            DirectDebitRemmitanceReject directDebitRemmitanceReject1 = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);

            DirectDebitRemmitanceReject directDebitRemmitanceReject2 = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitance2MessageID,
                directDebitTransactionRejectsList2);

            List<DirectDebitRemmitanceReject> directDebitRemmitanceRejectsList = new List<DirectDebitRemmitanceReject>()
            { directDebitRemmitanceReject1, directDebitRemmitanceReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = 2;
            decimal controlSum = 230;

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            PaymentStatusReportCreationResult paymentStatusReportCreationResult = paymentStatusReportManager.CreatePaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                numberOfTransactions,
                controlSum,
                directDebitRemmitanceRejectsList);

            PaymentStatusReport paymentStatusReport = paymentStatusReportCreationResult.PaymentStatusreport;
            string errorMessage = paymentStatusReportCreationResult.ErrorMessages[0];
            string expectedErrorMessage = "The Number of Transactions is wrong. Provided: 2. Expected: 3. Initialized with expected value";

            Assert.AreEqual(1, paymentStatusReportCreationResult.ErrorMessages.Count);
            Assert.AreEqual(expectedErrorMessage, errorMessage);
            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(3, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(230, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(directDebitRemmitanceRejectsList, paymentStatusReport.DirectDebitRemmitanceRejects);
        }

        [TestMethod]
        public void IfGivenIncorrectControlSumThePaymentStatusReportIsCorrectlyCreatedButAnErrorMessageIsGenerated()
        {
            DirectDebitRemmitanceReject directDebitRemmitanceReject1 = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);

            DirectDebitRemmitanceReject directDebitRemmitanceReject2 = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitance2MessageID,
                directDebitTransactionRejectsList2);

            List<DirectDebitRemmitanceReject> directDebitRemmitanceRejectsList = new List<DirectDebitRemmitanceReject>()
            { directDebitRemmitanceReject1, directDebitRemmitanceReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = 3;
            decimal controlSum = 330;

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            PaymentStatusReportCreationResult paymentStatusReportCreationResult = paymentStatusReportManager.CreatePaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                numberOfTransactions,
                controlSum,
                directDebitRemmitanceRejectsList);

            PaymentStatusReport paymentStatusReport = paymentStatusReportCreationResult.PaymentStatusreport;
            string errorMessage = paymentStatusReportCreationResult.ErrorMessages[0];
            string expectedErrorMessage = "The Control Sum is wrong. Provided: 330. Expected: 230. Initialized with expected value";

            Assert.AreEqual(1, paymentStatusReportCreationResult.ErrorMessages.Count);
            Assert.AreEqual(expectedErrorMessage, errorMessage);
            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(3, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(230, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(directDebitRemmitanceRejectsList, paymentStatusReport.DirectDebitRemmitanceRejects);
        }

        [TestMethod]
        public void IfGivenBothIncorrectNumberOfTransactionsAndIncorrectControlSumThePaymentStatusReportIsCorrectlyCreatedButAnErrorMessageIsGenerated()
        {
            DirectDebitRemmitanceReject directDebitRemmitanceReject1 = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);

            DirectDebitRemmitanceReject directDebitRemmitanceReject2 = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitance2MessageID,
                directDebitTransactionRejectsList2);

            List<DirectDebitRemmitanceReject> directDebitRemmitanceRejectsList = new List<DirectDebitRemmitanceReject>()
            { directDebitRemmitanceReject1, directDebitRemmitanceReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = 2;
            decimal controlSum = 330;

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            PaymentStatusReportCreationResult paymentStatusReportCreationResult = paymentStatusReportManager.CreatePaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                numberOfTransactions,
                controlSum,
                directDebitRemmitanceRejectsList);

            PaymentStatusReport paymentStatusReport = paymentStatusReportCreationResult.PaymentStatusreport;
            List<string> errorMessages = paymentStatusReportCreationResult.ErrorMessages;
            List<string> expectedErrorMessages = new List<string>()
            {
                "The Number of Transactions is wrong. Provided: 2. Expected: 3. Initialized with expected value",
                "The Control Sum is wrong. Provided: 330. Expected: 230. Initialized with expected value"
            };

            Assert.AreEqual(2, paymentStatusReportCreationResult.ErrorMessages.Count);
            CollectionAssert.AreEqual(expectedErrorMessages, errorMessages);
            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(3, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(230, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(directDebitRemmitanceRejectsList, paymentStatusReport.DirectDebitRemmitanceRejects);
        }

        [TestMethod]
        public void APaymentStatusReportIsCorrectlyCreatedWithoutProvidingNumberOfTransactionsNorControlSum()
        {
            DirectDebitRemmitanceReject directDebitRemmitanceReject1 = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);

            DirectDebitRemmitanceReject directDebitRemmitanceReject2 = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitance2MessageID,
                directDebitTransactionRejectsList2);

            List<DirectDebitRemmitanceReject> directDebitRemmitanceRejectsList = new List<DirectDebitRemmitanceReject>()
            { directDebitRemmitanceReject1, directDebitRemmitanceReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = directDebitRemmitanceReject1.NumberOfTransactions + directDebitRemmitanceReject2.NumberOfTransactions;
            decimal controlSum = directDebitRemmitanceReject1.ControlSum + directDebitRemmitanceReject2.ControlSum;

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            PaymentStatusReport paymentStatusReport = paymentStatusReportManager.CreatePaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitRemmitanceRejectsList);

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(numberOfTransactions, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(controlSum, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(directDebitRemmitanceRejectsList, paymentStatusReport.DirectDebitRemmitanceRejects);
        }

        [TestMethod]
        public void AnEmptyPaymentStatusReportIsCorrectlyCreated()
        {
            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            PaymentStatusReport paymentStatusReport = paymentStatusReportManager.CreateAnEmptyPaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime);

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(0, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(0, paymentStatusReport.ControlSum);
            Assert.AreEqual(0, paymentStatusReport.DirectDebitRemmitanceRejects.Count);
        }

        [TestMethod]
        public void ADirectDebitRemmitanceIsCorrectlyAddedToAPaymentStatusReport()
        {
            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");

            List<DirectDebitRemmitanceReject> directDebitRemmitanceRejects = new List<DirectDebitRemmitanceReject>();

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitRemmitanceRejects);

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            DirectDebitRemmitanceReject directDebitRemmitanceReject1 = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);

            paymentStatusReportManager.AddRejectedRemmitanceToPaymentStatusReport(paymentStatusReport, directDebitRemmitanceReject1);

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(2, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(150, paymentStatusReport.ControlSum);
            Assert.AreEqual(1, paymentStatusReport.DirectDebitRemmitanceRejects.Count);
        }




    }
}
