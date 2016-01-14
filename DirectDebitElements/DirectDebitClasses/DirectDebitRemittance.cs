using System;
using System.Collections.Generic;
using System.Linq;

namespace DirectDebitElements
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
            string messageID,
            DateTime creationDateTime,
            DateTime requestedCollectionDate, 
            DirectDebitInitiationContract directDebitInitiationContract)
        {
            CheckMandatoryFields(messageID, directDebitInitiationContract);

            this.messageID = messageID;
            this.creationDateTime = creationDateTime;
            this.requestedCollectionDate = requestedCollectionDate;
            this.directDebitInitiationContract=directDebitInitiationContract;
            directDebitTransactionGroupPaymentCollection = new List<DirectDebitTransactionsGroupPayment>();
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

        private void CheckMandatoryFields(string messageID, DirectDebitInitiationContract directDebitInitiationContract)
        {
            if (messageID == null) throw new ArgumentNullException("MessageID", "MessageID can't be null");
            if (messageID.Trim().Length == 0) throw new ArgumentException("MessageID can't be empty", "MessageID");
            if (messageID.Trim().Length > 35) throw new ArgumentOutOfRangeException("MessageID", "MessageID can't be longer than 35 characters");
            if (directDebitInitiationContract == null) throw new ArgumentNullException("DirectDebitInitiationContract", "DirectDebitInitiationContract can't be null");
        }
    }
}
