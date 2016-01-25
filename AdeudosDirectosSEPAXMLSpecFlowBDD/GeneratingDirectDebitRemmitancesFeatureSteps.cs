using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing;
using DirectDebitElements;
using ReferencesAndTools;
using XMLSerializerValidator;

namespace AdeudosDirectosSEPAXMLSpecFlowBDD
{
    [Binding, Scope(Feature = "Generating Direct Debit Remittances")]
    public class GeneratingDirectDebitRemittancesFeatureSteps
    {
        private readonly DebtorsManagementContextData membersManagementContextData;
        private DirectDebitRemittancesManager directDebitRemittancesManager;

        public GeneratingDirectDebitRemittancesFeatureSteps(
            DebtorsManagementContextData membersManagementContextData)
        {
            this.membersManagementContextData = membersManagementContextData;
            directDebitRemittancesManager = new DirectDebitRemittancesManager();
        }

        [Given(@"My Direct Debit Initiation Contract is")]
        public void GivenMyDirectDebitInitiationContractIs(Table contractTable)
        {
            Creditor creditor = new Creditor(contractTable.Rows[0]["NIF"], contractTable.Rows[0]["Name"]);
            BankCode creditorAgentBankCode = new BankCode(
                contractTable.Rows[0]["LocalBankCode"],
                contractTable.Rows[0]["CreditorAgentName"],
                contractTable.Rows[0]["BIC"]);
            CreditorAgent creditorAgent = new CreditorAgent(creditorAgentBankCode);
            BankAccount creditorAccount = new BankAccount(new InternationalAccountBankNumberIBAN(contractTable.Rows[0]["CreditorAccount"]));
            string creditorBussinessCode = contractTable.Rows[0]["CreditorBussinesCode"];
            DirectDebitInitiationContract directDebitContract = new DirectDebitInitiationContract(
                creditorAccount,
                creditor.NIF,
                contractTable.Rows[0]["CreditorBussinesCode"],
                creditorAgent);
            ScenarioContext.Current.Add("Creditor", creditor);
            ScenarioContext.Current.Add("CreditorAgent", creditorAgent);
            ScenarioContext.Current.Add("DirectDebitInitiationContract", directDebitContract);
        }
        
        [Given(@"These debtors")]
        public void GivenTheseDebtors(Table membersTable)
        {
            Dictionary<string, Debtor> debtorsCollection = new Dictionary<string, Debtor>();
            Dictionary<string, string> bICDictionary = new Dictionary<string, string>();
            foreach (var row in membersTable.Rows)
            {
                BankAccount memberAccount = new BankAccount(new ClientAccountCodeCCC((string)row["Account"]));
                bICDictionary.Add(memberAccount.BankAccountFieldCodes.BankCode, row["BIC"]);
                Debtor debtor = new Debtor(
                    row["DebtorID"],
                    row["Name"],
                    row["FirstSurname"],
                    row["SecondSurname"]);
                DateTime mandateCreationDate = new DateTime(2009, 10, 30);
                int directDebitReferenceNumber = int.Parse(row["Reference"]);
                DirectDebitMandate directDebitMandate = new DirectDebitMandate(
                    directDebitReferenceNumber,
                    mandateCreationDate,
                    memberAccount,
                    debtor.FullName);
                debtor.AddDirectDebitMandate(directDebitMandate);
                debtorsCollection.Add(debtor.DebtorID, debtor);
            }
            ScenarioContext.Current.Add("BICDictionary", bICDictionary);
            ScenarioContext.Current.Add("Debtors", debtorsCollection);
        }
        
        [Given(@"These bills")]
        public void GivenTheseBills(Table billsTable)
        {
            Dictionary<string, Debtor> debtorsCollection = (Dictionary<string, Debtor>)ScenarioContext.Current["Debtors"];
            foreach (var row in billsTable.Rows)
            {
                string billID = row["TransactionConcept"] + "/01";
                string description = row["TransactionConcept"];
                double amount = double.Parse(row["Amount"]);
                SimplifiedBill bill = new SimplifiedBill(billID, description, (decimal) amount, DateTime.Today, DateTime.Today.AddMonths(1));
                Debtor debtor = debtorsCollection[row["DebtorID"]];
                debtor.AddSimplifiedBill(bill);
            }
        }
        
