using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectDebitElements
{
    public class DirectDebitRemmitanceRejectCreationResult
    {
        DirectDebitRemmitanceReject directDebitRemmitanceReject;
        string errorMesssage;

        public DirectDebitRemmitanceRejectCreationResult(
            string originalDirectDebitRemmitanceMessageID,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitTransactionReject> directDebitTransactionRejects)
        {
            this.directDebitRemmitanceReject = new DirectDebitRemmitanceReject(originalDirectDebitRemmitanceMessageID, directDebitTransactionRejects);

            int calculatedNumberOfTransactions = directDebitTransactionRejects.Count;
            decimal calculatedControlSum = directDebitTransactionRejects.Select(ddTransactionReject => ddTransactionReject.Amount).Sum();
            if (numberOfTransactions != calculatedNumberOfTransactions)
                errorMesssage += AddErroneousNumberOfTransactionsErrorMessage(numberOfTransactions, calculatedNumberOfTransactions);
            if (controlSum != calculatedControlSum)
            {
                if (errorMesssage != null) errorMesssage += Environment.NewLine;
                errorMesssage += AddErroneousControlSumErrorMessage(controlSum, calculatedControlSum);
            }
        }

        private string AddErroneousNumberOfTransactionsErrorMessage(int providedNumberOfTransactions, int calculatedNumberOfTransactions)
        {
            string errorMessage =
                "The Number of Transactions is wrong. Provided: " + providedNumberOfTransactions.ToString() + ". Expected: " + calculatedNumberOfTransactions.ToString() + ". Initialized with expected value";
            return errorMessage;
        }

        private string AddErroneousControlSumErrorMessage(decimal providadControlSum, decimal calculatedControlSum)
        {
            string errorMessage =
                "The Control Sum is wrong. Provided: " + providadControlSum.ToString() + ". Expected: " + calculatedControlSum.ToString() + ". Initialized with expected value";
            return errorMessage;
        }

        public DirectDebitRemmitanceReject DirectDebitRemmitanceReject
        {
            get
            {
                return directDebitRemmitanceReject;
            }
        }

        public string ErrorMesssage
        {
            get
            {
                return errorMesssage;
            }
        }
    }
}
