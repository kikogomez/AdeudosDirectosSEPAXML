using System;

namespace DirectDebitElements
{
    public class DirectDebitAmendmentInformation
    {
        string oldMandateID;
        BankAccount oldBankAccount;

        public DirectDebitAmendmentInformation(string oldMandateID, BankAccount oldBankAccount)
        {
            this.oldMandateID = oldMandateID;
            this.oldBankAccount = oldBankAccount;
            try
            {
                CheckMandateIDAndBankAccount();
            } 
            catch (ArgumentException argumentException)
            {
                throw new TypeInitializationException("DirectDebitAmendmentInformation", argumentException);
            }
                
        }

        public string OldMandateID
        {
            get { return oldMandateID; }
        }

        public BankAccount OldBankAccount
        {
            get { return oldBankAccount; }
        }

        private void CheckMandateIDAndBankAccount()
        {
            if (oldMandateID != null && oldMandateID.Trim() == "") throw new ArgumentException("The MandateID can't be empty", "mandateID");
            if (oldBankAccount != null && !oldBankAccount.HasValidIBAN) throw new ArgumentException("The Bank Account must have a valid IBAN", "oldBankAccount");
        }
    }
}
