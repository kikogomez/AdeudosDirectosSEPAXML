namespace ISO20022PaymentInitiations.DirectDebitPOCOClasses
{
    public class Creditor_POCOClass
    {
        string name;
        string identification;
        string iBAN;

        public Creditor_POCOClass(string name, string identification, string iBAN)
        {
            this.name = name;
            this.identification = identification;
            this.iBAN = iBAN;
        }

        public string Name
        {
            get { return name; }
        }

        public string Identification
        {
            get { return identification; }
        }

        public string IBAN
        {
            get { return iBAN; }
        }
    }
}
