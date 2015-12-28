using System;
using System.Collections.Generic;
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

            DirectDebitTransactionReject paymentTransactionReject = new DirectDebitTransactionReject(
                originalTransactionIdentification,
                originalEndtoEndTransactionIdentification,
                requestedCollectionDate,
                amount,
                mandateID,
                debtorAccount,
                rejectReason);

            Assert.AreEqual(originalTransactionIdentification, paymentTransactionReject.OriginalTransactionIdentification);
            Assert.AreEqual(originalEndtoEndTransactionIdentification, paymentTransactionReject.OriginalEndtoEndTransactionIdentification);
            Assert.AreEqual(requestedCollectionDate, paymentTransactionReject.RequestedCollectionDate);
            Assert.AreEqual(amount, paymentTransactionReject.Amount);
            Assert.AreEqual(mandateID, paymentTransactionReject.MandateID);
            Assert.AreEqual(debtorAccount, paymentTransactionReject.DebtorAccount);
            Assert.AreEqual(rejectReason, paymentTransactionReject.RejectReason);
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
        public void TheDirectDebitTransactionRejectInternalReferenceListIsCorreclyExtractedFromARejectedTransactionGroup()
        {
            //Starting with an XDocument(linq), extract all <OrgnlEndToEndId>
            string xMLFilePath = @"XML Test Files\pain.002.001.03\LaCaixa_pain00200103_Example1.xml";
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ICanSequentiallyExtractAllOriginalEndtoEndOriginalTransactionsIDIntoaList()
        {
            //If the XML message is HUGE, I can extract the references by going through sequentilly all nodes
            string xMLFilePath = "";
            Assert.Inconclusive();
        }

    }
}
