using System.Xml;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace DirectDebitElements
{
    public class PaymentStatusReport
    {
        string messageID;
        DateTime messageCreationDateTime;
        DateTime rejectAccountChargeDateTime;
        int numberOfTransactions;
        decimal controlSum;
        List<DirectDebitTransactionGroupPaymentReject> directDebitRemmitanceRejects;

        public PaymentStatusReport(string messageID, DateTime messageCreationDateTime, DateTime rejectAccountChargeDateTime, List<DirectDebitTransactionGroupPaymentReject> directDebitRemmitanceRejects)
        {
            this.messageID = messageID;
            this.messageCreationDateTime = messageCreationDateTime;
            this.rejectAccountChargeDateTime = rejectAccountChargeDateTime;
            this.numberOfTransactions = directDebitRemmitanceRejects.Select(ddRemmitanceReject => ddRemmitanceReject.NumberOfTransactions).Sum();
            this.controlSum = directDebitRemmitanceRejects.Select(ddRemmitanceReject => ddRemmitanceReject.ControlSum).Sum();
            this.directDebitRemmitanceRejects = directDebitRemmitanceRejects;

            foreach (DirectDebitTransactionGroupPaymentReject directDebitRemmitanceReject in directDebitRemmitanceRejects)
            {
                SuscribeTo_AddedNewTransactionEvent(directDebitRemmitanceReject);
            }
        }

        public PaymentStatusReport(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitTransactionGroupPaymentReject> directDebitRemmitanceRejects)
        {
            this.messageID = messageID;
            this.messageCreationDateTime = messageCreationDateTime;
            this.rejectAccountChargeDateTime = rejectAccountChargeDateTime;
            this.numberOfTransactions = directDebitRemmitanceRejects.Select(ddRemmitanceReject => ddRemmitanceReject.NumberOfTransactions).Sum();
            this.controlSum = directDebitRemmitanceRejects.Select(ddRemmitanceReject => ddRemmitanceReject.ControlSum).Sum();
            this.directDebitRemmitanceRejects = directDebitRemmitanceRejects;

            try
            {
                CheckNumberOfTransactionsAndAmount(numberOfTransactions, controlSum);
            }
            catch (ArgumentException argumentException)
            {
                throw new TypeInitializationException("PaymentStatusReport", argumentException);
            }
            
            foreach (DirectDebitTransactionGroupPaymentReject directDebitRemmitanceReject in directDebitRemmitanceRejects)
            {
                SuscribeTo_AddedNewTransactionEvent(directDebitRemmitanceReject);
            }
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

        public List<DirectDebitTransactionGroupPaymentReject> DirectDebitRemmitanceRejects
        {
            get { return directDebitRemmitanceRejects; }
        }

        public void AddRemmitanceReject(DirectDebitTransactionGroupPaymentReject directDebitRemmitanceReject)
        {
            directDebitRemmitanceRejects.Add(directDebitRemmitanceReject);
            numberOfTransactions += directDebitRemmitanceReject.NumberOfTransactions;
            controlSum += directDebitRemmitanceReject.ControlSum;
        }

        private void SuscribeTo_AddedNewTransactionEvent(DirectDebitTransactionGroupPaymentReject directDebitRemmitanceReject)
        {
            directDebitRemmitanceReject.AddedNewDirectDebitTransactionReject += AddDirectDebitTransactionRejectEventHandler;
        }

        private void AddDirectDebitTransactionRejectEventHandler(Object sender, decimal directDebitTransactionRejectAmount)
        {
            numberOfTransactions++;
            controlSum += directDebitTransactionRejectAmount;
        }

        private void CheckNumberOfTransactionsAndAmount(int numberOfTransactions, decimal controlSum)
        {
            if (this.numberOfTransactions != numberOfTransactions)
            {
                string errorMessage = string.Format("The {0} is wrong. It should be {1}, but {2} is provided", "Number of Transactions", this.numberOfTransactions, numberOfTransactions);
                throw new ArgumentException(errorMessage, "numberOfTransactions");
            }
            if (this.controlSum != controlSum)
            {
                string errorMessage = string.Format("The {0} is wrong. It should be {1}, but {2} is provided", "Control Sum", this.controlSum, controlSum);
                throw new ArgumentException(errorMessage, "controlSum");
            }
        }
    }
}
