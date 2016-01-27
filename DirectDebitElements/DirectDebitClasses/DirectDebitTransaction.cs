using System;
using System.Collections.Generic;
using System.Linq;
using ISO20022PaymentInitiations;
using Billing;

namespace DirectDebitElements
{
    public class DirectDebitTransaction
    {
        public event EventHandler<decimal> ANewBillHasBeenAdded;

        List<SimplifiedBill> billsInTransaction;
        decimal totalAmount;
        int numberOfBills;
        string transactionID;
        string mandateID;
        DateTime mandateSignatureDate;
        BankAccount debtorAccount;
        string accountHolderName;
        DirectDebitAmendmentInformation amendmentInformation;
        bool firstDebit;

        public DirectDebitTransaction(
            List<SimplifiedBill> billsInTransaction,
            string transactionID,
            string mandateID,
            DateTime mandateSignatureDate,
            BankAccount debtorAccount,
            string accountHolderName, 
            DirectDebitAmendmentInformation amendmentInformation,
            bool firstDebit)
        {
            InitializeFields(transactionID, mandateID, mandateSignatureDate, debtorAccount, accountHolderName, amendmentInformation, firstDebit);
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

        public bool FirstDebit
        {
            get { return firstDebit; }
        }

        public void AddBill(SimplifiedBill bill)
        {
            this.billsInTransaction.Add(bill);
            UpdateAmountAndNumberOfBills();
            SignalANewBillHasBeenAdded(bill);
        }

        private void InitializeFields(
            string transactionID,
            string mandateID,
            DateTime mandateSignatureDate,
            BankAccount debtorAccount,
            string accountHolderName,
            DirectDebitAmendmentInformation amendmentInformation,
            bool firstDebit)
        {
            CheckMandatoryFields(transactionID, mandateID, debtorAccount, amendmentInformation, firstDebit);

            this.transactionID = transactionID.Trim();
            this.mandateID = mandateID.Trim();
            this.mandateSignatureDate = mandateSignatureDate;
            this.debtorAccount = debtorAccount;
            this.accountHolderName = accountHolderName;
            this.amendmentInformation = amendmentInformation;
            this.firstDebit = firstDebit;
        }

        private void UpdateAmountAndNumberOfBills()
        {
            totalAmount = billsInTransaction.Select(bill => bill.Amount).Sum();
            numberOfBills = billsInTransaction.Count;
        }

        private void CheckMandatoryFields(
            string transactionID,
            string mandateID,
            BankAccount debtorAccount,
            DirectDebitAmendmentInformation amendmentInformation,
            bool firstDebit)
        {
            if (transactionID == null) throw new TypeInitializationException("DirectDebitTransaction", new ArgumentNullException("transactionID", "TransactionID can't be null"));
            if (transactionID.Trim().Length == 0) throw new TypeInitializationException("DirectDebitTransaction", new ArgumentException("TransactionID can't be empty", "transactionID"));
            if (transactionID.Trim().Length > 35) throw new TypeInitializationException("DirectDebitTransaction", new ArgumentOutOfRangeException("transactionID", "TransactionID can't be longer than 35 characters"));
            if (mandateID == null) throw new TypeInitializationException("DirectDebitTransaction", new ArgumentNullException("MandateID", "MandateID can't be null"));
            if (mandateID.Trim().Length == 0) throw new TypeInitializationException("DirectDebitTransaction", new ArgumentException("MandateID can't be empty", "MandateID"));
            if (mandateID.Trim().Length > 35) throw new TypeInitializationException("DirectDebitTransaction", new ArgumentOutOfRangeException("MandateID", "MandateID can't be longer than 35 characters"));
            if (debtorAccount == null) throw new TypeInitializationException("DirectDebitTransaction", new ArgumentNullException("DebtorAccount", "DebtorAccount can't be null"));
            if (!debtorAccount.HasValidIBAN) throw new TypeInitializationException("DirectDebitTransaction", new ArgumentException("DebtorAccount must be a valid IBAN", "DebtorAccount"));
            if (!firstDebit && BankHasBeenChangedInAmendmentInformation(debtorAccount, amendmentInformation))
                throw new TypeInitializationException("DirectDebitTransaction", new ArgumentException("FirstDebit must be true when changing debtor bank", "firstDebit"));
        }

        private void SignalANewBillHasBeenAdded(SimplifiedBill bill)
        {
            if (ANewBillHasBeenAdded != null)
            {
                ANewBillHasBeenAdded(this, bill.Amount);
            }
        }

        private bool BankHasBeenChangedInAmendmentInformation(BankAccount debtorAccount, DirectDebitAmendmentInformation amendmentInformation)
        {
            if (amendmentInformation == null || amendmentInformation.OldBankAccount == null) return false;
            return (debtorAccount.BankAccountFieldCodes.BankCode != amendmentInformation.OldBankAccount.BankAccountFieldCodes.BankCode);
        }
    }
}
