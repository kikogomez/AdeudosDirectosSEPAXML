using System;
using System.Collections.Generic;

namespace RCNGCMembersManagementAppLogic.Billing
{
    public class ProFormaInvoice: BaseInvoice
    {
        public ProFormaInvoice(InvoiceCustomerData invoiceCustomerData, List<Transaction> transactionsList, DateTime issueDate)
            : base(transactionsList, issueDate)
        {
            InitializeProformaInvoice(invoiceCustomerData);
        }
        
        public decimal BillsTotalAmountToCollect
        {
            get { return 0; }
        }

        public void SetNewInvoiceDetail(List<Transaction> invoiceDetail)
        {
            this.invoiceDetail = invoiceDetail;
        }

        protected override string GetNewInvoiceID()
        {
            string invoicePrefix = billingDataManager.ProFormaInvoicePrefix;
            string invoiceYear = "2013";
            return invoicePrefix + invoiceYear + billingDataManager.ProFormaInvoiceSequenceNumber.ToString("000000");
        }

        protected override void UpdateInvoiceSequenceNumber()
        {
            uint currentInvoiceSequenceNumber=ExtractInvoiceSequenceNumberFromInvoiceID();
            billingDataManager.ProFormaInvoiceSequenceNumber = currentInvoiceSequenceNumber + 1;
        }

        private void InitializeProformaInvoice(InvoiceCustomerData invoiceCustomerData)
        {
            CheckProFormaInvoiceDetail();
            this.customerData = invoiceCustomerData;
        }

        private void CheckProFormaInvoiceDetail()
        {
            foreach (Transaction transaction in invoiceDetail)
            {
                if (transaction.Units < 1)
                    throw new System.ArgumentOutOfRangeException("units", "Pro Forma Invoice transactions must have at least one element to transact");
            }
        }
    }
}
