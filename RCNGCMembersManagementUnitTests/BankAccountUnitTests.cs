using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementAppLogic.Billing.DirectDebit;

namespace RCNGCMembersManagementUnitTests
{
    [TestClass]
    public class BankAccountUnitTests
    {
        [TestMethod]
        public void ThisIsAValidCCC()
        {
            Assert.IsTrue(BankAccount.IsValidCCC("1234", "5678", "06", "1234567890"));
        }

        [TestMethod]
        public void ThisIsAnotherValidCCC()
        {
            Assert.IsTrue(BankAccount.IsValidCCC("0128", "0035", "69", "0987654321"));
        }

        [TestMethod]
        public void IsNotValidCCC()
        {
            Assert.IsFalse(BankAccount.IsValidCCC("1234", "5678", "05", "1234567890"));
        }

        [TestMethod]
        public void NoExceptionsOnNullsEmptyStringsAndLongDigitStringsWhenValidating()
        {
            Assert.IsFalse(BankAccount.IsValidCCC(null, "", "05", "123456789123456789"));
        }

        [TestMethod]
        public void CCCWithAlphabeticCharactersIsInvalid()
        {
            Assert.IsFalse(BankAccount.IsValidCCC("aaaa", "5678", "06", "1234567890"));
        }

        [TestMethod]
        public void CCCWithSpacesIsInvalid()
        {
            Assert.IsFalse(BankAccount.IsValidCCC("1234", "5678", "06", "1234 56789"));
        }

        [TestMethod]
        public void CCCWithSeparatorsIsInvalid()
        {
            Assert.IsFalse(BankAccount.IsValidCCC("1234", "5678", "06", "12-3456/7"));
        }


        [TestMethod]
        public void CCCWithIncompleteDataIsInvalid()
        {
            Assert.IsFalse(BankAccount.IsValidCCC("12", "350", "69", "987654321"));
        }

        [TestMethod]
        public void CCCWithNullCheckDigitsIsNotValid()
        {
            Assert.IsFalse(BankAccount.IsValidCCC("aaaa", "5678", "", "bbbbbb"));
        }

        [TestMethod]
        public void DontFillWithZerosToTheLeft()
        {
            Assert.IsTrue(BankAccount.IsValidCCC("0128", "0035", "69", "0987654321"));
            Assert.IsFalse(BankAccount.IsValidCCC("128", "35", "69", "987654321"));
        }

        [TestMethod]
        public void SpacesAreNotEqualZeros()
        {
            Assert.IsTrue(BankAccount.IsValidCCC("0128", "0035", "69", "0987654321"));
            Assert.IsFalse(BankAccount.IsValidCCC(" 128", "  35", "69", "987654321"));
        }

        [TestMethod]
        public void IsValidSingleStringCCC()
        {
            Assert.IsTrue(BankAccount.IsValidCCC("12345678061234567890"));
        }

        [TestMethod]
        public void IsNotValidSingleStringCCC()
        {
            Assert.IsFalse(BankAccount.IsValidCCC("12345678051234567890"));
        }

        [TestMethod]
        public void NullSingleStringCCCIsInvalid()
        {
            Assert.IsFalse(BankAccount.IsValidCCC(null));
        }

        [TestMethod]
        public void EmptyStringSingleStringCCCIsInvalid()
        {
            Assert.IsFalse(BankAccount.IsValidCCC(""));
        }

        [TestMethod]
        public void AlfanumericOrWithSpacesSingleStringCCCAreInvalid()
        {
            Assert.IsFalse(BankAccount.IsValidCCC("  abcd56gg / 7890128"));
        }

        [TestMethod]
        public void CCCLongerThan20DigitsIsInvalid()
        {
            Assert.IsFalse(BankAccount.IsValidCCC("12345678901234567890123134"));
        }

        [TestMethod]
        public void CCCShorterThan20DigitsIsInvalid()
        {
            Assert.IsFalse(BankAccount.IsValidCCC("12345678901123134"));
        }

        [TestMethod]
        public void CCCCheckDigitsAreWellCalculated()
        {
            ClientAccountCodeCCC.CCCCheckDigits expectedCheckDigits = new ClientAccountCodeCCC.CCCCheckDigits { bankOfficeCheckDigit = "0", accountNumberCheckDigit = "6" };
            Assert.AreEqual(BankAccount.CalculateCCCCheckDigits("1234", "5678", "1234567890"), expectedCheckDigits);
        }

