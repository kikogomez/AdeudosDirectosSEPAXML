using System;
using System.Collections.Generic;
using System.Linq;
using DirectDebitElements;
using ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);

            Assert.AreEqual(originalDirectDebitRemmitance1MessageID, directDebitRemmitanceReject.OriginalDirectDebitRemmitanceMessageID);
            Assert.AreEqual(2, directDebitRemmitanceReject.NumberOfTransactions);
            Assert.AreEqual(150, directDebitRemmitanceReject.ControlSum);
            Assert.AreEqual("2015120100124", directDebitRemmitanceReject.DirectDebitTransactionRejects[0].OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual("2015120100312", directDebitRemmitanceReject.DirectDebitTransactionRejects[1].OriginalEndtoEndTransactionIdentification);
        }

        //[TestMethod]
        //[ExpectedException(typeof(System.ArgumentException))]
        //public void IfTheProvidedNumberOfTransactionsInARemmitanceRejectIsWorgAnExceptionIsThrown()
        //{
        //    try
        //    {
        //        DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
        //            originalDirectDebitRemmitance1MessageID,
        //            3,
        //            150,
        //            directDebitTransactionRejectsList1);
        //    }
        //    catch (System.ArgumentException e)
        //    {
        //        Assert.AreEqual("numberOfTransactions", e.ParamName);
        //        Assert.AreEqual("The Number of Transactions is wrong. Provided: 3. Expected: 2. Initialized with expected value", e.GetMessageWithoutParamName());
        //        throw;
        //    }
        //}

        //[TestMethod]
        //public void IfTheProvidedNumberOfTransactionsInARemmitanceRejectIsWorgItIsCorrected()
        //{
        //    try
        //    {
        //        DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
        //            originalDirectDebitRemmitance1MessageID,
        //            3,
        //            150,
        //            directDebitTransactionRejectsList1);
        //        Assert.AreEqual(2, directDebitRemmitanceReject.NumberOfTransactions);
        //    }
        //    catch (ArgumentException) { }
        //}

        //[TestMethod]
        //[ExpectedException(typeof(System.ArgumentException))]
        //public void IfTheProvidedControlSumInARemmitanceRejectIsWorgAnExceptionIsThrown()
        //{
        //    try
        //    {
        //        DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
        //            originalDirectDebitRemmitance1MessageID,
        //            2,
        //            100,
        //            directDebitTransactionRejectsList1);
        //    }
        //    catch (System.ArgumentException e)
        //    {
        //        Assert.AreEqual("controlSum", e.ParamName);
        //        Assert.AreEqual("The Control Sum is wrong. Provided: 100. Expected: 150. Initialized with expected value", e.GetMessageWithoutParamName());
        //        throw;
        //    }
        //}

        //[TestMethod]
        //public void IfTheProvidedControlSumInARemmitanceRejectIsWorgItIsCorrected()
        //{
        //    try
        //    {
        //        DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
        //            originalDirectDebitRemmitance1MessageID,
        //            2,
        //            100,
        //            directDebitTransactionRejectsList1);
        //        Assert.AreEqual(150, directDebitRemmitanceReject.ControlSum);
        //    }
        //    catch (ArgumentException) { }
        //}

        [TestMethod]
        public void AnEmptyDirectDebitRemmitanceRejectIsCorrectlyCreated()
        {
            string originalDirectDebitRemmitanceMessageID = "PRE201207010001";
            List<DirectDebitTransactionReject> directDebitTransactionRejects = new List<DirectDebitTransactionReject>();

            DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitanceMessageID,
                directDebitTransactionRejects);

            Assert.AreEqual(originalDirectDebitRemmitanceMessageID, directDebitRemmitanceReject.OriginalDirectDebitRemmitanceMessageID);
            Assert.AreEqual(0, directDebitRemmitanceReject.NumberOfTransactions);
            Assert.AreEqual(0, directDebitRemmitanceReject.ControlSum);
            Assert.AreEqual(0, directDebitRemmitanceReject.DirectDebitTransactionRejects.Count);
        }

        [TestMethod]
        public void ADirectDebitTransactionRejectIsCorrectlyAddedToADirctDebitRemmitanceReject()
        {
            string originalDirectDebitRemmitanceMessageID = "PRE201207010001";
            List<DirectDebitTransactionReject> directDebitTransactionRejects = new List<DirectDebitTransactionReject>();

            DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitanceMessageID,
                directDebitTransactionRejects);

            directDebitRemmitanceReject.AddDirectDebitTransactionReject(directDebitTransactionReject1);

            Assert.AreEqual(1, directDebitRemmitanceReject.NumberOfTransactions);
            Assert.AreEqual(80, directDebitRemmitanceReject.ControlSum);
            Assert.AreEqual(1, directDebitRemmitanceReject.DirectDebitTransactionRejects.Count);
            Assert.AreEqual("2015120100124", directDebitRemmitanceReject.DirectDebitTransactionRejects[0].OriginalEndtoEndTransactionIdentification);
        }

        [TestMethod]
        public void ICanGetaListofAllTheOriginalEndtoEndTransactionIdentificationInADirectDebitTransactionRemmitanceReject()
        {
            DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);

            List<string> expectedOriginalEndtoEndTransactionIdentificationList = new List<string>()
            { "2015120100124", "2015120100312"};

            CollectionAssert.AreEqual(expectedOriginalEndtoEndTransactionIdentificationList, directDebitRemmitanceReject.OriginalEndtoEndTransactionIdentificationList);
        }

        [TestMethod]
        public void APaymentStatusReportIsCorrectlyCreated()
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
            CollectionAssert.AreEqual(directDebitRemmitanceRejectsList, paymentStatusReport.DirectDebitRemmitanceRejects);
        }

        //[TestMethod]
        //[ExpectedException(typeof(System.ArgumentException))]
        //public void IfTheProvidedNumberOfTransactionsInAPaymentStatusReportIsWorgAnExceptionIsThrown()
        //{
        //    try
        //    {
        //        DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
        //            originalDirectDebitRemmitance1MessageID,
        //            3,
        //            150,
        //            directDebitTransactionRejectsList1);
        //    }
        //    catch (System.ArgumentException e)
        //    {
        //        Assert.AreEqual("numberOfTransactions", e.ParamName);
        //        Assert.AreEqual("The Number of Transactions is wrong. Provided: 3. Expected: 2. Initialized with expected value", e.GetMessageWithoutParamName());
        //        throw;
        //    }
        //    Assert.Inconclusive();
        //}

        //[TestMethod]
        //public void IfTheProvidedNumberOfTransactionsInAPaymentStatusReportIsWorgItIsCorrected()
        //{
        //    try
        //    {
        //        DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
        //            originalDirectDebitRemmitance1MessageID,
        //            3,
        //            150,
        //            directDebitTransactionRejectsList1);
        //        Assert.AreEqual(2, directDebitRemmitanceReject.NumberOfTransactions);
        //    }
        //    catch (ArgumentException) { }
        //    Assert.Inconclusive();
        //}

        //[TestMethod]
        //[ExpectedException(typeof(System.ArgumentException))]
        //public void IfTheProvidedControlSumInAPaymentStatusReportIsWorgAnExceptionIsThrown()
        //{
        //    try
        //    {
        //        DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
        //            originalDirectDebitRemmitance1MessageID,
        //            2,
        //            100,
        //            directDebitTransactionRejectsList1);
        //    }
        //    catch (System.ArgumentException e)
        //    {
        //        Assert.AreEqual("controlSum", e.ParamName);
        //        Assert.AreEqual("The Control Sum is wrong. Provided: 100. Expected: 150. Initialized with expected value", e.GetMessageWithoutParamName());
        //        throw;
        //    }
        //    Assert.Inconclusive();
        //}

        //[TestMethod]
        //public void IfTheProvidedControlSumInAPaymentStatusReportIsWorgItIsCorrected()
        //{
        //    try
        //    {
        //        DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
        //            originalDirectDebitRemmitance1MessageID,
        //            2,
        //            100,
        //            directDebitTransactionRejectsList1);
        //        Assert.AreEqual(150, directDebitRemmitanceReject.ControlSum);
        //    }
        //    catch (ArgumentException) { }
        //    Assert.Inconclusive();
        //}

        [TestMethod]
        public void AnEmptyPaymentStatusReportIsCorrectlyCreated()
        {
            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            List<DirectDebitRemmitanceReject> directDebitRemmitanceRejectsList = new List<DirectDebitRemmitanceReject>();

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
            CollectionAssert.AreEqual(directDebitRemmitanceRejectsList, paymentStatusReport.DirectDebitRemmitanceRejects);
        }

        [TestMethod]
        public void ICanAddMoreRemmitanceRejectsToAnExistingPaymentStatusReport()
        {
            DirectDebitRemmitanceReject directDebitRemmitanceReject1 = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitance1MessageID,
                directDebitTransactionRejectsList1);

            List<DirectDebitRemmitanceReject> directDebitRemmitanceRejectsList = new List<DirectDebitRemmitanceReject>()
            { directDebitRemmitanceReject1 };

            string messageID = "DATIR00112G12345678100";
            DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
            DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int numberOfTransactions = directDebitRemmitanceRejectsList.Select(ddRemmitanceReject => ddRemmitanceReject.NumberOfTransactions).Sum();
            decimal controlSum = directDebitRemmitanceRejectsList.Select(ddRemmitanceReject => ddRemmitanceReject.ControlSum).Sum();

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                numberOfTransactions,
                controlSum,
                directDebitRemmitanceRejectsList);

            DirectDebitRemmitanceReject directDebitRemmitanceReject2 = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitance2MessageID,
                directDebitTransactionRejectsList2);

            paymentStatusReport.AddRemmitance(directDebitRemmitanceReject2);

            List<DirectDebitRemmitanceReject> expectedDirectDebitRemmitanceRejectsList = new List<DirectDebitRemmitanceReject>()
            {directDebitRemmitanceReject1, directDebitRemmitanceReject2 };

            Assert.AreEqual("DATIR00112G12345678100", paymentStatusReport.MessageID);
            Assert.AreEqual(messageCreationDateTime, paymentStatusReport.MessageCreationDateTime);
            Assert.AreEqual(rejectAccountChargeDateTime, paymentStatusReport.RejectAccountChargeDateTime);
            Assert.AreEqual(3, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(230, paymentStatusReport.ControlSum);
            CollectionAssert.AreEqual(expectedDirectDebitRemmitanceRejectsList, paymentStatusReport.DirectDebitRemmitanceRejects);
        }

        [TestMethod]
        public void IfIAddANewDirecDebitTransactionRejectTheTotalNumberOfTransactionsAndAmountOfAPaymentStatusReportIsUpdated()
        {
            //Creating payment status report

            string originalDirectDebitRemmitanceMessageID = "PRE201207010001";
            DirectDebitTransactionReject directDebitTransactionReject = new DirectDebitTransactionReject(
                "0123456788",
                "2015120100124",
                DateTime.Parse("2015-12-01"),
                80,
                "000001102564",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES6812345678061234567890")),
                "MS02");
            List<DirectDebitTransactionReject> directDebitTransactionRejectList = new List<DirectDebitTransactionReject>()
            { directDebitTransactionReject };

            DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitanceMessageID,
                directDebitTransactionRejectList);

            string paymentStatusReportMessageID = "DATIR00112G12345678100";
            DateTime paymentStatusReportMessaceCreationDate = DateTime.Parse("2012-07-18T06:00:01");
            DateTime paymentStatusReportRejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
            int paymentStatusReportNumberOfTransactions = 1;
            decimal paymentStatusReportControlSum = 80;
            List<DirectDebitRemmitanceReject> directDebitRemmitanceRejectsList = new List<DirectDebitRemmitanceReject>()
            { directDebitRemmitanceReject };

            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                paymentStatusReportMessageID,
                paymentStatusReportMessaceCreationDate,
                paymentStatusReportRejectAccountChargeDateTime,
                paymentStatusReportNumberOfTransactions,
                paymentStatusReportControlSum,
                directDebitRemmitanceRejectsList);

            //Adding new DirectDebitTransactionReject

            DirectDebitTransactionReject newDirectDebitTransactionReject = new DirectDebitTransactionReject(
                "0123456789",
                "2015120100312",
                DateTime.Parse("2015-12-01"),
                70,
                "00000110421",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES3011112222003333333333")),
                "MS01");

            paymentStatusReport.DirectDebitRemmitanceRejects[0].AddDirectDebitTransactionReject(newDirectDebitTransactionReject);

            Assert.AreEqual(2, paymentStatusReport.NumberOfTransactions);
            Assert.AreEqual(150, paymentStatusReport.ControlSum);
        }
    }
}
