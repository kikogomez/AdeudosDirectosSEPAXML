using System;
using System.Collections.Generic;
using Billing;


namespace DirectDebitElements
{
    public class DirectDebitRemittancesManager
    {
        public DirectDebitRemittancesManager()
        {
        }

        public DirectDebitRemittance CreateAnEmptyDirectDebitRemittance(string messageID, DateTime creationDateTime, DateTime requestedCollectionDate, DirectDebitInitiationContract directDebitInitiationContract)
        {
            DirectDebitRemittance directDebitRemittance = new DirectDebitRemittance(messageID, creationDateTime, requestedCollectionDate, directDebitInitiationContract);
            return directDebitRemittance;
        }

        public DirectDebitPaymentInstruction CreateAnEmptyDirectDebitPaymentInstruction(string paymentInformationID, string localInstrument)
        {
            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(paymentInformationID, localInstrument);
            return directDebitPaymentInstruction;
        }

        public DirectDebitTransaction CreateAnEmptyDirectDebitTransaction(
            string internalUniqueInstructionID,
            string mandateID,
            DirectDebitMandate directDebitmandate,
            DirectDebitAmendmentInformation amendmentInformation)
        {
            if (directDebitmandate == null) throw new ArgumentNullException("directDebitMandate", "DirectDebitMandate can't be null");

            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                new List<SimplifiedBill>(),
                internalUniqueInstructionID,
                mandateID,
                directDebitmandate.DirectDebitMandateCreationDate,
                directDebitmandate.BankAccount,
                directDebitmandate.AccountHolderName,
                amendmentInformation);
            return directDebitTransaction;
        }

        public DirectDebitTransaction CreateADirectDebitTransactionFromAGroupOfBills(
            string internalUniqueInstructionID,
            string mandateID,
            DirectDebitMandate directDebitmandate,
            List<SimplifiedBill> billsList,
            DirectDebitAmendmentInformation amendmentInformation)
        {
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                billsList,
                internalUniqueInstructionID,
                mandateID,
                directDebitmandate.DirectDebitMandateCreationDate,
                directDebitmandate.BankAccount,
                directDebitmandate.AccountHolderName,
                amendmentInformation);
            return directDebitTransaction;
        }

        public void AddBilllToExistingDirectDebitTransaction(DirectDebitTransaction directDebitTransaction, SimplifiedBill bill)
        {
            directDebitTransaction.AddBill(bill);
        }

        public void AddDirectDebitTransactionToGroupPayment(
            DirectDebitTransaction directDebitTransaction,
            DirectDebitPaymentInstruction directDebitPaymentInstruction)
        {
            directDebitPaymentInstruction.AddDirectDebitTransaction(directDebitTransaction);
        }

        public void AddDirectDebitTransactionGroupPaymentToDirectDebitRemittance(
            DirectDebitRemittance directDebitRemittance,
            DirectDebitPaymentInstruction directDebitPaymentInstruction)
        {
            directDebitRemittance.AddDirectDebitPaymentInstruction(directDebitPaymentInstruction);
        }

    }
}
