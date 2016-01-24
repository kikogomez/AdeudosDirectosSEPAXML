using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectDebitElements
{
    public class DirectDebitTransactionReject
    {
        string originalTransactionIdentification;
        string originalEndtoEndTransactionIdentification;
        DateTime requestedCollectionDate;
        decimal amount;
        string mandateID;
        BankAccount debtorAccount;
        string rejectReason;

        public DirectDebitTransactionReject(
            string originalTransactionIdentification,
            string originalEndtoEndTransactionIdentification,
            DateTime requestedCollectionDate,
            decimal amount,
            string mandateID,
            BankAccount debtorAccount,
            string rejectReason)
        {
            CheckMandatoryFields(originalEndtoEndTransactionIdentification);
            this.originalTransactionIdentification = originalTransactionIdentification;
            this.originalEndtoEndTransactionIdentification = originalEndtoEndTransactionIdentification;
            this.requestedCollectionDate = requestedCollectionDate;
            this.amount = amount;
            this.mandateID = mandateID;
            this.debtorAccount = debtorAccount;
            this.rejectReason = rejectReason;
        }

        public string OriginalTransactionIdentification
        {
            get { return originalTransactionIdentification; }
        }

        public string OriginalEndtoEndTransactionIdentification
        {
            get { return originalEndtoEndTransactionIdentification; }
        }

        public DateTime RequestedCollectionDate
        {
            get { return requestedCollectionDate; }
        }

        public decimal Amount
        {
            get { return amount; }
        }

        public string MandateID
        {
            get { return mandateID; }
        }

        public BankAccount DebtorAccount
        {
            get { return debtorAccount; }
        }

        public string RejectReason
        {
            get { return rejectReason; }
        }

        private void CheckMandatoryFields(string originalEndtoEndTransactionIdentification) //, string mandateID, BankAccount debtorAccount)
        {
            if (originalEndtoEndTransactionIdentification == null) throw new ArgumentNullException("originalEndtoEndTransactionIdentification", "OriginalEndtoEndTransactionIdentification can't be null");
            if (originalEndtoEndTransactionIdentification.Trim().Length == 0) throw new ArgumentException("OriginalEndtoEndTransactionIdentification can't be empty", "originalEndtoEndTransactionIdentification");
            if (originalEndtoEndTransactionIdentification.Trim().Length > 35) throw new ArgumentOutOfRangeException("originalEndtoEndTransactionIdentification", "OriginalEndtoEndTransactionIdentification can't be longer than 35 characters");
            //if (mandateID == null) throw new ArgumentNullException("MandateID", "MandateID can't be null");
            //if (mandateID.Trim().Length == 0) throw new ArgumentException("MandateID can't be empty", "MandateID");
            //if (mandateID.Trim().Length > 35) throw new ArgumentOutOfRangeException("MandateID", "MandateID can't be longer than 35 characters");
            //if (debtorAccount == null) throw new ArgumentNullException("DebtorAccount", "DebtorAccount can't be null");
            //if (!debtorAccount.HasValidIBAN) throw new ArgumentException("DebtorAccount", "DebtorAccount must be a valid IBAN");
        }
    }
}
