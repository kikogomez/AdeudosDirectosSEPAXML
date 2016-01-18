using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectDebitElements
{
    public class DirectDebitRemmitanceReject
    {
        public event EventHandler<decimal> AddedNewDirectDebitTransactionReject;

        string originalDirectDebitRemmitanceMessageID;
        int numberOfTransactions;
        decimal controlSum;
        List<DirectDebitTransactionReject> directDebitTransactionRejects;

        public DirectDebitRemmitanceReject(
            string originalDirectDebitRemmitanceMessageID,
            List<DirectDebitTransactionReject> directDebitTransactionRejects)
        {
            this.originalDirectDebitRemmitanceMessageID = originalDirectDebitRemmitanceMessageID;
            this.numberOfTransactions = directDebitTransactionRejects.Count;
            this.controlSum = directDebitTransactionRejects.Select(ddTransactionReject => ddTransactionReject.Amount).Sum();
            this.directDebitTransactionRejects = directDebitTransactionRejects;
        }


        public DirectDebitRemmitanceReject(
            string originalDirectDebitRemmitanceMessageID,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitTransactionReject> directDebitTransactionRejects)
        {
            this.originalDirectDebitRemmitanceMessageID = originalDirectDebitRemmitanceMessageID;
            this.numberOfTransactions = directDebitTransactionRejects.Count;
            this.controlSum = directDebitTransactionRejects.Select(ddTransactionReject => ddTransactionReject.Amount).Sum();
            this.directDebitTransactionRejects = directDebitTransactionRejects;
            try
            {
                CheckNumberOfTransactionsAndAmount(numberOfTransactions, controlSum);
            }
            catch (ArgumentException argumentException)
            {
                throw new TypeInitializationException("DirectDebitRemmitanceReject", argumentException);
            }
        }

        public string OriginalDirectDebitRemmitanceMessageID
        {
            get { return originalDirectDebitRemmitanceMessageID; }

        }

        public int NumberOfTransactions
        {
            get { return numberOfTransactions; }
        }

        public decimal ControlSum
        {
            get { return controlSum; }
        }

        public List<DirectDebitTransactionReject> DirectDebitTransactionRejects
        {
            get { return directDebitTransactionRejects; }
        }

        public List<string> OriginalEndtoEndTransactionIdentificationList
        {
            get { return GetAllOriginalEndtoEndTransactionIdentifications(); }
        }

        public void AddDirectDebitTransactionReject(DirectDebitTransactionReject directDebitTransactionReject)
        {
            directDebitTransactionRejects.Add(directDebitTransactionReject);
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

        private List<string> GetAllOriginalEndtoEndTransactionIdentifications()
        {
            List<string> originalEndtoEndTransactionIdentificationsList = directDebitTransactionRejects.Select(directDebitTransactionReject => directDebitTransactionReject.OriginalEndtoEndTransactionIdentification).ToList();
            return originalEndtoEndTransactionIdentificationsList;
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
