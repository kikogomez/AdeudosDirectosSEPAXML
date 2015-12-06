using System;
using System.Collections.Generic;
using System.Linq;

namespace RCNGCMembersManagementAppLogic.Billing
{
    public class Invoice: BaseInvoice
    {
        Dictionary<string, Bill> invoiceBills;
        InvoicePaymentState invoiceState;
        Dictionary<DateTime,PaymentAgreement> paymentAgreements;

        public Invoice(InvoiceCustomerData invoiceCustomerData, List<Transaction> transactionsList, DateTime issueDate)
            : this(invoiceCustomerData, transactionsList, null, issueDate)
        {
        }

        public Invoice(string invoiceID, InvoiceCustomerData invoiceCustomerData, List<Transaction> transactionsList, DateTime issueDate)
            : this(invoiceID, invoiceCustomerData, transactionsList, null, issueDate)
        {
        }

        public Invoice(InvoiceCustomerData invoiceCustomerData, List<Transaction> transactionsList, List<Bill> billsList, DateTime issueDate)
            :base(transactionsList, issueDate)
        {
            InitializeInvoice(invoiceCustomerData, billsList);
        }

        public Invoice(string invoiceID, InvoiceCustomerData invoiceCustomerData, List<Transaction> transactionsList, List<Bill> billsList, DateTime issueDate)
            : base(invoiceID,transactionsList, issueDate)
        {
            InitializeInvoice(invoiceCustomerData, billsList);
        }

        public enum InvoicePaymentState { ToBePaid, Paid, Unpaid, Cancelled, Uncollectible }

        public InvoiceCustomerData CustomerData
        {
            get { return customerData; }
        }

        public DateTime IssueDate
        {
            get { return issueDate; }
        }

        public List<Transaction> InvoiceDetail
        {
            get { return invoiceDetail; }
        }

        public Dictionary<string, Bill> Bills
        {
            get { return invoiceBills; }
        }

        public decimal BillsTotalAmountToCollect
        {
            get { return GetBillsTotalAmount(Bill.BillPaymentResult.ToCollect); }
        }

        public InvoicePaymentState InvoiceState
        {
            get { return invoiceState; }
        }

        public Dictionary<DateTime, PaymentAgreement> PaymentAgreements
        {
            get { return paymentAgreements; }
        }

        public void RenegotiateBillsIntoInstalments (PaymentAgreement paymentAgreement, List<Bill> billsToRenegotiate, List<Bill> billsToAdd)
        {
            this.paymentAgreements.Add(paymentAgreement.AgreementDate.Date, paymentAgreement);
            ReplaceNegotiatedBills(billsToRenegotiate, billsToAdd, paymentAgreement);
        }

        private void ReplaceNegotiatedBills(List<Bill> billsToRenegotiate, List<Bill> billsToAdd, PaymentAgreement paymentAgreement)
        {
            AssignPaymentAgreementToBills(paymentAgreement, billsToAdd);
            MarkRenegotiatedBills(billsToRenegotiate, paymentAgreement);
            AddBillsToInvoice(billsToAdd);
        }

        public void Cancel()
        {
            this.invoiceState = InvoicePaymentState.Cancelled;
            CancelAllPendingBills();
        }

        public void CheckIfInvoiceIsFullPaid()
        {
            if (BillsTotalAmountToCollect == 0) invoiceState = InvoicePaymentState.Paid;
        }

        public void SetInvoiceAsUnpaid()
        {
            this.invoiceState = InvoicePaymentState.Unpaid;
        }

        public void SetInvoiceToBePaidIfHasNoUnpaidBills()
        {
            if (InvoiceHasBillsToCollect() && InvoiceHasNoUnpaidBills()) this.invoiceState = InvoicePaymentState.ToBePaid;
        }

        private bool InvoiceHasNoUnpaidBills()
        {
            Dictionary<string, Bill> billsCollection = this.invoiceBills;
            var unpaidBills = billsCollection.Where(bill => bill.Value.PaymentResult == Bill.BillPaymentResult.Unpaid);
            return (unpaidBills.Count() == 0);
        }

        private bool InvoiceHasBillsToCollect()
        {
            Dictionary<string, Bill> billsCollection = this.invoiceBills;
            var toCollectBills = billsCollection.Where(bill => bill.Value.PaymentResult == Bill.BillPaymentResult.ToCollect);
            return (toCollectBills.Count() != 0);
        }

        

        protected override string GetNewInvoiceID()
        {
            string invoicePrefix = billingDataManager.InvoicePrefix;
            string invoiceYear = "2013";
            return invoicePrefix + invoiceYear + billingDataManager.InvoiceSequenceNumber.ToString("000000");
        }

        protected override void UpdateInvoiceSequenceNumber()
        {
            uint currentInvoiceSequenceNumber=ExtractInvoiceSequenceNumberFromInvoiceID();
            billingDataManager.InvoiceSequenceNumber = currentInvoiceSequenceNumber + 1;
        }

        private decimal GetBillsTotalAmount(Bill.BillPaymentResult paymentResult)
        {
            decimal amount = 0;
            foreach (KeyValuePair<string, Bill> bill in invoiceBills)
            {
                if (bill.Value.PaymentResult == paymentResult) amount += bill.Value.Amount;
            }
            return amount;
        }

        private void InitializeInvoice(InvoiceCustomerData invoiceCustomerData, List<Bill> billsList)
        {
            this.customerData = invoiceCustomerData;
            CheckInvoiceDetail();
            invoiceBills = new Dictionary<string, Bill>();
            if (billsList == null) billsList = new List<Bill> { CreateASingleBillForInvoiceTotal() };
            AddBillsToInvoice(billsList);
            invoiceState = InvoicePaymentState.ToBePaid;
            paymentAgreements = new Dictionary<DateTime,PaymentAgreement>();
        }

        private Bill CreateASingleBillForInvoiceTotal()
        {
            string billID = invoiceID + "/001";
            string description = invoiceDetail[0].Description;
            DateTime dueDate = issueDate.AddDays(30);
            return new Bill(billID, description, NetAmount, issueDate, dueDate);
        }

        private void AddBillsToInvoice(List<Bill> billsList)
        {
            int billsCounter = invoiceBills.Count;
            foreach (Bill bill in billsList)
            {
                billsCounter++;
                if (bill.BillID == null) bill.BillID = invoiceID + "/" + billsCounter.ToString("000");
                invoiceBills.Add(bill.BillID, bill);
            }
        }

        private void CheckInvoiceDetail()
        {
            if (invoiceDetail.Count == 0) throw new System.ArgumentNullException("invoiceDetail","The invoice detail can't be empty");
        }

        private void CancelAllPendingBills()
        {
            List<Bill> pendingBills = Bills
                .Select(billsDictionayElement => billsDictionayElement.Value)
                .Where(bill => bill.PaymentResult == Bill.BillPaymentResult.ToCollect || bill.PaymentResult == Bill.BillPaymentResult.Unpaid).ToList();
            foreach (Bill bill in pendingBills) bill.CancelBill();
        }

        private void AssignPaymentAgreementToBills(PaymentAgreement paymentAgreement, List<Bill> billsList)
        {
            foreach (Bill bill in billsList) bill.AssignAgreement(paymentAgreement);
        }

        private void MarkRenegotiatedBills(List<Bill> billsToMark, PaymentAgreement paymentAgreement)
        {
            foreach (Bill bill in billsToMark) bill.RenegotiateBill(paymentAgreement);
        }
    }
}
