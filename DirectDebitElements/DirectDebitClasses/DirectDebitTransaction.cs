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


        public DirectDebitTransaction(
            List<SimplifiedBill> billsInTransaction,
            string internalUniqueInstructionID,
            string mandateID,
            DateTime mandateSignatureDate,
            BankAccount debtorAccount,
            string accountHolderName)
        {
            InitializeFields(internalUniqueInstructionID, mandateID, mandateSignatureDate, debtorAccount, accountHolderName);
            this.billsInTransaction = billsInTransaction;
            UpdateAmountAndNumberOfBills();
        }

        public DirectDebitTransaction(
            string internalUniqueInstructionID,
            string mandateID,
            DateTime mandateSignatureDate,
            BankAccount debtorAccount,
            string accountHolderName)
        {
            InitializeFields(internalUniqueInstructionID, mandateID, mandateSignatureDate, debtorAccount, accountHolderName);
            billsInTransaction = new List<SimplifiedBill>();
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
            //set { internalUniqueInstructionID = value; }
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
            string accountHolderName)
        {
            this.internalUniqueInstructionID = internalUniqueInstructionID;
            this.mandateID = mandateID;
            this.mandateSignatureDate = mandateSignatureDate;
            this.debtorAccount = debtorAccount;
            this.accountHolderName = accountHolderName;
        }

        //private void GenerateAT01MandateID()
        //{
        //    string mandateID = CalculateOldCSB19Code();
        //    //SEPAAttributes sEPAttributes = new SEPAAttributes();
        //    //mandateID = sEPAttributes.AT01PlainText1914MandateReference(csb19ReferenceNumber);
        //}

        //private string CalculateOldCSB19Code()
        //{
        //    return "0000" + creditorBusinessCode + mandateInternalReferenceNumber.ToString("00000");
        //}

        private void UpdateAmountAndNumberOfBills()
        {
            totalAmount = billsInTransaction.Select(bill => bill.Amount).Sum();
            numberOfBills = billsInTransaction.Count;
        }
    }
}
