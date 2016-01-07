using System.Collections.Generic;

namespace ReferencesAndTools
{
    public static class BankAccountNumberChecker
    {
        #region Check Numbers Calculators

        public static string CalculateCCCCheckDigit(string tenDigitsLongInt)
        {
            int[] pesos = { 1, 2, 4, 8, 5, 10, 9, 7, 3, 6 };
            int calculocheck = 0;

            if (!IsATenDigitsLongInt(tenDigitsLongInt)) return null;

            for (int i = 0; i <= 9; i++)
            {
                calculocheck += int.Parse(tenDigitsLongInt[i].ToString()) * pesos[i];
            }
            calculocheck = calculocheck % 11;
            if (calculocheck > 1) calculocheck = 11 - calculocheck;
            return calculocheck.ToString();
        }

        public static string CalculateSpanishIBANCheckDigits(string ccc)
        {
            return ISO7064CheckDigits.CalculateIBANCheckDigits("ES", ccc);
        }

        private static bool ItParsesLongInt(string stringToCheck)
        {
            long longResult;
            return long.TryParse(stringToCheck, out longResult);
        }

        private static bool IsTenCharactersStringWithoutSpaces(string stringToCheck)
        {
            return (stringToCheck ?? "").Replace(" ", "").Length == 10;
        }

        private static bool IsATenDigitsLongInt(string stringToCheck)
        {
            return (IsTenCharactersStringWithoutSpaces(stringToCheck) && ItParsesLongInt(stringToCheck));
        }
        #endregion

        #region CCC Ckecking

        public static CCCCheckDigits CalculateCCCCheckDigits(string bank, string office, string accountNumber)
        {
            return new CCCCheckDigits {
                bankOfficeCheckDigit = BankOfficeCCCCheck(bank, office),
                accountNumberCheckDigit = AccountNumberCCCCheck(accountNumber) };
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

        public static Dictionary<string, string> SplitCCC(string ccc)
        {
            return new Dictionary<string, string> {
                {"bank",ccc.Substring(0, CCCFieldLenghts.BankLength)},
                {"office",ccc.Substring(4, CCCFieldLenghts.OfficeLenght)},
                {"checkDigits", ccc.Substring(8,CCCFieldLenghts.CheckDigitsLenght)},
                {"accountNumber", ccc.Substring(10, CCCFieldLenghts.AccountNumberLenght)}};
        }

        public static void CheckBankAccountFieldsLength(string bank, string office, string checkDigits, string accountNumber)
        {
            ThrowExceptionOnTooLongAccountDataString("banco", bank, CCCFieldLenghts.BankLength);
            ThrowExceptionOnTooLongAccountDataString("sucursal", office, CCCFieldLenghts.OfficeLenght);
            ThrowExceptionOnTooLongAccountDataString("dígito de control", checkDigits, CCCFieldLenghts.CheckDigitsLenght);
            ThrowExceptionOnTooLongAccountDataString("número de cuenta", accountNumber, CCCFieldLenghts.AccountNumberLenght);
        }

        private static void ThrowExceptionOnTooLongAccountDataString(string fieldName, string fieldValue, int maxLenght)
        {
            if ((fieldValue ?? "").Length > maxLenght) throw new System.ArgumentException("El código de " + fieldName + " es demasiado largo", fieldName);
        }

        private static string BankOfficeCCCCheck(string bank, string office)
        {
            return CalculateCCCCheckDigit("00" + bank + office);
        }

        private static string AccountNumberCCCCheck(string accountNumber)
        {
            return CalculateCCCCheckDigit(accountNumber);
        }

        private static bool CheckDigitsAreRightSize(string checkDigits)
        {
            return (checkDigits ?? "").Trim().Length == CCCFieldLenghts.CheckDigitsLenght;
        }

        private static bool CCCIsRightSize(string ccc)
        {
            return (ccc ?? "").Trim().Length == CCCFieldLenghts.CCCLength;
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

        #endregion

        #region IBAN Checking

        public static string CalculateSpanishIBANCheck(string bank, string office, string cccCheckDigits, string accountNumber)
        {
            string ccc = bank + office + cccCheckDigits + accountNumber;
            return CalculateSpanishIBANCheck(ccc);
        }

        public static string CalculateSpanishIBANCheck(string ccc)
        {
            if (!IsValidCCC(ccc)) return null;
            return CalculateSpanishIBANCheckDigits(ccc);
        }

        public static string CalculateSpanishIBAN(string bank, string office, string accountNumber)
        {
            CCCCheckDigits cccCheckDigits = CalculateCCCCheckDigits(bank, office, accountNumber);
            string cccCheckDigitsString = cccCheckDigits.bankOfficeCheckDigit + cccCheckDigits.accountNumberCheckDigit;
            return CalculateSpanishIBAN(bank, office, cccCheckDigitsString, accountNumber);
        }

        public static string CalculateSpanishIBAN(string bank, string office, string checkDigits, string accountNumber)
        {
            string ccc = bank + office + checkDigits + accountNumber;
            return CalculateSpanishIBAN(ccc);
        }

        public static string CalculateSpanishIBAN(string ccc)
        {
            if (!IsValidCCC(ccc)) return null;
            string iban = "ES" + CalculateSpanishIBANCheckDigits(ccc) + ccc;
            return iban;
        }

        public static bool IsValidIBAN(string iban)
        {
            if (((iban ?? "").Length) != 24) return false;
            string ccc = iban.Substring(iban.Length - CCCFieldLenghts.CCCLength);
            string ibanCheckDigits = iban.Substring(2, 2);
            string countryCode = iban.Substring(0, 2);
            return (
                countryCode == "ES" &&
                IsValidCCC(ccc) &&
                CalculateSpanishIBANCheckDigits(ccc) == ibanCheckDigits);
        }

        #endregion

    }
}
