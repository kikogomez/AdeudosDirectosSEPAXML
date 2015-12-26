using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectDebitElements.DirectDebitClasses
{
    public class PaymentRemmitanceReject
    {
        string originalDirectDebitRemmitanceMessageID;
        int numberOfTransactions;
        decimal controlSum;
        List<PaymentTransactionReject> paymentTransactionRejects;

        public PaymentRemmitanceReject()
        {
        }
    }
}
