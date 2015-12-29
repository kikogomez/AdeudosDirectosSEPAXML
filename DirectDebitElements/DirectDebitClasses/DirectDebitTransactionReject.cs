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
    }
}
