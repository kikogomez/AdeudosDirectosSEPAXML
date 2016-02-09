using System;
using System.Collections.Generic;

namespace DirectDebitElements
{
    public class DirectDebitMandate
    {
        DirectDebitmandateStatus status;
        int internalReferenceNumber;
        DateTime directDebitMandateCreationDate;
        BankAccount bankAccount;
        string bankBIC;
        string accountHolderName;
        DateTime bankAccountActivationDate;
        Dictionary<DateTime, BankAccountHistoricalData> bankAccountHistory;

        public DirectDebitMandate(int internalReferenceNumber, DateTime directDebitMandateCreationDate, BankAccount bankAccount, string bankBIC, string accountHolderName)
        {
            this.status = DirectDebitmandateStatus.Active;
            this.directDebitMandateCreationDate = directDebitMandateCreationDate;
            this.bankAccount = bankAccount;
            this.bankBIC = bankBIC;
            this.accountHolderName = accountHolderName;
            this.bankAccountActivationDate = directDebitMandateCreationDate;
            bankAccountHistory = new Dictionary<DateTime, BankAccountHistoricalData>();
            this.internalReferenceNumber = internalReferenceNumber;
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

        public string BankBIC
        {
            get { return bankBIC; }
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
    }
}
