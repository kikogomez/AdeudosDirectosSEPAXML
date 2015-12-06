using System;
using System.Collections.Generic;

namespace RCNGCMembersManagementAppLogic.Billing.DirectDebit
{
    public class DirectDebitMandate
    {
        BillingDataManager billingDataManager;

        DirectDebitmandateStatus status;
        int internalReferenceNumber;
        DateTime directDebitMandateCreationDate;
        BankAccount bankAccount;
        string accountHolderName;
        DateTime bankAccountActivationDate;
        Dictionary<DateTime, BankAccountHistoricalData> bankAccountHistory;

        public DirectDebitMandate(int internalReferenceNumber, DateTime directDebitMandateCreationDate, BankAccount bankAccount, string accountHolderName)
        {
            this.billingDataManager = BillingDataManager.Instance;

            this.status = DirectDebitmandateStatus.Active;
            this.directDebitMandateCreationDate = directDebitMandateCreationDate;
            this.bankAccount = bankAccount;
            this.accountHolderName = accountHolderName;
            this.bankAccountActivationDate = directDebitMandateCreationDate;
            bankAccountHistory = new Dictionary<DateTime, BankAccountHistoricalData>();
            SetInternalReferenceNumber(internalReferenceNumber);
        }

        public DirectDebitMandate(DateTime directDebitMandateCreationDate, BankAccount bankAccount, string accountHolderName)
            : this(1, directDebitMandateCreationDate, bankAccount, accountHolderName)
        {
            GetInternalReferenceSequenceNumber();
        }

        public enum DirectDebitmandateStatus { Active, Inactive }

        public DirectDebitmandateStatus Status
        {
            get { return status; }
        }

        public int InternalReferenceNumber
        {
            get { return internalReferenceNumber; }
        }

        public DateTime DirectDebitMandateCreationDate
        {
            get { return directDebitMandateCreationDate; }
        }

        public BankAccount BankAccount
        {
            get { return bankAccount; }
        }

        public string AccountHolderName
        {
            get { return accountHolderName; }
        }

        public DateTime BankAccountActivationDate
        {
            get { return bankAccountActivationDate; }
        }

        public Dictionary<DateTime, BankAccountHistoricalData> BankAccountHistory
        {
            get { return bankAccountHistory; }
        }

        public void ChangeBankAccount(BankAccount bankAccount, DateTime changingDate)
        {
            AddCurrentAccountToHistorical(changingDate);
            this.bankAccount = bankAccount;
            this.bankAccountActivationDate = changingDate;
        }

        private void AddCurrentAccountToHistorical(DateTime changingDate)
        {
            BankAccountHistoricalData oldBankAccount = new BankAccountHistoricalData(this.bankAccount, this.bankAccountActivationDate, changingDate);
            bankAccountHistory.Add(changingDate, oldBankAccount);
        }

        public void DeactivateMandate()
        {
            this.status = DirectDebitmandateStatus.Inactive;
        }

        public void ActivateMandate()
        {
            this.status = DirectDebitmandateStatus.Active;
        }

        private void GetInternalReferenceSequenceNumber()
        {
            internalReferenceNumber = (int)billingDataManager.DirectDebitSequenceNumber;
            billingDataManager.DirectDebitSequenceNumber++;
        }

        private void SetInternalReferenceNumber(int internalReferenceNumber)
        {
            billingDataManager.CheckIfDirectDebitSequenceNumberIsInRange((uint)internalReferenceNumber);
            this.internalReferenceNumber = internalReferenceNumber;
        }
    }
}
