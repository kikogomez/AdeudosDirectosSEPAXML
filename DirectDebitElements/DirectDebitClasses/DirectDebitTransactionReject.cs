using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectDebitElements.DirectDebitClasses
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

        public DirectDebitTransactionReject()
        {

        }
    }
}
