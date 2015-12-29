using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectDebitElements
{
    public class PaymentStatusReportManager
    {
        public PaymentStatusReportManager() { }

        public PaymentStatusReport CreatePaymentStatusReport(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitRemmitanceReject> directDebitRemmitanceRejectsList)
        { 
            PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
                messageID,
                messageCreationDateTime,
                rejectAccountChargeDateTime,
                numberOfTransactions,
                controlSum,
                directDebitRemmitanceRejectsList);

            return paymentStatusReport;
        }

        //public DirectDebitTransactionsGroupPayment CreateANewGroupOfDirectDebitTransactions(string localInstrument)
        //{
        //    DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment = new DirectDebitTransactionsGroupPayment(localInstrument);
        //    return directDebitTransactionsGroupPayment;
        //}

        //public DirectDebitTransaction CreateANewEmptyDirectDebitTransaction(DirectDebitMandate directDebitmandate)
        //{
        //    DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
        //        directDebitmandate.InternalReferenceNumber,
        //        directDebitmandate.BankAccount,
        //        directDebitmandate.AccountHolderName,
        //        directDebitmandate.DirectDebitMandateCreationDate);
        //    return directDebitTransaction;
        //}

        //public DirectDebitTransaction CreateANewDirectDebitTransactionFromAGroupOfBills(DirectDebitMandate directDebitmandate, List<SimplifiedBill> billsList)
        //{
        //    DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
        //        billsList,
        //        directDebitmandate.InternalReferenceNumber,
        //        directDebitmandate.BankAccount,
        //        directDebitmandate.AccountHolderName,
        //        directDebitmandate.DirectDebitMandateCreationDate);
        //    return directDebitTransaction;
        //}

        //public void AddBilllToExistingDirectDebitTransaction(DirectDebitTransaction directDebitTransaction, SimplifiedBill bill)
        //{
        //    directDebitTransaction.AddBill(bill);
        //}

        //public void AddDirectDebitTransactionToGroupPayment(
        //    DirectDebitTransaction directDebitTransaction,
        //    DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment)
        //{
        //    directDebitTransactionsGroupPayment.AddDirectDebitTransaction(directDebitTransaction);
        //}

        //public void AddDirectDebitTransactionGroupPaymentToDirectDebitRemittance(
        //    DirectDebitRemittance directDebitRemmitance,
        //    DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment)
        //{
        //    directDebitRemmitance.AddDirectDebitTransactionsGroupPayment(directDebitTransactionsGroupPayment);
        //}
    }
}
