using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ISO20022PaymentInitiations;
using ISO20022PaymentInitiations.SchemaSerializableClasses;
using ISO20022PaymentInitiations.SchemaSerializableClasses.DDInitiation;
using ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport;
using XMLSerializerValidator;
using ExtensionMethods;

namespace DirectDebitElements
{
    public class SEPAMessagesManager
    {
        public string GenerateISO20022CustomerDirectDebitInitiationStringMessage(
            Creditor creditor,
            CreditorAgent creditorAgent,
            DirectDebitRemittance directDebitRemittance,
            bool singleUnstructuredConcept,
            bool conceptsIncludeAmounts)
        {
            CustomerDirectDebitInitiationDocument document_Document = CreateCustomerDirectDebitInitiationDocument(
                creditor,
                creditorAgent,
                directDebitRemittance,
                singleUnstructuredConcept,
                conceptsIncludeAmounts);

            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";
            string xmlString = XMLSerializer.XMLSerializeToString<CustomerDirectDebitInitiationDocument>(document_Document, "Document", xMLNamespace);
            return xmlString;
        }

        public string GenerateISO20022CustomerDirectDebitInitiationStringMessage(
            Creditor creditor,
            CreditorAgent creditorAgent,
            DirectDebitRemittance directDebitRemittance,
            bool singleUnstructuredConcept,
            bool conceptsIncludeAmounts,
            Encoding encoding)
        {
            CustomerDirectDebitInitiationDocument document_Document = CreateCustomerDirectDebitInitiationDocument(
                creditor,
                creditorAgent,
                directDebitRemittance,
                singleUnstructuredConcept,
                conceptsIncludeAmounts);

            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";
            string xmlString = XMLSerializer.XMLSerializeToString<CustomerDirectDebitInitiationDocument>(document_Document, "Document", xMLNamespace, encoding);
            return xmlString;
        }

        public void GenerateISO20022CustomerDirectDebitInitiationFileMessage(
            Creditor creditor,
            CreditorAgent creditorAgent,
            DirectDebitRemittance directDebitRemittance,
            bool singleUnstructuredConcept,
            bool conceptsIncludeAmounts,
            string xMLFilePath)
        {

            CustomerDirectDebitInitiationDocument document_Document = CreateCustomerDirectDebitInitiationDocument(
                creditor,
                creditorAgent,
                directDebitRemittance,
                singleUnstructuredConcept,
                conceptsIncludeAmounts);

            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";
            XMLSerializer.XMLSerializeToFile<CustomerDirectDebitInitiationDocument>(document_Document, "Document", xMLNamespace, xMLFilePath);
        }

        public void GenerateISO20022CustomerDirectDebitInitiationFileMessage(
            Creditor creditor,
            CreditorAgent creditorAgent,
            DirectDebitRemittance directDebitRemittance,
            bool singleUnstructuredConcept,
            bool conceptsIncludeAmounts,
            string xMLFilePath,
            Encoding encoding)
        {

            CustomerDirectDebitInitiationDocument document_Document = CreateCustomerDirectDebitInitiationDocument(
                creditor,
                creditorAgent,
                directDebitRemittance,
                singleUnstructuredConcept,
                conceptsIncludeAmounts);

            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";
            XMLSerializer.XMLSerializeToFile<CustomerDirectDebitInitiationDocument>(document_Document, "Document", xMLNamespace, xMLFilePath, encoding);
        }

        public PaymentStatusReport ReadISO20022PaymentStatusReportStringMessage(string paymentStatusReportXMLMessage)
        {
            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03";
            string rootElementName = "Document";

            ValidatePaymentStatusReportString(paymentStatusReportXMLMessage);
            CustomerPaymentStatusReportDocument customerPaymentStatusReportDocument = customerPaymentStatusReportDocument = XMLSerializer.XMLDeserializeFromString<CustomerPaymentStatusReportDocument>(paymentStatusReportXMLMessage, rootElementName, xMLNamespace);
            return ProcessCustomerPaymentStatusReportDocument(customerPaymentStatusReportDocument);
        }

        public PaymentStatusReport ReadISO20022PaymentStatusReportFile(string paymentStatusReportXMLFilePath)
        {
            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03";
            string rootElementName = "Document";

            ValidatePaymentStatusReportFile(paymentStatusReportXMLFilePath);
            CustomerPaymentStatusReportDocument customerPaymentStatusReportDocument = XMLSerializer.XMLDeserializeFromFile<CustomerPaymentStatusReportDocument>(paymentStatusReportXMLFilePath, rootElementName, xMLNamespace);
            return ProcessCustomerPaymentStatusReportDocument(customerPaymentStatusReportDocument);
        }

        private void ValidatePaymentStatusReportFile(string paymentStatusReportXMLFilePath)
        {
            string xMLValidationErrors = SchemaValidators.ValidatePaymentStatusReportFile(paymentStatusReportXMLFilePath);
            if (xMLValidationErrors != "") throw new System.Xml.Schema.XmlSchemaValidationException(xMLValidationErrors);

        }

        private void ValidatePaymentStatusReportString(string paymentStatusReportXMLMessage)
        {
            string xMLValidationErrors = SchemaValidators.ValidatePaymentStatusReportString(paymentStatusReportXMLMessage);
            if (xMLValidationErrors != "") throw new System.Xml.Schema.XmlSchemaValidationException(xMLValidationErrors);
        }

