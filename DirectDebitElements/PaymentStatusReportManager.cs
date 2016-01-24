using System;
using System.Collections.Generic;


namespace DirectDebitElements
{
    public class PaymentStatusReportManager
    {
        public PaymentStatusReportManager() { }

        public DirectDebitTransactionReject CreateDirectDebitTransactionReject(
            string originalTransactiontransactionID,
            string originalEndtoEndTransactiontransactionID,
            DateTime requestedCollectionDate,
            decimal amount,
            string mandateID,
            BankAccount debtorAccount,
            string rejectReason)
        {
            DirectDebitTransactionReject directDebitTransactionReject = new DirectDebitTransactionReject(
                originalTransactiontransactionID,
                originalEndtoEndTransactiontransactionID,
                requestedCollectionDate,
                amount,
                mandateID,
                debtorAccount,
                rejectReason);

            return directDebitTransactionReject;
        }

        public DirectDebitPaymentInstructionReject CreateAnEmptyDirectDebitPaymentInstructionReject(string originalPaymentInformationID)
        {
            List<DirectDebitTransactionReject> directDebitTransactionRejects = new List<DirectDebitTransactionReject>();

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID,
                directDebitTransactionRejects);

            return directDebitPaymentInstructionReject;
        }

        public DirectDebitPaymentInstructionReject CreateADirectDebitPaymentInstructionReject(
            string originalPaymentInformationID,
            List<DirectDebitTransactionReject> directDebitTransactionRejects)
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID,
                directDebitTransactionRejects);
            return directDebitPaymentInstructionReject;
        }

        public DirectDebitPaymentInstructionReject CreateADirectDebitPaymentInstructionReject(
            string originalPaymentInformationID,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitTransactionReject> directDebitTransactionRejects)
        {
            DirectDebitPaymentInstructionReject directDebitRemittanceRejectCreationResult = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID,
                numberOfTransactions,
                controlSum,
                directDebitTransactionRejects);

            return directDebitRemittanceRejectCreationResult;
        }

        public void AddTransactionRejectToPaymentInstructionReject(
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject,
            DirectDebitTransactionReject directDebitTransactionReject)
        {
            directDebitPaymentInstructionReject.AddDirectDebitTransactionReject(directDebitTransactionReject);
        }

        public PaymentStatusReport CreateAnEmptyPaymentStatusReport(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime)
        {
            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime);

            return paymentStatusReport;
        }

        public PaymentStatusReport CreateAPaymentStatusReport(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime,
            List<DirectDebitPaymentInstructionReject> directDebitRemittanceRejects)
        {
            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitRemittanceRejects);

            return paymentStatusReport;
        }

        public PaymentStatusReport CreateAPaymentStatusReport(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects)
        {
            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                numberOfTransactions,
                controlSum,
                directDebitPaymentInstructionRejects);

            return paymentStatusReport;
        }

        public void AddDirectDebitPaymentInstructionRejectToPaymentStatusReport(
            PaymentStatusReport paymentStatusReport,
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject)
        {
            paymentStatusReport.AddDirectDebitPaymentInstructionReject(directDebitPaymentInstructionReject);
        }
    }
}
