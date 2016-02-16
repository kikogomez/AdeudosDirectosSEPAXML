using System;
using ISO20022PaymentInitiations;
using ISO20022PaymentInitiations.SchemaSerializableClasses;
using ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport;
using XMLSerializerValidator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ISO20022PaymentInitiationsUnitTests
{
    [TestClass]
    public class ISO20020PaymentStatusReportTests
    {
        static string xMLNamespace;
        //static string xSDFilePath;

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03";
            //xSDFilePath = @"XSDFiles\pain.002.001.03.xsd";
        }

        [TestMethod]
        public void OriginalTransactionReference_OrgnlTxRef_IsCorrectlyDeserialized()
        {
            OriginalTransactionReference13 originalTransactionReference = XMLSerializer.XMLDeserializeFromFile<OriginalTransactionReference13>(@"XML Test Files\pain.002.001.03\OriginalTransactionReference.xml", "OrgnlTxRef", xMLNamespace);

            decimal amount = ((ActiveOrHistoricCurrencyAndAmount)originalTransactionReference.Amt.Item).Value;
            DateTime collectionDate = originalTransactionReference.ReqdColltnDt;
            bool collectionDateSpecified = originalTransactionReference.ReqdColltnDtSpecified;
            string creditorID = ((PersonIdentification5)originalTransactionReference.CdtrSchmeId.Id.Item).Othr[0].Id;
            string creditorShemeName = ((PersonIdentification5)originalTransactionReference.CdtrSchmeId.Id.Item).Othr[0].SchmeNm.Item;
            string paymentTypeInformation_ServiceLevel = originalTransactionReference.PmtTpInf.SvcLvl.Item;
            string paymentTypeInformation_LocaInstrument = originalTransactionReference.PmtTpInf.LclInstrm.Item;
            SequenceType1Code paymentTypeInformation_SequenceType = originalTransactionReference.PmtTpInf.SeqTp;
            string mandateRelatedInformation_MandateID = originalTransactionReference.MndtRltdInf.MndtId;
            DateTime mandateRelatedInformation_DateOfSignature = originalTransactionReference.MndtRltdInf.DtOfSgntr;
            bool mandateRelatedInformation_DateOfSignatureSpecified = originalTransactionReference.MndtRltdInf.DtOfSgntrSpecified;
            string RemittanceInformation = originalTransactionReference.RmtInf.Ustrd[0];
            string debtor_Name = originalTransactionReference.Dbtr.Nm;
            string debtor_PostalAddress_Country = originalTransactionReference.Dbtr.PstlAdr.Ctry;
            string debtor_PostalAddres_AddressLine = originalTransactionReference.Dbtr.PstlAdr.AdrLine[0];
            string debtorAccount = (string)originalTransactionReference.DbtrAcct.Id.Item;
            string debtorAgentBIC = originalTransactionReference.DbtrAgt.FinInstnId.BIC;
            string creditorAgentBIC = originalTransactionReference.CdtrAgt.FinInstnId.BIC;
            string creditor_Name = originalTransactionReference.Cdtr.Nm;
            string creditor_PostalAddress_Country = originalTransactionReference.Cdtr.PstlAdr.Ctry;
            string creditor_PostalAddress_AddressLine = originalTransactionReference.Cdtr.PstlAdr.AdrLine[0];
            string creditorAccount = (string)originalTransactionReference.CdtrAcct.Id.Item;

            Assert.AreEqual(100, amount);
            DateTime expectedCollectionDate = System.DateTime.Parse("2012-07-16");
            Assert.AreEqual(expectedCollectionDate, collectionDate);
            Assert.IsTrue(collectionDateSpecified);
            Assert.AreEqual("ES29000G12345678", creditorID);
            Assert.AreEqual("SEPA", creditorShemeName);
            Assert.AreEqual("SEPA", paymentTypeInformation_ServiceLevel);
            Assert.AreEqual("CORE", paymentTypeInformation_LocaInstrument);
            Assert.AreEqual(SequenceType1Code.RCUR, paymentTypeInformation_SequenceType);
            Assert.AreEqual("000001101000", mandateRelatedInformation_MandateID);
            DateTime expectedDateOfSignature = DateTime.Parse("2011-09-27");
            Assert.AreEqual(expectedDateOfSignature, mandateRelatedInformation_DateOfSignature);
            Assert.IsTrue(mandateRelatedInformation_DateOfSignatureSpecified);
            Assert.AreEqual("FACTURA NUM 7877", RemittanceInformation);
            Assert.AreEqual("NURIA SAULER PORTAL", debtor_Name);
            Assert.AreEqual("ES", debtor_PostalAddress_Country);
            Assert.AreEqual("CALLE ANDRADE, 80  08013 BARCELONA", debtor_PostalAddres_AddressLine);
            Assert.AreEqual("ES9821008701231234567890", debtorAccount);
            Assert.AreEqual("CAIXESBBXXX", debtorAgentBIC);
            Assert.AreEqual("CAIXESBBXXX", creditorAgentBIC);
            Assert.AreEqual("NOMBRE ACREEDOR DE PRUEBAS", creditor_Name);
            Assert.AreEqual("ES", creditor_PostalAddress_Country);
            Assert.AreEqual("CALLE ROMA, 102 LUGO", creditor_PostalAddress_AddressLine);
            Assert.AreEqual("ES0921009999961234567890", creditorAccount);
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
        public void OriginalGroupInformationAndStatusElement_OrgnlGrpInfAndSts_IsCorrectlyDeserialized()
        {
            OriginalGroupInformation20 originalGroupInformation = XMLSerializer.XMLDeserializeFromFile<OriginalGroupInformation20>(@"XML Test Files\pain.002.001.03\OriginalGroupInformationAndStatus.xml", "OrgnlGrpInfAndSts", xMLNamespace);
            Assert.AreEqual("2012-07-18DEV0801009310G12345678100", originalGroupInformation.OrgnlMsgId);
            Assert.AreEqual("NOTPROVIDED", originalGroupInformation.OrgnlMsgNmId);
            Assert.AreEqual("3", originalGroupInformation.OrgnlNbOfTxs);
            Assert.AreEqual(220, originalGroupInformation.OrgnlCtrlSum);
        }

        [TestMethod]
        public void GroupHeader_GrpHdr_IsCorrectlyDeserialized()
        {
            GroupHeader36 groupHeader = XMLSerializer.XMLDeserializeFromFile<GroupHeader36>(@"XML Test Files\pain.002.001.03\GroupHeader.xml", "GrpHdr", xMLNamespace);

            string messageIdentification = groupHeader.MsgId;
            DateTime creationDateTime = groupHeader.CreDtTm;
            string initiatingParty_OrganisationIdentification_BIC = ((OrganisationIdentification4)groupHeader.InitgPty.Id.Item).BICOrBEI;

            Assert.AreEqual("DATIR00112G12345678100", messageIdentification);
            DateTime expectedCreationDatetime = DateTime.Parse("2012-07-18T06:00:01");
            Assert.AreEqual(expectedCreationDatetime, creationDateTime);
            Assert.AreEqual("CAIXESBBXXX", initiatingParty_OrganisationIdentification_BIC);
        }

        [TestMethod]
        public void CustomerPaymentStatusReport_CstmrPmtStsRpt_IsCorrectlyDeserialized()
        {
            CustomerPaymentStatusReportV03 customerPaymentStatusReport = XMLSerializer.XMLDeserializeFromFile<CustomerPaymentStatusReportV03>(@"XML Test Files\pain.002.001.03\CustomerPaymentStatusReport.xml", "CstmrPmtStsRpt", xMLNamespace);

            GroupHeader36 groupHeader = customerPaymentStatusReport.GrpHdr;
            OriginalGroupInformation20 originalGroupInformation = customerPaymentStatusReport.OrgnlGrpInfAndSts;
            OriginalPaymentInformation1[] originalPaymentInformation = customerPaymentStatusReport.OrgnlPmtInfAndSts;

            Assert.AreEqual("DATIR00112G12345678100", groupHeader.MsgId);
            Assert.AreEqual("2012-07-18DEV0801009310G12345678100", originalGroupInformation.OrgnlMsgId);
            Assert.AreEqual(2, originalPaymentInformation.Length);
            Assert.AreEqual("PRE201207010001", originalPaymentInformation[0].OrgnlPmtInfId);
            Assert.AreEqual("PRE201205270001", originalPaymentInformation[1].OrgnlPmtInfId);
        }

        [TestMethod]
        public void CustomerPaymentStatusReportDocument_Document_IsCorrectlyDeserialized()
        {
            CustomerPaymentStatusReportDocument customerPaymentStatusReportDocument = XMLSerializer.XMLDeserializeFromFile<CustomerPaymentStatusReportDocument>(@"XML Test Files\pain.002.001.03\LaCaixa_pain00200103_Example1.xml", "Document", xMLNamespace);

            GroupHeader36 groupHeader = customerPaymentStatusReportDocument.CstmrPmtStsRpt.GrpHdr;
            OriginalGroupInformation20 originalGroupInformation = customerPaymentStatusReportDocument.CstmrPmtStsRpt.OrgnlGrpInfAndSts;
            OriginalPaymentInformation1[] originalPaymentInformation = customerPaymentStatusReportDocument.CstmrPmtStsRpt.OrgnlPmtInfAndSts;

            Assert.AreEqual("DATIR00112G12345678100", groupHeader.MsgId);
            Assert.AreEqual("2012-07-18DEV0801009310G12345678100", originalGroupInformation.OrgnlMsgId);
            Assert.AreEqual(2, originalPaymentInformation.Length);
            Assert.AreEqual("PRE201207010001", originalPaymentInformation[0].OrgnlPmtInfId);
            Assert.AreEqual("PRE201205270001", originalPaymentInformation[1].OrgnlPmtInfId);
        }
    }
}
