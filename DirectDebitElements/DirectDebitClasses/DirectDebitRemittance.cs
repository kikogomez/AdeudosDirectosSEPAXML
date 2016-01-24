using System;
using System.Collections.Generic;
using System.Linq;

namespace DirectDebitElements
{
    public class DirectDebitRemittance
    {
        string messageID;
        DateTime creationDateTime;
        DateTime requestedCollectionDate;
        DirectDebitInitiationContract directDebitInitiationContract;
        int numberOfTransactions;
        decimal controlSum;
        List<DirectDebitPaymentInstruction> directDebitPaymentInstructions;

        public DirectDebitRemittance(
            string messageID,
            DateTime creationDateTime,
            DateTime requestedCollectionDate, 
            DirectDebitInitiationContract directDebitInitiationContract)
        {
            CheckMandatoryFields(messageID, directDebitInitiationContract);

            this.messageID = messageID;
            this.creationDateTime = creationDateTime;
            this.requestedCollectionDate = requestedCollectionDate;
            this.directDebitInitiationContract=directDebitInitiationContract;
            directDebitPaymentInstructions = new List<DirectDebitPaymentInstruction>();
            numberOfTransactions = 0;
            controlSum = 0;
        }

        public DirectDebitRemittance(
            string messageID,
            DateTime creationDateTime,
            DateTime requestedCollectionDate,
            DirectDebitInitiationContract directDebitInitiationContract,
            List<DirectDebitPaymentInstruction> directDebitPaymentInstructions)
            :this(messageID, creationDateTime, requestedCollectionDate, directDebitInitiationContract)
        {
            this.directDebitPaymentInstructions = directDebitPaymentInstructions;
            UpdateNumberOfDirectDebitTransactionsAndAmount();
            //SuscribeTo_ANewDirectDebitTransactionHasBeenAdded_FromAllPaymentsInstructions();
        }

        public DirectDebitRemittance(
            string messageID,
            DateTime creationDateTime,
            DateTime requestedCollectionDate,
            DirectDebitInitiationContract directDebitInitiationContract,
            int numberOfTransactions,
            decimal controlSum,
            List<DirectDebitPaymentInstruction> directDebitPaymentInstructions)
            :this(messageID, creationDateTime, requestedCollectionDate, directDebitInitiationContract, directDebitPaymentInstructions) 
        {
            try
            {
                CheckNumberOfTransactionsAndAmount(numberOfTransactions, controlSum);
            }
            catch (ArgumentException argumentException)
            {
                throw new TypeInitializationException("DirectDebitRemittance", argumentException);
            }
        }

        public string MessageID
        {
            get { return messageID; }
        }

        public DateTime CreationDate
        {
            get { return creationDateTime; }
        }

        public DateTime RequestedCollectionDate
        {
            get { return requestedCollectionDate; }
        }

        public DirectDebitInitiationContract DirectDebitInitiationContract
        {
            get { return directDebitInitiationContract; }
        }

        public List<DirectDebitPaymentInstruction> DirectDebitPaymentInstructions
        {
            get { return directDebitPaymentInstructions; }
        }

        public int NumberOfTransactions
        {
            get { return numberOfTransactions; }
        }

        public decimal ControlSum
        {
            get { return controlSum; }
        }

        public void AddDirectDebitPaymentInstruction(DirectDebitPaymentInstruction directDebitPaymentInstruction)
        {
            directDebitPaymentInstructions.Add(directDebitPaymentInstruction);
            UpdateNumberOfDirectDebitTransactionsAndAmount();
            //Suscribirse al evento ANewDirectDebitTransactionHasBeenAdded
        }

        private void UpdateNumberOfDirectDebitTransactionsAndAmount()
        {
            this.numberOfTransactions = directDebitPaymentInstructions.Select(
                directDebitPaymentInstruction => directDebitPaymentInstruction.NumberOfDirectDebitTransactions).Sum();
            this.controlSum = directDebitPaymentInstructions.Select(
                directDebitPaymentInstruction => directDebitPaymentInstruction.TotalAmount).Sum();
        }

        private void CheckMandatoryFields(string messageID, DirectDebitInitiationContract directDebitInitiationContract)
        {
            if (messageID == null) throw new ArgumentNullException("MessageID", "MessageID can't be null");
            if (messageID.Trim().Length == 0) throw new ArgumentException("MessageID can't be empty", "MessageID");
            if (messageID.Trim().Length > 35) throw new ArgumentOutOfRangeException("MessageID", "MessageID can't be longer than 35 characters");
            if (directDebitInitiationContract == null) throw new ArgumentNullException("DirectDebitInitiationContract", "DirectDebitInitiationContract can't be null");
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

        //private void ANewDirectDebitTransactionHasBeenAddedEventHandler(Object sender, decimal directDebitTransactionAmount)
        //{
        //    numberOfTransactions++;
        //    controlSum += directDebitTransactionAmount;
        //}

        //private void SuscribeTo_ANewDirectDebitTransactionHasBeenAdded_FromAllPaymentsInstructions()
        //{
        //    foreach (DirectDebitPaymentInstruction directDebitPaymentInstruction in directDebitPaymentInstructions)
        //    {
        //        directDebitPaymentInstruction.ANewDirectDebitTransactionHasBeenAdded += ANewDirectDebitTransactionHasBeenAddedEventHandler;
        //    }
        //}
    }
}
