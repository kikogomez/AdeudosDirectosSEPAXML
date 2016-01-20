using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISO20022PaymentInitiations;
using ISO20022PaymentInitiations.SchemaSerializableClasses;
using ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport;

namespace DirectDebitElements
{
    public static class SEPAElementsReader
    {

        public static DirectDebitPaymentInstructionReject CreateDirectDebitPaymentInstructionReject(OriginalPaymentInformation1 originalPaymentInformation1)
        {
            PaymentStatusReportManager paymentStatusReportManager = new PaymentStatusReportManager();

            string originalPaymentInformationID = originalPaymentInformation1.OrgnlPmtInfId;
            int numberOfTransactions = Int32.Parse(originalPaymentInformation1.OrgnlNbOfTxs);
            decimal controlSum = originalPaymentInformation1.OrgnlCtrlSum;
            List<DirectDebitTransactionReject> directDebitTransactionRejects = new List<DirectDebitTransactionReject>();

            foreach (PaymentTransactionInformation25 paymentTransactionInformation25 in originalPaymentInformation1.TxInfAndSts)
            {
                string originalTransactionIdentification = paymentTransactionInformation25.OrgnlInstrId;
                string originalEndtoEndTransactionIdentification = paymentTransactionInformation25.OrgnlEndToEndId;
                Nullable<DateTime> requestedCollectionDate = paymentTransactionInformation25.OrgnlTxRef.ReqdColltnDt;
                decimal amount = ((ActiveOrHistoricCurrencyAndAmount)paymentTransactionInformation25.OrgnlTxRef.Amt.Item).Value;
                string mandateID = paymentTransactionInformation25.OrgnlTxRef.MndtRltdInf.MndtId;
                InternationalAccountBankNumberIBAN iban = new InternationalAccountBankNumberIBAN((string)paymentTransactionInformation25.OrgnlTxRef.DbtrAcct.Id.Item);
                BankAccount debtorAccount = new BankAccount(iban);
                string rejectReason = paymentTransactionInformation25.StsRsnInf[0].Rsn.Item;

                DirectDebitTransactionReject directDebitTransactionReject = paymentStatusReportManager.CreateDirectDebitTransactionReject(
                    originalTransactionIdentification,
                    originalEndtoEndTransactionIdentification,
                    requestedCollectionDate ?? DateTime.MinValue,
                    amount,
                    mandateID,
                    debtorAccount,
                    rejectReason);

                directDebitTransactionRejects.Add(directDebitTransactionReject);
            }

            DirectDebitPaymentInstructionReject directDebitRemmitanceReject = paymentStatusReportManager.CreateCheckedDirectDebitPaymentInstructionReject(
                originalPaymentInformationID,
                numberOfTransactions,
                controlSum,
                directDebitTransactionRejects);

            return directDebitRemmitanceReject;
        }
    }
}
