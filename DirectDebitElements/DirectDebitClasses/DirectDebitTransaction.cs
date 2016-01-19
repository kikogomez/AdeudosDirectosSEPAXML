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
        string internalUniqueInstructionID;
        string mandateID;
        DateTime mandateSignatureDate;
        BankAccount debtorAccount;
        string accountHolderName;
        DirectDebitAmendmentInformation amendmentInformation;

        public DirectDebitTransaction(
            List<SimplifiedBill> billsInTransaction,
            string internalUniqueInstructionID,
            string mandateID,
            DateTime mandateSignatureDate,
            BankAccount debtorAccount,
            string accountHolderName, 
            DirectDebitAmendmentInformation amendmentInformation)
        {
            InitializeFields(internalUniqueInstructionID, mandateID, mandateSignatureDate, debtorAccount, accountHolderName, amendmentInformation);
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

        public string InternalUniqueInstructionID
        {
            get { return internalUniqueInstructionID; }
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
            string internalUniqueInstructionID,
            string mandateID,
            DateTime mandateSignatureDate,
            BankAccount debtorAccount,
            string accountHolderName,
            DirectDebitAmendmentInformation amendmentInformation)
        {
            CheckMandatoryFields(internalUniqueInstructionID, mandateID, debtorAccount);

            this.internalUniqueInstructionID = internalUniqueInstructionID.Trim();
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

        private void CheckMandatoryFields(string internalUniqueInstructionID, string mandateID, BankAccount debtorAccount)
        {
            if (internalUniqueInstructionID == null) throw new ArgumentNullException("InternalUniqueInstructionID", "InternalUniqueInstructionID can't be null");
            if (internalUniqueInstructionID.Trim().Length==0) throw new ArgumentException("InternalUniqueInstructionID can't be empty", "InternalUniqueInstructionID");
            if (internalUniqueInstructionID.Trim().Length > 35) throw new ArgumentOutOfRangeException("InternalUniqueInstructionID", "InternalUniqueInstructionID can't be longer than 35 characters");
            if (mandateID == null) throw new ArgumentNullException("MandateID", "MandateID can't be null");
            if (mandateID.Trim().Length == 0) throw new ArgumentException("MandateID can't be empty", "MandateID");
            if (mandateID.Trim().Length > 35) throw new ArgumentOutOfRangeException("MandateID", "MandateID can't be longer than 35 characters");
            if (debtorAccount == null) throw new ArgumentNullException("DebtorAccount", "DebtorAccount can't be null");
            if (!debtorAccount.HasValidIBAN) throw new ArgumentException("DebtorAccount", "DebtorAccount must be a valid IBAN");
        }
    }
}
