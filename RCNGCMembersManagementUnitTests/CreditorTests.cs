using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementAppLogic.Billing.DirectDebit;
using ExtensionMethods;

namespace RCNGCMembersManagementUnitTests
{
    [TestClass]
    public class CreditorTests
    {
        [TestMethod]
        public void TheCreditorInfoIsCorrectlyCreated()
        {
            Creditor creditor = new Creditor("G35008770", "Real Club Náutico de Gran Canaria");
            Assert.AreEqual("G35008770", creditor.NIF);
            Assert.AreEqual("Real Club Náutico de Gran Canaria", creditor.Name);
        }

        [TestMethod]
        public void ANewCreditorAgentIsCorrectlyCreated()
        {
            BankCode bankCode = new BankCode("2038", "Bankia, S.A.", "CAHMESMMXXX");
            CreditorAgent creditAgent = new CreditorAgent(bankCode);
            Assert.AreEqual("2038",creditAgent.LocalBankCode);
            Assert.AreEqual("Bankia, S.A.", creditAgent.BankName);
            Assert.AreEqual("CAHMESMMXXX", creditAgent.BankBIC);
        }

        [TestMethod]
        public void ANewDirectDebitInitiationContractIsCorrectlyCreated()
        {
            Creditor creditor = new Creditor("G35008770", "Real Club Náutico de Gran Canaria");
            BankAccount creditorAccount = new BankAccount(new ClientAccountCodeCCC("20381111401111111111"));
            BankCode bankCode = new BankCode("2038", "Bankia, S.A.", "CAHMESMMXXX");
            CreditorAgent creditorAgent = new CreditorAgent(bankCode);
            DirectDebitInitiationContract directDebitInitiationContract = new DirectDebitInitiationContract(
                creditorAccount, creditor.NIF, "777", creditorAgent);
            Assert.AreEqual("20381111401111111111", directDebitInitiationContract.CreditorAcount.CCC.CCC);
            Assert.AreEqual("CAHMESMMXXX", directDebitInitiationContract.CreditorAgent.BankBIC);
            Assert.AreEqual("777", directDebitInitiationContract.CreditorBussinessCode);
            Assert.AreEqual("ES90777G35008770", directDebitInitiationContract.CreditorID);
        }

        [TestMethod]
        public void ADirectDebitInitiationContractIsCorrectlyRegisteredByTheCreditor()
        {
            Creditor creditor = new Creditor("G35008770", "Real Club Náutico de Gran Canaria");
            BankAccount creditorAccount = new BankAccount(new ClientAccountCodeCCC("20381111401111111111"));
            BankCode bankCode = new BankCode("2038", "Bankia, S.A.", "CAHMESMMXXX");
            CreditorAgent creditorAgent = new CreditorAgent(bankCode);
            DirectDebitInitiationContract directDebitInitiationContract = new DirectDebitInitiationContract(
                creditorAccount, creditor.NIF, "777", creditorAgent);
            creditor.AddDirectDebitInitiacionContract(directDebitInitiationContract);
            Assert.AreEqual("20381111401111111111", creditor.DirectDebitInitiationContracts["777"].CreditorAcount.CCC.CCC);
            Assert.AreEqual("CAHMESMMXXX", creditor.DirectDebitInitiationContracts["777"].CreditorAgent.BankBIC);
            Assert.AreEqual("777", creditor.DirectDebitInitiationContracts["777"].CreditorBussinessCode);
            Assert.AreEqual("ES90777G35008770", creditor.DirectDebitInitiationContracts["777"].CreditorID);
        }

