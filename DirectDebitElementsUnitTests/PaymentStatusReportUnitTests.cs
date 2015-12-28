using System;
using DirectDebitElements;
using DirectDebitElements.DirectDebitClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class PaymentStatusReportUnitTests
    {


        [TestMethod]
        public void APaymentTransactionRejectIsCorrectlyCreated()
        {

            string originalTransactionIdentification = "0123456789";
            string originalEndtoEndTransactionIdentification = "2015120100124";
            DateTime requestedCollectionDate = DateTime.Parse("2015-12-01");
            decimal amount = 10;
            string mandateID = "000001102564";
            BankAccount debtorAccount = new BankAccount(new InternationalAccountBankNumberIBAN("ES6812345678061234567890"));
            string rejectReason = "MS02";

            PaymentTransactionReject paymentTransactionReject = new PaymentTransactionReject(
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
