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
        static string originalPaymentInformationID1;
        static string originalPaymentInformationID2;

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

            originalPaymentInformationID1 = "PRE201512010001";
            originalPaymentInformationID2 = "PRE201511150001";
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
        public void ADirectDebitPaymentInstructionRejectIsCorrectlyCreated()
        {
            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = paymentStatusReportManager.CreateDirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionsRejectsList1);

            Assert.AreEqual(originalPaymentInformationID1, directDebitPaymentInstructionReject.OriginalPaymentInformationID);
            Assert.AreEqual(2, directDebitPaymentInstructionReject.NumberOfTransactions);
            Assert.AreEqual(150, directDebitPaymentInstructionReject.ControlSum);
            Assert.AreEqual(2, directDebitPaymentInstructionReject.DirectDebitTransactionsRejects.Count);
            Assert.AreEqual("2015120100124", directDebitPaymentInstructionReject.DirectDebitTransactionsRejects[0].OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual("2015120100312", directDebitPaymentInstructionReject.DirectDebitTransactionsRejects[1].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        public void ADirectDebitPaymentInstructionRejectIsCorrectlyCreatedGivenACorrectNumberOfTransactionsAndControlSum()
        {
            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            int numberofTransactions = 2;
            decimal controlSum = 150;

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = paymentStatusReportManager.CreateCheckedDirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                numberofTransactions,
                controlSum,
                directDebitTransactionsRejectsList1);

            Assert.AreEqual(originalPaymentInformationID1, directDebitPaymentInstructionReject.OriginalPaymentInformationID);
            Assert.AreEqual(2, directDebitPaymentInstructionReject.NumberOfTransactions);
            Assert.AreEqual(150, directDebitPaymentInstructionReject.ControlSum);
            Assert.AreEqual(2, directDebitPaymentInstructionReject.DirectDebitTransactionsRejects.Count);
            Assert.AreEqual("2015120100124", directDebitPaymentInstructionReject.DirectDebitTransactionsRejects[0].OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual("2015120100312", directDebitPaymentInstructionReject.DirectDebitTransactionsRejects[1].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void IfGivenIncorrectNumberOfTransactionsTheDirectDebitPaymentInstructionRejectThrowsATypeInitializationException()
        {
            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            int numberofTransactions = 1;
            decimal controlSum = 150;

            try
            {
                DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = paymentStatusReportManager.CreateCheckedDirectDebitPaymentInstructionReject(
                    originalPaymentInformationID1,
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
        public void IfGivenIncorrectControlSumTheDirectDebitPaymentInstructionRejectThrowsATypeInitializationException()
        {
            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            int numberofTransactions = 2;
            decimal controlSum = 100;

            try
            {
                DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = paymentStatusReportManager.CreateCheckedDirectDebitPaymentInstructionReject(
                    originalPaymentInformationID1,
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
        public void AnEmptyDirectDebitPaymentInstructionRejectIsCorrectlyCreated()
        {
            string originalPaymentInformationID = "PRE201207010001";

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = paymentStatusReportManager.CreateAnEmptyDirectDebitPaymentInstructionReject(originalPaymentInformationID);

            Assert.AreEqual(originalPaymentInformationID, directDebitPaymentInstructionReject.OriginalPaymentInformationID);
            Assert.AreEqual(0, directDebitPaymentInstructionReject.NumberOfTransactions);
            Assert.AreEqual(0, directDebitPaymentInstructionReject.ControlSum);
            Assert.AreEqual(0, directDebitPaymentInstructionReject.DirectDebitTransactionsRejects.Count);
        }

        [TestMethod]
        public void ADirectDebitTransactionRejectIsCorrectlyAddedToADirectDebitPaymentInstructionReject()
        {
            string originalPaymentInformationID = "PRE201207010001";
            List<DirectDebitTransactionReject> directDebitTransactionRejects = new List<DirectDebitTransactionReject>();

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID,
                directDebitTransactionRejects);

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            paymentStatusReportManager.AddRejectedTransactionToPaymentInstructionReject(directDebitPaymentInstructionReject, directDebitTransactionReject1);

            Assert.AreEqual(originalPaymentInformationID, directDebitPaymentInstructionReject.OriginalPaymentInformationID);
            Assert.AreEqual(1, directDebitPaymentInstructionReject.NumberOfTransactions);
            Assert.AreEqual(80, directDebitPaymentInstructionReject.ControlSum);
            Assert.AreEqual(1, directDebitPaymentInstructionReject.DirectDebitTransactionsRejects.Count);
        }

        [TestMethod]
        public void ACheckedPaymentStatusReportIsCorrectlyCreatedGivenACorrectNumberOfTransactionsAndControlSum()
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionsRejectsList1);

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject2 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID2,
                directDebitTransactionsRejectsList2);

            List<DirectDebitPaymentInstructionReject> directDebitRemmitanceRejectsList = new List<DirectDebitPaymentInstructionReject>()
            { directDebitPaymentInstructionReject1, directDebitPaymentInstructionReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = directDebitPaymentInstructionReject1.NumberOfTransactions + directDebitPaymentInstructionReject2.NumberOfTransactions;
            decimal controlSum = directDebitPaymentInstructionReject1.ControlSum + directDebitPaymentInstructionReject2.ControlSum;

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
            CollectionAssert.AreEqual(directDebitRemmitanceRejectsList, paymentStatusReport.DirectDebitPaymentInstructionRejects);
        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void IfGivenIncorrectNumberOfTransactionsTheCheckedPaymentStatusReportThrowsATypeInitializationException()
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionsRejectsList1);

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject2 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID2,
                directDebitTransactionsRejectsList2);

            List<DirectDebitPaymentInstructionReject> directDebitRemmitanceRejectsList = new List<DirectDebitPaymentInstructionReject>()
            { directDebitPaymentInstructionReject1, directDebitPaymentInstructionReject2 };

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
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionsRejectsList1);

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject2 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID2,
                directDebitTransactionsRejectsList2);

            List<DirectDebitPaymentInstructionReject> directDebitRemmitanceRejectsList = new List<DirectDebitPaymentInstructionReject>()
            { directDebitPaymentInstructionReject1, directDebitPaymentInstructionReject2 };

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
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionsRejectsList1);

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject2 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID2,
                directDebitTransactionsRejectsList2);

            List<DirectDebitPaymentInstructionReject> directDebitRemmitanceRejectsList = new List<DirectDebitPaymentInstructionReject>()
            { directDebitPaymentInstructionReject1, directDebitPaymentInstructionReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = directDebitPaymentInstructionReject1.NumberOfTransactions + directDebitPaymentInstructionReject2.NumberOfTransactions;
            decimal controlSum = directDebitPaymentInstructionReject1.ControlSum + directDebitPaymentInstructionReject2.ControlSum;

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
            CollectionAssert.AreEqual(directDebitRemmitanceRejectsList, paymentStatusReport.DirectDebitPaymentInstructionRejects);
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
            Assert.AreEqual(0, paymentStatusReport.DirectDebitPaymentInstructionRejects.Count);
        }

        [TestMethod]
        public void ADirectDebitPaymentInstructionRejectIsCorrectlyAddedToAPaymentStatusReport()
        {
            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");

            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>();

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitPaymentInstructionRejects);

            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionsRejectsList1);

            paymentStatusReportManager.AddRejectedDirectDebitPaymentInstructionToPaymentStatusReport(paymentStatusReport, directDebitPaymentInstructionReject1);

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(2, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(150, paymentStatusReport.ControlSum);
            Assert.AreEqual(1, paymentStatusReport.DirectDebitPaymentInstructionRejects.Count);
        }
    }
}