        private PaymentStatusReport ProcessCustomerPaymentStatusReportDocument(CustomerPaymentStatusReportDocument customerPaymentStatusReportDocument)
        {
            string paymentStatusReport_MessageID = customerPaymentStatusReportDocument.CstmrPmtStsRpt.GrpHdr.MsgId;
            DateTime paymentStatusReport_MessageCreationDateTime = customerPaymentStatusReportDocument.CstmrPmtStsRpt.GrpHdr.CreDtTm;
            //This is only valid for La Caixa CreditorAgent
            //DateTime paymentStatusReport_RejectAccountChargeDateTime = ExtractRejectAccountChargeDateTimeFrom_OrgnlGrpInfAndSts_OrgnlMsgId(customerPaymentStatusReportDocument.CstmrPmtStsRpt.OrgnlGrpInfAndSts.OrgnlMsgId);
            DateTime paymentStatusReport_RejectAccountChargeDateTime = paymentStatusReport_MessageCreationDateTime.Date; //Unaccurate
            int paymentStatusReport_NumberOfTransactions = Int32.Parse(customerPaymentStatusReportDocument.CstmrPmtStsRpt.OrgnlGrpInfAndSts.OrgnlNbOfTxs);
            decimal paymentStatusReport_ControlSum = customerPaymentStatusReportDocument.CstmrPmtStsRpt.OrgnlGrpInfAndSts.OrgnlCtrlSum;
            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>();

            foreach (OriginalPaymentInformation1 originalPaymentInformation1 in customerPaymentStatusReportDocument.CstmrPmtStsRpt.OrgnlPmtInfAndSts)
            {
                directDebitPaymentInstructionRejects.Add(SEPAElementsReader.CreateDirectDebitPaymentInstructionReject(originalPaymentInformation1));
            }

            PaymentStatusReportManager paymentStatusReportmanager = new PaymentStatusReportManager();
            PaymentStatusReport paymentStatusReport = paymentStatusReportmanager.CreateAPaymentStatusReport(
                paymentStatusReport_MessageID,
                paymentStatusReport_MessageCreationDateTime,
                paymentStatusReport_RejectAccountChargeDateTime,
                paymentStatusReport_NumberOfTransactions,
                paymentStatusReport_ControlSum,
                directDebitPaymentInstructionRejects);

            return paymentStatusReport;
        }

        private CustomerDirectDebitInitiationDocument CreateCustomerDirectDebitInitiationDocument(
            Creditor creditor,
            CreditorAgent creditorAgent,
            DirectDebitRemittance directDebitRemittance,
            bool singleUnstructuredConcept,
            bool conceptsIncludeAmounts)
        {
            DirectDebitInitiationContract directDebitInitiationContract = directDebitRemittance.DirectDebitInitiationContract;
            DateTime generationDateTime = directDebitRemittance.CreationDate;
            DateTime requestedCollectionDate = directDebitRemittance.RequestedCollectionDate;

            PartyIdentification32 initiationParty_InitPty = SEPAElementsGenerator.GenerateInitiationParty_InitPty(creditor, directDebitInitiationContract);
            GroupHeader39 groupHeader_GrpHdr = SEPAElementsGenerator.GenerateGroupHeader_GrpHdr(
                directDebitRemittance.MessageID,
                generationDateTime,
                directDebitRemittance.NumberOfTransactions,
                directDebitRemittance.ControlSum,
                initiationParty_InitPty);
            List<PaymentInstructionInformation4> paymentInformation_PmtInf_List = new List<PaymentInstructionInformation4>();

            foreach (DirectDebitPaymentInstruction directDebitPaymentInstruction in directDebitRemittance.DirectDebitPaymentInstructions)
            {
                PaymentInstructionInformation4 paymentInformation_PmtInf = SEPAElementsGenerator.GeneratePaymentInformation_PmtInf(
                    creditor,
                    creditorAgent,
                    directDebitInitiationContract,
                    directDebitPaymentInstruction,
                    requestedCollectionDate,
                    singleUnstructuredConcept,
                    conceptsIncludeAmounts);

                paymentInformation_PmtInf_List.Add(paymentInformation_PmtInf);
            }

            PaymentInstructionInformation4[] paymentInformation_PmtInf_Array = paymentInformation_PmtInf_List.ToArray();

            CustomerDirectDebitInitiationV02 customerDebitInitiationV02_Document = new CustomerDirectDebitInitiationV02(
                groupHeader_GrpHdr,                     //<GrpHdr>
                paymentInformation_PmtInf_Array);       //<PmtInf>

            CustomerDirectDebitInitiationDocument document_Document = new CustomerDirectDebitInitiationDocument(customerDebitInitiationV02_Document);
            return document_Document;
        }

        //This method is only useful for La Caixa CreditorAgent
        private DateTime ExtractRejectAccountChargeDateTimeFrom_OrgnlGrpInfAndSts_OrgnlMsgId(string orgnlGrpInfAndSts_OrgnlMsgId)
        {
            return DateTime.Parse(orgnlGrpInfAndSts_OrgnlMsgId.Substring(0, 10));
        }
    }
}
