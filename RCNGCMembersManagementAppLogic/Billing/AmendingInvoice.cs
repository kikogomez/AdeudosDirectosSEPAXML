using System;
using System.Collections.Generic;

namespace RCNGCMembersManagementAppLogic.Billing
{
    public class AmendingInvoice: BaseInvoice
    {
        public AmendingInvoice(Invoice invoiceToAmend)
            :base(null,DateTime.Now)
        {
            InitializeAmendingInvoice(invoiceToAmend);
        }

        public InvoiceCustomerData CustomerData
        {
            get { return customerData; }
        }

        public List<Transaction> InvoiceDetail
        {
            get { return invoiceDetail; }
        }

        protected override string GetNewInvoiceID()
        {
            return null;
        }

        protected override void UpdateInvoiceSequenceNumber()
        {
        }

        private void InitializeAmendingInvoice(Invoice invoiceToAmend)
        {
            this.invoiceID = GenerateAmendingInvoiceID(invoiceToAmend);
            this.invoiceDetail = CreateAmendingTransactions(invoiceToAmend);
            this.customerData = CloneCustomerDataFromInvoice(invoiceToAmend);
        }

        private List<Transaction> CreateAmendingTransactions(Invoice invoiceToAmend)
        {
            List<Transaction> amendingInvoiceDetail = new List<Transaction>();
            Tax voidTax = new Tax("", 0);
            Transaction originalInvoiceReference = new Transaction("Amending invoice " + invoiceToAmend.InvoiceID + "as detailed", 1, 0, voidTax, 0);
            amendingInvoiceDetail.Add(originalInvoiceReference);
            foreach (Transaction transaction in invoiceToAmend.InvoiceDetail)
            {
                Transaction amendedTransaction = new Transaction(
                    "Amending " + transaction.Description, -transaction.Units, transaction.UnitCost, transaction.Tax, transaction.Discount);
                amendingInvoiceDetail.Add(amendedTransaction);
            }
            return amendingInvoiceDetail;
        }

        private string GenerateAmendingInvoiceID(Invoice invoiceToAmend)
        {
            BillingDataManager billDataManager = BillingDataManager.Instance;
            return invoiceToAmend.InvoiceID.Replace(billDataManager.InvoicePrefix, billDataManager.AmendingInvoicePrefix);
        }

        private InvoiceCustomerData CloneCustomerDataFromInvoice(Invoice invoiceToAmend)
        {
            return invoiceToAmend.CustomerData;
        }
    }
}
