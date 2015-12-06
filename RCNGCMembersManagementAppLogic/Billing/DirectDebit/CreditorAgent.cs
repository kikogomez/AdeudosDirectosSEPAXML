namespace RCNGCMembersManagementAppLogic.Billing.DirectDebit
{
    public class CreditorAgent
    {
        BankCode bankCode;

        public CreditorAgent(BankCode bankCode)
        {
            this.bankCode = bankCode;
        }

        public string LocalBankCode
        {
            get { return bankCode.LocalBankCode; }
        }

        public string BankName
        {
            get { return bankCode.BankName; }
        }

        public string BankBIC
        {
            get { return bankCode.BankBIC; }
        }
    }
}
