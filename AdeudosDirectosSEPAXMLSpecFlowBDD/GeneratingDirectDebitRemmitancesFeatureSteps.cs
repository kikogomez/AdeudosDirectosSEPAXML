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
    [Binding, Scope(Feature = "Generating Direct Debit Remmitances")]
    public class GeneratingDirectDebitRemmitancesFeatureSteps
    {
        private readonly DebtorsManagementContextData membersManagementContextData;
        private DirectDebitRemittancesManager directDebitRemittancesManager;

        public GeneratingDirectDebitRemmitancesFeatureSteps(
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
        
        [When(@"I generate a new direct debit remmitance")]
        public void WhenIGenerateANewDirectDebitRemmitance()
        {
            DateTime creationDate = new DateTime(2013, 11, 11);
            DateTime requestedCollectionDate = new DateTime(2013, 11, 12);
            DirectDebitInitiationContract directDebitInitiationContract = (DirectDebitInitiationContract)ScenarioContext.Current["DirectDebitInitiationContract"];
            DirectDebitRemittance directDebitRemmitance = directDebitRemittancesManager.CreateADirectDebitRemmitance(creationDate, requestedCollectionDate, directDebitInitiationContract);
            ScenarioContext.Current.Add("CreationDate", creationDate);
            ScenarioContext.Current.Add("DirectDebitRemittance", directDebitRemmitance);
        }
        
        [Then(@"An empty direct debit remmitance is created")]
        public void ThenAnEmptyDirectDebitRemmitanceIsCreated()
        {
            DateTime creationDate = (DateTime)ScenarioContext.Current["CreationDate"];
            DirectDebitInitiationContract directDebitInitiationContract = (DirectDebitInitiationContract)ScenarioContext.Current["DirectDebitInitiationContract"];
            DirectDebitRemittance directDebitRemmitance = (DirectDebitRemittance)ScenarioContext.Current["DirectDebitRemittance"];
            Assert.AreEqual(creationDate, directDebitRemmitance.CreationDate);
            Assert.AreEqual(directDebitInitiationContract, directDebitRemmitance.DirectDebitInitiationContract);
        }

        [Given(@"I will send the payments using ""(.*)"" local instrument")]
        public void GivenIHaveAWillSendThePaymentsUsingLocalInstrument(string localInstrument)
        {
            ScenarioContext.Current.Add("LocalInstrument", localInstrument);
        }

        [When(@"I generate an empty group of direct debit payments")]
        public void WhenIGenerateAnEmptyGroupOfDirectDebitPayments()
        {
            string localInstrument = (string)ScenarioContext.Current["LocalInstrument"];
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment =
                directDebitRemittancesManager.CreateANewGroupOfDirectDebitTransactions(localInstrument);
            ScenarioContext.Current.Add("EmptyDirectDebitTransactionsGroupPayment", directDebitTransactionsGroupPayment);
        }

        [Then(@"An empty group of direct debit payments using ""(.*)"" is generated")]
        public void ThenAnEmptyGroupOfDirectDebitPaymentsUsingIsGenerated(string localInstrument)
        {
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment =
                (DirectDebitTransactionsGroupPayment)ScenarioContext.Current["EmptyDirectDebitTransactionsGroupPayment"];
            Assert.AreEqual(localInstrument, directDebitTransactionsGroupPayment.LocalInstrument);
            Assert.AreEqual(0, directDebitTransactionsGroupPayment.NumberOfDirectDebitTransactions);
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
            SimplifiedBill bill = (SimplifiedBill)ScenarioContext.Current["Bill"];
            DirectDebitMandate directDebitmandate = (DirectDebitMandate)ScenarioContext.Current["DirectDebitMandate"];
            DirectDebitTransaction directDebitTransaction = directDebitRemittancesManager.CreateANewDirectDebitTransactionFromAGroupOfBills(
                directDebitmandate,
                new List<SimplifiedBill>() { bill });
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
            Debtor debtor = ((Dictionary<string, Debtor>)ScenarioContext.Current["Debtors"])["00002"];
            DirectDebitMandate directDebitmandate = debtor.DirectDebitmandates.Values.ElementAt(0);
            SimplifiedBill bill = new SimplifiedBill("0001", "Recibo 1", amount, DateTime.Today, DateTime.Today.AddMonths(1));
            DirectDebitTransaction directDebitTransaction = directDebitRemittancesManager.CreateANewDirectDebitTransactionFromAGroupOfBills(
                directDebitmandate,
                new List<SimplifiedBill>() { bill });
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
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment =
                directDebitRemittancesManager.CreateANewGroupOfDirectDebitTransactions("COR1");
            ScenarioContext.Current.Add("DirectDebitTransactionsGroupPayment", directDebitTransactionsGroupPayment);
        }

        [When(@"I add the direct debit transaction to the group of payments")]
        public void WhenIAddTheDirectDebitTransactionToTheGroupOfPayments()
        {
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment =
                (DirectDebitTransactionsGroupPayment)ScenarioContext.Current["DirectDebitTransactionsGroupPayment"];
            DirectDebitTransaction directDebitTransaction = (DirectDebitTransaction)ScenarioContext.Current["DirectDebitTransaction"];
            directDebitRemittancesManager.AddDirectDebitTransactionToGroupPayment(directDebitTransaction, directDebitTransactionsGroupPayment);
        }

        [Then(@"The group of payments is updated with (.*) direct debit and total amount of (.*)")]
        public void ThenTheGroupOfPaymentsIsUpdatedWithDirectDebitAndTotalAmountOf(int numberOfDirectDebitmandates, decimal amount)
        {
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment =
                (DirectDebitTransactionsGroupPayment)ScenarioContext.Current["DirectDebitTransactionsGroupPayment"];
            Assert.AreEqual(numberOfDirectDebitmandates, directDebitTransactionsGroupPayment.NumberOfDirectDebitTransactions);
            Assert.AreEqual(amount, directDebitTransactionsGroupPayment.TotalAmount);
        }

        [Given(@"I have a group of payments with (.*) direct debit transaction and amount of (.*)")]
        public void GivenIHaveAGroupOfPaymentsWithDirectDebitTransactionAndAmountOf(int numberOfDirectDebitTransactions, decimal amount)
        {
            Debtor debtor = ((Dictionary<string, Debtor>)ScenarioContext.Current["Debtors"])["00001"];
            DirectDebitMandate directDebitmandate = debtor.DirectDebitmandates.Values.ElementAt(0);
            SimplifiedBill bill = new SimplifiedBill("0001", "Recibo 1", amount, DateTime.Today, DateTime.Today.AddMonths(1));
            DirectDebitTransaction directDebitTransaction = directDebitRemittancesManager.CreateANewDirectDebitTransactionFromAGroupOfBills(
                directDebitmandate,
                new List<SimplifiedBill>() { bill });
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment =
                directDebitRemittancesManager.CreateANewGroupOfDirectDebitTransactions("COR1");
            directDebitRemittancesManager.AddDirectDebitTransactionToGroupPayment(directDebitTransaction, directDebitTransactionsGroupPayment);
            Assert.AreEqual(numberOfDirectDebitTransactions, directDebitTransactionsGroupPayment.NumberOfDirectDebitTransactions);
            Assert.AreEqual(amount, directDebitTransactionsGroupPayment.TotalAmount);
            ScenarioContext.Current.Add("DirectDebitTransactionsGroupPayment", directDebitTransactionsGroupPayment);
        }

        [When(@"I add a new direct debit transaction with amount of (.*)")]
        public void WhenIAddANewDirectDebitTransactionWithAmountOf(decimal amount)
        {
            Debtor debtor = ((Dictionary<string, Debtor>)ScenarioContext.Current["Debtors"])["00002"];
            DirectDebitMandate directDebitmandate = debtor.DirectDebitmandates.Values.ElementAt(0);
            SimplifiedBill bill = new SimplifiedBill("0002", "Recibo 2", amount, DateTime.Today, DateTime.Today.AddMonths(2));
            DirectDebitTransaction directDebitTransaction = directDebitRemittancesManager.CreateANewDirectDebitTransactionFromAGroupOfBills(
                directDebitmandate,
                new List<SimplifiedBill>() { bill });
            Assert.AreEqual(amount, directDebitTransaction.Amount);
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment =
                (DirectDebitTransactionsGroupPayment)ScenarioContext.Current["DirectDebitTransactionsGroupPayment"];
            directDebitRemittancesManager.AddDirectDebitTransactionToGroupPayment(directDebitTransaction, directDebitTransactionsGroupPayment);
        }

        [Given(@"I have an empty direct debit remmitance")]
        public void GivenIHaveAnEmptyDirectDebitRemmitance()
        {
            DateTime creationDate = DateTime.Today;
            DateTime requestedCollectionDate = DateTime.Today.AddDays(3);
            DirectDebitInitiationContract directDebitInitiationContract = (DirectDebitInitiationContract)ScenarioContext.Current["DirectDebitInitiationContract"];
            DirectDebitRemittance directDebitRemmitance = directDebitRemittancesManager.CreateADirectDebitRemmitance(creationDate, requestedCollectionDate, directDebitInitiationContract);
            ScenarioContext.Current.Add("DirectDebitRemittance", directDebitRemmitance);
        }

        [When(@"I add the group to the direct debit remittance")]
        public void WhenIAddTheGroupToTheDirectDebitRemittance()
        {
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment =
                (DirectDebitTransactionsGroupPayment)ScenarioContext.Current["DirectDebitTransactionsGroupPayment"];
            DirectDebitRemittance directDebitRemmitance = (DirectDebitRemittance)ScenarioContext.Current["DirectDebitRemittance"];
            directDebitRemittancesManager.AddDirectDebitTransactionGroupPaymentToDirectDebitRemittance(directDebitRemmitance, directDebitTransactionsGroupPayment);
        }

        [Then(@"The direct debit remittance is updated with (.*) direct debit and total amount of (.*)")]
        public void ThenTheDirectDebitRemittanceIsUpdatedWithDirectDebitAndTotalAmountOf(int numberOfTransactions, decimal controlSum)
        {
            DirectDebitRemittance directDebitRemmitance = (DirectDebitRemittance)ScenarioContext.Current["DirectDebitRemittance"];
            Assert.AreEqual(numberOfTransactions, directDebitRemmitance.NumberOfTransactions);
            Assert.AreEqual(controlSum, directDebitRemmitance.ControlSum);
        }

        [Given(@"I have a prepared Direct Debit Remmitance")]
        public void GivenIHaveAPreparedDirectDebitRemmitance()
        {
            DateTime creationDate = new DateTime(2013, 11, 11);
            DateTime requestedCollectionDate = new DateTime(2013, 11, 12);
            DirectDebitInitiationContract directDebitInitiationContract = (DirectDebitInitiationContract)ScenarioContext.Current["DirectDebitInitiationContract"];
            DirectDebitRemittance directDebitRemmitance = directDebitRemittancesManager.CreateADirectDebitRemmitance(creationDate, requestedCollectionDate, directDebitInitiationContract);
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment =
                directDebitRemittancesManager.CreateANewGroupOfDirectDebitTransactions("COR1");
            
            List<Debtor> debtors = ((Dictionary<string, Debtor>)ScenarioContext.Current["Debtors"]).Values.ToList();
            int transactionsCounter = 0;
            foreach (Debtor debtor in debtors)
            {
                DirectDebitMandate directDebitmandate = debtor.DirectDebitmandates.Values.ElementAt(0);
                DirectDebitTransaction directDebitTransaction = directDebitRemittancesManager.CreateANewEmptyDirectDebitTransaction(directDebitmandate);
                transactionsCounter++;
                directDebitTransaction.GenerateDirectDebitTransactionInternalReference(transactionsCounter);
                directDebitTransaction.GenerateMandateID(directDebitInitiationContract.CreditorBussinessCode);
                foreach (SimplifiedBill bill in debtor.SimplifiedBills.Values)
                {
                    directDebitRemittancesManager.AddBilllToExistingDirectDebitTransaction(directDebitTransaction, bill);
                }
                directDebitRemittancesManager.AddDirectDebitTransactionToGroupPayment(
                    directDebitTransaction, directDebitTransactionsGroupPayment);
            }
            directDebitRemittancesManager.AddDirectDebitTransactionGroupPaymentToDirectDebitRemittance(
                directDebitRemmitance, directDebitTransactionsGroupPayment);
            directDebitTransactionsGroupPayment.GeneratePaymentInformationID(1);
            ScenarioContext.Current.Add("DirectDebitRemittance", directDebitRemmitance);
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
            string xMLString = sEPAMessagesManager.GenerateISO20022CustomerDirectDebitInitiationMessage(
                generationTime, creditor, creditorAgent, directDebitInitiationContract, directDebitRemittance);
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
