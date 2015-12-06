using System;

namespace RCNGCMembersManagementUnitTests.DirectDebitPOCOClasses
{
    public class DirectDebitTransactionInfo_POCOTestClass
    {
        string debtorName;
        string txInternalId;
        string[] remitanceInformation;
        double amount;
        string mandateID;
        string iBAN;
        DateTime mandateSignatureDate;
        string previousMandateID;
        string previuosIBAN;

        public DirectDebitTransactionInfo_POCOTestClass(
            string debtorName,
            string txInternalId,
            string[] remitanceInformation,
            double amount,
            string mandateID,
            string iBAN,
            DateTime mandateSignatureDate,
            string previousMandateID,
            string previuosIBAN)
        {
            this.debtorName = debtorName;
            this.txInternalId = txInternalId;
            this.remitanceInformation=(string[])remitanceInformation.Clone();
            this.amount = amount;
            this.mandateID = mandateID;
            this.iBAN = iBAN;
            this.mandateSignatureDate = mandateSignatureDate;
            this.previousMandateID = previousMandateID;
            this.previuosIBAN = previuosIBAN;
        }

        public string DebtorName
        {
            get { return debtorName; }
        }

        public string TxInternalId
        {
            get { return txInternalId; }
        }

        public string[] RemitanceInformation
        {
            get { return remitanceInformation; }
        }

        public double Amount
        {
            get { return amount; }
        }

        public string MandateID
        {
            get { return mandateID; }
        }

        public string IBAN
        {
            get { return iBAN; }
        }

        public DateTime MandateSignatureDate
        {
            get { return mandateSignatureDate; }
        }

        public string PreviousMandateID
        {
            get { return previousMandateID; }
        }

        public string PreviuosIBAN
        {
            get { return previuosIBAN; }
        }


    }
}
