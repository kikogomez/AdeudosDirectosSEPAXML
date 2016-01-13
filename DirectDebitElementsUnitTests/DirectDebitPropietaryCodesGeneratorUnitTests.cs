using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectDebitElements;
using ReferencesAndTools;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class DirectDebitPropietaryCodesGeneratorUnitTests
    {
        [TestMethod]
        public void ADirectDebitPropietaryCodesGeneratorIsCorrectlyGenerated()
        {
            Creditor creditor = new Creditor("G12345678", "NOMBRE ACREEDOR PRUEBAS");
            CreditorAgent creditorAgent = new CreditorAgent(new BankCode("2100", "CaixaBank", "CAIXESBBXXX"));
            DirectDebitInitiationContract directDebitInitiationContract = new DirectDebitInitiationContract(
                new BankAccount(new InternationalAccountBankNumberIBAN("ES5621001111301111111111")),
                creditor.NIF,
                "777",
                creditorAgent);

            DirectDebitPropietaryCodesGenerator directDebitPropietaryCodesGenerator = new DirectDebitPropietaryCodesGenerator(directDebitInitiationContract);

            Assert.AreEqual(directDebitInitiationContract, directDebitPropietaryCodesGenerator.DirectDebitInitiationContract);
        }

        [TestMethod]
        public void TheMandateIDIsCorrectlyCalculatedGivenAnInternalReferenceNumber()
        {
            Creditor creditor = new Creditor("G12345678", "NOMBRE ACREEDOR PRUEBAS");
            CreditorAgent creditorAgent = new CreditorAgent(new BankCode("2100", "CaixaBank", "CAIXESBBXXX"));
            DirectDebitInitiationContract directDebitInitiationContract = new DirectDebitInitiationContract(
                new BankAccount(new InternationalAccountBankNumberIBAN("ES5621001111301111111111")),
                creditor.NIF,
                "011",
                creditorAgent);

            DirectDebitPropietaryCodesGenerator directDebitPropietaryCodesGenerator = new DirectDebitPropietaryCodesGenerator(directDebitInitiationContract);

            Assert.AreEqual("000001112345", directDebitPropietaryCodesGenerator.CalculateMyOldCSB19Code(12345));
        }
    }
}
