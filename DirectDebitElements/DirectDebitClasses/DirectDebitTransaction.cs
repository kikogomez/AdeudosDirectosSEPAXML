using System;
using System.Collections.Generic;
using System.Linq;
using ISO20022PaymentInitiations;
using Billing;

namespace DirectDebitElements
{
    public class DirectDebitTransaction
    {
        string internalUniqueInstructionID;
        List<SimplifiedBill> billsInTransaction;
        BankAccount debtorAccount;
        string accountHolderName;
        DateTime mandateSignatureDate;
        int mandateInternalReferenceNumber;
        string mandateID;

        decimal totalAmount;
        int numberOfBills;

        public DirectDebitTransaction(List<SimplifiedBill> billsInTransaction, int mandateInternalReferenceNumber, BankAccount debtorAccount, string accountHolderName, DateTime mandateSignatureDate)
        {
            this.billsInTransaction = billsInTransaction;
            this.mandateInternalReferenceNumber = mandateInternalReferenceNumber;
            this.debtorAccount = debtorAccount;
            this.accountHolderName = accountHolderName;
            this.mandateSignatureDate = mandateSignatureDate;
            UpdateAmountAndNumberOfBills();
        }

        public DirectDebitTransaction(int mandateInternalReferenceNumber, BankAccount debtorAccount, string accountHolderName, DateTime mandateSignatureDate)
        {
            this.mandateInternalReferenceNumber = mandateInternalReferenceNumber;
            this.debtorAccount = debtorAccount;
            this.accountHolderName = accountHolderName;
            this.mandateSignatureDate = mandateSignatureDate;
            billsInTransaction = new List<SimplifiedBill>();
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

        public List<SimplifiedBill> BillsInTransaction
        {
            get { return billsInTransaction; }
        }

        public int MandateInternalReferenceNumber
        {
            get { return mandateInternalReferenceNumber; }
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

        public void AddBill(SimplifiedBill bill)
        {
            this.billsInTransaction.Add(bill);
            UpdateAmountAndNumberOfBills();
        }

        public void GenerateInternalUniqueInstructionID(int sequenceNumber)
        {
            internalUniqueInstructionID = sequenceNumber.ToString("000000");
        }

        public void GenerateAT01MandateID(string creditorBusinessCode)
        {
            string csb19ReferenceNumber = CalculateOldCSB19Code(creditorBusinessCode);
            SEPAAttributes sEPAttributes = new SEPAAttributes();
            mandateID = sEPAttributes.AT01MandateReference(csb19ReferenceNumber);
        }

        private string CalculateOldCSB19Code(string creditorBusinessCode)
        {
            return "0000" + creditorBusinessCode + mandateInternalReferenceNumber.ToString("00000");
        }

        private void UpdateAmountAndNumberOfBills()
        {
            totalAmount = billsInTransaction.Select(bill => bill.Amount).Sum();
            numberOfBills = billsInTransaction.Count;
        }
    }
}
