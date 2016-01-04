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

        //public DirectDebitRemmitanceReject(string originalDirectDebitRemmitanceMessageID, int numberOfTransactions, decimal controlSum, List<DirectDebitTransactionReject> directDebitTransactionRejects)
        //{
        //    this.originalDirectDebitRemmitanceMessageID = originalDirectDebitRemmitanceMessageID;
        //    this.numberOfTransactions = numberOfTransactions;
        //    this.controlSum = controlSum;
        //    this.directDebitTransactionRejects = directDebitTransactionRejects;

        //    if (TheProvidedNumberOfTransactionsIsWrong()) ChangeNumberOfTransactionsAndRiseException();
        //    if (TheProvidedControlSumIsWrong()) ChangeControlSumAndRiseException();
        //}

        public DirectDebitRemmitanceReject(string originalDirectDebitRemmitanceMessageID, List<DirectDebitTransactionReject> directDebitTransactionRejects)
        {
            this.originalDirectDebitRemmitanceMessageID = originalDirectDebitRemmitanceMessageID;
            this.numberOfTransactions = directDebitTransactionRejects.Count;
            this.controlSum = directDebitTransactionRejects.Select(ddTransactionReject => ddTransactionReject.Amount).Sum();
            this.directDebitTransactionRejects = directDebitTransactionRejects;
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

        private void ChangeNumberOfTransactionsAndRiseException()
        {
            int calculatedNumberOfTransactions = directDebitTransactionRejects.Count;
            string exceptionMessage =
                "The Number of Transactions is wrong. Provided: " + numberOfTransactions.ToString() + ". Expected: " + calculatedNumberOfTransactions.ToString() + ". Initialized with expected value";
            numberOfTransactions = calculatedNumberOfTransactions;
            throw new ArgumentException(exceptionMessage, "numberOfTransactions");
        }

        private void ChangeControlSumAndRiseException()
        {
            decimal calculatedControlSum = directDebitTransactionRejects.Select(ddTransactionReject => ddTransactionReject.Amount).Sum();
            string exceptionMessage =
                "The Control Sum is wrong. Provided: " + controlSum.ToString() + ". Expected: " + calculatedControlSum.ToString() + ". Initialized with expected value";
            controlSum = calculatedControlSum;
            throw new ArgumentException(exceptionMessage, "controlSum");
        }

        private bool TheProvidedNumberOfTransactionsIsWrong()
        {
            int calculatedNumberOfTransactions = directDebitTransactionRejects.Count;
            return (numberOfTransactions != calculatedNumberOfTransactions);
        }

        private bool TheProvidedControlSumIsWrong()
        {
            decimal calculatedControlSum = directDebitTransactionRejects.Select(ddRemmitanceReject => ddRemmitanceReject.Amount).Sum();
            return (controlSum != calculatedControlSum);
        }
    }
}
