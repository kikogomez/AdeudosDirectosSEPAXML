using ISO20022PaymentInitiations;

namespace DirectDebitElements
{
    public class DirectDebitInitiationContract
    {
        BankAccount creditorAccount;
        string creditorBusinessCode;
        CreditorAgent creditorAgent;
        string creditorID;

        public DirectDebitInitiationContract(BankAccount creditorAccount, string nIF, string creditorBusinessCode, CreditorAgent creditorAgent)
        {
            CheckArguments(creditorAccount, nIF, creditorBusinessCode, creditorAgent);
            this.creditorAccount = creditorAccount;
            this.creditorBusinessCode = creditorBusinessCode;
            GenerateCreditorID(nIF,creditorBusinessCode);
            this.creditorAgent = creditorAgent;
        }

        private void CheckArguments(BankAccount creditorAccount, string nIF, string creditorBusinessCode, CreditorAgent creditorAgent)
        {
            if (creditorAccount == null) throw new System.ArgumentNullException("creditorAccount");
            if (creditorAccount.HasValidIBAN == false) throw new System.ArgumentException("The Creditor Account IBAN is invalid", "creditorAccount");
            if (nIF == null) throw new System.ArgumentNullException("NIF");
            if (creditorBusinessCode == null) throw new System.ArgumentNullException("CreditorBusinessCode");
            if (creditorAgent == null) throw new System.ArgumentNullException("CreditorAgent");
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
