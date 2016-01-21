using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectDebitElements
{
    public class DirectDebitPaymentInstructionReject
    {
        public event EventHandler<decimal> AddedNewDirectDebitTransactionReject;

        string originalPaymentInformationID;
        int numberOfTransactions;
        decimal controlSum;
        List<DirectDebitTransactionReject> directDebitTransactionsRejects;

        public DirectDebitPaymentInstructionReject(
            string originalPaymentInformationID,
            List<DirectDebitTransactionReject> directDebitTransactionsRejects)
        {
            this.originalPaymentInformationID = originalPaymentInformationID;
            this.numberOfTransactions = directDebitTransactionsRejects.Count;
            this.controlSum = directDebitTransactionsRejects.Select(ddTransactionReject => ddTransactionReject.Amount).Sum();
            this.directDebitTransactionsRejects = directDebitTransactionsRejects;
        }

        public DirectDebitPaymentInstructionReject(
            string originalPaymentInformationID,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitTransactionReject> directDebitTransactionsRejects)
        {
            this.originalPaymentInformationID = originalPaymentInformationID;
            this.numberOfTransactions = directDebitTransactionsRejects.Count;
            this.controlSum = directDebitTransactionsRejects.Select(ddTransactionReject => ddTransactionReject.Amount).Sum();
            this.directDebitTransactionsRejects = directDebitTransactionsRejects;
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

        public List<string> OriginalEndtoEndTransactionInternalUniqueInstructionIDList
        {
            get { return GetAllOriginalEndtoEndTransactionInternalUniqueInstructionIDs(); }
        }

        public void AddDirectDebitTransactionReject(DirectDebitTransactionReject directDebitTransactionReject)
        {
            directDebitTransactionsRejects.Add(directDebitTransactionReject);
            numberOfTransactions++;
            controlSum += directDebitTransactionReject.Amount;
            SignalANewDirectDebitTransactionRejectHasBeenAdded(directDebitTransactionReject);
        }

        private void SignalANewDirectDebitTransactionRejectHasBeenAdded(DirectDebitTransactionReject directDebitTransactionReject)
        {
            if (AddedNewDirectDebitTransactionReject != null)
            {
                AddedNewDirectDebitTransactionReject(this, directDebitTransactionReject.Amount);
            }
        }

        private List<string> GetAllOriginalEndtoEndTransactionInternalUniqueInstructionIDs()
        {
            List<string> originalEndtoEndTransactionInternalUniqueInstructionIDList = directDebitTransactionsRejects.Select(directDebitTransactionReject => directDebitTransactionReject.OriginalEndtoEndTransactionIdentification).ToList();
            return originalEndtoEndTransactionInternalUniqueInstructionIDList;
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
