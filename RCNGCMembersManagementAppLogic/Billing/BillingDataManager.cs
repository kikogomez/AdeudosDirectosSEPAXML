using System;

namespace RCNGCMembersManagementAppLogic.Billing
{
    public sealed class BillingDataManager
    {
        private static readonly BillingDataManager instance = new BillingDataManager();

        static IBillingSequenceNumbersManager billingSequenceNumbersManager;
        
        string invoicePrefix;
        string proFormaInvoicePrefix;
        string amendingInvoicePrefix;

        private BillingDataManager()
        {
            LoadConfig();
        }

        public static BillingDataManager Instance
        {
            get { return instance; }
        }

        public uint InvoiceSequenceNumber
        {
            get { return GetInvoiceSequenceNumber(); }
            set { SetInvoiceSequenceNumber(value); }
        }

        public uint ProFormaInvoiceSequenceNumber
        {
            get { return GetProFormaInvoiceSequenceNumber(); }
            set { SetProFormaInvoiceSequenceNumber(value); }
        }

        public uint DirectDebitSequenceNumber
        {
            get { return GetDirectDebitSequenceNumber(); }
            set { SetDirectDebitSequenceNumber(value); }
        }

        public string InvoicePrefix
        {
            get { return invoicePrefix; }
        }

        public string ProFormaInvoicePrefix
        {
            get { return proFormaInvoicePrefix; }
        }

        public string AmendingInvoicePrefix
        {
            get { return amendingInvoicePrefix; }
        }

        public void SetBillingSequenceNumberCollaborator(IBillingSequenceNumbersManager billingSequenceNumbersManager)
        {
            BillingDataManager.billingSequenceNumbersManager = billingSequenceNumbersManager;
        }

        public void CheckIfDirectDebitSequenceNumberIsInRange(uint directDebitSequenceNumber)
        {
            if (!DirectDebitSequenceNumberIsInRange(directDebitSequenceNumber))
                throw new ArgumentOutOfRangeException("directDebitSequenceNumber", "Direct Debit Sequence Number out of range (1-99999)");
        }

        private uint GetInvoiceSequenceNumber()
        {
            uint invoiceSequenceNumber=billingSequenceNumbersManager.GetInvoiceSequenceNumber();
            if (invoiceSequenceNumber >= 1000000)
                throw new ArgumentOutOfRangeException("invoiceSequenceNumber", "Max 999999 invoices per year");
            return invoiceSequenceNumber;
        }

        private void SetInvoiceSequenceNumber(uint invoiceSequenceNumber)
        {
            if (!InvoiceSequenceNuberIsInRange(invoiceSequenceNumber))
                throw new ArgumentOutOfRangeException("invoiceSequenceNumber", "Invoice ID out of range (1-999999)");
            billingSequenceNumbersManager.SetInvoiceSequenceNumber(invoiceSequenceNumber);
        }

        private uint GetProFormaInvoiceSequenceNumber()
        {
            uint proFormaInvoiceSequenceNumber = billingSequenceNumbersManager.GetProFormaInvoiceSequenceNumber();
            if (proFormaInvoiceSequenceNumber >= 1000000)
                throw new ArgumentOutOfRangeException("invoiceSequenceNumber", "Max 999999 invoices per year");
            return proFormaInvoiceSequenceNumber;
        }

        private void SetProFormaInvoiceSequenceNumber(uint proFormaInvoiceSequenceNumber)
        {
            if (!InvoiceSequenceNuberIsInRange(proFormaInvoiceSequenceNumber))
                throw new ArgumentOutOfRangeException("invoiceSequenceNumber", "Pro Forma Invoice ID out of range (1-999999)");
            billingSequenceNumbersManager.SetProFormaInvoiceSequenceNumber(proFormaInvoiceSequenceNumber);
        }

        private uint GetDirectDebitSequenceNumber()
        {
            uint directDebitSequenceNumber = billingSequenceNumbersManager.GetDirectDebitReferenceSequenceNumber();
            CheckIfDirectDebitSequenceNumberIsInRange(directDebitSequenceNumber);
            return directDebitSequenceNumber;
        }

        private void SetDirectDebitSequenceNumber(uint directDebitSequenceNumber)
        {
            CheckIfDirectDebitSequenceNumberIsInRange(directDebitSequenceNumber);
            billingSequenceNumbersManager.SetDirectDebitReferenceSequenceNumber(directDebitSequenceNumber);
        }

        private bool InvoiceSequenceNuberIsInRange(uint invoiceNumber)
        {
            return (1 <= invoiceNumber && invoiceNumber < 1000000);
        }

        private bool DirectDebitSequenceNumberIsInRange(uint directDebitSequenceNumber)
        {
            return (1 <= directDebitSequenceNumber && directDebitSequenceNumber < 100000);
        }

        private void LoadConfig()
        {
            invoicePrefix = "INV";
            proFormaInvoicePrefix = "PRF";
            amendingInvoicePrefix = "AMN";
        }
    }
}
