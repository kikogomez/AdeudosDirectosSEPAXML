using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class PaymentStatusReportUnitTests
    {


        [TestMethod]
        public void ADirectDebitTransactionRejectIsCorrectlyCreated()
        {
            string xMLFilePath = "";
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TheDirectDebitTransactionRejectInternalReferenceListIsCorreclyExtractedFromARejectedTransactionGroup()
        {
            //Starting with an XDocument(linq), extract all <OrgnlEndToEndId>
            string xMLFilePath = @"XML Test Files\pain.002.001.03\LaCaixa_pain00200103_Example1.xml";
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ICanSequentiallyExtractAllOriginalEndtoEndOriginalTransactionsIDIntoaList()
        {
            //If the XML message is HUGE, I can extract the references by going through sequentilly all nodes
            string xMLFilePath = "";
            Assert.Inconclusive();
        }

    }
}
