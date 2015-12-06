namespace RCNGCMembersManagementAppLogic.Billing.DirectDebit
{
    public class InternationalAccountBankNumberIBAN
    {
        string iban;

        public InternationalAccountBankNumberIBAN(string iban)
        {
            if (IsValidIBAN(iban)) this.iban = iban;
        }

        public InternationalAccountBankNumberIBAN(ClientAccountCodeCCC ccc)
        {
            if (ClientAccountCodeCCC.IsValidCCC(ccc.CCC))
            {
                this.iban = CalculateSpanishIBAN(ccc.CCC);
            }
        }

        public string IBAN
        {
            get { return iban; }
        }

        public string FormattedIBAN
        {
            get
            {
                if (iban!=null) return "IBAN " + splitIBAN(iban);
                return null;
            }
        }

        public string CCC
        {
            get { return iban.Substring(4); }
        }

        public string InternationalCode
        {
            get { return iban.Substring(0,2); }
        }

        public string IBANCheck
        {
            get { return iban.Substring(2,2); }
        }

        public static bool IsValidIBAN(string iban)
        {
            if (((iban ?? "").Length)!=24) return false;
            string ccc = iban.Substring(iban.Length-ClientAccountCodeCCC.CCCFieldLenghts.CCCLength);
            string ibanCheckDigits = iban.Substring(2,2);
            string countryCode = iban.Substring(0, 2);
            return (
                countryCode=="ES" &&
                ClientAccountCodeCCC.IsValidCCC(ccc) &&
                BankAccountCheckNumbersCalculator.CalculateSpanishIBANCheckDigits(ccc)== ibanCheckDigits);
        }

        public static string CalculateSpanishIBAN(string ccc)
        {
            if (!ClientAccountCodeCCC.IsValidCCC(ccc)) return null;
            string iban = "ES" + BankAccountCheckNumbersCalculator.CalculateSpanishIBANCheckDigits(ccc) + ccc;
            return iban;
        }

        public static string CalculateSpanishIBANCheck(string ccc)
        {
            if (!ClientAccountCodeCCC.IsValidCCC(ccc)) return null;
            return BankAccountCheckNumbersCalculator.CalculateSpanishIBANCheckDigits(ccc);
        }

        private string splitIBAN(string iban)
        {
            string splittedIBAN=string.Empty;
            for (int i = 0; i <= 20; i += 4)
            {
                splittedIBAN += iban.Substring(i, 4) + " ";
            }
            return splittedIBAN.TrimEnd();
        }
    }
}
