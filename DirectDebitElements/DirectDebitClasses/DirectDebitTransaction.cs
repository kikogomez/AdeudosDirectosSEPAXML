using System;
using System.Collections.Generic;
using System.Linq;
using ISO20022PaymentInitiations;
using Billing;

namespace DirectDebitElements
{
    public class DirectDebitTransaction
    {
        List<SimplifiedBill> billsInTransaction;
        decimal totalAmount;
        int numberOfBills;
        string transactionID;
        string mandateID;
        DateTime mandateSignatureDate;
        BankAccount debtorAccount;
        string accountHolderName;
        DirectDebitAmendmentInformation amendmentInformation;

        public DirectDebitTransaction(
            List<SimplifiedBill> billsInTransaction,
            string transactionID,
            string mandateID,
            DateTime mandateSignatureDate,
            BankAccount debtorAccount,
            string accountHolderName, 
            DirectDebitAmendmentInformation amendmentInformation)
        {
            InitializeFields(transactionID, mandateID, mandateSignatureDate, debtorAccount, accountHolderName, amendmentInformation);
            this.billsInTransaction = billsInTransaction;
            UpdateAmountAndNumberOfBills();
        }

        public List<SimplifiedBill> BillsInTransaction
        {
            get { return billsInTransaction; }
        }

        public decimal Amount
        {
            get { return totalAmount; }
        }

        public int NumberOfBills
        {
            get { return numberOfBills; }
        }

        public string TransactionID
        {
            get { return transactionID; }
        }

        public string MandateID
        {
            get { return mandateID; }
        }

        public DateTime MandateSigatureDate
        {
            get { return mandateSignatureDate; }
        }

        public BankAccount DebtorAccount
        {
            get { return debtorAccount; }
        }

        public string AccountHolderName
        {
            get { return accountHolderName; }
        }

        public DirectDebitAmendmentInformation AmendmentInformation
        {
            get { return amendmentInformation; }
        }

        public void AddBill(SimplifiedBill bill)
        {
            this.billsInTransaction.Add(bill);
            UpdateAmountAndNumberOfBills();
        }

        private void InitializeFields(
            string transactionID,
            string mandateID,
            DateTime mandateSignatureDate,
            BankAccount debtorAccount,
            string accountHolderName,
            DirectDebitAmendmentInformation amendmentInformation)
        {
            CheckMandatoryFields(transactionID, mandateID, debtorAccount);

            this.transactionID = transactionID.Trim();
            this.mandateID = mandateID.Trim();
            this.mandateSignatureDate = mandateSignatureDate;
            this.debtorAccount = debtorAccount;
            this.accountHolderName = accountHolderName;
            this.amendmentInformation = amendmentInformation;
        }

        private void UpdateAmountAndNumberOfBills()
        {
            totalAmount = billsInTransaction.Select(bill => bill.Amount).Sum();
            numberOfBills = billsInTransaction.Count;
        }

        private void CheckMandatoryFields(string transactionID, string mandateID, BankAccount debtorAccount)
        {
            if (transactionID == null) throw new ArgumentNullException("transactionID", "TransactionID can't be null");
            if (transactionID.Trim().Length==0) throw new ArgumentException("TransactionID can't be empty", "transactionID");
            if (transactionID.Trim().Length > 35) throw new ArgumentOutOfRangeException("transactionID", "TransactionID can't be longer than 35 characters");
            if (mandateID == null) throw new ArgumentNullException("MandateID", "MandateID can't be null");
            if (mandateID.Trim().Length == 0) throw new ArgumentException("MandateID can't be empty", "MandateID");
            if (mandateID.Trim().Length > 35) throw new ArgumentOutOfRangeException("MandateID", "MandateID can't be longer than 35 characters");
            if (debtorAccount == null) throw new ArgumentNullException("DebtorAccount", "DebtorAccount can't be null");
            if (!debtorAccount.HasValidIBAN) throw new ArgumentException("DebtorAccount", "DebtorAccount must be a valid IBAN");
        }
    }
}
