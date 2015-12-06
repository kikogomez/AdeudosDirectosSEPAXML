using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace RCNGCMembersManagementAppLogic.Billing.DirectDebit
{
    public static class ISO7064CheckDigits
    {
        static string validAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string CalculateIBANCheckDigits(string nationalIdentifier, string ccc)
        {
            string longNumber = ccc + nationalIdentifier + "00";
            int resultMod97_10 = CalculateMod97_10(longNumber);
            return (resultMod97_10 == -1 ? null : resultMod97_10.ToString());
        }

        public static string CalculateSEPACreditIdentifierCheckDigits(string nationalIdentifier, string nIF)
        {
            nIF = DiscardNonAlfanumericCharacters(nIF);
            string longNumber = nIF + nationalIdentifier + "00";
            int resultMod97_10 = CalculateMod97_10(longNumber);
            return (resultMod97_10 == -1 ? null : resultMod97_10.ToString());
        }

        private static int CalculateMod97_10(string alphanumericString)
        {
            string numericString = ConvertAlphanumericStringToNumericString(alphanumericString, 10);
            if (numericString == null) return (-1);
            ulong modulus = CalculateLongNumberModulus(numericString, 97);
            return (int)(98 - modulus);
        }

        private static ulong CalculateLongNumberModulus(string longNumber, ulong baseNumber)
        {
            const int ulongMaxLenght = 19;
            string leftPart;
            string rightPart;
            ulong leftPartModulus;

            while (longNumber.Length > 19)
            {
                leftPart = longNumber.Substring(0, ulongMaxLenght);
                rightPart = longNumber.Substring(ulongMaxLenght);
                leftPartModulus = ulong.Parse(leftPart) % baseNumber;
                longNumber = leftPartModulus.ToString() + rightPart;
            }
            return ulong.Parse(longNumber) % baseNumber;
        }

        private static string ConvertAlphanumericStringToNumericString(string alphanumeric, int baseNumber)
        {
            bool containsOnlyAlphanumeric = Regex.IsMatch(alphanumeric, @"^[A-Z0-9]+$", RegexOptions.IgnoreCase);

            if (!containsOnlyAlphanumeric) return null;
            string[] arrayOfNumber = alphanumeric.Select(n => CharacterToNumber(n, baseNumber)).ToArray();
            return String.Join(String.Empty, arrayOfNumber);
        }

        private static string CharacterToNumber(char character, int baseNumber)
        {
            int number;
            bool isNumber = int.TryParse(character.ToString(), out number);
            if (isNumber) return character.ToString();
            int position = validAlphabet.IndexOf(character);
            return (position >= 0 ? (position + baseNumber).ToString() : null);
        }

        private static string DiscardNonAlfanumericCharacters(string sourceString)
        {
            return Regex.Replace(sourceString, @"[^A-Z0-9]", String.Empty, RegexOptions.IgnoreCase);
        }
    }
}
