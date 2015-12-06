namespace RCNGCMembersManagementAppLogic.Billing.DirectDebit
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
            this.iban = new InternationalAccountBankNumberIBAN(ccc);
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

        public static ClientAccountCodeCCC.CCCCheckDigits CalculateCCCCheckDigits(string bank, string office, string accountNumber)
        {
            return ClientAccountCodeCCC.CalculateCCCCheckDigits(bank, office, accountNumber);
        }

        public static string CalculateCCC(string bank, string office, string accountNumber)
        {
            return ClientAccountCodeCCC.CalculateCCC(bank, office, accountNumber);
        }

        public static bool IsValidCCC(string bank, string office, string checkDigits, string accountNumber)
        {
            string ccc = bank + office + checkDigits + accountNumber;
            return IsValidCCC(ccc);
        }

        public static bool IsValidCCC(string ccc)
        {
            return ClientAccountCodeCCC.IsValidCCC(ccc);
        }

        public static string CalculateSpanishIBANCheck(string bank, string office, string cccCheckDigits, string accountNumber)
        {
            string ccc = bank + office + cccCheckDigits + accountNumber;
            return CalculateSpanishIBANCheck(ccc);
        }

        public static string CalculateSpanishIBANCheck(string ccc)
        {
            return InternationalAccountBankNumberIBAN.CalculateSpanishIBANCheck(ccc);
        }

        public static string CalculateSpanishIBAN(string bank, string office, string accountNumber)
        {
            ClientAccountCodeCCC.CCCCheckDigits cccCheckDigits = CalculateCCCCheckDigits(bank, office, accountNumber);
            string ccc = bank + office + cccCheckDigits.bankOfficeCheckDigit + cccCheckDigits.accountNumberCheckDigit + accountNumber;
            return CalculateSpanishIBAN(ccc);
        }

        public static string CalculateSpanishIBAN(string bank, string office, string checkDigits, string accountNumber)
        {
            string ccc = bank + office + checkDigits + accountNumber;
            return CalculateSpanishIBAN(ccc);
        }

        public static string CalculateSpanishIBAN(string ccc)
        {
            return InternationalAccountBankNumberIBAN.CalculateSpanishIBAN(ccc);
        }

        public static bool IsValidIBAN(string iban)
        {
            return InternationalAccountBankNumberIBAN.IsValidIBAN(iban);
        }
    }
}
