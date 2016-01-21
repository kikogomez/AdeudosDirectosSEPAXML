using System;
using System.Collections.Generic;
using System.Linq;

namespace DirectDebitElements
{
    public class DirectDebitPaymentInstruction
    {
        string paymentInformationID;
        string localInstrument;
        List<DirectDebitTransaction> directDebitTransactions;

        int numberOfTransactions;
        decimal controlSum;

        public DirectDebitPaymentInstruction(string paymentInformationID, string localInstrument)
        {
            this.paymentInformationID = CheckPaymentInformationID(paymentInformationID);
            this.localInstrument = localInstrument;
            directDebitTransactions = new List<DirectDebitTransaction>();
        }

        public DirectDebitPaymentInstruction(string paymentInformationID, string localInstrument, List<DirectDebitTransaction> directDebitTransactions)
        {
            this.paymentInformationID = CheckPaymentInformationID(paymentInformationID);
            this.localInstrument = localInstrument;
            this.directDebitTransactions = directDebitTransactions;
            UpdateNumberOfDirectDebitTransactionsAndAmount();
        }

        public DirectDebitPaymentInstruction(
            string paymentInformationID,
            string localInstrument,
            List<DirectDebitTransaction> directDebitTransactions,
            int numberOfTransactions,
            decimal controlSum)
            :this(paymentInformationID, localInstrument, directDebitTransactions)
        {
            //this.paymentInformationID = CheckPaymentInformationID(paymentInformationID);
            //this.localInstrument = localInstrument;
            //this.directDebitTransactions = directDebitTransactions;
            //UpdateNumberOfDirectDebitTransactionsAndAmount();
            try
            {
                CheckNumberOfTransactionsAndAmount(numberOfTransactions, controlSum);
            }
            catch (ArgumentException argumentException)
            {
                throw new TypeInitializationException("DirectDebitPaymentInstruction", argumentException);
            }
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
            get { return numberOfTransactions; }
        }

        public decimal TotalAmount
        {
            get { return controlSum; }
        }

        public List<DirectDebitTransaction> DirectDebitTransactions
        {
            get { return directDebitTransactions; }
        }

        public void UpdateNumberOfDirectDebitTransactionsAndAmount()
        {
            this.numberOfTransactions = directDebitTransactions.Count;
            this.controlSum = directDebitTransactions.Select(directDebitTransaction => directDebitTransaction.Amount).Sum();
        }

        public void AddDirectDebitTransaction(DirectDebitTransaction directDebitTransaction)
        {
            directDebitTransactions.Add(directDebitTransaction);
            UpdateNumberOfDirectDebitTransactionsAndAmount();
        }

        private string CheckPaymentInformationID(string paymentInformationID)
        {
            if (paymentInformationID == null) throw new System.ArgumentNullException("PaymentInformationID", "PaymentInformationID can't be null");
            if (paymentInformationID.Trim().Length > 35) throw new System.ArgumentOutOfRangeException("PaymentInformationID", "PaymentInformationID lenght can't exceed 35 characters");
            if (paymentInformationID.Trim().Length == 0) throw new System.ArgumentException("PaymentInformationID lenght can't be empty", "PaymentInformationID");
            return paymentInformationID;
        }

        private void CheckNumberOfTransactionsAndAmount(int numberOfTransactions, decimal controlSum)
        {
            if (this.numberOfTransactions != numberOfTransactions)
            {
                string errorMessage = string.Format("The {0} is wrong. It should be {1}, but {2} is provided", "Number of Transactions", this.numberOfTransactions, numberOfTransactions);
                throw new ArgumentException(errorMessage, "numberOfTransactions");
            }
            if (this.controlSum != controlSum)
            {
                string errorMessage = string.Format("The {0} is wrong. It should be {1}, but {2} is provided", "Control Sum", this.controlSum, controlSum);
                throw new ArgumentException(errorMessage, "controlSum");
            }
        }
    }
}
