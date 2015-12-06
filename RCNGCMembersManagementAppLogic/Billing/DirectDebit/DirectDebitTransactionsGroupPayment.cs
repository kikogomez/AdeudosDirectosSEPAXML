using System.Collections.Generic;
using System.Linq;

namespace RCNGCMembersManagementAppLogic.Billing.DirectDebit
{
    public class DirectDebitTransactionsGroupPayment
    {
        string paymentInformationID;
        string localInstrument;
        List<DirectDebitTransaction> directDebitTransactionsCollection;

        int numberOfDirectDebitTransactions;
        decimal totalAmount;

        public DirectDebitTransactionsGroupPayment(string localInstrument)
        {
            this.localInstrument = localInstrument;
            directDebitTransactionsCollection = new List<DirectDebitTransaction>();
        }

        public string PaymentInformationID
        {
            get { return paymentInformationID; }
        }
       
        public string LocalInstrument
        {
            get { return localInstrument; }
        }

        public int NumberOfDirectDebitTransactions
        {
            get { return numberOfDirectDebitTransactions; }
        }

        public decimal TotalAmount
        {
            get { return totalAmount; }
        }

        public List<DirectDebitTransaction> DirectDebitTransactionsCollection
        {
            get { return directDebitTransactionsCollection; }
        }

        public void UpdateNumberOfDirectDebitTransactionsAndAmount()
        {
            this.numberOfDirectDebitTransactions = directDebitTransactionsCollection.Count;
            this.totalAmount = directDebitTransactionsCollection.Select(directDebitTransaction => directDebitTransaction.Amount).Sum();
        }

        public void GeneratePaymentInformationID(int sequenceNumber)
        {
            paymentInformationID = sequenceNumber.ToString("000");
        }

        public void AddDirectDebitTransaction(DirectDebitTransaction directDebitTransaction)
        {
            directDebitTransactionsCollection.Add(directDebitTransaction);
            UpdateNumberOfDirectDebitTransactionsAndAmount();
        }



    }
}
