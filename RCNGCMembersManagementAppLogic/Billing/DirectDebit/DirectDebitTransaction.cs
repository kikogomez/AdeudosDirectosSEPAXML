using System;
using System.Collections.Generic;
using System.Linq;

namespace RCNGCMembersManagementAppLogic.Billing.DirectDebit
{
    public class DirectDebitTransaction
    {
        string directDebitTransactionInternalReference;
        List<Bill> billsInTransaction;
        int internalDirectDebitReferenceNumber;
        BankAccount debtorAccount;
        string accountHolderName;
        DateTime mandateSignatureDate;
        string mandateID;

        decimal totalAmount;
        int numberOfBills;

        public DirectDebitTransaction(List<Bill> billsInTransaction, int internalDirectDebitReferenceNumber, BankAccount debtorAccount, string accountHolderName, DateTime mandateSignatureDate)
        {
            this.billsInTransaction = billsInTransaction;
            this.internalDirectDebitReferenceNumber = internalDirectDebitReferenceNumber;
            this.debtorAccount = debtorAccount;
            this.accountHolderName = accountHolderName;
            this.mandateSignatureDate = mandateSignatureDate;
            UpdateAmountAndNumberOfBills();
        }

        public DirectDebitTransaction(int internalDirectDebitReferenceNumber, BankAccount debtorAccount, string accountHolderName, DateTime mandateSignatureDate)
        {
            this.internalDirectDebitReferenceNumber = internalDirectDebitReferenceNumber;
            this.debtorAccount = debtorAccount;
            this.accountHolderName = accountHolderName;
            this.mandateSignatureDate = mandateSignatureDate;
            billsInTransaction = new List<Bill>();
        }

        public string DirectDebitTransactionInternalReference
        {
            get { return directDebitTransactionInternalReference; }
        }

        public string MandateID
        {
            get { return mandateID; }
        }

        public DateTime MandateSigatureDate
        {
            get { return mandateSignatureDate; }
        }

        public List<Bill> BillsInTransaction
        {
            get { return billsInTransaction; }
        }

        public int InternalDirectDebitReferenceNumber
        {
            get { return internalDirectDebitReferenceNumber; }
        }

        public BankAccount DebtorAccount
        {
            get { return debtorAccount; }
        }

        public string AccountHolderName
        {
            get { return accountHolderName; }
        }

        public decimal Amount
        {
            get { return totalAmount; }
        }

        public int NumberOfBills
        {
            get { return numberOfBills; }
        }

        public void AddBill(Bill bill)
        {
            this.billsInTransaction.Add(bill);
            UpdateAmountAndNumberOfBills();
        }

        public void GenerateDirectDebitTransactionInternalReference(int sequenceNumber)
        {
            directDebitTransactionInternalReference = sequenceNumber.ToString("000000");
        }

        public void GenerateMandateID(string creditorBusinessCode)
        {
            string csb19ReferenceNumber = CalculateOldCSB19Code(creditorBusinessCode);
            SEPAAttributes sEPAttributes = new SEPAAttributes();
            mandateID = sEPAttributes.AT01MandateReference(csb19ReferenceNumber);
        }

        private string CalculateOldCSB19Code(string creditorBusinessCode)
        {
            return "0000" + creditorBusinessCode + internalDirectDebitReferenceNumber.ToString("00000");
        }

        private void UpdateAmountAndNumberOfBills()
        {
            totalAmount = billsInTransaction.Select(bill => bill.Amount).Sum();
            numberOfBills = billsInTransaction.Count;
        }
    }
}
