using System;
using System.Collections.Generic;
using System.Linq;

namespace RCNGCMembersManagementAppLogic.Billing.DirectDebit
{
    public class DirectDebitRemittance
    {
        string messageID;
        DateTime creationDateTime;
        DateTime requestedCollectionDate;
        int numberOfTransactions;
        decimal controlSum;
        List<DirectDebitTransactionsGroupPayment> directDebitTransactionGroupPaymentCollection;
        DirectDebitInitiationContract directDebitInitiationContract;

        public DirectDebitRemittance(
            DateTime creationDateTime,
            DateTime requestedCollectionDate, 
            DirectDebitInitiationContract directDebitInitiationContract)
        {
            this.creationDateTime = creationDateTime;
            this.requestedCollectionDate = requestedCollectionDate;
            this.directDebitInitiationContract=directDebitInitiationContract;
            directDebitTransactionGroupPaymentCollection = new List<DirectDebitTransactionsGroupPayment>();
            GenerateRemmitanceID();
        }

        public string MessageID
        {
            get { return messageID; }
        }

        public DateTime CreationDate
        {
            get { return creationDateTime; }
        }

        public DateTime RequestedCollectionDate
        {
            get { return requestedCollectionDate; }
        }

        public DirectDebitInitiationContract DirectDebitInitiationContract
        {
            get { return directDebitInitiationContract; }
        }

        public List<DirectDebitTransactionsGroupPayment> DirectDebitTransactionGroupPaymentCollection
        {
            get { return directDebitTransactionGroupPaymentCollection; }
        }

        public int NumberOfTransactions
        {
            get { return numberOfTransactions; }
        }

        public decimal ControlSum
        {
            get { return controlSum; }
        }

        public void AddDirectDebitTransactionsGroupPayment(DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment)
        {
            directDebitTransactionGroupPaymentCollection.Add(directDebitTransactionsGroupPayment);
            UpdateNumberOfDirectDebitTransactionsAndAmount();
        }

        public void UpdateNumberOfDirectDebitTransactionsAndAmount()
        {
            this.numberOfTransactions = 
                directDebitTransactionGroupPaymentCollection.Select(dDTxGPC => dDTxGPC.NumberOfDirectDebitTransactions).Sum();
            this.controlSum = directDebitTransactionGroupPaymentCollection.Select(
                directDebitTransactionGroupPayment => directDebitTransactionGroupPayment.TotalAmount).Sum();
        }

        private void GenerateRemmitanceID()
        {
            messageID = directDebitInitiationContract.CreditorID + creationDateTime.ToString("yyyyMMddHH:mm:ss");
        }
    }
}