        [TestMethod]
        public void CCCCheckDigitsCalculationOnInvalidArgumentsReturnsNulls()
        {
            ClientAccountCodeCCC.CCCCheckDigits expectedCheckDigits = new ClientAccountCodeCCC.CCCCheckDigits { bankOfficeCheckDigit = null, accountNumberCheckDigit = null };
            Assert.AreEqual(BankAccount.CalculateCCCCheckDigits(null, "", "cccccccccc"), expectedCheckDigits);
        }

        [TestMethod]
        public void NoExceptionsOnLongDigitStrings()
        {
            ClientAccountCodeCCC.CCCCheckDigits expectedCheckDigits = new ClientAccountCodeCCC.CCCCheckDigits { bankOfficeCheckDigit = "0", accountNumberCheckDigit = null };
            Assert.AreEqual(BankAccount.CalculateCCCCheckDigits("1234", "5678", "546546564234567890"), expectedCheckDigits);
        }

        [TestMethod]
        public void CCCCIsWellCalculated()
        {
            Assert.AreEqual(BankAccount.CalculateCCC("1111", "2222", "3333333333"), "11112222003333333333");
        }

        [TestMethod]
        public void IfInvalidDataCCCCalculationReturnsNull()
        {
            Assert.AreEqual(BankAccount.CalculateCCC("1 24", null, "100/234-1"), null);
        }

        [TestMethod]
        public void ThisIsAValidIBAN()
        {
            Assert.IsTrue(BankAccount.IsValidIBAN("ES6812345678061234567890"));
        }

        [TestMethod]
        public void ThisIsAnotherValidIBAN()
        {
            Assert.IsTrue(BankAccount.IsValidIBAN("ES3011112222003333333333"));
        }

        [TestMethod]
        public void ThisIsAnInvalidIBAN()
        {
            Assert.IsFalse(BankAccount.IsValidIBAN("ES1111111111111111111111"));
        }

        [TestMethod]
        public void ThisIBANHasAValidIBANCheckButHasAnInvalidCCC()
        {
            Assert.IsFalse(BankAccount.IsValidIBAN("ES3312345678051234567890"));
        }

        [TestMethod]
        public void ThisIBANHasAValidCCCButhasAnInvalidIBANCheck()
        {
            Assert.IsFalse(BankAccount.IsValidIBAN("ES6412345678061234567890"));
        }

        [TestMethod]
        public void OnlyAcceptSpanishIBAN()
        {
            Assert.IsFalse(BankAccount.IsValidIBAN("UK3011112222003333333333"));
        }

        [TestMethod]
        public void NullIBANIsInvalid()
        {
            Assert.IsFalse(BankAccount.IsValidIBAN(null));
        }

        [TestMethod]
        public void EmptyStringIBANIsInvalid()
        {
            Assert.IsFalse(BankAccount.IsValidIBAN(""));
        }

        [TestMethod]
        public void IBANShorterThan24IsInvalid()
        {
            Assert.IsFalse(BankAccount.IsValidIBAN("ES301111222200"));
        }

        [TestMethod]
        public void IBANLongerThan24IsInvalid()
        {
            Assert.IsFalse(BankAccount.IsValidIBAN("ES3011112222003333333333343443"));
        }

        [TestMethod]
        public void WrongStructureSpanishIBANIsInvalid()
        {
            Assert.IsFalse(BankAccount.IsValidIBAN("ES  301111222/3aaa3 3333"));
        }


        [TestMethod]
        public void IBANCheckIsWellCalculatedGivenAccountFields()
        {
            Assert.AreEqual(BankAccount.CalculateSpanishIBANCheck("1111", "2222", "00", "3333333333"), "30");
        }

        [TestMethod]
        public void IBANCheckIsWellCalculatedGivenCCC()
        {
            Assert.AreEqual(BankAccount.CalculateSpanishIBANCheck("11112222003333333333"), "30");
        }

        [TestMethod]
        public void IfInvalidFieldsIBANCheckCalculationReturnsNull()
        {
            Assert.AreEqual(BankAccount.CalculateSpanishIBANCheck("111", "22 22", null, "3/a333333"), null);
        }

        [TestMethod]
        public void IfInvalidCCCIBANCheckCalculationReturnsNull()
        {
            Assert.AreEqual(BankAccount.CalculateSpanishIBANCheck("123456fdsfsd//"), null);
        }

        [TestMethod]
        public void IfNullCCCIBANCheckCalculationReturnsNull()
        {
            Assert.AreEqual(BankAccount.CalculateSpanishIBANCheck(null), null);
        }

