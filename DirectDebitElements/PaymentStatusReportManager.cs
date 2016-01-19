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
            List<DirectDebitTransactionGroupPaymentReject> directDebitRemmitanceRejectsList)
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
            List<DirectDebitTransactionGroupPaymentReject> directDebitRemmitanceRejectsList)
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
            List<DirectDebitTransactionGroupPaymentReject> directDebitRemmitanceRejectsList = new List<DirectDebitTransactionGroupPaymentReject>();
            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitRemmitanceRejectsList);

            return paymentStatusReport;
        }

        public DirectDebitTransactionGroupPaymentReject CreateDirectDebitTransactionGroupPaymentReject(
            string originalDirectDebitTransactionGroupPaymentPaymentInformationID,
            List<DirectDebitTransactionReject> directDebitTransactionRejectsList)
        {
            DirectDebitTransactionGroupPaymentReject directDebitTransactionGroupPaymentReject = new DirectDebitTransactionGroupPaymentReject(
                originalDirectDebitTransactionGroupPaymentPaymentInformationID,
                directDebitTransactionRejectsList);
            return directDebitTransactionGroupPaymentReject;
        }

        public DirectDebitTransactionGroupPaymentReject CreateCheckedDirectDebitTransactionGroupPaymentReject(
            string originalDirectDebitTransactionGroupPaymentPaymentInformationID,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitTransactionReject> directDebitTransactionRejectsList)
        {
            DirectDebitTransactionGroupPaymentReject directDebitRemmitanceRejectCreationResult = new DirectDebitTransactionGroupPaymentReject(
                originalDirectDebitTransactionGroupPaymentPaymentInformationID,
                numberOfTransactions,
                controlSum,
                directDebitTransactionRejectsList);

            return directDebitRemmitanceRejectCreationResult;
        }

        public DirectDebitTransactionGroupPaymentReject CreateAnEmptyDirectDebitRemmitanceReject(string originalDirectDebitRemmitanceMessageID)
        {
            List<DirectDebitTransactionReject> directDebitTransactionRejects = new List<DirectDebitTransactionReject>();

            DirectDebitTransactionGroupPaymentReject directDebitRemmitanceReject = new DirectDebitTransactionGroupPaymentReject(
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

        public void AddRejectedRemmitanceToPaymentStatusReport(
            PaymentStatusReport paymentStatusReport,
            DirectDebitTransactionGroupPaymentReject directDebitRemmitanceReject)
        {
            paymentStatusReport.AddRemmitanceReject(directDebitRemmitanceReject);
        }

        public void AddRejectedTransactionToRemmitanceReject(
            DirectDebitTransactionGroupPaymentReject directDebitRemmitanceReject,
            DirectDebitTransactionReject directDebitTransactionReject)
        {
            directDebitRemmitanceReject.AddDirectDebitTransactionReject(directDebitTransactionReject);
        }
    }
}
