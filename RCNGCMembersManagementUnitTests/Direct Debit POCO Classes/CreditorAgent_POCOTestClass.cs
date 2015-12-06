namespace RCNGCMembersManagementUnitTests.DirectDebitPOCOClasses
{
    public class CreditorAgent_POCOTestClass
    {
        string bIC;

        public CreditorAgent_POCOTestClass(string bIC)
        {
            this.bIC= bIC;
        }

        public string BIC
        {
            get { return bIC; }
        }
    }
}
