using System;

namespace RCNGCMembersManagementAppLogic.Billing.DirectDebit
{
    public class BankAccountFields
    {
        string bank;
        string office;
        string checkDigits;
        string accountNumber;

        public BankAccountFields(string bankCode, string officeCode, string checkDigits, string accountNumber)
        {
            try
            {
                CheckBankAccountFieldsLength(bankCode,officeCode,checkDigits,accountNumber);
            }
            catch (ArgumentException e)
            {
                throw e;
            }
                        
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
            return ClientAccountCodeCCC.IsValidCCC(bank, office, checkDigits, accountNumber);
        }


        private void CheckBankAccountFieldsLength(string bank, string office, string checkDigits, string accountNumber)
        {
            try
            {
                ThrowExceptionOnTooLongAccountDataString("banco", bank, FieldLenghts.BankLength);
                ThrowExceptionOnTooLongAccountDataString("sucursal", office, FieldLenghts.OfficeLenght);
                ThrowExceptionOnTooLongAccountDataString("dígito de control", checkDigits, FieldLenghts.CheckDigitsLenght);
                ThrowExceptionOnTooLongAccountDataString("número de cuenta", accountNumber, FieldLenghts.AccountNumberLenght);
            }
            catch (System.ArgumentException e)
            {
                throw e;
            }           
        }
        
        private void ThrowExceptionOnTooLongAccountDataString(string fieldName, string fieldValue, int maxLenght)
        {
             if ((fieldValue ?? "").Length>maxLenght) throw new System.ArgumentException("El código de " + fieldName + " es demasiado largo", fieldName);
        }

        public struct FieldLenghts
        {
            public const int BankLength = 4;
            public const int OfficeLenght = 4;
            public const int CheckDigitsLenght = 2;
            public const int AccountNumberLenght = 10;
            public const int CCCLength = 20;
        }
    }
}
