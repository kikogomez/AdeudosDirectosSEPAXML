using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReferencesAndTools;

namespace ReferencesAndToolsUnitTests
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
