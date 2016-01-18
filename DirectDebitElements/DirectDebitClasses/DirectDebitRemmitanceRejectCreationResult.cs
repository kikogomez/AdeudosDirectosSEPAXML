using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectDebitElements
{
    //
    //The use of this class has been discarded, but keeped (just in case...)
    //
    //public class DirectDebitRemmitanceRejectCreationResult
    //{
    //    DirectDebitRemmitanceReject directDebitRemmitanceReject;
    //    List<string> errorMessages;

    //    public DirectDebitRemmitanceRejectCreationResult(
    //        string originalDirectDebitRemmitanceMessageID,
    //        int numberOfTransactions,
    //        decimal controlSum,
    //        List<DirectDebitTransactionReject> directDebitTransactionRejects)
    //    {
    //        this.directDebitRemmitanceReject = new DirectDebitRemmitanceReject(originalDirectDebitRemmitanceMessageID, directDebitTransactionRejects);
    //        CheckForErrors(numberOfTransactions, controlSum, directDebitTransactionRejects);
    //    }

    //    private void CheckForErrors (int numberOfTransactions, decimal controlSum, List<DirectDebitTransactionReject> directDebitTransactionRejects)
    //    {
    //        int calculatedNumberOfTransactions = directDebitTransactionRejects.Count;
    //        decimal calculatedControlSum = directDebitTransactionRejects.Select(ddTransactionReject => ddTransactionReject.Amount).Sum();
    //        errorMessages = new List<string>();
    //        if (numberOfTransactions != calculatedNumberOfTransactions)
    //            errorMessages.Add(AddErroneousNumberOfTransactionsErrorMessage(numberOfTransactions, calculatedNumberOfTransactions));
    //        if (controlSum != calculatedControlSum)
    //            errorMessages.Add(AddErroneousControlSumErrorMessage(controlSum, calculatedControlSum));
    //    }

    //    private string AddErroneousNumberOfTransactionsErrorMessage(int providedNumberOfTransactions, int calculatedNumberOfTransactions)
    //    {
    //        string errorMessage =
    //            "The Number of Transactions is wrong. Provided: " + providedNumberOfTransactions.ToString() + ". Expected: " + calculatedNumberOfTransactions.ToString() + ". Initialized with expected value";
    //        return errorMessage;
    //    }

    //    private string AddErroneousControlSumErrorMessage(decimal providedControlSum, decimal calculatedControlSum)
    //    {
    //        string errorMessage =
    //            "The Control Sum is wrong. Provided: " + providedControlSum.ToString() + ". Expected: " + calculatedControlSum.ToString() + ". Initialized with expected value";
    //        return errorMessage;
    //    }

    //    public DirectDebitRemmitanceReject DirectDebitRemmitanceReject
    //    {
    //        get
    //        {
    //            return directDebitRemmitanceReject;
    //        }
    //    }

    //    public List<string> ErrorMessages
    //    {
    //        get
    //        {
    //            return errorMessages;
    //        }
    //    }
    //}
}