        [TestMethod]
        public void IBANIsWellCalculatedGivenAccountFieldsWithoutCCCCheckDigits()
        {
            Assert.AreEqual(BankAccount.CalculateSpanishIBAN("1111", "2222", "3333333333"), "ES3011112222003333333333");
        }

        [TestMethod]
        public void IBANIsWellCalculatedGivenAccountFields()
        {
            Assert.AreEqual(BankAccount.CalculateSpanishIBAN("1111", "2222", "00", "3333333333"), "ES3011112222003333333333");
        }

        [TestMethod]
        public void IBANIsWellCalculatedGivenCCC()
        {
            Assert.AreEqual(BankAccount.CalculateSpanishIBAN("11112222003333333333"), "ES3011112222003333333333");
        }

        [TestMethod]
        public void IfInvalidFieldsIBANCalculationReturnsNull()
        {
            Assert.AreEqual(BankAccount.CalculateSpanishIBAN("111", "22 22", null, "3/a333333"), null);
        }

        [TestMethod]
        public void IfInvalidCCCIBANCalculationReturnsNull()
        {
            Assert.AreEqual(BankAccount.CalculateSpanishIBAN("123456fdsfsd//"), null);
        }

        [TestMethod]
        public void IfNullCCCIBANCalculationReturnsNull()
        {
            Assert.AreEqual(BankAccount.CalculateSpanishIBAN(null), null);
        }

        [TestMethod]
        public void AcceptsInvalidAcountNumbers()
        {
            BankAccountFields bankAccountFields = new BankAccountFields("124", "1 00", " 4", "100/234-1");
            BankAccount testAccount = new BankAccount(bankAccountFields);
            string givenData = "124" + "1 00" + " 4" + "100/234-1";
            string storedData =
                testAccount.BankAccountFieldCodes.BankCode +
                testAccount.BankAccountFieldCodes.OfficeCode +
                testAccount.BankAccountFieldCodes.CheckDigits +
                testAccount.BankAccountFieldCodes.AccountNumber;
            Assert.IsFalse(testAccount.HasValidBankAccountFields);
            Assert.AreEqual(givenData, storedData);
        }

        [TestMethod]
        public void AcceptsEmptyAcountNumbers()
        {
            BankAccountFields bankAccountFields = new BankAccountFields("", "", "", "");
            BankAccount testAccount = new BankAccount(bankAccountFields);
            string storedData =
                testAccount.BankAccountFieldCodes.BankCode +
                testAccount.BankAccountFieldCodes.OfficeCode +
                testAccount.BankAccountFieldCodes.CheckDigits +
                testAccount.BankAccountFieldCodes.AccountNumber;
            Assert.IsFalse(testAccount.HasValidBankAccountFields);
            Assert.AreEqual("", storedData);
        }

        [TestMethod]
        public void AcceptsNullOnAcountNumbers()
        {
            BankAccountFields bankAccountFields = new BankAccountFields(null, "", "02", "aaaaa");
            BankAccount testAccount = new BankAccount(bankAccountFields);
            Assert.IsFalse(testAccount.HasValidBankAccountFields);
            Assert.AreEqual(null, testAccount.BankAccountFieldCodes.BankCode);
        }

