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

        public PaymentStatusReportCreationResult CreateCheckedPaymentStatusReport(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitRemmitanceReject> directDebitRemmitanceRejectsList)
        {
            PaymentStatusReportCreationResult paymentStatusReportCreationResult = new PaymentStatusReportCreationResult(
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
            List<DirectDebitRemmitanceReject> directDebitRemmitanceRejectsList)
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
            List<DirectDebitRemmitanceReject> directDebitRemmitanceRejectsList = new List<DirectDebitRemmitanceReject>();
            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitRemmitanceRejectsList);

            return paymentStatusReport;
        }

        public void AddRejectedRemmitanceToPaymentStatusReport(
            PaymentStatusReport paymentStatusReport,
            DirectDebitRemmitanceReject directDebitRemmitanceReject)
        {
            paymentStatusReport.AddRemmitanceReject(directDebitRemmitanceReject);
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
            DirectDebitRemmitanceReject directDebitRemmitanceReject = new DirectDebitRemmitanceReject(
                originalDirectDebitRemmitanceMessageID,
                directDebitTransactionRejectsList);
            return directDebitRemmitanceReject;
        }

        public DirectDebitRemmitanceRejectCreationResult CreateDirectDebitRemmitanceReject(
            string originalDirectDebitRemmitanceMessageID,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitTransactionReject> directDebitTransactionRejectsList)
        {
            DirectDebitRemmitanceRejectCreationResult directDebitRemmitanceRejectCreationResult= new DirectDebitRemmitanceRejectCreationResult(
                originalDirectDebitRemmitanceMessageID,
                numberOfTransactions,
                controlSum,
                directDebitTransactionRejectsList);

            return directDebitRemmitanceRejectCreationResult;
        }

        public void AddRejectedTransactionToRemmitanceReject(
            DirectDebitRemmitanceReject directDebitRemmitanceReject,
            DirectDebitTransactionReject directDebitTransactionReject)
        {
            directDebitRemmitanceReject.AddDirectDebitTransactionReject(directDebitTransactionReject);
        }
    }
}
