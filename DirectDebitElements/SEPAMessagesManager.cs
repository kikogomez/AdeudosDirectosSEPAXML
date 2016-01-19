using System;
using System.Collections.Generic;
using System.Linq;
using ISO20022PaymentInitiations.SchemaSerializableClasses;
using ISO20022PaymentInitiations.SchemaSerializableClasses.DDInitiation;
using ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport;
using XMLSerializerValidator;
using ExtensionMethods;

namespace DirectDebitElements
{
    public class SEPAMessagesManager
    {
        public string GenerateISO20022CustomerDirectDebitInitiationMessage(
            Creditor creditor,
            CreditorAgent creditorAgent,
            DirectDebitRemittance directDebitRemmitance,
            bool singleUnstructuredConcept)
        {
            DirectDebitInitiationContract directDebitInitiationContract = directDebitRemmitance.DirectDebitInitiationContract;
            DateTime generationDateTime = directDebitRemmitance.CreationDate;

            PartyIdentification32 initiationParty_InitPty = SEPAElementsGenerator.GenerateInitiationParty_InitPty(creditor, directDebitInitiationContract);
            GroupHeader39 groupHeader_GrpHdr = SEPAElementsGenerator.GenerateGroupHeader_GrpHdr(
                directDebitRemmitance.MessageID,
                generationDateTime,
                directDebitRemmitance.NumberOfTransactions,
                directDebitRemmitance.ControlSum,
                initiationParty_InitPty);
            List<PaymentInstructionInformation4> paymentInformation_PmtInf_List = new List<PaymentInstructionInformation4>();

            List<DirectDebitTransactionInformation9> directDebitTransactionInfoList = new List<DirectDebitTransactionInformation9>();
            foreach (DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment in directDebitRemmitance.DirectDebitTransactionGroupPaymentCollection)
            {
                foreach (DirectDebitTransaction directDebitTransaction in directDebitTransactionsGroupPayment.DirectDebitTransactionsCollection)
                {
                    DirectDebitTransactionInformation9 directDebitTransactionInfo_DrctDbtTxInf = SEPAElementsGenerator.GenerateDirectDebitTransactionInfo_DrctDbtTxInf(
                        creditorAgent,
                        directDebitTransaction,
                        singleUnstructuredConcept);
                    directDebitTransactionInfoList.Add(directDebitTransactionInfo_DrctDbtTxInf);
                }

                PaymentInstructionInformation4 paymentInformation_PmtInf = SEPAElementsGenerator.GeneratePaymentInformation_PmtInf(
                    creditor,
                    creditorAgent,
                    directDebitInitiationContract,
                    directDebitTransactionsGroupPayment,
                    singleUnstructuredConcept);

                paymentInformation_PmtInf_List.Add(paymentInformation_PmtInf);
            }

            PaymentInstructionInformation4[] paymentInformation_PmtInf_Array = paymentInformation_PmtInf_List.ToArray();

            CustomerDirectDebitInitiationV02 customerDebitInitiationV02_Document = new CustomerDirectDebitInitiationV02(
                groupHeader_GrpHdr,                     //<GrpHdr>
                paymentInformation_PmtInf_Array);       //<PmtInf>

            CustomerDirectDebitInitiationDocument document_Document = new CustomerDirectDebitInitiationDocument(customerDebitInitiationV02_Document);

            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";
            string xmlString = XMLSerializer.XMLSerializeToString<CustomerDirectDebitInitiationDocument>(document_Document, "Document", xMLNamespace);
            return xmlString;
        }

        public PaymentStatusReport ReadISO20022PaymentStatusReportMessage(string paymentStatusReportXMLMessage)
        {
            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03";
            string rootElementName = "Document";
            CustomerPaymentStatusReportDocument customerPaymentStatusReportDocument = XMLSerializer.XMLDeserializeFromString<CustomerPaymentStatusReportDocument>(paymentStatusReportXMLMessage,rootElementName, xMLNamespace);

            string paymentStatusReport_MessageID = customerPaymentStatusReportDocument.CstmrPmtStsRpt.GrpHdr.MsgId;
            DateTime paymentStatusReport_MessageCreationDateTime = customerPaymentStatusReportDocument.CstmrPmtStsRpt.GrpHdr.CreDtTm;
            DateTime paymentStatusReport_RejectAccountChargeDateTime = ExtractRejectAccountChargeDateTimeFrom_OrgnlGrpInfAndSts_OrgnlMsgId(customerPaymentStatusReportDocument.CstmrPmtStsRpt.OrgnlGrpInfAndSts.OrgnlMsgId);
            int paymentStatusReport_NumberOfTransactions = Int32.Parse(customerPaymentStatusReportDocument.CstmrPmtStsRpt.OrgnlGrpInfAndSts.OrgnlNbOfTxs);
            decimal paymentStatusReport_ControlSum = customerPaymentStatusReportDocument.CstmrPmtStsRpt.OrgnlGrpInfAndSts.OrgnlCtrlSum;
            List<DirectDebitTransactionsGroupPaymentReject> directDebitRemmitanceRejectsList = new List<DirectDebitTransactionsGroupPaymentReject>();

            foreach (OriginalPaymentInformation1 originalPaymentInformation1 in customerPaymentStatusReportDocument.CstmrPmtStsRpt.OrgnlPmtInfAndSts)
            {
                directDebitRemmitanceRejectsList.Add(SEPAElementsReader.CreateDirectDebitRemmitanceReject(originalPaymentInformation1));
            }

            PaymentStatusReportManager paymentStatusReportmanager = new PaymentStatusReportManager();
            PaymentStatusReport paymentStatusReport = paymentStatusReportmanager.CreateCheckedPaymentStatusReport(
                paymentStatusReport_MessageID,
                paymentStatusReport_MessageCreationDateTime,
                paymentStatusReport_RejectAccountChargeDateTime,
                paymentStatusReport_NumberOfTransactions,
                paymentStatusReport_ControlSum,
                directDebitRemmitanceRejectsList);

            return paymentStatusReport;
        }

        private DateTime ExtractRejectAccountChargeDateTimeFrom_OrgnlGrpInfAndSts_OrgnlMsgId(string orgnlGrpInfAndSts_OrgnlMsgId)
        {
            return DateTime.Parse(orgnlGrpInfAndSts_OrgnlMsgId.Substring(0,10));
        }
    }
}
