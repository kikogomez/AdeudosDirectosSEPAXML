using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectDebitElements;
using ReferencesAndTools;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class DirectDebitPropietaryCodesGeneratorUnitTests
    {
        static Creditor creditor;
        static CreditorAgent creditorAgent;
        static DirectDebitInitiationContract directDebitInitiationContract;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            creditor = new Creditor("G12345678", "NOMBRE ACREEDOR PRUEBAS");
            creditorAgent = new CreditorAgent(new BankCode("2100", "CaixaBank", "CAIXESBBXXX"));
            directDebitInitiationContract = new DirectDebitInitiationContract(
                new BankAccount(new InternationalAccountBankNumberIBAN("ES5621001111301111111111")),
                creditor.NIF,
                "777",
                creditorAgent);
        }

        [TestMethod]
        public void ADirectDebitPropietaryCodesGeneratorIsCorrectlyGenerated()
        {
            DirectDebitPropietaryCodesGenerator directDebitPropietaryCodesGenerator = new DirectDebitPropietaryCodesGenerator(directDebitInitiationContract);

            Assert.AreEqual(directDebitInitiationContract, directDebitPropietaryCodesGenerator.DirectDebitInitiationContract);
        }

        [TestMethod]
        public void TheMandateIDIsCorrectlyCalculatedGivenAnInternalReferenceNumber()
        {
            DirectDebitPropietaryCodesGenerator directDebitPropietaryCodesGenerator = new DirectDebitPropietaryCodesGenerator(directDebitInitiationContract);

            Assert.AreEqual("000077712345", directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(12345));
        }

        [TestMethod]
        public void TheDirectDebitRemmitanceMessageIDIsCorrectlyCalculatedGivenAGenrationDate()
        {
            DirectDebitPropietaryCodesGenerator directDebitPropietaryCodesGenerator = new DirectDebitPropietaryCodesGenerator(directDebitInitiationContract);

            DateTime generationDate = DateTime.Parse("30-10-2015");
            Assert.AreEqual("ES26777G123456782015103000:00:00", directDebitPropietaryCodesGenerator.GenerateRemmitanceID(generationDate));
        }
    }
}
