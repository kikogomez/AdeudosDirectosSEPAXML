using System.Collections.Generic;
using ReferencesAndTools;

namespace DirectDebitElements
{
    public class ClientAccountCodeCCC
    {

        BankAccountNumberChecker.CCCCheckDigits checkDigits;
        string ccc;

        public ClientAccountCodeCCC(string bankCode, string officeCode, string checkDigits, string accountNumber)
        {
            if (BankAccountNumberChecker.IsValidCCC(bankCode, officeCode, checkDigits, accountNumber))
            {
                this.checkDigits.bankOfficeCheckDigit = checkDigits[0].ToString();
                this.checkDigits.accountNumberCheckDigit = checkDigits[1].ToString();
                this.ccc =
                    bankCode +
                    officeCode +
                    checkDigits +
                    accountNumber;
            }
        }

        public ClientAccountCodeCCC(string ccc)
        {
            if (BankAccountNumberChecker.IsValidCCC(ccc))
            {
                Dictionary<string, string> splittedCCC = BankAccountNumberChecker.SplitCCC(ccc);
                this.checkDigits.bankOfficeCheckDigit = splittedCCC["checkDigits"][0].ToString();
                this.checkDigits.accountNumberCheckDigit = splittedCCC["checkDigits"][1].ToString();
                this.ccc = ccc;
            }
        }

        public string BankCode
        {
            get { return BankAccountNumberChecker.SplitCCC(ccc)["bank"]; }
        }

        public string OfficeCode
        {
            get { return BankAccountNumberChecker.SplitCCC(ccc)["office"]; }
        }

        public BankAccountNumberChecker.CCCCheckDigits CCCCheck
        {
            get { return checkDigits; }
        }

        public string AccountNumber
        {
            get { return BankAccountNumberChecker.SplitCCC(ccc)["accountNumber"]; }
        }

        public string CCC
        {
            get { return ccc; }
        }

        public string FormattedCCC
        {
            get
            {
                if (ccc != null) return (BankCode + " " + OfficeCode + " " + CCCCheck.bankOfficeCheckDigit + CCCCheck.accountNumberCheckDigit + " " + AccountNumber);
                return null;
            }
        }
    }
}
