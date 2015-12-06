using System;
using System.Collections.Generic;

namespace RCNGCMembersManagementAppLogic.Billing
{
    public class Bill
    {
        string billID;
        string description;
        decimal amount;
        DateTime issueDate;
        DateTime dueDate;
        PaymentMethod assignedPaymentMethod;
        Dictionary<DateTime,PaymentAgreement> paymentAgreements;
        PaymentAgreement renegotiationAgreement;
        Payment payment;
        BillPaymentResult paymentResult;

        public Bill(string billID, string description, decimal amount, DateTime issueDate, DateTime dueDate)
            : this(billID, description, amount, issueDate, dueDate, null) { }

        public Bill(string description, decimal amount, DateTime issueDate, DateTime dueDate)
            : this(null, description, amount, issueDate, dueDate, null) { }

        public Bill(string billID, string description, decimal amount, DateTime issueDate, DateTime dueDate, PaymentMethod paymentMethod)
        {
            this.billID = billID;
            this.description = description;
            this.amount = amount;
            this.issueDate = issueDate;
            this.dueDate = dueDate.Date;
            this.paymentResult = (int)BillPaymentResult.ToCollect;
            this.assignedPaymentMethod = paymentMethod;
            this.paymentAgreements = new Dictionary<DateTime, PaymentAgreement>();
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

        public Dictionary<DateTime, PaymentAgreement> PaymentAgreements
        {
            get { return paymentAgreements; }
        }

        public PaymentAgreement RenegotiationAgreement
        {
            get { return renegotiationAgreement; }
        }

        public Payment Payment
        {
            get { return payment; }
        }

        public BillPaymentResult PaymentResult
        {
            get { return paymentResult; }
        }

        public void PayBill(Payment payment)
        {
            if (this.amount != payment.PaymentAmount)
                throw new System.ArgumentException("Only payments for the bill total amount are accepted", "payment");
            this.payment = payment;
            paymentResult = BillPaymentResult.Paid;
        }

        public void CancelBill()
        {
            paymentResult = BillPaymentResult.CancelledOut;
        }

        public void RenegotiateBill(PaymentAgreement renegotiationAgreement)
        {
            paymentResult = BillPaymentResult.Renegotiated;
            this.renegotiationAgreement = renegotiationAgreement;
        }

        public void AssignAgreement(PaymentAgreement paymentAgreement)
        {
            this.paymentAgreements.Add(paymentAgreement.AgreementDate.Date, paymentAgreement);
        }

        public void AssignPaymentMethod(PaymentMethod paymentMethod)
        {
            this.AssignedPaymentMethod = paymentMethod;
        }

        public void CheckDueDate(DateTime today)
        {
            if (today > dueDate && paymentResult == BillPaymentResult.ToCollect) SetBillAsUnpaid();
        }

        public void RenewDueDate(DateTime newDueDate, DateTime todayDate)
        {
            this.dueDate = newDueDate;
            if (todayDate < newDueDate && paymentResult == Bill.BillPaymentResult.Unpaid) SetBillAsToCollect();
        }

        private void SetBillAsUnpaid()
        {
            paymentResult = BillPaymentResult.Unpaid;
            CancellAnyAgreementsActiveForBill();
        }

        private void SetBillAsToCollect()
        {
            paymentResult = BillPaymentResult.ToCollect;
        }

        private void CancellAnyAgreementsActiveForBill()
        {
            foreach (PaymentAgreement paymentAgreement in paymentAgreements.Values)
            {
                if (paymentAgreement.PaymentAgreementActualStatus == PaymentAgreement.PaymentAgreementStatus.Active)
                    paymentAgreement.PaymentAgreementActualStatus = PaymentAgreement.PaymentAgreementStatus.NotAcomplished;
            }
        }
    }
}
