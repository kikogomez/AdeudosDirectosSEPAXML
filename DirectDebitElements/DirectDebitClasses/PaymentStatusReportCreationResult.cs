using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectDebitElements
{
    //public class PaymentStatusReportCreationResult
    //{
    //    PaymentStatusReport paymentStatusreport;
    //    List<string> errorMessages;

    //    public PaymentStatusReportCreationResult(string messageID, DateTime messageCreationDateTime, DateTime rejectAccountChargeDateTime, int numberOfTransactions, decimal controlSum, List<DirectDebitRemmitanceReject> directDebitRemmitanceRejects)
    //    {
    //        this.paymentStatusreport = new PaymentStatusReport(
    //            messageID,
    //            messageCreationDateTime,
    //            rejectAccountChargeDateTime,
    //            directDebitRemmitanceRejects);
    //        CheckForErrors(numberOfTransactions, controlSum, directDebitRemmitanceRejects);
    //    }

    //    private void CheckForErrors(int numberOfTransactions, decimal controlSum, List<DirectDebitRemmitanceReject> directDebitRemmitanceRejects)
    //    {
    //        int calculatedNumberOfTransactions = directDebitRemmitanceRejects.Select(ddRemmitanceRejects => ddRemmitanceRejects.NumberOfTransactions).Sum();
    //        decimal calculatedControlSum = directDebitRemmitanceRejects.Select(ddRemmitanceRejects => ddRemmitanceRejects.ControlSum).Sum();
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

    //    public PaymentStatusReport PaymentStatusreport
    //    {
    //        get
    //        {
    //            return paymentStatusreport;
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
