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

        //public DirectDebitRemittance CreateAPaymentStatusReport(DateTime creationDateTime, DateTime requestedCollectionDate, DirectDebitInitiationContract directDebitInitiationContract)
        //{
        //    string messageID = "DATIR00112G12345678100";
        //    DateTime messageCreationDateTime = DateTime.Parse("2012-07-18T06:00:01");
        //    DateTime rejectAccountChargeDateTime = DateTime.Parse("2012-07-18");
        //    int numberOfTransactions = directDebitRemmitanceReject1.NumberOfTransactions + directDebitRemmitanceReject2.NumberOfTransactions;
        //    decimal controlSum = directDebitRemmitanceReject1.ControlSum + directDebitRemmitanceReject2.ControlSum;

        //    PaymentStatusReport paymentStatusReport = new PaymentStatusReport(
        //        messageID,
        //        messageCreationDateTime,
        //        rejectAccountChargeDateTime,
        //        numberOfTransactions,
        //        controlSum,
        //        directDebitRemmitanceRejectsList);

        //    //DirectDebitRemittance directDebitRemmitance = new DirectDebitRemittance(creationDateTime, requestedCollectionDate, directDebitInitiationContract);
        //    //return directDebitRemmitance;
        //}

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
