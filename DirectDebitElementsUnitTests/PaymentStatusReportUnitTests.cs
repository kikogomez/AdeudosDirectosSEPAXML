using System;
using System.Collections.Generic;
using System.Linq;
using DirectDebitElements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExtensionMethods;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class PaymentStatusReportUnitTests
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

            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);

            int numberOfTransactions = directDebitTransactionRejectsList1.Count;
            decimal controlSum = directDebitTransactionRejectsList1.Select(ddRemmitanceReject => ddRemmitanceReject.Amount).Sum();
            Assert.AreEqual(originalDirectDebitRemmitance1MessageID, directDebitRemmitanceReject.OriginalDirectDebitTransactionsGroupPaymentPaymentInformationID);
            Assert.AreEqual(numberOfTransactions, directDebitRemmitanceReject.NumberOfTransactions);
            Assert.AreEqual(controlSum, directDebitRemmitanceReject.ControlSum);
            Assert.AreEqual("2015120100124", directDebitRemmitanceReject.DirectDebitTransactionsRejects[0].OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual("2015120100312", directDebitRemmitanceReject.DirectDebitTransactionsRejects[1].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        public void ADirectDebitRemmitanceRejectIsCorrectlyCreatedGivenACorrectNumberOfTransactionsAndControlSum()
        {
            int numberOfTransactions = directDebitTransactionRejectsList1.Count;
            decimal controlSum = directDebitTransactionRejectsList1.Select(ddRemmitanceReject => ddRemmitanceReject.Amount).Sum();
            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitRemmitance1MessageID,
                numberOfTransactions,
                controlSum,
                directDebitTransactionRejectsList1);

            Assert.AreEqual(originalDirectDebitRemmitance1MessageID, directDebitRemmitanceReject.OriginalDirectDebitTransactionsGroupPaymentPaymentInformationID);
            Assert.AreEqual(numberOfTransactions, directDebitRemmitanceReject.NumberOfTransactions);
            Assert.AreEqual(controlSum, directDebitRemmitanceReject.ControlSum);
            Assert.AreEqual("2015120100124", directDebitRemmitanceReject.DirectDebitTransactionsRejects[0].OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual("2015120100312", directDebitRemmitanceReject.DirectDebitTransactionsRejects[1].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void IfGivenIncorrectNumberOfTransactionsTheDirectDebitRemmitanceRejectThrowsATypeInitializationErrorException()
        {
            int numberofTransactions = 1;
            decimal controlSum = 150;

            try
            {
                DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject = new DirectDebitTransactionsGroupPaymentReject(
                    originalDirectDebitRemmitance1MessageID,
                    numberofTransactions,
                    controlSum,
                    directDebitTransactionRejectsList1);
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
        public void IfGivenIncorrectControlSumTheDirectDebitRemmitanceRejectThrowsATypeInitializationErrorException()
        {
            int numberofTransactions = 2;
            decimal controlSum = 100;

            try
            {
                DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject = new DirectDebitTransactionsGroupPaymentReject(
                    originalDirectDebitRemmitance1MessageID,
                    numberofTransactions,
                    controlSum,
                    directDebitTransactionRejectsList1);
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
        public void AnEmptyDirectDebitTransactionsGroupPaymentRejectIsCorrectlyCreated()
        {
            string originalDirectDebitTransactionsGroupPaymentPaymentInformationID = "PRE201207010001";
            List<DirectDebitTransactionReject> directDebitTransactionRejects = new List<DirectDebitTransactionReject>();

            DirectDebitTransactionsGroupPaymentReject directDebitTransactionsGroupPaymentReject = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitTransactionsGroupPaymentPaymentInformationID,
                directDebitTransactionRejects);

            Assert.AreEqual(originalDirectDebitTransactionsGroupPaymentPaymentInformationID, directDebitTransactionsGroupPaymentReject.OriginalDirectDebitTransactionsGroupPaymentPaymentInformationID);
            Assert.AreEqual(0, directDebitTransactionsGroupPaymentReject.NumberOfTransactions);
            Assert.AreEqual(0, directDebitTransactionsGroupPaymentReject.ControlSum);
            Assert.AreEqual(0, directDebitTransactionsGroupPaymentReject.DirectDebitTransactionsRejects.Count);
        }

        [TestMethod]
        public void ADirectDebitTransactionRejectIsCorrectlyAddedToADirctDebitTransactionsGroupPaymentReject()
        {
            string originalDirectDebitTransactionsGroupPaymentPaymentInformationID = "PRE201207010001";
            List<DirectDebitTransactionReject> directDebitTransactionRejects = new List<DirectDebitTransactionReject>();

            DirectDebitTransactionsGroupPaymentReject directDebitTransactionsGroupPaymentReject = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitTransactionsGroupPaymentPaymentInformationID,
                directDebitTransactionRejects);

            directDebitTransactionsGroupPaymentReject.AddDirectDebitTransactionReject(directDebitTransactionReject1);

            Assert.AreEqual(1, directDebitTransactionsGroupPaymentReject.NumberOfTransactions);
            Assert.AreEqual(80, directDebitTransactionsGroupPaymentReject.ControlSum);
            Assert.AreEqual(1, directDebitTransactionsGroupPaymentReject.DirectDebitTransactionsRejects.Count);
            Assert.AreEqual("2015120100124", directDebitTransactionsGroupPaymentReject.DirectDebitTransactionsRejects[0].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        public void ICanGetaListofAllTheOriginalEndtoEndTransactionIdentificationInADirectDebitTransactionRemmitanceReject()
        {
            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);

            List<string> expectedOriginalEndtoEndTransactionIdentificationList = new List<string>()
            { "2015120100124", "2015120100312"};

            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList, directDebitRemmitanceReject.OriginalEndtoEndTransactionInternalUniqueInstructionIDList);
        }

        [TestMethod]
        public void APaymentStatusReportIsCorrectlyCreated()
        {
            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject1 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);

            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject2 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitRemmitance2MessageID,
                directDebitTransactionRejectsList2);

            List<DirectDebitTransactionsGroupPaymentReject> directDebitRemmitanceRejectsList = new List<DirectDebitTransactionsGroupPaymentReject>()
            { directDebitRemmitanceReject1, directDebitRemmitanceReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = directDebitRemmitanceReject1.NumberOfTransactions + directDebitRemmitanceReject2.NumberOfTransactions;
            decimal controlSum = directDebitRemmitanceReject1.ControlSum + directDebitRemmitanceReject2.ControlSum;

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
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
        public void APaymentStatusReportIsCorrectlyCreatedGivenACorrectNumberOfTransactionsAndControlSum()
        {
            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject1 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);

            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject2 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitRemmitance2MessageID,
                directDebitTransactionRejectsList2);

            List<DirectDebitTransactionsGroupPaymentReject> directDebitRemmitanceRejectsList = new List<DirectDebitTransactionsGroupPaymentReject>()
            { directDebitRemmitanceReject1, directDebitRemmitanceReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = directDebitRemmitanceReject1.NumberOfTransactions + directDebitRemmitanceReject2.NumberOfTransactions;
            decimal controlSum = directDebitRemmitanceReject1.ControlSum + directDebitRemmitanceReject2.ControlSum;

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
            CollectionAssert.AreEqual(directDebitRemmitanceRejectsList, paymentStatusReport.DirectDebitTransactionsGroupPaymentRejects);
        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void IfGivenIncorrectNumberOfTransactionsThePaymentStatusReportThrowsATypeInitializationException()
        {
            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject1 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);

            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject2 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitRemmitance2MessageID,
                directDebitTransactionRejectsList2);

            List<DirectDebitTransactionsGroupPaymentReject> directDebitRemmitanceRejectsList = new List<DirectDebitTransactionsGroupPaymentReject>()
            { directDebitRemmitanceReject1, directDebitRemmitanceReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = 2;
            decimal controlSum = 230;

            try
            {
                PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
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
        public void IfGivenIncorrectControlSumThePaymentStatusReportThrowsATypeInitializationException()
        {
            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject1 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);

            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject2 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitRemmitance2MessageID,
                directDebitTransactionRejectsList2);

            List<DirectDebitTransactionsGroupPaymentReject> directDebitRemmitanceRejectsList = new List<DirectDebitTransactionsGroupPaymentReject>()
            { directDebitRemmitanceReject1, directDebitRemmitanceReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = 3;
            decimal controlSum = 330;

            try
            {
                PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
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
        public void AnEmptyPaymentStatusReportIsCorrectlyCreated()
        {
            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            List<DirectDebitTransactionsGroupPaymentReject> directDebitRemmitanceRejectsList = new List<DirectDebitTransactionsGroupPaymentReject>();

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitRemmitanceRejectsList);

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(0, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(0, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(directDebitRemmitanceRejectsList, paymentStatusReport.DirectDebitTransactionsGroupPaymentRejects);
        }

        [TestMethod]
        public void ICanAddMoreRemmitanceRejectsToAnExistingPaymentStatusReport()
        {
            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject1 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);

            List<DirectDebitTransactionsGroupPaymentReject> directDebitRemmitanceRejectsList = new List<DirectDebitTransactionsGroupPaymentReject>()
            { directDebitRemmitanceReject1 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitRemmitanceRejectsList);

            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject2 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitRemmitance2MessageID,
                directDebitTransactionRejectsList2);

            paymentStatusReport.AddTransactionsGroupPaymentReject(directDebitRemmitanceReject2);

            List<DirectDebitTransactionsGroupPaymentReject> expectedDirectDebitRemmitanceRejectsList = new List<DirectDebitTransactionsGroupPaymentReject>()
            {directDebitRemmitanceReject1, directDebitRemmitanceReject2 };

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(3, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(230, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(expectedDirectDebitRemmitanceRejectsList, paymentStatusReport.DirectDebitTransactionsGroupPaymentRejects);
        }

        [TestMethod]
        public void IfIAddANewDirecDebitTransactionRejectTheTotalNumberOfTransactionsAndAmountOfAPaymentStatusReportIsUpdated()
        {
            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceReject1 = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);
            List<DirectDebitTransactionsGroupPaymentReject> directDebitRemmitanceRejectsList = new List<DirectDebitTransactionsGroupPaymentReject>() {directDebitRemmitanceReject1};
            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitRemmitanceRejectsList);
            DirectDebitTransactionReject newDirectDebitTransactionReject = directDebitTransactionRejectsList2[0];

            paymentStatusReport.DirectDebitTransactionsGroupPaymentRejects[0].AddDirectDebitTransactionReject(newDirectDebitTransactionReject);

            Assert.AreEqual(3, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(230, paymentStatusReport.ControlSum);
        }
    }
}
