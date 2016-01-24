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
            DateTime rejectAccountChargeDateTime)
        {
            this.messageID = messageID;
            this.messageCreationDateTime = messageCreationDateTime;
            this.rejectAccountChargeDateTime = rejectAccountChargeDateTime;
            directDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>();
            numberOfTransactions = 0;
            controlSum = 0;
        }

        public PaymentStatusReport(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime,
            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects)
            :this(messageID, messageCreationDateTime, rejectAccountChargeDateTime)
        {
            this.directDebitPaymentInstructionRejects = directDebitPaymentInstructionRejects;
            UpdateNumberOfDirectDebitTransactionRejectsAndAmount();
            SuscribeTo_ANewDirectDebitTransactionRejectHasBeenAdded_FromAllPaymentsInstructionsRejects(directDebitPaymentInstructionRejects);
        }

        public PaymentStatusReport(
            string messageID,
            DateTime messageCreationDateTime,
            DateTime rejectAccountChargeDateTime,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects)
            :this(messageID, messageCreationDateTime, rejectAccountChargeDateTime, directDebitPaymentInstructionRejects)
        {
            try
            {
                CheckNumberOfTransactionsAndAmount(numberOfTransactions, controlSum);
            }
            catch (ArgumentException argumentException)
            {
                throw new TypeInitializationException("PaymentStatusReport", argumentException);
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

        public List<DirectDebitPaymentInstructionReject> DirectDebitPaymentInstructionRejects
        {
            get { return directDebitPaymentInstructionRejects; }
        }

        public void AddDirectDebitPaymentInstructionReject(DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject)
        {
            directDebitPaymentInstructionRejects.Add(directDebitPaymentInstructionReject);
            numberOfTransactions += directDebitPaymentInstructionReject.NumberOfTransactions;
            controlSum += directDebitPaymentInstructionReject.ControlSum;
            //Suscribirse al evento ANewDirectDebitTransactionRejectHasBeenAdded
        }

        private void UpdateNumberOfDirectDebitTransactionRejectsAndAmount()
        {
            this.numberOfTransactions = directDebitPaymentInstructionRejects.Select(
                directDebitPaymentInstructionReject => directDebitPaymentInstructionReject.NumberOfTransactions).Sum();
            this.controlSum = directDebitPaymentInstructionRejects.Select(
                directDebitPaymentInstructionReject => directDebitPaymentInstructionReject.ControlSum).Sum();
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

        private void ANewDirectDebitTransactionRejectHasBeenAddedEventHandler(Object sender, decimal directDebitTransactionRejectAmount)
        {
            numberOfTransactions++;
            controlSum += directDebitTransactionRejectAmount;
        }

        private void SuscribeTo_ANewDirectDebitTransactionRejectHasBeenAdded_FromAllPaymentsInstructionsRejects(List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects)
        {
            foreach (DirectDebitPaymentInstructionReject directDebitPaymentInstructionReject in directDebitPaymentInstructionRejects)
            {
                directDebitPaymentInstructionReject.ANewDirectDebitTransactionRejectHasBeenAdded += ANewDirectDebitTransactionRejectHasBeenAddedEventHandler;
            }
        }
    }
}