        [Given(@"I have a I have a direct debit initiation contract")]
        public void GivenIHaveAIHaveADirectDebitInitiationContract()
        {
            DirectDebitInitiationContract directDebitInitiationContract = (DirectDebitInitiationContract)ScenarioContext.Current["DirectDebitInitiationContract"];
            Assert.AreEqual("777", directDebitInitiationContract.CreditorBussinessCode);
            Assert.AreEqual("ES26777G12345678", directDebitInitiationContract.CreditorID);
            Assert.AreEqual("ES5621001111301111111111", directDebitInitiationContract.CreditorAcount.IBAN.IBAN);
        }
        
        [When(@"I generate a new direct debit Remittance")]
        public void WhenIGenerateANewDirectDebitRemittance()
        {           
            DateTime creationDate = new DateTime(2013, 11, 11);
            string messageID = "ES26777G12345678" + creationDate.ToString("yyyyMMddHH:mm:ss");
            DateTime requestedCollectionDate = new DateTime(2013, 11, 12);
            DirectDebitInitiationContract directDebitInitiationContract = (DirectDebitInitiationContract)ScenarioContext.Current["DirectDebitInitiationContract"];

            DirectDebitRemittance directDebitRemittance = directDebitRemittancesManager.CreateAnEmptyDirectDebitRemittance(messageID, creationDate, requestedCollectionDate, directDebitInitiationContract);
            ScenarioContext.Current.Add("CreationDate", creationDate);
            ScenarioContext.Current.Add("RequestedCollectionDate", requestedCollectionDate);
            ScenarioContext.Current.Add("MessageID", messageID);
            ScenarioContext.Current.Add("DirectDebitRemittance", directDebitRemittance);
        }
        
        [Then(@"An empty direct debit Remittance is created")]
        public void ThenAnEmptyDirectDebitRemittanceIsCreated()
        {
            string messageID = (string)ScenarioContext.Current["MessageID"];
            DateTime creationDate = (DateTime)ScenarioContext.Current["CreationDate"];
            DateTime requestedCollectionDate = (DateTime)ScenarioContext.Current["RequestedCollectionDate"];
            DirectDebitInitiationContract directDebitInitiationContract = (DirectDebitInitiationContract)ScenarioContext.Current["DirectDebitInitiationContract"];
            DirectDebitRemittance directDebitRemittance = (DirectDebitRemittance)ScenarioContext.Current["DirectDebitRemittance"];
            List<DirectDebitPaymentInstruction> emptyDirectDebitPaymentInstructionsList = new List<DirectDebitPaymentInstruction>();

            Assert.AreEqual(messageID, directDebitRemittance.MessageID);
            Assert.AreEqual(creationDate, directDebitRemittance.CreationDate);
            Assert.AreEqual(requestedCollectionDate, directDebitRemittance.RequestedCollectionDate);
            Assert.AreEqual(directDebitInitiationContract, directDebitRemittance.DirectDebitInitiationContract);
            Assert.AreEqual(0, directDebitRemittance.NumberOfTransactions);
            Assert.AreEqual(0, directDebitRemittance.ControlSum);
            CollectionAssert.AreEqual(emptyDirectDebitPaymentInstructionsList, directDebitRemittance.DirectDebitPaymentInstructions);
        }

        [Given(@"I will send the payments using ""(.*)"" local instrument")]
        public void GivenIHaveAWillSendThePaymentsUsingLocalInstrument(string localInstrument)
        {
            ScenarioContext.Current.Add("LocalInstrument", localInstrument);
        }

        [When(@"I generate an empty group of direct debit payments")]
        public void WhenIGenerateAnEmptyGroupOfDirectDebitPayments()
        {
            string paymentInformationID = "PaymentGroup1";
            ScenarioContext.Current.Add("PaymentInformationID", paymentInformationID);
            string localInstrument = (string)ScenarioContext.Current["LocalInstrument"];
            DirectDebitPaymentInstruction directDebitPaymentInstruction =
                directDebitRemittancesManager.CreateAnEmptyDirectDebitPaymentInstruction(paymentInformationID, localInstrument);
            ScenarioContext.Current.Add("EmptyDirectDebitPaymentInstruction", directDebitPaymentInstruction);
        }

