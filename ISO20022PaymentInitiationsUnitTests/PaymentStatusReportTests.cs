using ISO20022PaymentInitiations;
using ISO20022PaymentInitiations.SchemaSerializableClasses;
using ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport;
using XMLSerializerValidator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ISO20022PaymentInitiationsUnitTests
{
    [TestClass]
    public class PaymentStatusReportTests
    {
        static string xMLNamespace;
        static string xSDFilePath;

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            //SEPAAttributes sEPAAttributes = new SEPAAttributes();
            //bankCodes = new BankCodes(@"XMLFiles\SpanishBankCodes.xml", BankCodes.BankCodesFileFormat.XML);
            //creditor = new Creditor_POCOClass(
            //    "Real Club Náutico de Gran Canaria",
            //    sEPAAttributes.AT02CreditorIdentifier("ES", "G35008770", "777"),
            //    sEPAAttributes.AT07IBAN_Spanish("12345678061234567890"));
            //creditorAgent = new CreditorAgent_POCOClass(bankCodes.BankDictionaryByLocalBankCode["3183"].BankBIC);
            //directDebitMandateInfo1 = new DirectDebitTransactionInfo_POCOClass(
            //    "Pedro Piqueras",
            //    "InternalID2510201300099",
            //    new string[] { "Cuota Mensual Numerario Septiembre 2013", "Cuota Mensual Numerario Octubre 2013" },
            //    (double)158,
            //    sEPAAttributes.AT01MandateReference("000001102345"),
            //    sEPAAttributes.AT07IBAN_Spanish("01000100709999999999"),
            //    sEPAAttributes.AT25DateOfMandateSigning_MigrationValue,
            //    sEPAAttributes.AT01MandateReference("000001101111"),
            //    sEPAAttributes.AT07IBAN_Spanish("01000100761234567890"));

            //directDebitMandateInfo2 = new DirectDebitTransactionInfo_POCOClass(
            //    "Manuel Moreno",
            //    "InternalID2510201300100",
            //    new string[] { "Cuota Mensual Numerario Octubre 2013" },
            //    (double)79,
            //    sEPAAttributes.AT01MandateReference("000001102346"),
            //    sEPAAttributes.AT07IBAN_Spanish("01821234861234567890"),
            //    sEPAAttributes.AT25DateOfMandateSigning_MigrationValue,
            //    null,
            //    null);

            //directDebitMandateInfoList = new List<DirectDebitTransactionInfo_POCOClass>() { directDebitMandateInfo1, directDebitMandateInfo2 };

            xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03";
            xSDFilePath = @"XSDFiles\pain.002.001.03.xsd";
        }
        [TestMethod]
        public void OriginalGroupInformationAndStatusElement_OrgnlGrpInfAndSts_IsCorrectlyDeserialized()
        {
            OriginalGroupInformation20 originalGroupInformation = XMLSerializer.XMLDeserializeFromFile<OriginalGroupInformation20>(@"XML Test Files\pain.002.001.03\OriginalGroupInformationAndStatus.xml", "OrgnlGrpInfAndSts", xMLNamespace);
            Assert.AreEqual("2012-07-18DEV0801009310G12345678100", originalGroupInformation.OrgnlMsgId);
            Assert.AreEqual("NOTPROVIDED", originalGroupInformation.OrgnlMsgNmId);
            Assert.AreEqual("3", originalGroupInformation.OrgnlNbOfTxs);
            Assert.AreEqual(220, originalGroupInformation.OrgnlCtrlSum);
        }

        [TestMethod]
        public void PaymentTransactionInformation_TxInfAndSts_IsCorrectlyDeserialized()
        {
            PaymentTransactionInformation25 paymentTransactionInformation = XMLSerializer.XMLDeserializeFromFile<PaymentTransactionInformation25>(@"XML Test Files\pain.002.001.03\PaymentTransactionInformation.xml", "TxInfAndSts", xMLNamespace);
            string devolReason = paymentTransactionInformation.StsRsnInf[0].Rsn.Item;

            Assert.AreEqual("064869001000000107", paymentTransactionInformation.OrgnlInstrId);
            Assert.AreEqual("201207010001/01002", paymentTransactionInformation.OrgnlEndToEndId);
            Assert.AreEqual("MS02", devolReason);
        }

        [TestMethod]
        public void OriginalTransactionReference_OrgnlTxRef_IsCorrectlyDeserialized()
        {
            OriginalTransactionReference13 originalTransactionReference = XMLSerializer.XMLDeserializeFromFile<OriginalTransactionReference13>(@"XML Test Files\pain.002.001.03\OriginalTransactionReference.xml", "OrgnlTxRef", xMLNamespace);
            decimal amount = ((ActiveOrHistoricCurrencyAndAmount)originalTransactionReference.Amt.Item).Value;
            System.DateTime collectionDate = originalTransactionReference.ReqdColltnDt;
            bool collectionDateSpecified = originalTransactionReference.ReqdColltnDtSpecified;
            string creditorID = ((PersonIdentification5)originalTransactionReference.CdtrSchmeId.Id.Item).Othr[0].Id;
            PersonIdentificationSchemeName1Choice creditorSchemeNameType = ((PersonIdentification5)originalTransactionReference.CdtrSchmeId.Id.Item).Othr[0].SchmeNm;
            string creditorShemeName = ((PersonIdentification5)originalTransactionReference.CdtrSchmeId.Id.Item).Othr[0].SchmeNm.Item;
            string paymentTypeInformation_ServiceLevel = originalTransactionReference.PmtTpInf.SvcLvl.Item;
            string paymentTypeInformation_LocaInstrument = originalTransactionReference.PmtTpInf.LclInstrm.Item;
            SequenceType1Code paymentTypeInformation_SequenceType = originalTransactionReference.PmtTpInf.SeqTp;

            //string devolReason = originalTransactionReference.StsRsnInf[0].Rsn.Item;

            Assert.AreEqual(100, amount);
            System.DateTime expectedDate = System.DateTime.Parse("2012-07-16");
            Assert.AreEqual(expectedDate, collectionDate);
            Assert.IsTrue(collectionDateSpecified);
            Assert.AreEqual("ES29000G12345678", creditorID);
            Assert.AreEqual("SEPA", creditorShemeName);
            Assert.AreEqual("SEPA", paymentTypeInformation_ServiceLevel);
            Assert.AreEqual("CORE", paymentTypeInformation_LocaInstrument);


            //Assert.AreEqual("064869001000000107", originalTransactionReference.OrgnlInstrId);
            //Assert.AreEqual("201207010001/01002", originalTransactionReference.OrgnlEndToEndId);
            //Assert.AreEqual("MS02", devolReason);
        }
    }
}
