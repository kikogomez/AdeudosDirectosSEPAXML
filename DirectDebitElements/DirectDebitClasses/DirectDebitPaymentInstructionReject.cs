using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectDebitElements
{
    public class DirectDebitPaymentInstructionReject
    {
        public event EventHandler<decimal> ANewDirectDebitTransactionRejectHasBeenAdded;

        string originalPaymentInformationID;
        int numberOfTransactions;
        decimal controlSum;
        List<DirectDebitTransactionReject> directDebitTransactionsRejects;

        public DirectDebitPaymentInstructionReject(string originalPaymentInformationID)
        {
            CheckOriginalPaymentInformationID(originalPaymentInformationID);
            this.originalPaymentInformationID = originalPaymentInformationID;
            this.directDebitTransactionsRejects = new List<DirectDebitTransactionReject>();
            this.numberOfTransactions = 0;
            this.controlSum = 0;
        }

        public DirectDebitPaymentInstructionReject(
            string originalPaymentInformationID,
            List<DirectDebitTransactionReject> directDebitTransactionsRejects)
            :this(originalPaymentInformationID)
        {
            this.directDebitTransactionsRejects = directDebitTransactionsRejects;
            UpdateNumberOfDirectDebitTransactionRejectsAndAmount();
        }

        public DirectDebitPaymentInstructionReject(
            string originalPaymentInformationID,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitTransactionReject> directDebitTransactionsRejects)
            :this(originalPaymentInformationID, directDebitTransactionsRejects)
        {
            try
            {
                CheckNumberOfTransactionsAndAmount(numberOfTransactions, controlSum);
            }
            catch (ArgumentException argumentException)
            {
                throw new TypeInitializationException("DirectDebitPaymentInstructionReject", argumentException);
            }
        }

        public string OriginalPaymentInformationID
        {
            get { return originalPaymentInformationID; }

        }

        public int NumberOfTransactions
        {
            get { return numberOfTransactions; }
        }

        public decimal ControlSum
        {
            get { return controlSum; }
        }

        public List<DirectDebitTransactionReject> DirectDebitTransactionsRejects
        {
            get { return directDebitTransactionsRejects; }
        }

        public List<string> OriginalEndtoEndTransactiontransactionIDList
        {
            get { return GetAllOriginalEndtoEndTransactiontransactionIDs(); }
        }

        public void AddDirectDebitTransactionReject(DirectDebitTransactionReject directDebitTransactionReject)
        {
            directDebitTransactionsRejects.Add(directDebitTransactionReject);
            numberOfTransactions++;
            controlSum += directDebitTransactionReject.Amount;
            SignalANewDirectDebitTransactionRejectHasBeenAdded(directDebitTransactionReject);
        }

        private void UpdateNumberOfDirectDebitTransactionRejectsAndAmount()
        {
            this.numberOfTransactions = this.directDebitTransactionsRejects.Count;
            this.controlSum = this.directDebitTransactionsRejects.Select(ddTransactionReject => ddTransactionReject.Amount).Sum();
        }

        private List<string> GetAllOriginalEndtoEndTransactiontransactionIDs()
        {
            List<string> originalEndtoEndTransactiontransactionIDList = directDebitTransactionsRejects.Select(directDebitTransactionReject => directDebitTransactionReject.OriginalEndtoEndTransactionIdentification).ToList();
            return originalEndtoEndTransactiontransactionIDList;
        }

        private void CheckOriginalPaymentInformationID(string originalPaymentInformationID)
        {
            if (originalPaymentInformationID == null) throw new System.ArgumentNullException("originalPaymentInformationID", "OriginalPaymentInformationID can't be null");
            if (originalPaymentInformationID.Trim().Length > 35) throw new System.ArgumentOutOfRangeException("originalPaymentInformationID", "OriginalPaymentInformationID lenght can't exceed 35 characters");
            if (originalPaymentInformationID.Trim().Length == 0) throw new System.ArgumentException("OriginalPaymentInformationID can't be empty", "originalPaymentInformationID");
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

        private void SignalANewDirectDebitTransactionRejectHasBeenAdded(DirectDebitTransactionReject directDebitTransactionReject)
        {
            if (ANewDirectDebitTransactionRejectHasBeenAdded != null)
            {
                ANewDirectDebitTransactionRejectHasBeenAdded(this, directDebitTransactionReject.Amount);
            }
        }
    }
}
