using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectDebitElements;
using ExtensionMethods;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class PaymentStatusReportManagerTests
    {
        static DirectDebitTransactionReject directDebitTransactionReject1;
        static DirectDebitTransactionReject directDebitTransactionReject2;
        static DirectDebitTransactionReject directDebitTransactionReject3;
        static List<DirectDebitTransactionReject> directDebitTransactionsRejectsList1;
        static List<DirectDebitTransactionReject> directDebitTransactionsRejectsList2;
        static string originalDirectDebitTransactionsGroupPayment1PaymentInformationID;
        static string originalDirectDebitTransactionsGroupPayment2PaymentInformationID;

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

            directDebitTransactionsRejectsList1 =
                new List<DirectDebitTransactionReject>()
                { directDebitTransactionReject1, directDebitTransactionReject2 };

            directDebitTransactionsRejectsList2 =
                new List<DirectDebitTransactionReject>()
                { directDebitTransactionReject3};

            originalDirectDebitTransactionsGroupPayment1PaymentInformationID = "PRE201512010001";
            originalDirectDebitTransactionsGroupPayment2PaymentInformationID = "PRE201511150001";
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
        public void ADirectDebitTransactionsGroupPaymentRejectIsCorrectlyCreated()
        {
            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            DirectDebitTransactionsGroupPaymentReject directDebitTransactionsGroupPaymentReject = paymentStatusReportManager.CreateDirectDebitTransactionGroupPaymentReject(
                originalDirectDebitTransactionsGroupPayment1PaymentInformationID,
                directDebitTransactionsRejectsList1);

            Assert.AreEqual(originalDirectDebitTransactionsGroupPayment1PaymentInformationID, directDebitTransactionsGroupPaymentReject.OriginalDirectDebitTransactionsGroupPaymentPaymentInformationID);
            Assert.AreEqual(2, directDebitTransactionsGroupPaymentReject.NumberOfTransactions);
            Assert.AreEqual(150, directDebitTransactionsGroupPaymentReject.ControlSum);
            Assert.AreEqual(2, directDebitTransactionsGroupPaymentReject.DirectDebitTransactionsRejects.Count);
            Assert.AreEqual("2015120100124", directDebitTransactionsGroupPaymentReject.DirectDebitTransactionsRejects[0].OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual("2015120100312", directDebitTransactionsGroupPaymentReject.DirectDebitTransactionsRejects[1].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        public void ADirectDebitRemmitanceRejectIsCorrectlyCreatedGivenACorrectNumberOfTransactionsAndControlSum()
        {
            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            int numberofTransactions = 2;
            decimal controlSum = 150;

            DirectDebitTransactionsGroupPaymentReject directDebitTransactionsGroupPaymentReject = paymentStatusReportManager.CreateCheckedDirectDebitTransactionGroupPaymentReject(
                originalDirectDebitTransactionsGroupPayment1PaymentInformationID,
                numberofTransactions,
                controlSum,
                directDebitTransactionsRejectsList1);

            Assert.AreEqual(originalDirectDebitTransactionsGroupPayment1PaymentInformationID, directDebitTransactionsGroupPaymentReject.OriginalDirectDebitTransactionsGroupPaymentPaymentInformationID);
            Assert.AreEqual(2, directDebitTransactionsGroupPaymentReject.NumberOfTransactions);
            Assert.AreEqual(150, directDebitTransactionsGroupPaymentReject.ControlSum);
            Assert.AreEqual(2, directDebitTransactionsGroupPaymentReject.DirectDebitTransactionsRejects.Count);
            Assert.AreEqual("2015120100124", directDebitTransactionsGroupPaymentReject.DirectDebitTransactionsRejects[0].OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual("2015120100312", directDebitTransactionsGroupPaymentReject.DirectDebitTransactionsRejects[1].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void IfGivenIncorrectNumberOfTransactionsTheDirectDebitRemmitanceRejectThrowsATypeInitializationException()
        {
            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            int numberofTransactions = 1;
            decimal controlSum = 150;

            try
            {
                DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject = paymentStatusReportManager.CreateCheckedDirectDebitTransactionGroupPaymentReject(
                    originalDirectDebitTransactionsGroupPayment1PaymentInformationID,
                    numberofTransactions,
                    controlSum,
                    directDebitTransactionsRejectsList1);
            }
            catch (TypeInitializationException typeInitializationException)
            {
                string expectedErrorMessage = "The Number of Transactions is wrong. It should be 2, but 1 is provided";
                ArgumentException argumentException = (ArgumentException)typeInitializationException.InnerException;
                string paramName = argumentException.ParamName;
                string exceptionMessage = argumentException.GetMessageWithoutParamName();
                Assert.AreEqual("numberOfTransactions", paramName);
                Assert.AreEqual(expectedErrorMessage, exceptionMessage);
                throw typeInitializationException;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void IfGivenIncorrectControlSumTheDirectDebirRemmitanceRejectIsCorrectlyCreatedButAnErrorMessageIsGenerated()
        {
            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            int numberofTransactions = 2;
            decimal controlSum = 100;

            try
            {
                DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject = paymentStatusReportManager.CreateCheckedDirectDebitTransactionGroupPaymentReject(
                    originalDirectDebitTransactionsGroupPayment1PaymentInformationID,
                    numberofTransactions,
                    controlSum,
                    directDebitTransactionsRejectsList1);
            }
            catch (TypeInitializationException typeInitializationException)
            {
                string expectedErrorMessage = "The Control Sum is wrong. It should be 150, but 100 is provided";
                ArgumentException argumentException = (ArgumentException)typeInitializationException.InnerException;
                string paramName = argumentException.ParamName;
                string exceptionMessage = argumentException.GetMessageWithoutParamName();
                Assert.AreEqual("controlSum", paramName);
                Assert.AreEqual(expectedErrorMessage, exceptionMessage);
                throw typeInitializationException;
            }
        }

        [TestMethod]
        public void AnEmptyDirectDebiTransactionsGroupPaymentRejectIsCorrectlyCreated()
        {
            string originalDirectDebitTransactionsGroupPaymentPaymentInformationID = "PRE201207010001";

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            DirectDebitTransactionsGroupPaymentReject directDebitTransactionsGroupPaymentReject = paymentStatusReportManager.CreateAnEmptyDirectDebitTransactionGroupPaymentReject(originalDirectDebitTransactionsGroupPaymentPaymentInformationID);

            Assert.AreEqual(originalDirectDebitTransactionsGroupPaymentPaymentInformationID, directDebitTransactionsGroupPaymentReject.OriginalDirectDebitTransactionsGroupPaymentPaymentInformationID);
            Assert.AreEqual(0, directDebitTransactionsGroupPaymentReject.NumberOfTransactions);
            Assert.AreEqual(0, directDebitTransactionsGroupPaymentReject.ControlSum);
            Assert.AreEqual(0, directDebitTransactionsGroupPaymentReject.DirectDebitTransactionsRejects.Count);
        }

        [TestMethod]
        public void ADirectDebitTransactionRejectIsCorrectlyAddedToADirectDebitRemmitanceReject()
        {
            string originalDirectDebitTransactionsGroupPaymentPaymentInformationID = "PRE201207010001";
            List<DirectDebitTransactionReject> directDebitTransactionRejects = new List<DirectDebitTransactionReject>();

            DirectDebitTransactionsGroupPaymentReject directDebitTransactionsGroupPaymentReject = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitTransactionsGroupPaymentPaymentInformationID,
                directDebitTransactionRejects);

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            paymentStatusReportManager.AddRejectedTransactionToTransactionsGroupPaymentReject(directDebitTransactionsGroupPaymentReject, directDebitTransactionReject1);

            Assert.AreEqual(originalDirectDebitTransactionsGroupPaymentPaymentInformationID, directDebitTransactionsGroupPaymentReject.OriginalDirectDebitTransactionsGroupPaymentPaymentInformationID);
            Assert.AreEqual(1, directDebitTransactionsGroupPaymentReject.NumberOfTransactions);
            Assert.AreEqual(80, directDebitTransactionsGroupPaymentReject.ControlSum);
            Assert.AreEqual(1, directDebitTransactionsGroupPaymentReject.DirectDebitTransactionsRejects.Count);
        }

        [TestMethod]
        public void ACheckedPaymentStatusReportIsCorrectlyCreatedGivenACorrectNumberOfTransactionsAndControlSum()
        {
            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject1 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitTransactionsGroupPayment1PaymentInformationID,
                directDebitTransactionsRejectsList1);

            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject2 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitTransactionsGroupPayment2PaymentInformationID,
                directDebitTransactionsRejectsList2);

            List<DirectDebitTransactionsGroupPaymentReject> directDebitRemmitanceRejectsList = new List<DirectDebitTransactionsGroupPaymentReject>()
            { directDebitRemmitanceReject1, directDebitRemmitanceReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = directDebitRemmitanceReject1.NumberOfTransactions + directDebitRemmitanceReject2.NumberOfTransactions;
            decimal controlSum = directDebitRemmitanceReject1.ControlSum + directDebitRemmitanceReject2.ControlSum;

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            PaymentStatusReport paymentStatusReport = paymentStatusReportManager.CreateCheckedPaymentStatusReport(
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
            CollectionAssert.AreEqual(directDebitRemmitanceRejectsList, paymentStatusReport.DirectDebitTransactionsGroupPaymentRejects);
        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void IfGivenIncorrectNumberOfTransactionsTheCheckedPaymentStatusReportThrowsATypeInitializationException()
        {
            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject1 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitTransactionsGroupPayment1PaymentInformationID,
                directDebitTransactionsRejectsList1);

            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject2 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitTransactionsGroupPayment2PaymentInformationID,
                directDebitTransactionsRejectsList2);

            List<DirectDebitTransactionsGroupPaymentReject> directDebitRemmitanceRejectsList = new List<DirectDebitTransactionsGroupPaymentReject>()
            { directDebitRemmitanceReject1, directDebitRemmitanceReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = 2;
            decimal controlSum = 230;

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            try
            {
                PaymentStatusReport paymentStatusReport = paymentStatusReportManager.CreateCheckedPaymentStatusReport(
                    messageID,
                    messageCreationDateTime,
                    rejectAccountChargeDateTime,
                    numberOfTransactions,
                    controlSum,
                    directDebitRemmitanceRejectsList);
            }
            catch (TypeInitializationException typeInitializationException)
            {
                string expectedErrorMessage = "The Number of Transactions is wrong. It should be 3, but 2 is provided";
                ArgumentException argumentException = (ArgumentException)typeInitializationException.InnerException;
                string paramName = argumentException.ParamName;
                string exceptionMessage = argumentException.GetMessageWithoutParamName();
                Assert.AreEqual("numberOfTransactions", paramName);
                Assert.AreEqual(expectedErrorMessage, exceptionMessage);
                throw typeInitializationException;
            }



        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void IfGivenIncorrectControlSumTheCheckedPaymentStatusReportThrowsATypeInitializationException()
        {
            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject1 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitTransactionsGroupPayment1PaymentInformationID,
                directDebitTransactionsRejectsList1);

            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject2 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitTransactionsGroupPayment2PaymentInformationID,
                directDebitTransactionsRejectsList2);

            List<DirectDebitTransactionsGroupPaymentReject> directDebitRemmitanceRejectsList = new List<DirectDebitTransactionsGroupPaymentReject>()
            { directDebitRemmitanceReject1, directDebitRemmitanceReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = 3;
            decimal controlSum = 330;

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            try
            {
                PaymentStatusReport paymentStatusReport = paymentStatusReportManager.CreateCheckedPaymentStatusReport(
                    messageID,
                    messageCreationDateTime,
                    rejectAccountChargeDateTime,
                    numberOfTransactions,
                    controlSum,
                    directDebitRemmitanceRejectsList);
            }
            catch (TypeInitializationException typeInitializationException)
            {
                string expectedErrorMessage = "The Control Sum is wrong. It should be 230, but 330 is provided";
                ArgumentException argumentException = (ArgumentException)typeInitializationException.InnerException;
                string paramName = argumentException.ParamName;
                string exceptionMessage = argumentException.GetMessageWithoutParamName();
                Assert.AreEqual("controlSum", paramName);
                Assert.AreEqual(expectedErrorMessage, exceptionMessage);
                throw typeInitializationException;
            }
        }

        [TestMethod]
        public void APaymentStatusReportIsCorrectlyCreatedWithoutProvidingNumberOfTransactionsNorControlSum()
        {
            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject1 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitTransactionsGroupPayment1PaymentInformationID,
                directDebitTransactionsRejectsList1);

            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject2 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitTransactionsGroupPayment2PaymentInformationID,
                directDebitTransactionsRejectsList2);

            List<DirectDebitTransactionsGroupPaymentReject> directDebitRemmitanceRejectsList = new List<DirectDebitTransactionsGroupPaymentReject>()
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
            CollectionAssert.AreEqual(directDebitRemmitanceRejectsList, paymentStatusReport.DirectDebitTransactionsGroupPaymentRejects);
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
            Assert.AreEqual(0, paymentStatusReport.DirectDebitTransactionsGroupPaymentRejects.Count);
        }

        [TestMethod]
        public void ADirectDebitRemmitanceIsCorrectlyAddedToAPaymentStatusReport()
        {
            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");

            List<DirectDebitTransactionsGroupPaymentReject> directDebitRemmitanceRejects = new List<DirectDebitTransactionsGroupPaymentReject>();

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitRemmitanceRejects);

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject1 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitTransactionsGroupPayment1PaymentInformationID,
                directDebitTransactionsRejectsList1);

            paymentStatusReportManager.AddRejectedTransactionsGroupPaymentToPaymentStatusReport(paymentStatusReport, directDebitRemmitanceReject1);

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(2, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(150, paymentStatusReport.ControlSum);
            Assert.AreEqual(1, paymentStatusReport.DirectDebitTransactionsGroupPaymentRejects.Count);
        }
    }
}