        [Then(@"An empty group of direct debit payments using ""(.*)"" is generated")]
        public void ThenAnEmptyGroupOfDirectDebitPaymentsUsingIsGenerated(string localInstrument)
        {
            string paymentImformationID = (string)ScenarioContext.Current["PaymentInformationID"];
            DirectDebitPaymentInstruction directDebitPaymentInstruction =
                (DirectDebitPaymentInstruction)ScenarioContext.Current["EmptyDirectDebitPaymentInstruction"];
            Assert.AreEqual(paymentImformationID, directDebitPaymentInstruction.PaymentInformationID);
            Assert.AreEqual(localInstrument, directDebitPaymentInstruction.LocalInstrument);
            Assert.AreEqual(0, directDebitPaymentInstruction.NumberOfDirectDebitTransactions);
        }

        [Given(@"I have a debtor")]
        public void GivenIHaveAMember()
        {
            Debtor debtor = ((Dictionary<string, Debtor>)ScenarioContext.Current["Debtors"])["00001"];
            ScenarioContext.Current.Add("Debtor", debtor);
        }

        [Given(@"The debtor has a bill")]
        public void GivenTheMemberHasABill()
        {
            Debtor debtor = (Debtor)ScenarioContext.Current["Debtor"];
            SimplifiedBill bill = debtor.SimplifiedBills.Values.ElementAt(0);
            ScenarioContext.Current.Add("Bill", bill);
        }

        [Given(@"The debtor has a Direct Debit Mandate")]
        public void GivenTheMemberHasADirectDebitMandate()
        {
            Debtor debtor = (Debtor)ScenarioContext.Current["Debtor"];
            DirectDebitMandate directDebitmandate = debtor.DirectDebitmandates.Values.ElementAt(0);
            ScenarioContext.Current.Add("DirectDebitMandate", directDebitmandate);
        }

        [When(@"I generate Direct Debit Transaction")]
        public void WhenIGenerateDirectDebitTransaction()
        {
            string transactionID = "00001";
            DirectDebitInitiationContract directDebitInitiationContract = (DirectDebitInitiationContract)ScenarioContext.Current["DirectDebitInitiationContract"];
            DirectDebitPropietaryCodesGenerator directDebitPropietaryCodesGenerator = new DirectDebitPropietaryCodesGenerator(directDebitInitiationContract);
            DirectDebitMandate directDebitmandate = (DirectDebitMandate)ScenarioContext.Current["DirectDebitMandate"];
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitmandate.InternalReferenceNumber);
            SimplifiedBill bill = (SimplifiedBill)ScenarioContext.Current["Bill"];

            DirectDebitTransaction directDebitTransaction = directDebitRemittancesManager.CreateADirectDebitTransaction(
                transactionID,
                mandateID,
                directDebitmandate,
                new List<SimplifiedBill>() { bill },
                null);
            ScenarioContext.Current.Add("DirectDebitTransaction", directDebitTransaction);
        }

        [Then(@"The direct debit transaction is correctly created")]
        public void ThenTheDirectDebitTransactionIsCorrectlyCreated()
        {
            SimplifiedBill bill = (SimplifiedBill)ScenarioContext.Current["Bill"];
            DirectDebitTransaction directDebitTransaction = (DirectDebitTransaction)ScenarioContext.Current["DirectDebitTransaction"];
            Assert.AreEqual(1, directDebitTransaction.BillsInTransaction.Count);
            Assert.AreEqual(bill.Amount, directDebitTransaction.Amount);
        }

