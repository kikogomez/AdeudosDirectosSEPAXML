using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReferencesAndTools;

namespace ReferencesAndToolsUnitTests
{

    [TestClass]
    public class BankAccountNumberCheckerUnitTests
    {
        [TestMethod]
        public void PublicStructCCCCheckDigitsInStaticClassBankAccountNumberCheckerInstantiatesCorrectly()
        {
            BankAccountNumberChecker.CCCCheckDigits checkDigits1 = new BankAccountNumberChecker.CCCCheckDigits { bankOfficeCheckDigit = "0", accountNumberCheckDigit = "6" };
            BankAccountNumberChecker.CCCCheckDigits checkDigits2 = new BankAccountNumberChecker.CCCCheckDigits { bankOfficeCheckDigit = "1", accountNumberCheckDigit = "7" };
            Assert.AreEqual("0", checkDigits1.bankOfficeCheckDigit);
            Assert.AreEqual("6", checkDigits1.accountNumberCheckDigit);
            Assert.AreEqual("1", checkDigits2.bankOfficeCheckDigit);
            Assert.AreEqual("7", checkDigits2.accountNumberCheckDigit);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void BankCodeMaxLengthIs4()
        {
            try
            {
                BankAccountNumberChecker.CheckBankAccountFieldsLength("04234", "466", "00", "12345678");
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("banco", e.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void OfficeCodeMaxLengthIs4()
        {
            try
            {
                BankAccountNumberChecker.CheckBankAccountFieldsLength(null, "65466", "00", "12345678");
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("sucursal", e.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void CheckDigitsMaxLegthIs2()
        {
            try
            {
                BankAccountNumberChecker.CheckBankAccountFieldsLength(null, "", "020", "12345678");
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("dígito de control", e.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void AccounNumberMaxLenghtIs10()
        {
            try
            {
                BankAccountNumberChecker.CheckBankAccountFieldsLength(null, "", "02", "1234561234578909");
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("número de cuenta", e.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void IfManyArgumentExceptionsOnlyFirstIsThrown()
        {
            try
            {
                BankAccountNumberChecker.CheckBankAccountFieldsLength("4234", "46565", "050", "12345678");
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("sucursal", e.ParamName);
                throw;
            }
        }

        [TestMethod]
        public void ThisIsAValidCCC()
        {
            Assert.IsTrue(BankAccountNumberChecker.IsValidCCC("1234", "5678", "06", "1234567890"));
        }

        [TestMethod]
        public void ThisIsAnotherValidCCC()
        {
            Assert.IsTrue(BankAccountNumberChecker.IsValidCCC("0128", "0035", "69", "0987654321"));
        }

        [TestMethod]
        public void IsNotValidCCC()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidCCC("1234", "5678", "05", "1234567890"));
        }

        [TestMethod]
        public void NoExceptionsOnNullsEmptyStringsAndLongDigitStringsWhenValidating()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidCCC(null, "", "05", "123456789123456789"));
        }

        [TestMethod]
        public void CCCWithAlphabeticCharactersIsInvalid()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidCCC("aaaa", "5678", "06", "1234567890"));
        }

        [TestMethod]
        public void CCCWithSpacesIsInvalid()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidCCC("1234", "5678", "06", "1234 56789"));
        }

        [TestMethod]
        public void CCCWithSeparatorsIsInvalid()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidCCC("1234", "5678", "06", "12-3456/7"));
        }


        [TestMethod]
        public void CCCWithIncompleteDataIsInvalid()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidCCC("12", "350", "69", "987654321"));
        }

        [TestMethod]
        public void CCCWithNullCheckDigitsIsNotValid()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidCCC("aaaa", "5678", "", "bbbbbb"));
        }

        [TestMethod]
        public void DontFillWithZerosToTheLeft()
        {
            Assert.IsTrue(BankAccountNumberChecker.IsValidCCC("0128", "0035", "69", "0987654321"));
            Assert.IsFalse(BankAccountNumberChecker.IsValidCCC("128", "35", "69", "987654321"));
        }

