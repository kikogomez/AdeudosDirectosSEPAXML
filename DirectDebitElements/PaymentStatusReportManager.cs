using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectDebitElements
{
    public class PaymentStatusReportManager
    {
        public PaymentStatusReportManager() { }

        public PaymentStatusReport CreatePaymentStatusReport(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitRemmitanceReject> directDebitRemmitanceRejectsList)
        { 
            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                numberOfTransactions,
                controlSum,
                directDebitRemmitanceRejectsList);

            return paymentStatusReport;
        }

        public PaymentStatusReport CreatePaymentStatusReport(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime,
            List<DirectDebitRemmitanceReject> directDebitRemmitanceRejectsList)
        {
            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitRemmitanceRejectsList);

            return paymentStatusReport;
        }

        public DirectDebitRemmitanceReject CreateAnEmptyDirectDebitRemmitanceReject(string originalDirectDebitRemmitanceMessageID)
        {
            List<DirectDebitTransactionReject> directDebitTransactionRejects = new List<DirectDebitTransactionReject>();

            DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitanceMessageID,
                directDebitTransactionRejects);

            return directDebitRemmitanceReject;
        }

        public DirectDebitTransactionReject CreateDirectDebitTransactionReject(
            string originalTransactionIdentification,
            string originalEndtoEndTransactionIdentification,
            DateTime requestedCollectionDate,
            decimal amount,
            string mandateID,
            BankAccount debtorAccount,
            string rejectReason)

        {
            DirectDebitTransactionReject directDebitTransactionReject = new DirectDebitTransactionReject(
                originalTransactionIdentification,
                originalEndtoEndTransactionIdentification,
                requestedCollectionDate,
                amount,
                mandateID,
                debtorAccount,
                rejectReason);

            return directDebitTransactionReject;
        }

        public DirectDebitRemmitanceReject CreateDirectDebitRemmitanceReject(
            string originalDirectDebitRemmitanceMessageID,
            List<DirectDebitTransactionReject> directDebitTransactionRejectsList)
        {
            //int numberOfTransactions = directDebitTransactionRejectsList.Count;
            //decimal controlSum = directDebitTransactionRejectsList.Select(ddTransactionReject => ddTransactionReject.Amount).Sum();
            DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitanceMessageID,
                directDebitTransactionRejectsList);
            return directDebitRemmitanceReject;
        }

        public DirectDebitRemmitanceReject CreateDirectDebitRemmitanceReject(
            string originalDirectDebitRemmitanceMessageID,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitTransactionReject> directDebitTransactionRejectsList)
        {
            DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitanceMessageID,
                directDebitTransactionRejectsList);

            int calculatedNumberOfTransactions = directDebitTransactionRejectsList.Count;
            decimal calculatedControlSum = directDebitTransactionRejectsList.Select(ddTransactionReject => ddTransactionReject.Amount).Sum();
            if (numberOfTransactions != calculatedNumberOfTransactions)
                RiseNumberOfTransctionsArgumentException(numberOfTransactions, calculatedNumberOfTransactions);
            if (controlSum != calculatedControlSum)
                RiseControlSumArgumentException(controlSum, calculatedControlSum);

            return directDebitRemmitanceReject;
        }

        private void RiseNumberOfTransctionsArgumentException(int providedNumberOfTransactions, int calculatedNumberOfTransactions)
        {
            string exceptionMessage =
                "The Number of Transactions is wrong. Provided: " + providedNumberOfTransactions.ToString() + ". Expected: " + calculatedNumberOfTransactions.ToString() + ". Initialized with expected value";
            throw new ArgumentException(exceptionMessage, "numberOfTransactions");
        }

        private void RiseControlSumArgumentException(decimal providedControlSum, decimal calculatedControlSum)
        {
            string exceptionMessage =
                "The Control Sum is wrong. Provided: " + providedControlSum.ToString() + ". Expected: " + calculatedControlSum.ToString() + ". Initialized with expected value";
            throw new ArgumentException(exceptionMessage, "controlSum");
        }

        //private bool TheProvidedNumberOfTransactionsIsWrong(int numberOfTransactions, List<DirectDebitTransactionReject> directDebitTransactionRejectsList)
        //{
        //    int calculatedNumberOfTransactions = directDebitTransactionRejectsList.Count;
        //    return (numberOfTransactions != calculatedNumberOfTransactions);
        //}

        //private bool TheProvidedControlSumIsWrong()
        //{
        //    decimal calculatedControlSum = directDebitTransactionRejects.Select(ddRemmitanceReject => ddRemmitanceReject.Amount).Sum();
        //    return (controlSum != calculatedControlSum);
        //}

        //public DirectDebitRemmitanceReject(string originalDirectDebitRemmitanceMessageID, int numberOfTransactions, decimal controlSum, List<DirectDebitTransactionReject> directDebitTransactionRejects)
        //{
        //    this.originalDirectDebitRemmitanceMessageID = originalDirectDebitRemmitanceMessageID;
        //    this.numberOfTransactions = numberOfTransactions;
        //    this.controlSum = controlSum;
        //    this.directDebitTransactionRejects = directDebitTransactionRejects;

        //    if (TheProvidedNumberOfTransactionsIsWrong()) ChangeNumberOfTransactionsAndRiseException();
        //    if (TheProvidedControlSumIsWrong()) ChangeControlSumAndRiseException();
        //}

        //public void AddBilllToExistingDirectDebitTransaction(DirectDebitTransaction directDebitTransaction, SimplifiedBill bill)
        //{
        //    directDebitTransaction.AddBill(bill);
        //}

        //public void AddDirectDebitTransactionToGroupPayment(
        //    DirectDebitTransaction directDebitTransaction,
        //    DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment)
        //{
        //    directDebitTransactionsGroupPayment.AddDirectDebitTransaction(directDebitTransaction);
        //}

        //public void AddDirectDebitTransactionGroupPaymentToDirectDebitRemittance(
        //    DirectDebitRemittance directDebitRemmitance,
        //    DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment)
        //{
        //    directDebitRemmitance.AddDirectDebitTransactionsGroupPayment(directDebitTransactionsGroupPayment);
        //}
    }
}
