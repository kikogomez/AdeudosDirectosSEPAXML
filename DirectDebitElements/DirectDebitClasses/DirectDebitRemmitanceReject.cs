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

        public DirectDebitRemmitanceReject(string originalDirectDebitRemmitanceMessageID, int numberOfTransactions, decimal controlSum, List<DirectDebitTransactionReject> paymentTransactionRejects)
        {
            this.originalDirectDebitRemmitanceMessageID = originalDirectDebitRemmitanceMessageID;
            this.numberOfTransactions = numberOfTransactions;
            this.controlSum = controlSum;
            this.directDebitTransactionRejects = paymentTransactionRejects;
        }

        public string OriginalDirectDebitRemmitanceMessageID
        {
            get
            {
                return originalDirectDebitRemmitanceMessageID;
            }

            set
            {
                originalDirectDebitRemmitanceMessageID = value;
            }
        }

        public int NumberOfTransactions
        {
            get
            {
                return numberOfTransactions;
            }

            set
            {
                numberOfTransactions = value;
            }
        }

        public decimal ControlSum
        {
            get
            {
                return controlSum;
            }

            set
            {
                controlSum = value;
            }
        }

        public List<DirectDebitTransactionReject> DirectDebitTransactionRejects
        {
            get
            {
                return directDebitTransactionRejects;
            }

            set
            {
                directDebitTransactionRejects = value;
            }
        }


    }
}
