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

        public PaymentStatusReport CreateCheckedPaymentStatusReport(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitTransactionsGroupPaymentReject> directDebitRemmitanceRejectsList)
        {
            PaymentStatusReport paymentStatusReportCreationResult = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                numberOfTransactions,
                controlSum,
                directDebitRemmitanceRejectsList);

            return paymentStatusReportCreationResult;
        }


        public PaymentStatusReport CreatePaymentStatusReport(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime,
            List<DirectDebitTransactionsGroupPaymentReject> directDebitRemmitanceRejectsList)
        {
            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitRemmitanceRejectsList);

            return paymentStatusReport;
        }

        public PaymentStatusReport CreateAnEmptyPaymentStatusReport(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime)
        {
            List<DirectDebitTransactionsGroupPaymentReject> directDebitRemmitanceRejectsList = new List<DirectDebitTransactionsGroupPaymentReject>();
            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitRemmitanceRejectsList);

            return paymentStatusReport;
        }

        public DirectDebitTransactionsGroupPaymentReject CreateDirectDebitTransactionGroupPaymentReject(
            string originalDirectDebitTransactionGroupPaymentPaymentInformationID,
            List<DirectDebitTransactionReject> directDebitTransactionRejectsList)
        {
            DirectDebitTransactionsGroupPaymentReject directDebitTransactionGroupPaymentReject = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitTransactionGroupPaymentPaymentInformationID,
                directDebitTransactionRejectsList);
            return directDebitTransactionGroupPaymentReject;
        }

        public DirectDebitTransactionsGroupPaymentReject CreateCheckedDirectDebitTransactionGroupPaymentReject(
            string originalDirectDebitTransactionGroupPaymentPaymentInformationID,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitTransactionReject> directDebitTransactionRejectsList)
        {
            DirectDebitTransactionsGroupPaymentReject directDebitRemmitanceRejectCreationResult = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitTransactionGroupPaymentPaymentInformationID,
                numberOfTransactions,
                controlSum,
                directDebitTransactionRejectsList);

            return directDebitRemmitanceRejectCreationResult;
        }

        public DirectDebitTransactionsGroupPaymentReject CreateAnEmptyDirectDebitTransactionGroupPaymentReject(string originalDirectDebitTransactionGroupPaymentPaymentInformationID)
        {
            List<DirectDebitTransactionReject> directDebitTransactionRejects = new List<DirectDebitTransactionReject>();

            DirectDebitTransactionsGroupPaymentReject directDebitTransactionsGroupPaymentReject = new DirectDebitTransactionsGroupPaymentReject(
                originalDirectDebitTransactionGroupPaymentPaymentInformationID,
                directDebitTransactionRejects);

            return directDebitTransactionsGroupPaymentReject;
        }

        public DirectDebitTransactionReject CreateDirectDebitTransactionReject(
            string originalTransactionInternalUniqueInstructionID,
            string originalEndtoEndTransactioninternalUniqueInstructionID,
            DateTime requestedCollectionDate,
            decimal amount,
            string mandateID,
            BankAccount debtorAccount,
            string rejectReason)

        {
            DirectDebitTransactionReject directDebitTransactionReject = new DirectDebitTransactionReject(
                originalTransactionInternalUniqueInstructionID,
                originalEndtoEndTransactioninternalUniqueInstructionID,
                requestedCollectionDate,
                amount,
                mandateID,
                debtorAccount,
                rejectReason);

            return directDebitTransactionReject;
        }

        public void AddRejectedTransactionsGroupPaymentToPaymentStatusReport(
            PaymentStatusReport paymentStatusReport,
            DirectDebitTransactionsGroupPaymentReject directDebitTransactionsGroupPaymentReject)
        {
            paymentStatusReport.AddRemmitanceReject(directDebitTransactionsGroupPaymentReject);
        }

        public void AddRejectedTransactionToTransactionsGroupPaymentReject(
            DirectDebitTransactionsGroupPaymentReject directDebitTransactionsGroupPaymentReject,
            DirectDebitTransactionReject directDebitTransactionReject)
        {
            directDebitTransactionsGroupPaymentReject.AddDirectDebitTransactionReject(directDebitTransactionReject);
        }
    }
}
