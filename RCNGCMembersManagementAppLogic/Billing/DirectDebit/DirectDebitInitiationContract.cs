namespace RCNGCMembersManagementAppLogic.Billing.DirectDebit
{
    public class DirectDebitInitiationContract
    {
        BankAccount creditorAccount;
        string creditorBusinessCode;
        CreditorAgent creditorAgent;
        string creditorID;

        public DirectDebitInitiationContract(BankAccount creditorAccount, string nIF, string creditorBusinessCode, CreditorAgent creditorAgent)
        {
            this.creditorAccount = creditorAccount;
            this.creditorBusinessCode = creditorBusinessCode;
            GenerateCreditorID(nIF,creditorBusinessCode);
            this.creditorAgent = creditorAgent;
        }

        public BankAccount CreditorAcount
        {
            get { return creditorAccount; }
        }

        public string CreditorBussinessCode
        {
            get { return creditorBusinessCode; }
        }

        public CreditorAgent CreditorAgent
        {
            get { return creditorAgent; }
        }

        public string CreditorID
        {
            get { return creditorID; }
        }

        public void ChangeCreditorBank(BankAccount creditorAccount)
        {
            if (creditorAccount.BankAccountFieldCodes.BankCode != this.creditorAgent.LocalBankCode) 
                throw new System.ArgumentException("The new account must be from the same Creditor Agent", "creditorAccount");  
            this.creditorAccount = creditorAccount;
        }

        private void GenerateCreditorID(string nIF, string creditorBusinessCode)
        {
            SEPAAttributes sEPAAttributes = new SEPAAttributes();
            creditorID = sEPAAttributes.AT02CreditorIdentifier("ES", nIF, creditorBusinessCode);
        }
    }
}
