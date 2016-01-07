using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectDebitElements;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class BankAccountUnitTests
    {
        [TestMethod]
        public void AcceptsInvalidAcountNumbersIfProvidingFields()
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
        public void AcceptsEmptyAcountNumbersIfProvidingFields()
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
        public void AcceptsNullOnAcountNumbersIfProvidingFields()
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
                throw;
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
                throw;
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
                throw;
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
                throw;
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
                throw;
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

        [TestMethod]
        public void AnIBANIsCorrectlyCreatedGivingAValidIBANString()
        {
            InternationalAccountBankNumberIBAN iban = new InternationalAccountBankNumberIBAN("ES7621000000650000000001");
            Assert.AreEqual("ES7621000000650000000001", iban.IBAN);
        }

        [TestMethod]
        public void AnIBANIsCorrectlyCreatedGivingAValidIBANString_EvenIfCheckNumbersHaveZeroes()
        {
            InternationalAccountBankNumberIBAN iban = new InternationalAccountBankNumberIBAN("ES0621000000610000000002");
            Assert.AreEqual("ES0621000000610000000002", iban.IBAN);
        }

        [TestMethod]
        public void AnIBANIsCorrectlyCreatedGivingAValidCCC()
        {
            ClientAccountCodeCCC ccc = new ClientAccountCodeCCC("21000000610000000002");
            InternationalAccountBankNumberIBAN iban = new InternationalAccountBankNumberIBAN(ccc);
            Assert.AreEqual("ES0621000000610000000002", iban.IBAN);
        }

        [TestMethod]
        public void AnEmptyIBANWithNullPropertiesIsCreatedGivingAnInvalidIBANString()
        {
            InternationalAccountBankNumberIBAN iban = new InternationalAccountBankNumberIBAN("ES0621000000000000000002");
            Assert.IsNotNull(iban);
            Assert.IsNull(iban.IBAN);
            Assert.IsNull(iban.CCC);
            Assert.IsNull(iban.FormattedIBAN);
            Assert.IsNull(iban.InternationalCode);
            Assert.IsNull(iban.IBANCheck);
        }

        [TestMethod]
        public void AnEmptyIBANWithNullPropertiesIsCreatedGivingAnInvalidCCC()
        {
            ClientAccountCodeCCC ccc = new ClientAccountCodeCCC("21000000000000000002");
            InternationalAccountBankNumberIBAN iban = new InternationalAccountBankNumberIBAN(ccc);
            Assert.IsNotNull(iban);
            Assert.IsNull(iban.IBAN);
            Assert.IsNull(iban.CCC);
            Assert.IsNull(iban.FormattedIBAN);
            Assert.IsNull(iban.InternationalCode);
            Assert.IsNull(iban.IBANCheck);
        }
    }
}
