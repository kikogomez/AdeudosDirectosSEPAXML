namespace DirectDebitElements
{
    public class BankAccount
    {
        BankAccountFields bankAccountFields;
        ClientAccountCodeCCC ccc;
        InternationalAccountBankNumberIBAN iban;

        public BankAccount(BankAccountFields bankAccountFields)
        {
            this.bankAccountFields = bankAccountFields;
            ccc = new ClientAccountCodeCCC(bankAccountFields.BankCode, bankAccountFields.OfficeCode, bankAccountFields.CheckDigits, bankAccountFields.AccountNumber);
            iban = new InternationalAccountBankNumberIBAN(ccc);
        }

        public BankAccount(ClientAccountCodeCCC ccc)
        {
            this.ccc = ccc;
            this.bankAccountFields = new BankAccountFields(
                ccc.BankCode, ccc.OfficeCode, ccc.CCCCheck.bankOfficeCheckDigit + ccc.CCCCheck.accountNumberCheckDigit, ccc.AccountNumber);
            iban = new InternationalAccountBankNumberIBAN(ccc);
        }

        public BankAccount(InternationalAccountBankNumberIBAN iban)
        {
            this.iban=iban;
            this.ccc = new ClientAccountCodeCCC(iban.CCC);
            this.bankAccountFields = new BankAccountFields(
                ccc.BankCode, ccc.OfficeCode, ccc.CCCCheck.bankOfficeCheckDigit + ccc.CCCCheck.accountNumberCheckDigit, ccc.AccountNumber);
        }

        public BankAccountFields BankAccountFieldCodes
        {
            get { return bankAccountFields; }
        }

        public ClientAccountCodeCCC CCC
        {
            get { return ccc; }
        }

        public InternationalAccountBankNumberIBAN IBAN
        {
            get { return iban; }
        }

        public bool HasValidBankAccountFields
        {
            get { return bankAccountFields.AreValid(); }
        }

        public bool HasValidCCC
        {
            get { return (ccc.CCC != null); }
        }

        public bool HasValidIBAN
        {
            get { return (iban.IBAN != null); }
        }
    }
}
