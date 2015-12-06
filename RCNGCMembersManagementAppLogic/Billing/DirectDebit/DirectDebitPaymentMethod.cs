namespace RCNGCMembersManagementAppLogic.Billing.DirectDebit
{
    class DirectDebitPaymentMethod: PaymentMethod
    {
        private DirectDebitMandate directDebitMandate;
        private string directDebitTransactionPaymentIdentification;

        public DirectDebitPaymentMethod(DirectDebitMandate directDebitMandate, string directDebitransactionPaymentIdentification)
            : base()
        {
            this.directDebitMandate = directDebitMandate;
            this.directDebitTransactionPaymentIdentification = directDebitransactionPaymentIdentification;
        }

        public DirectDebitMandate DirectDebitMandate
        {
            get { return directDebitMandate; }
        }

        public string DDTXPaymentIdentification
        {
            get { return directDebitTransactionPaymentIdentification; }
        }
    }
}
