namespace ISO20022PaymentInitiations.DirectDebitPOCOClasses
{
    public class CreditorAgent_POCOClass
    {
        string bIC;

        public CreditorAgent_POCOClass(string bIC)
        {
            this.bIC= bIC;
        }

        public string BIC
        {
            get { return bIC; }
        }
    }
}
