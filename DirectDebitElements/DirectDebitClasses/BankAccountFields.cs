﻿using System;
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
            CheckBankAccountFieldsLength(bankCode, officeCode, checkDigits, accountNumber);

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


        private void CheckBankAccountFieldsLength(string bank, string office, string checkDigits, string accountNumber)
        {
            ThrowExceptionOnTooLongAccountDataString("banco", bank, FieldLenghts.BankLength);
            ThrowExceptionOnTooLongAccountDataString("sucursal", office, FieldLenghts.OfficeLenght);
            ThrowExceptionOnTooLongAccountDataString("dígito de control", checkDigits, FieldLenghts.CheckDigitsLenght);
            ThrowExceptionOnTooLongAccountDataString("número de cuenta", accountNumber, FieldLenghts.AccountNumberLenght);        
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
