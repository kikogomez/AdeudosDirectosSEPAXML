﻿using System;
using System.Collections.Generic;
using Billing;
using System.Linq;
using ISO20022PaymentInitiations.DDInitiationSerializableClasses;
using XMLSerializerValidator;
using ExtensionMethods;

namespace DirectDebitElements
{
    public class DirectDebitRemittancesManager
    {
        public DirectDebitRemittancesManager()
        {
        }

        public DirectDebitRemittance CreateADirectDebitRemmitance(DateTime creationDateTime, DateTime requestedCollectionDate, DirectDebitInitiationContract directDebitInitiationContract)
        {
            DirectDebitRemittance directDebitRemmitance = new DirectDebitRemittance(creationDateTime, requestedCollectionDate, directDebitInitiationContract);
            return directDebitRemmitance;
        }

        public DirectDebitTransactionsGroupPayment CreateANewGroupOfDirectDebitTransactions(string localInstrument)
        {
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment = new DirectDebitTransactionsGroupPayment(localInstrument);
            return directDebitTransactionsGroupPayment;
        }

        public DirectDebitTransaction CreateANewEmptyDirectDebitTransaction(DirectDebitMandate directDebitmandate)
        {
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                directDebitmandate.InternalReferenceNumber,
                directDebitmandate.BankAccount,
                directDebitmandate.AccountHolderName,
                directDebitmandate.DirectDebitMandateCreationDate);
            return directDebitTransaction;
        }

        public DirectDebitTransaction CreateANewDirectDebitTransactionFromAGroupOfBills(DirectDebitMandate directDebitmandate, List<SimplifiedBill> billsList)
        {
            DirectDebitTransaction directDebitTransaction  = new DirectDebitTransaction(
                billsList,
                directDebitmandate.InternalReferenceNumber,
                directDebitmandate.BankAccount,
                directDebitmandate.AccountHolderName,
                directDebitmandate.DirectDebitMandateCreationDate);
            return directDebitTransaction;
        }

        public void AddBilllToExistingDirectDebitTransaction(DirectDebitTransaction directDebitTransaction, SimplifiedBill bill)
        {
            directDebitTransaction.AddBill(bill);
        }

        public void AddDirectDebitTransactionToGroupPayment(
            DirectDebitTransaction directDebitTransaction,
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment)
        {
            directDebitTransactionsGroupPayment.AddDirectDebitTransaction(directDebitTransaction);
        }

        public void AddDirectDebitTransactionGroupPaymentToDirectDebitRemittance(
            DirectDebitRemittance directDebitRemmitance,
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment)
        {
            directDebitRemmitance.AddDirectDebitTransactionsGroupPayment(directDebitTransactionsGroupPayment);
        }
    }
}
