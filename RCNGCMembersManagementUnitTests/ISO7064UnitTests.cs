using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementAppLogic.Billing.DirectDebit;

namespace RCNGCMembersManagementUnitTests
{
    [TestClass]
    public class ISO7064UnitTests
    {
        [TestClass]
        public class ISO7064Test
        {
            [TestMethod]
            public void CreditorIDCheckDigitsAreWellCalculated()
            {
                string nif = "G35008770";
                Assert.AreEqual("90", ISO7064CheckDigits.CalculateSEPACreditIdentifierCheckDigits("ES", nif));
            }
        }
    }
}
