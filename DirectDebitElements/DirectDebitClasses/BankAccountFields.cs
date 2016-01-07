using System;
using ReferencesAndTools;

namespace DirectDebitElements
{
    public class BankAccountFields
    {
        string bank;
        string office;
        string checkDigits;
        string accountNumber;

        public BankAccountFields(string bankCode, string officeCode, string checkDigits, string accountNumber)
        {
            BankAccountNumberChecker.CheckBankAccountFieldsLength(bankCode, officeCode, checkDigits, accountNumber);

            this.bank = bankCode;
            this.office = officeCode;
            this.checkDigits = checkDigits;
            this.accountNumber = accountNumber;
        }

        public string BankCode
        {
            get { return bank; }
        }

        public string OfficeCode
        {
            get { return office; }
        }

        public string CheckDigits
        {
            get { return checkDigits; }
        }

        public string AccountNumber
        {
            get { return accountNumber; }
        }

        public bool AreValid()
        {
            return BankAccountNumberChecker.IsValidCCC(bank, office, checkDigits, accountNumber);
        }
    }
}
