using System;
using System.Text.RegularExpressions;
using RCNGCISO20022CustomerDebitInitiation;

namespace RCNGCMembersManagementAppLogic.Billing.DirectDebit
{
    public class SEPAAttributes
    {
        public SEPAAttributes()
        {
        }

        public SequenceType1Code AT21TypeOfPayment_MigrationValue
        {
            get { return SequenceType1Code.RCUR; }
        }

        public DateTime AT25DateOfMandateSigning_MigrationValue
        {
            get { return new DateTime(2009, 10, 31); }
        }

        public string AT01MandateReference(string csb19ReferenceNumber)
        {
            return csb19ReferenceNumber.PadRight(35, ' ');
        }

        public string AT02CreditorIdentifier(string nationalIdentifier, string nIF, string suffix)
        {
            string checkDigits = ISO7064CheckDigits.CalculateSEPACreditIdentifierCheckDigits(nationalIdentifier, nIF);
            return nationalIdentifier + checkDigits + suffix + DiscardNonAlfanumericCharacters(nIF);
        }

        public string AT07IBAN(string nationalIdentifier, string ccc)
        {
            string iBAN = nationalIdentifier + ISO7064CheckDigits.CalculateIBANCheckDigits(nationalIdentifier, ccc) + ccc;
            return iBAN;
        }

        public string AT07IBAN_Spanish(string ccc)
        {
            string spanisIBAN = "ES" + ISO7064CheckDigits.CalculateIBANCheckDigits("ES", ccc) + ccc;
            return spanisIBAN;
        }

        private static string DiscardNonAlfanumericCharacters(string sourceString)
        {
            return Regex.Replace(sourceString, @"[^A-Z0-9]", String.Empty, RegexOptions.IgnoreCase);
        }
    }
}
