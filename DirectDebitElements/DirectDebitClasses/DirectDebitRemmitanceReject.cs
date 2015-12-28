using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectDebitElements.DirectDebitClasses
{
    public class DirectDebitRemmitanceReject
    {
        string originalDirectDebitRemmitanceMessageID;
        int numberOfTransactions;
        decimal controlSum;
        List<DirectDebitTransactionReject> directDebitTransactionRejects;

        public DirectDebitRemmitanceReject(string originalDirectDebitRemmitanceMessageID, int numberOfTransactions, decimal controlSum, List<DirectDebitTransactionReject> directDebitTransactionRejects)
        {
            this.originalDirectDebitRemmitanceMessageID = originalDirectDebitRemmitanceMessageID;
            this.numberOfTransactions = numberOfTransactions;
            this.controlSum = controlSum;
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
        }
        private List<string> GetAllOriginalEndtoEndTransactionIdentifications()
        {
            List<string> originalEndtoEndTransactionIdentificationsList = directDebitTransactionRejects.Select(directDebitTransactionReject => directDebitTransactionReject.OriginalEndtoEndTransactionIdentification).ToList();
            return originalEndtoEndTransactionIdentificationsList;
        }
    }
}