        [TestMethod]
        public void SpacesAreNotEqualZeros()
        {
            Assert.IsTrue(BankAccountNumberChecker.IsValidCCC("0128", "0035", "69", "0987654321"));
            Assert.IsFalse(BankAccountNumberChecker.IsValidCCC(" 128", "  35", "69", "987654321"));
        }

        [TestMethod]
        public void IsValidSingleStringCCC()
        {
            Assert.IsTrue(BankAccountNumberChecker.IsValidCCC("12345678061234567890"));
        }

        [TestMethod]
        public void IsNotValidSingleStringCCC()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidCCC("12345678051234567890"));
        }

        [TestMethod]
        public void NullSingleStringCCCIsInvalid()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidCCC(null));
        }

        [TestMethod]
        public void EmptyStringSingleStringCCCIsInvalid()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidCCC(""));
        }

        [TestMethod]
        public void AlfanumericOrWithSpacesSingleStringCCCAreInvalid()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidCCC("  abcd56gg / 7890128"));
        }

        [TestMethod]
        public void CCCLongerThan20DigitsIsInvalid()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidCCC("12345678901234567890123134"));
        }

        [TestMethod]
        public void CCCShorterThan20DigitsIsInvalid()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidCCC("12345678901123134"));
        }

        [TestMethod]
        public void CCCCheckDigitsAreWellCalculated()
        {
            BankAccountNumberChecker.CCCCheckDigits expectedCheckDigits = new BankAccountNumberChecker.CCCCheckDigits { bankOfficeCheckDigit = "0", accountNumberCheckDigit = "6" };
            Assert.AreEqual(BankAccountNumberChecker.CalculateCCCCheckDigits("1234", "5678", "1234567890"), expectedCheckDigits);
        }

        [TestMethod]
        public void CCCCheckDigitsCalculationOnInvalidArgumentsReturnsNulls()
        {
            BankAccountNumberChecker.CCCCheckDigits expectedCheckDigits = new BankAccountNumberChecker.CCCCheckDigits { bankOfficeCheckDigit = null, accountNumberCheckDigit = null };
            Assert.AreEqual(BankAccountNumberChecker.CalculateCCCCheckDigits(null, "", "cccccccccc"), expectedCheckDigits);
        }

        [TestMethod]
        public void NoExceptionsOnLongDigitStrings()
        {
            BankAccountNumberChecker.CCCCheckDigits expectedCheckDigits = new BankAccountNumberChecker.CCCCheckDigits { bankOfficeCheckDigit = "0", accountNumberCheckDigit = null };
            Assert.AreEqual(BankAccountNumberChecker.CalculateCCCCheckDigits("1234", "5678", "546546564234567890"), expectedCheckDigits);
        }

        [TestMethod]
        public void CCCCIsWellCalculated()
        {
            Assert.AreEqual(BankAccountNumberChecker.CalculateCCC("1111", "2222", "3333333333"), "11112222003333333333");
        }

        [TestMethod]
        public void IfInvalidDataCCCCalculationReturnsNull()
        {
            Assert.AreEqual(BankAccountNumberChecker.CalculateCCC("1 24", null, "100/234-1"), null);
        }

        [TestMethod]
        public void ThisIsAValidIBAN()
        {
            Assert.IsTrue(BankAccountNumberChecker.IsValidIBAN("ES6812345678061234567890"));
        }

        [TestMethod]
        public void ThisIsAnotherValidIBAN()
        {
            Assert.IsTrue(BankAccountNumberChecker.IsValidIBAN("ES3011112222003333333333"));
        }

        [TestMethod]
        public void ThisIsAnInvalidIBAN()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidIBAN("ES1111111111111111111111"));
        }


        [TestMethod]
        public void AnIBANZeroesInCheckNumberIsWellValidated()
        {
            Assert.IsTrue(BankAccountNumberChecker.IsValidIBAN("ES0621000000610000000002"));
        }


        [TestMethod]
        public void ThisIBANHasAValidIBANCheckButHasAnInvalidCCC()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidIBAN("ES3312345678051234567890"));
        }

        [TestMethod]
        public void ThisIBANHasAValidCCCButhasAnInvalidIBANCheck()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidIBAN("ES6412345678061234567890"));
        }

        [TestMethod]
        public void OnlyAcceptSpanishIBAN()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidIBAN("UK3011112222003333333333"));
        }

        [TestMethod]
        public void NullIBANIsInvalid()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidIBAN(null));
        }

        [TestMethod]
        public void EmptyStringIBANIsInvalid()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidIBAN(""));
        }

        [TestMethod]
        public void IBANShorterThan24IsInvalid()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidIBAN("ES301111222200"));
        }

        [TestMethod]
        public void IBANLongerThan24IsInvalid()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidIBAN("ES3011112222003333333333343443"));
        }

        [TestMethod]
        public void WrongStructureSpanishIBANIsInvalid()
        {
            Assert.IsFalse(BankAccountNumberChecker.IsValidIBAN("ES  301111222/3aaa3 3333"));
        }


        [TestMethod]
        public void IBANCheckIsWellCalculatedGivenAccountFields()
        {
            Assert.AreEqual(BankAccountNumberChecker.CalculateSpanishIBANCheck("1111", "2222", "00", "3333333333"), "30");
        }

        [TestMethod]
        public void IBANCheckIsWellCalculatedGivenCCC()
        {
            Assert.AreEqual(BankAccountNumberChecker.CalculateSpanishIBANCheck("11112222003333333333"), "30");
        }

        [TestMethod]
        public void IfInvalidFieldsIBANCheckCalculationReturnsNull()
        {
            Assert.AreEqual(BankAccountNumberChecker.CalculateSpanishIBANCheck("111", "22 22", null, "3/a333333"), null);
        }

        [TestMethod]
        public void IfInvalidCCCIBANCheckCalculationReturnsNull()
        {
            Assert.AreEqual(BankAccountNumberChecker.CalculateSpanishIBANCheck("123456fdsfsd//"), null);
        }

        [TestMethod]
        public void IfNullCCCIBANCheckCalculationReturnsNull()
        {
            Assert.AreEqual(BankAccountNumberChecker.CalculateSpanishIBANCheck(null), null);
        }

        [TestMethod]
        public void IBANIsWellCalculatedGivenAccountFieldsWithoutCCCCheckDigits()
        {
            Assert.AreEqual(BankAccountNumberChecker.CalculateSpanishIBAN("1111", "2222", "3333333333"), "ES3011112222003333333333");
        }

        [TestMethod]
        public void IBANIsWellCalculatedGivenAccountFields()
        {
            Assert.AreEqual(BankAccountNumberChecker.CalculateSpanishIBAN("1111", "2222", "00", "3333333333"), "ES3011112222003333333333");
        }

        [TestMethod]
        public void IBANIsWellCalculatedGivenCCC()
        {
            Assert.AreEqual(BankAccountNumberChecker.CalculateSpanishIBAN("11112222003333333333"), "ES3011112222003333333333");
        }

        [TestMethod]
        public void IfInvalidFieldsIBANCalculationReturnsNull()
        {
            Assert.AreEqual(BankAccountNumberChecker.CalculateSpanishIBAN("111", "22 22", null, "3/a333333"), null);
        }

        [TestMethod]
        public void IfInvalidCCCIBANCalculationReturnsNull()
        {
            Assert.AreEqual(BankAccountNumberChecker.CalculateSpanishIBAN("123456fdsfsd//"), null);
        }

        [TestMethod]
        public void IfNullCCCIBANCalculationReturnsNull()
        {
            Assert.AreEqual(BankAccountNumberChecker.CalculateSpanishIBAN(null), null);
        }
    }
}
