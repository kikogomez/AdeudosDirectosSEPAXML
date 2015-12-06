using System;
using System.Collections.Generic;

namespace RCNGCMembersManagementAppLogic.Billing
{
    public abstract class BaseInvoice
    {
        protected BillingDataManager billingDataManager;
        protected InvoiceCustomerData customerData;
        protected string invoiceID;
        protected DateTime issueDate;
        protected List<Transaction> invoiceDetail;

        public BaseInvoice(List<Transaction> transactionsList, DateTime issueDate)
            : this(null, transactionsList, issueDate)
        {
            UpdateInvoiceSequenceNumber();
        }

        public BaseInvoice(string invoiceID, List<Transaction> transactionsList, DateTime issueDate)
        {
            this.billingDataManager = BillingDataManager.Instance;
            this.invoiceID = (invoiceID == null) ? GetNewInvoiceID(): invoiceID;
            this.issueDate = issueDate;
            invoiceDetail = transactionsList;
        }

        public string InvoiceID
        {
            get { return this.invoiceID; }
        }

        public decimal NetAmount
        {
            get { return CalculateInvoiceAmounts(true); }
        }

        public decimal GrossAmount
        {
            get { return CalculateInvoiceAmounts(false); }
        }

        public decimal TaxAmount
        {
            get { return CalculateTaxAmount(); }
        }

        protected abstract string GetNewInvoiceID();

        protected abstract void UpdateInvoiceSequenceNumber();

        protected uint ExtractInvoiceSequenceNumberFromInvoiceID()
        {
            return uint.Parse(invoiceID.Substring(7));
        }

        protected decimal CalculateInvoiceAmounts(bool netAmount)
        {
            decimal amount = 0;
            foreach (Transaction line in invoiceDetail)
            {
                amount += (netAmount ? (decimal)line.NetAmount : line.GrossAmount);
            }
            return amount;
        }

        protected decimal CalculateTaxAmount()
        {
            decimal amount = 0;
            foreach (Transaction line in invoiceDetail)
            {
                amount += ((decimal)line.TaxAmount);
            }
            return amount;
        }
     }
}
