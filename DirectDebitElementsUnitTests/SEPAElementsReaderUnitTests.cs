using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectDebitElements;
using ISO20022PaymentInitiations.SchemaSerializableClasses;
using ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport;
using XMLSerializerValidator;


namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class SEPAElementsReaderUnitTests
    {
        [TestMethod]
        public void ADirectDebitPaymentInstructionRejectIsCorrectlyReadFromAOriginalPaymentInformation1()
        {
            string xMLFilePath = @"XML Test Files\pain.002.001.03\LaCaixa_pain00200103_Example1.xml";
            StreamReader fileReader = new StreamReader(xMLFilePath);
            string paymentStatusReportXMLStringMessage = fileReader.ReadToEnd();
            string xMLNamespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03";
            string rootElementName = "Document";
            CustomerPaymentStatusReportDocument customerPaymentStatusReportDocument = XMLSerializer.XMLDeserializeFromString<CustomerPaymentStatusReportDocument>(paymentStatusReportXMLStringMessage, rootElementName, xMLNamespace);
            OriginalPaymentInformation1 originalPaymentInformation = customerPaymentStatusReportDocument.CstmrPmtStsRpt.OrgnlPmtInfAndSts[0];

            DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject = SEPAElementsReader.CreateDirectDebitPaymentInstructionReject(originalPaymentInformation);

            Dictionary<string, DirectDebitTransactionReject> directDebitTransactionRejectDictionary =
                directDebitPaymentInstructionReject.DirectDebitTransactionsRejects.ToDictionary(
                    transactionReject => transactionReject.OriginalEndtoEndTransactionIdentification,
                    transactionReject => transactionReject);
            Assert.AreEqual(originalPaymentInformation.OrgnlPmtInfId, directDebitPaymentInstructionReject.OriginalPaymentInformationID);
            Assert.AreEqual(originalPaymentInformation.OrgnlNbOfTxs, directDebitPaymentInstructionReject.NumberOfTransactions.ToString());
            Assert.AreEqual(originalPaymentInformation.OrgnlCtrlSum, directDebitPaymentInstructionReject.ControlSum);
            foreach (PaymentTransactionInformation25 transactionInformationAndStatus in originalPaymentInformation.TxInfAndSts)
            {
                string key = transactionInformationAndStatus.OrgnlEndToEndId;
                DirectDebitTransactionReject directDebitTransactionReject = directDebitTransactionRejectDictionary[key];
                Assert.AreEqual(transactionInformationAndStatus.OrgnlInstrId, directDebitTransactionReject.OriginalTransactionIdentification);
                Assert.AreEqual(transactionInformationAndStatus.OrgnlEndToEndId, directDebitTransactionReject.OriginalEndtoEndTransactionIdentification);
                Assert.AreEqual(transactionInformationAndStatus.OrgnlTxRef.MndtRltdInf.MndtId, directDebitTransactionReject.MandateID);
                Assert.AreEqual(transactionInformationAndStatus.StsRsnInf[0].Rsn.Item.ToString(), directDebitTransactionReject.RejectReason);
                Assert.AreEqual(((ActiveOrHistoricCurrencyAndAmount)transactionInformationAndStatus.OrgnlTxRef.Amt.Item).Value, directDebitTransactionReject.Amount);
                Assert.AreEqual((string)transactionInformationAndStatus.OrgnlTxRef.DbtrAcct.Id.Item, directDebitTransactionReject.DebtorAccount.IBAN.IBAN);              
                Assert.AreEqual(transactionInformationAndStatus.OrgnlTxRef.ReqdColltnDt, directDebitTransactionReject.RequestedCollectionDate);
            }
        }
    }
}
