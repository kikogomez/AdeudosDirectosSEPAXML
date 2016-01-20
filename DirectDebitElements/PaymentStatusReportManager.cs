﻿using System;
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


        public PaymentStatusReport CreatePaymentStatusReport(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime,
            List<DirectDebitPaymentInstructionReject> directDebitRemmitanceRejects)
        {
            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitRemmitanceRejects);

            return paymentStatusReport;
        }

        public PaymentStatusReport CreateAnEmptyPaymentStatusReport(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime)
        {
            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>();
            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                directDebitPaymentInstructionRejects);

            return paymentStatusReport;
        }

        public DirectDebitPaymentInstructionReject CreateDirectDebitPaymentInstructionReject(
            string originalPaymentInformationID,
            List<DirectDebitTransactionReject> directDebitTransactionRejects)
        {
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID,
                directDebitTransactionRejects);
            return directDebitPaymentInstructionReject;
        }

        public DirectDebitPaymentInstructionReject CreateCheckedDirectDebitPaymentInstructionReject(
            string originalPaymentInformationID,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitTransactionReject> directDebitTransactionRejects)
        {
            DirectDebitPaymentInstructionReject directDebitRemmitanceRejectCreationResult = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID,
                numberOfTransactions,
                controlSum,
                directDebitTransactionRejects);

            return directDebitRemmitanceRejectCreationResult;
        }

        public DirectDebitPaymentInstructionReject CreateAnEmptyDirectDebitPaymentInstructionReject(string originalPaymentInformationID)
        {
            List<DirectDebitTransactionReject> directDebitTransactionRejects = new List<DirectDebitTransactionReject>();

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = new DirectDebitPaymentInstructionReject(
                originalPaymentInformationID,
                directDebitTransactionRejects);

            return directDebitPaymentInstructionReject;
        }

        public DirectDebitTransactionReject CreateDirectDebitTransactionReject(
            string originalTransactionInternalUniqueInstructionID,
            string originalEndtoEndTransactionInternalUniqueInstructionID,
            DateTime requestedCollectionDate,
            decimal amount,
            string mandateID,
            BankAccount debtorAccount,
            string rejectReason)
        {
            DirectDebitTransactionReject directDebitTransactionReject = new DirectDebitTransactionReject(
                originalTransactionInternalUniqueInstructionID,
                originalEndtoEndTransactionInternalUniqueInstructionID,
                requestedCollectionDate,
                amount,
                mandateID,
                debtorAccount,
                rejectReason);

            return directDebitTransactionReject;
        }

        public void AddRejectedDirectDebitPaymentInstructionToPaymentStatusReport(
            PaymentStatusReport paymentStatusReport,
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject)
        {
            paymentStatusReport.AddTransactionsGroupPaymentReject(directDebitPaymentInstructionReject);
        }

        public void AddRejectedTransactionToPaymentInstructionReject(
            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject,
            DirectDebitTransactionReject directDebitTransactionReject)
        {
            directDebitPaymentInstructionReject.AddDirectDebitTransactionReject(directDebitTransactionReject);
        }
    }
}
