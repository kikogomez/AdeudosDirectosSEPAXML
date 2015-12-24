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
            //PartyIdentification32 initiatingParty = XMLSerializer.XMLDeserializeFromFile<PartyIdentification32>(@"XML Test Files\pain.002.001.03\OriginalGroupInformationAndStatus.xml", "InitgPty", xMLNamespace);
            //Assert.AreEqual("Real Club Náutico de Gran Canaria", initiatingParty.Nm);
            //OrganisationIdentification4 orgId = (OrganisationIdentification4)initiatingParty.Id.Item;
            //string genericOrganisationInformationId = orgId.Othr[0].Id;
            //Assert.AreEqual("ES90777G35008770", genericOrganisationInformationId);
        }
    }
}
