using System.Collections.Generic;

namespace RCNGCMembersManagementAppLogic.Billing.DirectDebit
{
    public class ClientAccountCodeCCC
    {

        CCCCheckDigits checkDigits;
        string ccc;

        public ClientAccountCodeCCC(string bankCode, string officeCode, string checkDigits, string accountNumber)
        {
            if (IsValidCCC(bankCode, officeCode, checkDigits, accountNumber))
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
            if (IsValidCCC(ccc))
            {
                Dictionary<string, string> splittedCCC = SplitCCC(ccc);
                this.checkDigits.bankOfficeCheckDigit = splittedCCC["checkDigits"][0].ToString();
                this.checkDigits.accountNumberCheckDigit = splittedCCC["checkDigits"][1].ToString();
                this.ccc = ccc;
            }
        }

        public string BankCode
        {
            get { return SplitCCC(ccc)["bank"]; }
        }

        public string OfficeCode
        {
            get { return SplitCCC(ccc)["office"]; }
        }

        public CCCCheckDigits CCCCheck
        {
            get { return checkDigits; }
        }

        public string AccountNumber
        {
            get { return SplitCCC(ccc)["accountNumber"]; }
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

        public static CCCCheckDigits CalculateCCCCheckDigits(string bank, string office, string accountNumber)
        {
            return new CCCCheckDigits { bankOfficeCheckDigit = BankOfficeCCCCheck(bank, office), accountNumberCheckDigit = AccountNumberCCCCheck(accountNumber) };
        }

        public static string CalculateCCC(string bank, string office, string accountNumber)
        {
            CCCCheckDigits checkDigits = CalculateCCCCheckDigits(bank, office, accountNumber);
            if (checkDigits.bankOfficeCheckDigit != null && checkDigits.accountNumberCheckDigit != null)
            {
                return bank + office + checkDigits.bankOfficeCheckDigit + checkDigits.accountNumberCheckDigit + accountNumber;
            }
            return null;
        }

        public static bool IsValidCCC(string bank, string office, string checkDigits, string accountNumber)
        {
            if (!CheckDigitsAreRightSize(checkDigits)) return false;
            CCCCheckDigits calculatedCCCCheckDigits = CalculateCCCCheckDigits(bank, office, accountNumber);
            return (
                calculatedCCCCheckDigits.bankOfficeCheckDigit == checkDigits[0].ToString() &&
                calculatedCCCCheckDigits.accountNumberCheckDigit == checkDigits[1].ToString());
        }

        public static bool IsValidCCC(string ccc)
        {
            if (!CCCIsRightSize(ccc)) return false;
            Dictionary<string, string> splittedCCC = SplitCCC(ccc);
            return IsValidCCC(splittedCCC["bank"], splittedCCC["office"], splittedCCC["checkDigits"], splittedCCC["accountNumber"]);
        }

        private static string BankOfficeCCCCheck(string bank, string office)
        {
            return BankAccountCheckNumbersCalculator.CalculateCCCCheckDigit("00" + bank + office);
        }

        private static string AccountNumberCCCCheck(string accountNumber)
        {
            return BankAccountCheckNumbersCalculator.CalculateCCCCheckDigit(accountNumber);
        }

        private static bool CheckDigitsAreRightSize(string checkDigits)
        {
            return (checkDigits ?? "").Trim().Length == CCCFieldLenghts.CheckDigitsLenght;
        }

        private static bool CCCIsRightSize(string ccc)
        {
            return (ccc ?? "").Trim().Length == CCCFieldLenghts.CCCLength;
        }

        private static Dictionary<string, string> SplitCCC(string ccc)
        {
            return new Dictionary<string, string> {
                {"bank",ccc.Substring(0, CCCFieldLenghts.BankLength)},
                {"office",ccc.Substring(4, CCCFieldLenghts.OfficeLenght)},
                {"checkDigits", ccc.Substring(8,CCCFieldLenghts.CheckDigitsLenght)},
                {"accountNumber", ccc.Substring(10, CCCFieldLenghts.AccountNumberLenght)}};
        }

        public struct CCCFieldLenghts
        {
            public const int BankLength = 4;
            public const int OfficeLenght = 4;
            public const int CheckDigitsLenght = 2;
            public const int AccountNumberLenght = 10;
            public const int CCCLength = 20;
        }

        public struct CCCCheckDigits
        {
            public string bankOfficeCheckDigit;
            public string accountNumberCheckDigit;
        }
    }
}