        [Given(@"I have a direct debit with (.*) bill and amount of (.*)")]
        public void GivenIHaveADirectDebitWithBillAndAmountOf(int numberOfBills, decimal amount)
        {
            string transactionID = "00001";
            DirectDebitInitiationContract directDebitInitiationContract = (DirectDebitInitiationContract)ScenarioContext.Current["DirectDebitInitiationContract"];
            DirectDebitPropietaryCodesGenerator directDebitPropietaryCodesGenerator = new DirectDebitPropietaryCodesGenerator(directDebitInitiationContract);
            Debtor debtor = ((Dictionary<string, Debtor>)ScenarioContext.Current["Debtors"])["00002"];
            DirectDebitMandate directDebitmandate = debtor.DirectDebitmandates.Values.ElementAt(0);
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitmandate.InternalReferenceNumber);
            SimplifiedBill bill = new SimplifiedBill("0001", "Recibo 1", amount, DateTime.Today, DateTime.Today.AddMonths(1));
            DirectDebitTransaction directDebitTransaction = directDebitRemittancesManager.CreateADirectDebitTransaction(
                transactionID,
                mandateID,
                directDebitmandate,
                new List<SimplifiedBill>() { bill },
                null);
            Assert.AreEqual(numberOfBills, directDebitTransaction.NumberOfBills);
            Assert.AreEqual(amount, directDebitTransaction.Amount);
            ScenarioContext.Current.Add("DirectDebitTransaction", directDebitTransaction);
        }

        [When(@"I add a new bill with amount of (.*)")]
        public void WhenIAddANewBillWithAmountOf(decimal amount)
        {
            Debtor debtor = ((Dictionary<string, Debtor>)ScenarioContext.Current["Debtors"])["00002"];
            SimplifiedBill bill = new SimplifiedBill("0002", "Recibo 2", amount, DateTime.Today, DateTime.Today.AddMonths(2));
            DirectDebitTransaction directDebitTransaction = (DirectDebitTransaction)ScenarioContext.Current["DirectDebitTransaction"];
            directDebitRemittancesManager.AddBilllToExistingDirectDebitTransaction(directDebitTransaction, bill);
        }

        [Then(@"The direct debit transaction is updated with (.*) bills and amount of (.*)")]
        public void ThenTheDirectDebitTransactionIsUpdatedWithBillsAndAmountOf(int numberOfBills, decimal amount)
        {
            DirectDebitTransaction directDebitTransaction = (DirectDebitTransaction)ScenarioContext.Current["DirectDebitTransaction"];
            Assert.AreEqual(numberOfBills, directDebitTransaction.NumberOfBills);
            Assert.AreEqual(amount, directDebitTransaction.Amount);
        }

        [Given(@"I have an empty group of payments")]
        public void GivenIHaveAnEmptyGroupOfPayments()
        {
            DirectDebitPaymentInstruction directDebitPaymentInstruction =
                directDebitRemittancesManager.CreateAnEmptyDirectDebitPaymentInstruction("PaymentGroup1", "COR1");
            ScenarioContext.Current.Add("DirectDebitPaymentInstruction", directDebitPaymentInstruction);
        }

        [When(@"I add the direct debit transaction to the group of payments")]
        public void WhenIAddTheDirectDebitTransactionToTheGroupOfPayments()
        {
            DirectDebitPaymentInstruction directDebitPaymentInstruction =
                (DirectDebitPaymentInstruction)ScenarioContext.Current["DirectDebitPaymentInstruction"];
            DirectDebitTransaction directDebitTransaction = (DirectDebitTransaction)ScenarioContext.Current["DirectDebitTransaction"];
            directDebitRemittancesManager.AddDirectDebitTransactionToDirectDebitPaymentInstruction(directDebitTransaction, directDebitPaymentInstruction);
        }

        [Then(@"The group of payments is updated with (.*) direct debit and total amount of (.*)")]
        public void ThenTheGroupOfPaymentsIsUpdatedWithDirectDebitAndTotalAmountOf(int numberOfDirectDebitmandates, decimal amount)
        {
            DirectDebitPaymentInstruction directDebitPaymentInstruction =
                (DirectDebitPaymentInstruction)ScenarioContext.Current["DirectDebitPaymentInstruction"];
            Assert.AreEqual(numberOfDirectDebitmandates, directDebitPaymentInstruction.NumberOfDirectDebitTransactions);
            Assert.AreEqual(amount, directDebitPaymentInstruction.TotalAmount);
        }

        [Given(@"I have a group of payments with (.*) direct debit transaction and amount of (.*)")]
        public void GivenIHaveAGroupOfPaymentsWithDirectDebitTransactionAndAmountOf(int numberOfDirectDebitTransactions, decimal amount)
        {
            string transactionID = "00001";
            DirectDebitInitiationContract directDebitInitiationContract = (DirectDebitInitiationContract)ScenarioContext.Current["DirectDebitInitiationContract"];
            DirectDebitPropietaryCodesGenerator directDebitPropietaryCodesGenerator = new DirectDebitPropietaryCodesGenerator(directDebitInitiationContract);
            Debtor debtor = ((Dictionary<string, Debtor>)ScenarioContext.Current["Debtors"])["00001"];
            DirectDebitMandate directDebitmandate = debtor.DirectDebitmandates.Values.ElementAt(0);
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitmandate.InternalReferenceNumber);
            SimplifiedBill bill = new SimplifiedBill("0001", "Recibo 1", amount, DateTime.Today, DateTime.Today.AddMonths(1));
            DirectDebitTransaction directDebitTransaction = directDebitRemittancesManager.CreateADirectDebitTransaction(
                transactionID,
                mandateID,
                directDebitmandate,
                new List<SimplifiedBill>() { bill },
                null);
            DirectDebitPaymentInstruction directDebitPaymentInstruction =
                directDebitRemittancesManager.CreateAnEmptyDirectDebitPaymentInstruction("PaymentGroup1", "COR1");
            directDebitRemittancesManager.AddDirectDebitTransactionToDirectDebitPaymentInstruction(directDebitTransaction, directDebitPaymentInstruction);
            Assert.AreEqual(numberOfDirectDebitTransactions, directDebitPaymentInstruction.NumberOfDirectDebitTransactions);
            Assert.AreEqual(amount, directDebitPaymentInstruction.TotalAmount);
            ScenarioContext.Current.Add("DirectDebitPaymentInstruction", directDebitPaymentInstruction);
        }

        [When(@"I add a new direct debit transaction with amount of (.*)")]
        public void WhenIAddANewDirectDebitTransactionWithAmountOf(decimal amount)
        {
            string transactionID = "00002";
            DirectDebitInitiationContract directDebitInitiationContract = (DirectDebitInitiationContract)ScenarioContext.Current["DirectDebitInitiationContract"];
            DirectDebitPropietaryCodesGenerator directDebitPropietaryCodesGenerator = new DirectDebitPropietaryCodesGenerator(directDebitInitiationContract);
            Debtor debtor = ((Dictionary<string, Debtor>)ScenarioContext.Current["Debtors"])["00002"];
            DirectDebitMandate directDebitmandate = debtor.DirectDebitmandates.Values.ElementAt(0);
            string mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitmandate.InternalReferenceNumber);
            SimplifiedBill bill = new SimplifiedBill("0002", "Recibo 2", amount, DateTime.Today, DateTime.Today.AddMonths(2));
            DirectDebitTransaction directDebitTransaction = directDebitRemittancesManager.CreateADirectDebitTransaction(
                transactionID,
                mandateID,
                directDebitmandate,
                new List<SimplifiedBill>() { bill },
                null);
            Assert.AreEqual(amount, directDebitTransaction.Amount);
            DirectDebitPaymentInstruction directDebitPaymentInstruction =
                (DirectDebitPaymentInstruction)ScenarioContext.Current["DirectDebitPaymentInstruction"];
            directDebitRemittancesManager.AddDirectDebitTransactionToDirectDebitPaymentInstruction(directDebitTransaction, directDebitPaymentInstruction);
        }

        [Given(@"I have an empty direct debit Remittance")]
        public void GivenIHaveAnEmptyDirectDebitRemittance()
        {
            DateTime creationDate = DateTime.Today;
            string messageID = "ES26777G12345678" + creationDate.ToString("yyyyMMddHH:mm:ss");
            DateTime requestedCollectionDate = DateTime.Today.AddDays(3);
            DirectDebitInitiationContract directDebitInitiationContract = (DirectDebitInitiationContract)ScenarioContext.Current["DirectDebitInitiationContract"];
            DirectDebitRemittance directDebitRemittance = directDebitRemittancesManager.CreateAnEmptyDirectDebitRemittance(messageID, creationDate, requestedCollectionDate, directDebitInitiationContract);
            ScenarioContext.Current.Add("DirectDebitRemittance", directDebitRemittance);
        }

        [When(@"I add the group to the direct debit remittance")]
        public void WhenIAddTheGroupToTheDirectDebitRemittance()
        {
            DirectDebitPaymentInstruction directDebitPaymentInstruction =
                (DirectDebitPaymentInstruction)ScenarioContext.Current["DirectDebitPaymentInstruction"];
            DirectDebitRemittance directDebitRemittance = (DirectDebitRemittance)ScenarioContext.Current["DirectDebitRemittance"];
            directDebitRemittancesManager.AddDirectDebitPaymentInstructionToDirectDebitRemittance(directDebitRemittance, directDebitPaymentInstruction);
        }

        [Then(@"The direct debit remittance is updated with (.*) direct debit and total amount of (.*)")]
        public void ThenTheDirectDebitRemittanceIsUpdatedWithDirectDebitAndTotalAmountOf(int numberOfTransactions, decimal controlSum)
        {
            DirectDebitRemittance directDebitRemittance = (DirectDebitRemittance)ScenarioContext.Current["DirectDebitRemittance"];
            Assert.AreEqual(numberOfTransactions, directDebitRemittance.NumberOfTransactions);
            Assert.AreEqual(controlSum, directDebitRemittance.ControlSum);
        }

        [Given(@"I have a prepared Direct Debit Remittance")]
        public void GivenIHaveAPreparedDirectDebitRemittance()
        {
            DateTime creationDate = new DateTime(2013, 11, 11);
            string messageID = "ES26777G12345678" + creationDate.ToString("yyyyMMddHH:mm:ss");
            DateTime requestedCollectionDate = new DateTime(2013, 11, 12);
            DirectDebitInitiationContract directDebitInitiationContract = (DirectDebitInitiationContract)ScenarioContext.Current["DirectDebitInitiationContract"];
            DirectDebitRemittance directDebitRemittance = directDebitRemittancesManager.CreateAnEmptyDirectDebitRemittance(messageID, creationDate, requestedCollectionDate, directDebitInitiationContract);
            DirectDebitPaymentInstruction directDebitPaymentInstruction =
                directDebitRemittancesManager.CreateAnEmptyDirectDebitPaymentInstruction("PaymentGroup1", "COR1");
            DirectDebitPropietaryCodesGenerator directDebitPropietaryCodesGenerator = new DirectDebitPropietaryCodesGenerator(directDebitInitiationContract);

            List<Debtor> debtors = ((Dictionary<string, Debtor>)ScenarioContext.Current["Debtors"]).Values.ToList();
            int transactionsCounter = 0;
            string transactionID;
            string mandateID;
            foreach (Debtor debtor in debtors)
            {
                DirectDebitMandate directDebitmandate = debtor.DirectDebitmandates.Values.ElementAt(0);
                mandateID = directDebitPropietaryCodesGenerator.CalculateMyOldCSB19MandateID(directDebitmandate.InternalReferenceNumber);
                transactionID = "PaymentGroup1" + (transactionsCounter + 1).ToString("00000");
                DirectDebitTransaction directDebitTransaction =
                    directDebitRemittancesManager.CreateAnEmptyDirectDebitTransaction(transactionID, mandateID, directDebitmandate, null);
                transactionsCounter++;
                foreach (SimplifiedBill bill in debtor.SimplifiedBills.Values)
                {
                    directDebitRemittancesManager.AddBilllToExistingDirectDebitTransaction(directDebitTransaction, bill);
                }
                directDebitRemittancesManager.AddDirectDebitTransactionToDirectDebitPaymentInstruction(
                    directDebitTransaction, directDebitPaymentInstruction);
            }
            directDebitRemittancesManager.AddDirectDebitPaymentInstructionToDirectDebitRemittance(
                directDebitRemittance, directDebitPaymentInstruction);
            ScenarioContext.Current.Add("DirectDebitRemittance", directDebitRemittance);
        }

        [When(@"I generate de SEPA ISO(.*) XML CustomerDirectDebitInitiation message")]
        public void WhenIGenerateDeSEPAISOXMLCustomerDirectDebitInitiationMessage(int p0)
        {
            Creditor creditor = (Creditor)ScenarioContext.Current["Creditor"];
            CreditorAgent creditorAgent = (CreditorAgent)ScenarioContext.Current["CreditorAgent"];
            DirectDebitInitiationContract directDebitInitiationContract = (DirectDebitInitiationContract)ScenarioContext.Current["DirectDebitInitiationContract"];
            DirectDebitRemittance directDebitRemittance = (DirectDebitRemittance)ScenarioContext.Current["DirectDebitRemittance"];
            SEPAMessagesManager sEPAMessagesManager = new SEPAMessagesManager();
            DateTime generationTime = directDebitRemittance.CreationDate;
            bool singleUnstructuredConcept = false;
            string xMLString = sEPAMessagesManager.GenerateISO20022CustomerDirectDebitInitiationMessage(
                creditor,
                creditorAgent,
                directDebitRemittance,
                singleUnstructuredConcept);
            ScenarioContext.Current.Add("xMLString", xMLString);

        }

        [Then(@"The message is correctly created")]
        public void ThenTheMessageIsCorrectlyCreated()
        {
            string xMLString = (string)ScenarioContext.Current["xMLString"];
            string xSDFilePath = @"XSDFiles\pain.008.001.02.xsd";
            string validatingErrors = XMLValidator.ValidateXMLStringThroughXSDFile(xMLString, xSDFilePath);
            Assert.AreEqual("", validatingErrors);
        }
    }
}
