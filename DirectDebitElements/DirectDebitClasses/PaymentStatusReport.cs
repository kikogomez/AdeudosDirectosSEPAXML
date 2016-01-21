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
        List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects;

        public PaymentStatusReport(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime,
            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects)
        {
            InitializeFields(messageID, messageCreationDateTime, rejectAccountChargeDateTime, numberOfTransactions, controlSum, directDebitPaymentInstructionRejects);
            //this.messageID = messageID;
            //this.messageCreationDateTime = messageCreationDateTime;
            //this.rejectAccountChargeDateTime = rejectAccountChargeDateTime;
            //this.numberOfTransactions = directDebitPaymentInstructionRejects.Select(ddPaymentInstructionReject => ddPaymentInstructionReject.NumberOfTransactions).Sum();
            //this.controlSum = directDebitPaymentInstructionRejects.Select(ddPaymentInstructionReject => ddPaymentInstructionReject.ControlSum).Sum();
            //this.directDebitPaymentInstructionRejects = directDebitPaymentInstructionRejects;
            SuscribePaymentsInstructionsRejectsTo_AddedNewTransactionEvent(directDebitPaymentInstructionRejects);
            //foreach (DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject in directDebitPaymentInstructionRejects)
            //{
            //    SuscribeTo_AddedNewTransactionEvent(directDebitPaymentInstructionReject);
            //}
        }

        public PaymentStatusReport(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects)
        {
            InitializeFields(messageID, messageCreationDateTime, rejectAccountChargeDateTime, numberOfTransactions, controlSum, directDebitPaymentInstructionRejects);
            
            //this.messageID = messageID;
            //this.messageCreationDateTime = messageCreationDateTime;
            //this.rejectAccountChargeDateTime = rejectAccountChargeDateTime;
            //this.numberOfTransactions = directDebitPaymentInstructionRejects.Select(ddPaymentInstructionReject => ddPaymentInstructionReject.NumberOfTransactions).Sum();
            //this.controlSum = directDebitPaymentInstructionRejects.Select(ddPaymentInstructionReject => ddPaymentInstructionReject.ControlSum).Sum();
            //this.directDebitPaymentInstructionRejects = directDebitPaymentInstructionRejects;

            try
            {
                CheckNumberOfTransactionsAndAmount(numberOfTransactions, controlSum);
            }
            catch (ArgumentException argumentException)
            {
                throw new TypeInitializationException("PaymentStatusReport", argumentException);
            }

            SuscribePaymentsInstructionsRejectsTo_AddedNewTransactionEvent(directDebitPaymentInstructionRejects);

            //foreach (DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject in directDebitPaymentInstructionRejects)
            //{
            //    SuscribeTo_AddedNewTransactionEvent(directDebitPaymentInstructionReject);
            //}
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

        public List<DirectDebitPaymentInstructionReject> DirectDebitPaymentInstructionRejects
        {
            get { return directDebitPaymentInstructionRejects; }
        }

        public void AddDirectDebitPaymentInstructionReject(DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject)
        {
            directDebitPaymentInstructionRejects.Add(directDebitPaymentInstructionReject);
            numberOfTransactions += directDebitPaymentInstructionReject.NumberOfTransactions;
            controlSum += directDebitPaymentInstructionReject.ControlSum;
        }

        //private void SuscribeTo_AddedNewTransactionEvent(DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject)
        //{
        //    directDebitPaymentInstructionReject.AddedNewDirectDebitTransactionReject += AddDirectDebitTransactionRejectEventHandler;
        //}

        private void AddDirectDebitTransactionRejectEventHandler(Object sender, decimal directDebitTransactionRejectAmount)
        {
            numberOfTransactions++;
            controlSum += directDebitTransactionRejectAmount;
        }

        private void InitializeFields(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects)
        {
            this.messageID = messageID;
            this.messageCreationDateTime = messageCreationDateTime;
            this.rejectAccountChargeDateTime = rejectAccountChargeDateTime;
            this.numberOfTransactions = directDebitPaymentInstructionRejects.Select(ddPaymentInstructionReject => ddPaymentInstructionReject.NumberOfTransactions).Sum();
            this.controlSum = directDebitPaymentInstructionRejects.Select(ddPaymentInstructionReject => ddPaymentInstructionReject.ControlSum).Sum();
            this.directDebitPaymentInstructionRejects = directDebitPaymentInstructionRejects;
        }

        private void SuscribePaymentsInstructionsRejectsTo_AddedNewTransactionEvent(List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects)
        {
            foreach (DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject in directDebitPaymentInstructionRejects)
            {
                directDebitPaymentInstructionReject.ANewDirectDebitTransactionRejectHasBeenAdded += AddDirectDebitTransactionRejectEventHandler;
            }

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
