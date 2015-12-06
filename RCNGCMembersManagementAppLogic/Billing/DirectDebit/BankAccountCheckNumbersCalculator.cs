namespace RCNGCMembersManagementAppLogic.Billing.DirectDebit
{
    static class BankAccountCheckNumbersCalculator
    {
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
            return (stringToCheck ?? "").Replace(" ","").Length == 10;
        }

        private static bool IsATenDigitsLongInt(string stringToCheck)
        {
            return (IsTenCharactersStringWithoutSpaces(stringToCheck) && ItParsesLongInt(stringToCheck));
        }
    }
}
