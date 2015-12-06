using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementAppLogic.Billing.DirectDebit;
using RCNGCISO20022CustomerDebitInitiation;

namespace RCNGCMembersManagementUnitTests
{
    [TestClass]
    public class SEPAAttributesTest
    {
        [TestClass]
        public class SEPAIAttributesTests
        {
            static SEPAAttributes sEPAAttributes;

            [ClassInitialize()]
            public static void ClassInit(TestContext context)
            {
                sEPAAttributes = new SEPAAttributes();
            }

            [TestMethod]
            public void AT01MandateReferenceIDIsWellCalculated()
            {
                string csbReference = "000001110100";
                string mandateReference = sEPAAttributes.AT01MandateReference(csbReference);
                Assert.AreEqual("000001110100" + new string(' ', 23), mandateReference);
            }

            [TestMethod]
            public void AT02CreditorIDIsWellCalculated()
            {
                string nationalIdentifier = "ES";
                string nIF = "G35008770";
                string suffix = "777";
                string creditorIdentifier = sEPAAttributes.AT02CreditorIdentifier(nationalIdentifier, nIF, suffix);
                Assert.AreEqual("ES90777G35008770", creditorIdentifier);
            }

            [TestMethod]
            public void AT07IBANIsWellCalculated()
            {
                string ccc = "01821234861234567890";
                Assert.AreEqual("ES1801821234861234567890", sEPAAttributes.AT07IBAN("ES", ccc));
            }

            [TestMethod]
            public void AT07SpanishIBANIsWellCalculated()
            {
                string ccc = "01821234861234567890";
                Assert.AreEqual("ES1801821234861234567890", sEPAAttributes.AT07IBAN_Spanish(ccc));
            }

            [TestMethod]
            public void AT21TypeOfPaymentDuringMigrationIsRCUR()
            {
                Assert.AreEqual(SequenceType1Code.RCUR, sEPAAttributes.AT21TypeOfPayment_MigrationValue);
            }

            [TestMethod]
            public void AT25DateOfSigningDuringMigrationIs20091031()
            {
                Assert.AreEqual(new DateTime(2009, 10, 31), sEPAAttributes.AT25DateOfMandateSigning_MigrationValue);
            }
        }
    }
}