        [TestMethod]
        public void ICanRegisterMoreThanOneDirectDebitInitiationContract()
        {
            Creditor creditor = new Creditor("G35008770", "Real Club Náutico de Gran Canaria");

            BankCode bankCode = new BankCode("2038", "Bankia, S.A.", "CAHMESMMXXX");
            CreditorAgent creditorAgent = new CreditorAgent(bankCode);
            BankAccount creditorAccount = new BankAccount(new ClientAccountCodeCCC("20381111401111111111"));
            DirectDebitInitiationContract directDebitInitiationContract = new DirectDebitInitiationContract(
                creditorAccount, creditor.NIF, "777", creditorAgent);
            creditor.AddDirectDebitInitiacionContract(directDebitInitiationContract);

            BankCode bankCode2 = new BankCode("2100", "CaixaBank, S.A.", "CAIXESBBXXX");
            CreditorAgent creditorAgent2 = new CreditorAgent(bankCode2);
            BankAccount creditorAccount2 = new BankAccount(new ClientAccountCodeCCC("21001111301111111111"));
            DirectDebitInitiationContract directDebitInitiationContract2 = new DirectDebitInitiationContract(
                creditorAccount2, creditor.NIF, "333", creditorAgent2);
            creditor.AddDirectDebitInitiacionContract(directDebitInitiationContract2);

            Assert.AreEqual("20381111401111111111", creditor.DirectDebitInitiationContracts["777"].CreditorAcount.CCC.CCC);
            Assert.AreEqual("CAHMESMMXXX", creditor.DirectDebitInitiationContracts["777"].CreditorAgent.BankBIC);
            Assert.AreEqual("777", creditor.DirectDebitInitiationContracts["777"].CreditorBussinessCode);
            Assert.AreEqual("ES90777G35008770", creditor.DirectDebitInitiationContracts["777"].CreditorID);

            Assert.AreEqual("21001111301111111111", creditor.DirectDebitInitiationContracts["333"].CreditorAcount.CCC.CCC);
            Assert.AreEqual("CAIXESBBXXX", creditor.DirectDebitInitiationContracts["333"].CreditorAgent.BankBIC);
            Assert.AreEqual("333", creditor.DirectDebitInitiationContracts["333"].CreditorBussinessCode);
            Assert.AreEqual("ES90333G35008770", creditor.DirectDebitInitiationContracts["333"].CreditorID);
        }

        [TestMethod]
        public void TheCreditorAccountForADirectDebitContractCanBeChanged()
        {
            Creditor creditor = new Creditor("G35008770", "Real Club Náutico de Gran Canaria");
            BankCode bankCode = new BankCode("2038", "Bankia, S.A.", "CAHMESMMXXX");
            CreditorAgent creditorAgent = new CreditorAgent(bankCode);
            BankAccount creditorAccount = new BankAccount(new ClientAccountCodeCCC("20381111401111111111"));
            DirectDebitInitiationContract directDebitInitiationContract = new DirectDebitInitiationContract(
                creditorAccount, creditor.NIF, "777", creditorAgent);
            creditor.AddDirectDebitInitiacionContract(directDebitInitiationContract);
            BankAccount newCreditorAccount = new BankAccount(new ClientAccountCodeCCC("20382222102222222222"));
            creditor.ChangeDirectDebitContractAccount("777", newCreditorAccount);
            Assert.AreEqual("20382222102222222222", creditor.DirectDebitInitiationContracts["777"].CreditorAcount.CCC.CCC);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void OnlyCanChangeDirectDebitContractAccountsWithAccountsFromTheSameCreditorAgent()
        {
            Creditor creditor = new Creditor("G35008770", "Real Club Náutico de Gran Canaria");
            BankCode bankCode = new BankCode("2038", "Bankia, S.A.", "CAHMESMMXXX");
            CreditorAgent creditorAgent = new CreditorAgent(bankCode);
            BankAccount creditorAccount = new BankAccount(new ClientAccountCodeCCC("20381111401111111111"));
            DirectDebitInitiationContract directDebitInitiationContract = new DirectDebitInitiationContract(
                creditorAccount, creditor.NIF, "777", creditorAgent);
            BankAccount newBankAccount = new BankAccount(new ClientAccountCodeCCC("21001111301111111111")); 
            try
            {
                directDebitInitiationContract.ChangeCreditorBank(newBankAccount);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("The new account must be from the same Creditor Agent", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        public void ICanRemoveADirectDebitContract()
        {
            Creditor creditor = new Creditor("G35008770", "Real Club Náutico de Gran Canaria");
            BankCode bankCode = new BankCode("2038", "Bankia, S.A.", "CAHMESMMXXX");
            CreditorAgent creditorAgent = new CreditorAgent(bankCode);
            BankAccount creditorAccount = new BankAccount(new ClientAccountCodeCCC("20381111401111111111"));
            DirectDebitInitiationContract directDebitInitiationContract = new DirectDebitInitiationContract(
                creditorAccount, creditor.NIF, "777", creditorAgent);
            creditor.AddDirectDebitInitiacionContract(directDebitInitiationContract);
            creditor.RemoveDirectDebitInitiacionContract("777");
            Assert.IsFalse(creditor.DirectDebitInitiationContracts.ContainsKey("777"));
        }
    }
}
