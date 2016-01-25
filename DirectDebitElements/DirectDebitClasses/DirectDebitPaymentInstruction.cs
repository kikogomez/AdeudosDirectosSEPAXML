using System;
using System.Collections.Generic;
using System.Linq;

namespace DirectDebitElements
{
    public class DirectDebitPaymentInstruction
    {
        public event EventHandler<decimal> ANewDirectDebitTransactionHasBeenAdded;
        public event EventHandler<decimal> ABillHasBeenAdddedToAnExistingDirectDebitTransacion;

        string paymentInformationID;
        string localInstrument;
        List<DirectDebitTransaction> directDebitTransactions;

        int numberOfTransactions;
        decimal controlSum;

        public DirectDebitPaymentInstruction(string paymentInformationID, string localInstrument)
        {
            CheckPaymentInformationID(paymentInformationID);
            this.paymentInformationID = paymentInformationID;
            this.localInstrument = localInstrument;
            directDebitTransactions = new List<DirectDebitTransaction>();
            numberOfTransactions = 0;
            controlSum = 0;
        }

        public DirectDebitPaymentInstruction(
            string paymentInformationID,
            string localInstrument,
            List<DirectDebitTransaction> directDebitTransactions)
            :this(paymentInformationID, localInstrument)
        {
            this.directDebitTransactions = directDebitTransactions;
            try
            {
                CheckDuplicateTransactionIDs();
            }
            catch (ArgumentException argumentException)
            {
                throw new TypeInitializationException("DirectDebitPaymentInstruction", argumentException);
            }
            UpdateNumberOfDirectDebitTransactionsAndAmount();
            SuscribeTo_ANewBillHasBeenAddedEvent_FromAllTransactions();
        }

        public DirectDebitPaymentInstruction(
            string paymentInformationID,
            string localInstrument,
            List<DirectDebitTransaction> directDebitTransactions,
            int numberOfTransactions,
            decimal controlSum)
            :this(paymentInformationID, localInstrument, directDebitTransactions)
        {
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

        public void AddDirectDebitTransaction(DirectDebitTransaction directDebitTransaction)
        {
            CheckTransactionIDDoesntExists(directDebitTransaction.TransactionID);
            directDebitTransactions.Add(directDebitTransaction);
            UpdateNumberOfDirectDebitTransactionsAndAmount();
            SuscribeTo_ANewBillHasBeenAddedEvent(directDebitTransaction);
            SignalANewDirectDebitTransactiontHasBeenAdded(directDebitTransaction.Amount);
        }

        private void CheckPaymentInformationID(string paymentInformationID)
        {
            if (paymentInformationID == null) throw new System.ArgumentNullException("paymentInformationID", "PaymentInformationID can't be null");
            if (paymentInformationID.Trim().Length > 35) throw new System.ArgumentOutOfRangeException("paymentInformationID", "PaymentInformationID lenght can't exceed 35 characters");
            if (paymentInformationID.Trim().Length == 0) throw new System.ArgumentException("PaymentInformationID can't be empty", "paymentInformationID");

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

        private void CheckDuplicateTransactionIDs()
        {
            List<string> transactionIDs = this.directDebitTransactions.Select(directDebitTransaction => directDebitTransaction.TransactionID).ToList();
            int distinctIDsCount = transactionIDs.Distinct().Count();
            if (distinctIDsCount!= transactionIDs.Count()) throw new ArgumentException("The TransactionIDs are not unique", "transactionID");
        }

        private void CheckTransactionIDDoesntExists(string transactionID)
        {
            if (directDebitTransactions.Select(directDebitTransaction => directDebitTransaction.TransactionID).ToList().Contains(transactionID))
                throw new ArgumentException("The TransactionID already exists", "transactionID");
        }

        private void UpdateNumberOfDirectDebitTransactionsAndAmount()
        {
            this.numberOfTransactions = directDebitTransactions.Count;
            this.controlSum = directDebitTransactions.Select(directDebitTransaction => directDebitTransaction.Amount).Sum();
        }

        private void ANewBillHasBeenAddedEventHandler(Object sender, decimal billAmout)
        {
            //numberOfTransactions++;
            controlSum += billAmout;
            SignalABillHasBeenAdddedToAnExistingDirectDebitTransacion(billAmout);
        }

        private void SuscribeTo_ANewBillHasBeenAddedEvent_FromAllTransactions()
        {
            foreach (DirectDebitTransaction directDebitTransaction in directDebitTransactions)
            {
                SuscribeTo_ANewBillHasBeenAddedEvent(directDebitTransaction);
            }
        }

        private void SuscribeTo_ANewBillHasBeenAddedEvent(DirectDebitTransaction directDebitTransaction)
        {
            directDebitTransaction.ANewBillHasBeenAdded += ANewBillHasBeenAddedEventHandler;
        }

        private void SignalANewDirectDebitTransactiontHasBeenAdded(decimal amount)
        {
            if (ANewDirectDebitTransactionHasBeenAdded != null)
            {
                ANewDirectDebitTransactionHasBeenAdded(this, amount);
            }
        }

        private void SignalABillHasBeenAdddedToAnExistingDirectDebitTransacion(decimal amount)
        {
            if (ABillHasBeenAdddedToAnExistingDirectDebitTransacion != null)
            {
                ABillHasBeenAdddedToAnExistingDirectDebitTransacion(this, amount);
            }
        }
    }
}