        [TestMethod]
        public void NullParsedtoEmptyStringWhenConcatenated()
        {
            BankAccountFields bankAccountFields = new BankAccountFields(null, "01", null, "aaaaa");
            BankAccount testAccount = new BankAccount(bankAccountFields);
            string storedData = 
                testAccount.BankAccountFieldCodes.BankCode +
                testAccount.BankAccountFieldCodes.OfficeCode +
                testAccount.BankAccountFieldCodes.CheckDigits +
                testAccount.BankAccountFieldCodes.AccountNumber;
            Assert.IsFalse(testAccount.HasValidBankAccountFields);
            Assert.AreEqual("01aaaaa", storedData);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void BankCodeMaxLengthIs4()
        {
            try
            {
                BankAccountFields bankAccountFields = new BankAccountFields("04234", "466", "00", "12345678");
                BankAccount testAccount = new BankAccount(bankAccountFields);
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("banco", e.ParamName);
                throw e;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void OfficeCodeMaxLengthIs4()
        {
            try
            {
                BankAccountFields bankAccountFields = new BankAccountFields(null, "65466", "00", "12345678");
                BankAccount testAccount = new BankAccount(bankAccountFields);
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("sucursal", e.ParamName);
                throw e;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void CheckDigitsMaxLegthIs2()
        {
            try
            {
                BankAccountFields bankAccountFields = new BankAccountFields(null, "", "020", "12345678");
                BankAccount testAccount = new BankAccount(bankAccountFields);
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("dígito de control", e.ParamName);
                throw e;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void AccounNumberMaxLenghtIs10()
        {
            try
            {
                BankAccountFields bankAccountFields = new BankAccountFields(null, "", "02", "1234561234578909");
                BankAccount testAccount = new BankAccount(bankAccountFields);
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("número de cuenta", e.ParamName);
                throw e;
            }
        }


        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void IfManyArgumentExceptionsOnlyFirstIsThrown()
        {
            try
            {
                BankAccountFields bankAccountFields = new BankAccountFields("4234", "46565", "050", "12345678");
                BankAccount testAccount = new BankAccount(bankAccountFields);
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("sucursal", e.ParamName);
                throw e;
            }
        }

        [TestMethod]
        public void AddindAValidAccountGeneratesAValidCCC()
        {
            BankAccountFields bankAccountFields = new BankAccountFields("1234", "5678", "06", "1234567890");
            BankAccount testAccount = new BankAccount(bankAccountFields);
            Assert.IsTrue(testAccount.HasValidCCC);
        }

        [TestMethod]
        public void AddindAValidAccountGeneratesAValidIBAN()
        {
            BankAccountFields bankAccountFields = new BankAccountFields("1234", "5678", "06", "1234567890");
            BankAccount testAccount = new BankAccount(bankAccountFields);
            Assert.IsTrue(testAccount.HasValidIBAN);
        }

        [TestMethod]
        public void AddingAnInvalidAccountDoesNotGenerateAValidCCC()
        {
            BankAccountFields bankAccountFields = new BankAccountFields("1234", "5678", "05", "1234567890");
            BankAccount testAccount = new BankAccount(bankAccountFields);
            Assert.IsFalse(testAccount.HasValidCCC);
        }

        [TestMethod]
        public void AddingAnInvalidAccountDoesNotGenerateAValidIBAN()
        {
            BankAccountFields bankAccountFields = new BankAccountFields("1234", "5678", "05", "1234567890");
            BankAccount testAccount = new BankAccount(bankAccountFields);
            Assert.IsFalse(testAccount.HasValidIBAN);
        }

        [TestMethod]
        public void CreatedCCCAreWellFormattedWithSpaces()
        {
            BankAccountFields bankAccountFields = new BankAccountFields("1234", "5678", "06", "1234567890");
            BankAccount testAccount = new BankAccount(bankAccountFields);
            Assert.AreEqual(testAccount.CCC.FormattedCCC, "1234 5678 06 1234567890");
        }

        [TestMethod]
        public void CreatedIBANAreWellFormattedWithSpaces()
        {
            BankAccountFields bankAccountFields = new BankAccountFields("1234", "5678", "06", "1234567890");
            BankAccount testAccount = new BankAccount(bankAccountFields);
            Assert.AreEqual(testAccount.IBAN.FormattedIBAN, "IBAN ES68 1234 5678 0612 3456 7890");
        }

        [TestMethod]
        public void FormattedCCCOfANullCCCIsNull()
        {
            BankAccountFields bankAccountFields = new BankAccountFields("1234", "5678", "05", "1234567890");
            BankAccount testAccount = new BankAccount(bankAccountFields);
            Assert.AreEqual(testAccount.CCC.FormattedCCC, null);
        }

        [TestMethod]
        public void FormattedIBANOfANullIBANIsNull()
        {
            BankAccountFields bankAccountFields = new BankAccountFields("1234", "5678", "05", "1234567890");
            BankAccount testAccount = new BankAccount(bankAccountFields);
            Assert.AreEqual(testAccount.IBAN.FormattedIBAN, null);
        }

        [TestMethod]
        public void CountryCodeIBANPropertyIsCorrectlyDisplayed()
        {
            BankAccountFields bankAccountFields = new BankAccountFields("1234", "5678", "06", "1234567890");
            BankAccount testAccount = new BankAccount(bankAccountFields);
            Assert.AreEqual("ES", testAccount.IBAN.InternationalCode);
        }

        [TestMethod]
        public void IBANCheckPropertyIsCorrectlyDisplayed()
        {
            BankAccountFields bankAccountFields = new BankAccountFields("1234", "5678", "06", "1234567890");
            BankAccount testAccount = new BankAccount(bankAccountFields);
            Assert.AreEqual("68", testAccount.IBAN.IBANCheck);
        }
    }
}
