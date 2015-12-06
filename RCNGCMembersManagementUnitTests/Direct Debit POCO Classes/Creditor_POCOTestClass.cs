namespace RCNGCMembersManagementUnitTests.DirectDebitPOCOClasses
{
    public class Creditor_POCOTestClass
    {
        string name;
        string identification;
        string iBAN;

        public Creditor_POCOTestClass(string name, string identification, string iBAN)
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
