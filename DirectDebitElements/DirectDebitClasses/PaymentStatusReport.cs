﻿using System.Xml;
using System.Xml.Linq;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace DirectDebitElements.DirectDebitClasses
{
    public class PaymentStatusReport
    {
        string messageID;
        DateTime messageCreationDateTime;
        DateTime rejectAccountChargeDateTime;   //This info is extracted from the OriginalMessageIdentification <OrignlMsgId>
        int numberOfTransactions;
        decimal controlSum;
        List<DirectDebitRemmitanceReject> directDebitRemmitanceRejects;

        public PaymentStatusReport(string messageID, DateTime messageCreationDateTime, DateTime rejectAccountChargeDateTime, int numberOfTransactions, decimal controlSum, List<DirectDebitRemmitanceReject> directDebitRemmitanceRejects)
        {
            this.messageID = messageID;
            this.messageCreationDateTime = messageCreationDateTime;
            this.rejectAccountChargeDateTime = rejectAccountChargeDateTime;
            this.numberOfTransactions = numberOfTransactions;
            this.controlSum = controlSum;
            this.directDebitRemmitanceRejects = directDebitRemmitanceRejects;
        }

        public string MessageID
        {
            get { return messageID; }
        }

        public DateTime MessageCreationDateTime
        {
            get { return messageCreationDateTime; }
        }

        public DateTime RejectAccountChargeDateTime
        {
            get { return rejectAccountChargeDateTime; }
        }

        public int NumberOfTransactions
        {
            get { return numberOfTransactions; }
        }

        public decimal ControlSum
        {
            get { return controlSum; }
        }

        public List<DirectDebitRemmitanceReject> DirectDebitRemmitanceRejects
        {
            get { return directDebitRemmitanceRejects; }
        }
    }
}
