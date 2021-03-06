﻿using System;
using System.Collections.Generic;
using Billing;


namespace DirectDebitElements
{
    public class DirectDebitRemittancesManager
    {
        public DirectDebitRemittancesManager()
        {
        }

        public DirectDebitTransaction CreateAnEmptyDirectDebitTransaction(
            string transactionID,
            string mandateID,
            DirectDebitMandate directDebitmandate,
            DirectDebitAmendmentInformation amendmentInformation,
            bool firstDebit)
        {
            if (directDebitmandate == null) throw new ArgumentNullException("directDebitMandate", "DirectDebitMandate can't be null");

            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                new List<SimplifiedBill>(),
                transactionID,
                mandateID,
                directDebitmandate.DirectDebitMandateCreationDate,
                directDebitmandate.BankAccount,
                directDebitmandate.AccountHolderName,
                amendmentInformation,
                firstDebit);
            return directDebitTransaction;
        }

        public DirectDebitTransaction CreateADirectDebitTransaction(
            string transactionID,
            string mandateID,
            DirectDebitMandate directDebitmandate,
            List<SimplifiedBill> billsList,
            DirectDebitAmendmentInformation amendmentInformation,
            bool firstDebit)
        {
            if (directDebitmandate == null) throw new ArgumentNullException("directDebitMandate", "DirectDebitMandate can't be null");

            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                billsList,
                transactionID,
                mandateID,
                directDebitmandate.DirectDebitMandateCreationDate,
                directDebitmandate.BankAccount,
                directDebitmandate.AccountHolderName,
                amendmentInformation,
                firstDebit);
            return directDebitTransaction;
        }

        public void AddBilllToExistingDirectDebitTransaction(
            DirectDebitTransaction directDebitTransaction,
            SimplifiedBill bill)
        {
            directDebitTransaction.AddBill(bill);
        }

        public DirectDebitPaymentInstruction CreateAnEmptyDirectDebitPaymentInstruction(
            string paymentInformationID,
            string localInstrument,
            bool firstDebits)
        {
            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                paymentInformationID,
                localInstrument,
                firstDebits);
            return directDebitPaymentInstruction;
        }

        public DirectDebitPaymentInstruction CreateADirectDebitPaymentInstruction(
            string paymentInformationID,
            string localInstrument,
            bool firstDebits,
            List<DirectDebitTransaction> directDebitTransactions)
        {
            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                paymentInformationID,
                localInstrument,
                firstDebits,
                directDebitTransactions);
            return directDebitPaymentInstruction;
        }

        public DirectDebitPaymentInstruction CreateADirectDebitPaymentInstruction(
            string paymentInformationID,
            string localInstrument,
            bool firstDebits,
            List<DirectDebitTransaction> directDebitTransactions,
            int numberOfTransactions,
            decimal controlSum)
        {
            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                paymentInformationID,
                localInstrument,
                firstDebits,
                directDebitTransactions,
                numberOfTransactions,
                controlSum);
            return directDebitPaymentInstruction;
        }

        public void AddDirectDebitTransactionToDirectDebitPaymentInstruction(
            DirectDebitTransaction directDebitTransaction,
            DirectDebitPaymentInstruction directDebitPaymentInstruction)
        {
            directDebitPaymentInstruction.AddDirectDebitTransaction(directDebitTransaction);
        }

        public DirectDebitRemittance CreateAnEmptyDirectDebitRemittance(
            string messageID,
            DateTime creationDateTime,
            DateTime requestedCollectionDate,
            DirectDebitInitiationContract directDebitInitiationContract)
        {
            DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(
                messageID,
                creationDateTime,
                requestedCollectionDate,
                directDebitInitiationContract);
            return directDebitRemittance;
        }

        public DirectDebitRemittance CreateADirectDebitRemittance(
            string messageID,
            DateTime creationDateTime,
            DateTime requestedCollectionDate,
            DirectDebitInitiationContract directDebitInitiationContract,
            List<DirectDebitPaymentInstruction> directDebitPaymentInstructions)
        {
            DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(
                messageID,
                creationDateTime,
                requestedCollectionDate,
                directDebitInitiationContract,
                directDebitPaymentInstructions);
            return directDebitRemittance;
        }

        public DirectDebitRemittance CreateADirectDebitRemittance(
            string messageID,
            DateTime creationDateTime,
            DateTime requestedCollectionDate,
            DirectDebitInitiationContract directDebitInitiationContract,
            List<DirectDebitPaymentInstruction> directDebitPaymentInstructions,
            int numberOfTransactions,
            decimal controlSum)
        {
            DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(
                messageID,
                creationDateTime,
                requestedCollectionDate,
                directDebitInitiationContract,
                numberOfTransactions,
                controlSum,
                directDebitPaymentInstructions);
            return directDebitRemittance;
        }

        public void AddDirectDebitPaymentInstructionToDirectDebitRemittance(
            DirectDebitRemittance directDebitRemittance,
            DirectDebitPaymentInstruction directDebitPaymentInstruction)
        {
            directDebitRemittance.AddDirectDebitPaymentInstruction(directDebitPaymentInstruction);
        }

    }
}
