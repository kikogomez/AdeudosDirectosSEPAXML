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

            directDebitTransactionRejectsList1 =
                new List<DirectDebitTransactionReject>()
                { directDebitTransactionReject1, directDebitTransactionReject2 };

            directDebitTransactionRejectsList2 =
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
        public void ADirectDebitPaymentInstructionRejectIsCorrectlyCreated()
        {

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);

            int numberOfTransactions = directDebitTransactionRejectsList1.Count;
            decimal controlSum = directDebitTransactionRejectsList1.Select(ddRemmitanceReject => ddRemmitanceReject.Amount).Sum();
            Assert.AreEqual(originalPaymentInformationID1, directDebitPaymentInstructionReject.OriginalPaymentInformationID);
            Assert.AreEqual(numberOfTransactions, directDebitPaymentInstructionReject.NumberOfTransactions);
            Assert.AreEqual(controlSum, directDebitPaymentInstructionReject.ControlSum);
            Assert.AreEqual("2015120100124", directDebitPaymentInstructionReject.DirectDebitTransactionsRejects[0].OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual("2015120100312", directDebitPaymentInstructionReject.DirectDebitTransactionsRejects[1].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        public void ADirectDebitPaymentInstructionRejectIsCorrectlyCreatedGivenACorrectNumberOfTransactionsAndControlSum()
        {
            int numberOfTransactions = directDebitTransactionRejectsList1.Count;
            decimal controlSum = directDebitTransactionRejectsList1.Select(ddRemmitanceReject => ddRemmitanceReject.Amount).Sum();
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                numberOfTransactions,
                controlSum,
                directDebitTransactionRejectsList1);

            Assert.AreEqual(originalPaymentInformationID1, directDebitPaymentInstructionReject.OriginalPaymentInformationID);
            Assert.AreEqual(numberOfTransactions, directDebitPaymentInstructionReject.NumberOfTransactions);
            Assert.AreEqual(controlSum, directDebitPaymentInstructionReject.ControlSum);
            Assert.AreEqual("2015120100124", directDebitPaymentInstructionReject.DirectDebitTransactionsRejects[0].OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual("2015120100312", directDebitPaymentInstructionReject.DirectDebitTransactionsRejects[1].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void IfGivenIncorrectNumberOfTransactionsTheDirectDebitPaymentInstructionRejectThrowsATypeInitializationErrorException()
        {
            int numberofTransactions = 1;
            decimal controlSum = 150;

            try
            {
                DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                    originalPaymentInformationID1,
                    numberofTransactions,
                    controlSum,
                    directDebitTransactionRejectsList1);
            }
            catch (TypeInitializationException typeInitializationException)
            {
                Assert.AreEqual("DirectDebitPaymentInstructionReject", typeInitializationException.TypeName);

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
        public void IfGivenIncorrectControlSumTheDirectDirectDebitPaymentInstructionRejectThrowsATypeInitializationErrorException()
        {
            int numberofTransactions = 2;
            decimal controlSum = 100;

            try
            {
                DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                    originalPaymentInformationID1,
                    numberofTransactions,
                    controlSum,
                    directDebitTransactionRejectsList1);
            }
            catch (TypeInitializationException typeInitializationException)
            {
                Assert.AreEqual("DirectDebitPaymentInstructionReject", typeInitializationException.TypeName);

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
            List<DirectDebitTransactionReject> directDebitTransactionRejects = new List<DirectDebitTransactionReject>();

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID,
                directDebitTransactionRejects);

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

            directDebitPaymentInstructionReject.AddDirectDebitTransactionReject(directDebitTransactionReject1);

            Assert.AreEqual(1, directDebitPaymentInstructionReject.NumberOfTransactions);
            Assert.AreEqual(80, directDebitPaymentInstructionReject.ControlSum);
            Assert.AreEqual(1, directDebitPaymentInstructionReject.DirectDebitTransactionsRejects.Count);
            Assert.AreEqual("2015120100124", directDebitPaymentInstructionReject.DirectDebitTransactionsRejects[0].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        public void ICanGetaListofAllTheOriginalEndtoEndTransactionIdentificationInADirectDebitPaymentInstructionReject()
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);

            List<string> expectedOriginalEndtoEndTransactionIdentificationList = new List<string>()
            { "2015120100124", "2015120100312"};

            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList, directDebitPaymentInstructionReject.OriginalEndtoEndTransactionInternalUniqueInstructionIDList);
        }

        [TestMethod]
        public void APaymentStatusReportIsCorrectlyCreated()
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject2 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID2,
                directDebitTransactionRejectsList2);

            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>()
            { directDebitPaymentInstructionReject1, directDebitPaymentInstructionReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = directDebitPaymentInstructionReject1.NumberOfTransactions + directDebitPaymentInstructionReject2.NumberOfTransactions;
            decimal controlSum = directDebitPaymentInstructionReject1.ControlSum + directDebitPaymentInstructionReject2.ControlSum;

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitPaymentInstructionRejects);

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(numberOfTransactions, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(controlSum, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(directDebitPaymentInstructionRejects, paymentStatusReport.DirectDebitPaymentInstructionRejects);
        }

        [TestMethod]
        public void APaymentStatusReportIsCorrectlyCreatedGivenACorrectNumberOfTransactionsAndControlSum()
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject2 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID2,
                directDebitTransactionRejectsList2);

            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>()
            { directDebitPaymentInstructionReject1, directDebitPaymentInstructionReject2 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = directDebitPaymentInstructionReject1.NumberOfTransactions + directDebitPaymentInstructionReject2.NumberOfTransactions;
            decimal controlSum = directDebitPaymentInstructionReject1.ControlSum + directDebitPaymentInstructionReject2.ControlSum;

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                numberOfTransactions,
                controlSum,
                directDebitPaymentInstructionRejects);

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(numberOfTransactions, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(controlSum, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(directDebitPaymentInstructionRejects, paymentStatusReport.DirectDebitPaymentInstructionRejects);
        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void IfGivenIncorrectNumberOfTransactionsThePaymentStatusReportThrowsATypeInitializationException()
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject2 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID2,
                directDebitTransactionRejectsList2);

            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>()
            { directDebitPaymentInstructionReject1, directDebitPaymentInstructionReject2 };

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
                    directDebitPaymentInstructionRejects);
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
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject2 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID2,
                directDebitTransactionRejectsList2);

            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>()
            { directDebitPaymentInstructionReject1, directDebitPaymentInstructionReject2 };

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
                    directDebitPaymentInstructionRejects);
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
            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>();

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitPaymentInstructionRejects);

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(0, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(0, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(directDebitPaymentInstructionRejects, paymentStatusReport.DirectDebitPaymentInstructionRejects);
        }

        [TestMethod]
        public void ICanAddMoreRemmitanceRejectsToAnExistingPaymentStatusReport()
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);

            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>()
            { directDebitPaymentInstructionReject1 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitPaymentInstructionRejects);

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject2 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID2,
                directDebitTransactionRejectsList2);

            paymentStatusReport.AddDirectDebitPaymentInstructionReject(directDebitPaymentInstructionReject2);

            List<DirectDebitPaymentInstructionReject> expectedDirectDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>()
            {directDebitPaymentInstructionReject1, directDebitPaymentInstructionReject2 };

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(3, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(230, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(expectedDirectDebitPaymentInstructionRejects, paymentStatusReport.DirectDebitPaymentInstructionRejects);
        }

        [TestMethod]
        public void IfIAddANewDirecDebitTransactionRejectTheTotalNumberOfTransactionsAndAmountOfAPaymentStatusReportIsUpdated()
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject1 = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID1,
                directDebitTransactionRejectsList1);
            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects =
                new List<DirectDebitPaymentInstructionReject>() {directDebitPaymentInstructionReject1};
            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitPaymentInstructionRejects);
            DirectDebitTransactionReject newDirectDebitTransactionReject = directDebitTransactionRejectsList2[0];

            paymentStatusReport.DirectDebitPaymentInstructionRejects[0].AddDirectDebitTransactionReject(newDirectDebitTransactionReject);

            Assert.AreEqual(3, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(230, paymentStatusReport.ControlSum);
        }
    }
}
