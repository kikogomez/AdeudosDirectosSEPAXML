using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectDebitElements;
using ExtensionMethods;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class DirectDebitAmendmentInformationUnitTests
    {
        [TestMethod]
        public void ADirectDebitAmendmentInformationIsCorrectlyCreated()
        {
            string oldMandateID = "112211";
            BankAccount oldBankAccount = new BankAccount(new InternationalAccountBankNumberIBAN("ES5621001111301111111111"));

            DirectDebitAmendmentInformation directDebitAmendmentInformation = new DirectDebitAmendmentInformation(oldMandateID, oldBankAccount);

            Assert.AreEqual(oldMandateID, directDebitAmendmentInformation.OldMandateID);
            Assert.AreEqual(oldBankAccount, directDebitAmendmentInformation.OldBankAccount);
        }

        [TestMethod]
        public void TheMandateIDofAnAmendmentInformationCanBeNull()
        {
            string oldMandateID = null;
            BankAccount oldBankAccount = new BankAccount(new InternationalAccountBankNumberIBAN("ES5621001111301111111111"));

            DirectDebitAmendmentInformation directDebitAmendmentInformation = new DirectDebitAmendmentInformation(oldMandateID, oldBankAccount);

            Assert.AreEqual(null, directDebitAmendmentInformation.OldMandateID);
            Assert.AreEqual(oldBankAccount, directDebitAmendmentInformation.OldBankAccount);
        }

        [TestMethod]
        public void TheBankAcountOfAnAmendmentInformationCanBeNull()
        {
            string oldMandateID = "112211";
            BankAccount oldBankAccount = null;

            DirectDebitAmendmentInformation directDebitAmendmentInformation = new DirectDebitAmendmentInformation(oldMandateID, oldBankAccount);

            Assert.AreEqual(oldMandateID, directDebitAmendmentInformation.OldMandateID);
            Assert.AreEqual(null, directDebitAmendmentInformation.OldBankAccount);
        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void TheMandateIDOfAnAmendmentInformationCantBeEmptyOrOnlySpaces()
        {
            string oldMandateID = "   ";
            BankAccount oldBankAccount = null;

            try
            {
                DirectDebitAmendmentInformation directDebitAmendmentInformation = new DirectDebitAmendmentInformation(oldMandateID, oldBankAccount);
            }

            catch (TypeInitializationException typeInitializationException)
            {
                ArgumentException argumentException = (ArgumentException)typeInitializationException.InnerException;
                Assert.AreEqual("mandateID", argumentException.ParamName);
                Assert.AreEqual("The MandateID can't be empty", argumentException.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.TypeInitializationException))]
        public void TheBankAccountOfAndAmendmentInformationCantBeInvalid()
        {
            string oldMandateID = null;
            BankAccount oldBankAccount = new BankAccount(new BankAccountFields("1111", "1111", "11", "1111111111"));

            try
            {
                DirectDebitAmendmentInformation directDebitAmendmentInformation = new DirectDebitAmendmentInformation(oldMandateID, oldBankAccount);
            }

            catch (TypeInitializationException typeInitializationException)
            {
                ArgumentException argumentException = (ArgumentException)typeInitializationException.InnerException;
                Assert.AreEqual("oldBankAccount", argumentException.ParamName);
                Assert.AreEqual("The Bank Account must have a valid IBAN", argumentException.GetMessageWithoutParamName());
                throw;
            }
        }
    }
}
