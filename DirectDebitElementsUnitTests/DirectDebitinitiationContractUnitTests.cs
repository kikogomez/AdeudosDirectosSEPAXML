using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectDebitElements;
using ReferencesAndTools;
using ExtensionMethods;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class DirectDebitinitiationContractUnitTests
    {
        [TestMethod]
        public void ADirectDebitInitiationContractIsCorrectlyCreated()
        {
            BankAccount creditorAccount = new BankAccount(new InternationalAccountBankNumberIBAN("ES5621001111301111111111"));
            string nIF = "G12345678";
            string creditorBusinessCode = "011";
            BankCode bankCode = new BankCode("2100", "CaixaBank", "CAIXESBBXXX");
            CreditorAgent creditorAgent = new CreditorAgent(bankCode);
            DirectDebitInitiationContract directDebitInitiationContract = new DirectDebitInitiationContract(creditorAccount, nIF, creditorBusinessCode, creditorAgent);

            Assert.AreEqual(creditorAccount, directDebitInitiationContract.CreditorAcount);
            Assert.AreEqual(creditorAgent, directDebitInitiationContract.CreditorAgent);
            Assert.AreEqual("011", directDebitInitiationContract.CreditorBussinessCode);
            Assert.AreEqual("ES26011G12345678", directDebitInitiationContract.CreditorID);
        }


        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void ADirectDebitInitiationContractDoesNotAcceptNullCreditorAccount()
        {
            BankAccount creditorAccount = null;
            string nIF = "G12345678";
            string creditorBusinessCode = "011";
            BankCode bankCode = new BankCode("2100", "CaixaBank", "CAIXESBBXXX");
            CreditorAgent creditorAgent = new CreditorAgent(bankCode);

            try
            {
                DirectDebitInitiationContract directDebitInitiationContract = new DirectDebitInitiationContract(creditorAccount, nIF, creditorBusinessCode, creditorAgent);
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("creditorAccount", e.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TheProvidedCreditorAccountShouldHaveAValidIBAN()
        {
            BankAccount creditorAccount = new BankAccount(new BankAccountFields("2100", "1111", "11", "1111111111"));
            string nIF = "G12345678";
            string creditorBusinessCode = "011";
            BankCode bankCode = new BankCode("2100", "CaixaBank", "CAIXESBBXXX");
            CreditorAgent creditorAgent = new CreditorAgent(bankCode);

            try
            {
                DirectDebitInitiationContract directDebitInitiationContract = new DirectDebitInitiationContract(creditorAccount, nIF, creditorBusinessCode, creditorAgent);
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("creditorAccount", e.ParamName);
                Assert.AreEqual("The Creditor Account IBAN is invalid", e.GetMessageWithoutParamName());
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void ADirectDebitInitiationContractDoesNotAcceptNullNIF()
        {
            BankAccount creditorAccount = new BankAccount(new InternationalAccountBankNumberIBAN("ES5621001111301111111111"));
            string nIF = null;
            string creditorBusinessCode = "011";
            BankCode bankCode = new BankCode("2100", "CaixaBank", "CAIXESBBXXX");
            CreditorAgent creditorAgent = new CreditorAgent(bankCode);

            try
            {
                DirectDebitInitiationContract directDebitInitiationContract = new DirectDebitInitiationContract(creditorAccount, nIF, creditorBusinessCode, creditorAgent);
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("NIF", e.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void ADirectDebitInitiationContractDoesNotAcceptNullCreditorBussinessCode()
        {
            BankAccount creditorAccount = new BankAccount(new InternationalAccountBankNumberIBAN("ES5621001111301111111111"));
            string nIF = "G12345678";
            string creditorBusinessCode = null;
            BankCode bankCode = new BankCode("2100", "CaixaBank", "CAIXESBBXXX");
            CreditorAgent creditorAgent = new CreditorAgent(bankCode);

            try
            {
                DirectDebitInitiationContract directDebitInitiationContract = new DirectDebitInitiationContract(creditorAccount, nIF, creditorBusinessCode, creditorAgent);
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("CreditorBusinessCode", e.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void ADirectDebitInitiationContractDoesNotAcceptNullCreditorAgent()
        {
            BankAccount creditorAccount = new BankAccount(new InternationalAccountBankNumberIBAN("ES5621001111301111111111"));
            string nIF = "G12345678";
            string creditorBusinessCode = "011";
            BankCode bankCode = new BankCode("2100", "CaixaBank", "CAIXESBBXXX");
            CreditorAgent creditorAgent = null;

            try
            {
                DirectDebitInitiationContract directDebitInitiationContract = new DirectDebitInitiationContract(creditorAccount, nIF, creditorBusinessCode, creditorAgent);
            }

            catch (System.ArgumentNullException e)
            {
                Assert.AreEqual("CreditorAgent", e.ParamName);
                throw;
            }
        }

    }
}
