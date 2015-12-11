using System;
using System.Collections.Generic;

namespace Billing
{
    public class SimplifiedBill
    {
        string billID;
        string description;
        decimal amount;
        DateTime issueDate;
        DateTime dueDate;
        PaymentMethod assignedPaymentMethod;
        BillPaymentResult paymentResult;

        public SimplifiedBill(string billID, string description, decimal amount, DateTime issueDate, DateTime dueDate)
            : this(billID, description, amount, issueDate, dueDate, null) { }

        public SimplifiedBill(string billID, string description, decimal amount, DateTime issueDate, DateTime dueDate, PaymentMethod paymentMethod)
        {
            if (billID==null || billID=="") throw new ArgumentException();

            this.billID = billID;
            this.description = description;
            this.amount = amount;
            this.issueDate = issueDate;
            this.dueDate = dueDate.Date;
            this.paymentResult = (int)BillPaymentResult.ToCollect;
            this.assignedPaymentMethod = paymentMethod;
        }

        public enum BillPaymentResult { ToCollect, Paid, Unpaid, CancelledOut, Renegotiated, Failed };
        public enum BillPaymentMethod { Cash, CreditCard, Check, BankTransfer, DirectDebit };

        public string BillID
        {
            get { return billID; }
            set { billID = value; }
        }

        public string Description
        {
            get { return description; }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        public DateTime IssueDate
        {
            get { return issueDate; }
        }

        public DateTime DueDate
        {
            get { return dueDate; }
        }

        public PaymentMethod AssignedPaymentMethod
        {
            get { return assignedPaymentMethod; }
            set { assignedPaymentMethod = value; }
        }

        public BillPaymentResult PaymentResult
        {
            get { return paymentResult; }
        }
    }
}
