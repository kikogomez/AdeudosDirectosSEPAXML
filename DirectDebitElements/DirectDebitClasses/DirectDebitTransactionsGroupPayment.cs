using System.Collections.Generic;
using System.Linq;

namespace DirectDebitElements
{
    public class DirectDebitTransactionsGroupPayment
    {
        string paymentInformationID;
        string localInstrument;
        List<DirectDebitTransaction> directDebitTransactionsCollection;

        int numberOfDirectDebitTransactions;
        decimal totalAmount;

        public DirectDebitTransactionsGroupPayment(string paymentInformationID, string localInstrument)
        {
            this.paymentInformationID = CheckPaymentInformationID(paymentInformationID);
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

        public void AddDirectDebitTransaction(DirectDebitTransaction directDebitTransaction)
        {
            directDebitTransactionsCollection.Add(directDebitTransaction);
            UpdateNumberOfDirectDebitTransactionsAndAmount();
        }

        private string CheckPaymentInformationID(string paymentInformationID)
        {
            if (paymentInformationID == null) throw new System.ArgumentNullException("PaymentInformationID", "PaymentInformationID can't be null");
            if (paymentInformationID.Trim().Length > 35) throw new System.ArgumentOutOfRangeException("PaymentInformationID", "PaymentInformationID lenght can't exceed 35 characters");
            if (paymentInformationID.Trim().Length == 0) throw new System.ArgumentException("PaymentInformationID lenght can't be empty", "PaymentInformationID");
            return paymentInformationID;
        }
    }
}
