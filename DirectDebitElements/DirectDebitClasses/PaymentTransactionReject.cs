using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectDebitElements.DirectDebitClasses
{
    public class PaymentTransactionReject
    {
        string originalTransactionIdentification;
        string originalEndtoEndTransactionIdentification;
        DateTime requestedCollectionDate;
        decimal amount;
        string mandateID;
        BankAccount debtorAccount;
        string rejectReason;

        public PaymentTransactionReject(
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
            get
            {
                return originalTransactionIdentification;
            }

            set
            {
                originalTransactionIdentification = value;
            }
        }

        public string OriginalEndtoEndTransactionIdentification
        {
            get
            {
                return originalEndtoEndTransactionIdentification;
            }

            set
            {
                originalEndtoEndTransactionIdentification = value;
            }
        }

        public DateTime RequestedCollectionDate
        {
            get
            {
                return requestedCollectionDate;
            }

            set
            {
                requestedCollectionDate = value;
            }
        }

        public decimal Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
            }
        }

        public string MandateID
        {
            get
            {
                return mandateID;
            }

            set
            {
                mandateID = value;
            }
        }

        public BankAccount DebtorAccount
        {
            get
            {
                return debtorAccount;
            }

            set
            {
                debtorAccount = value;
            }
        }

        public string RejectReason
        {
            get
            {
                return rejectReason;
            }

            set
            {
                rejectReason = value;
            }
        }


    }
}
