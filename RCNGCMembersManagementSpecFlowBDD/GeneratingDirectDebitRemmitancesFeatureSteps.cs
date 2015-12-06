using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementAppLogic;
using RCNGCMembersManagementAppLogic.MembersManaging;
using RCNGCMembersManagementAppLogic.Billing;
using RCNGCMembersManagementAppLogic.Billing.DirectDebit;
using RCNGCMembersManagementMocks;
using RCNGCMembersManagementAppLogic.XML;

namespace RCNGCMembersManagementSpecFlowBDD
{
    [Binding, Scope(Feature = "Generating Direct Debit Remmitances")]
    public class GeneratingDirectDebitRemmitancesFeatureSteps
    {
        private readonly MembersManagementContextData membersManagementContextData;
        private readonly InvoiceContextData invoiceContextData;
        private InvoicesManager invoicesManager;
        private DirectDebitRemittancesManager directDebitRemittancesManager;

        public GeneratingDirectDebitRemmitancesFeatureSteps(
            MembersManagementContextData membersManagementContextData,
            InvoiceContextData invoiceContextData)
        {
            this.membersManagementContextData = membersManagementContextData;
            this.invoiceContextData = invoiceContextData;
            invoicesManager = new InvoicesManager();
            directDebitRemittancesManager = new DirectDebitRemittancesManager();
        }

        [BeforeScenario]
        public void InitializeSequenceNumbersManagers()
        {
            BillingSequenceNumbersMock billingSequenceNumbersMock = new BillingSequenceNumbersMock();
            invoiceContextData.billDataManager.SetBillingSequenceNumberCollaborator(billingSequenceNumbersMock);
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
        
        [Given(@"These Club Members")]
        public void GivenTheseClubMembers(Table membersTable)
        {
            Dictionary<string, ClubMember> membersCollection = new Dictionary<string, ClubMember>();
            Dictionary<string, string> bICDictionary = new Dictionary<string, string>();
            foreach (var row in membersTable.Rows)
            {
                BankAccount memberAccount = new BankAccount(new ClientAccountCodeCCC((string)row["Account"]));
                bICDictionary.Add(memberAccount.BankAccountFieldCodes.BankCode, row["BIC"]);
                ClubMember clubMember = new ClubMember(
                    row["MemberID"],
                    row["Name"],
                    row["FirstSurname"],
                    row["SecondSurname"]);
                DateTime mandateCreationDate = new DateTime(2009, 10, 30);
                int directDebitReferenceNumber = int.Parse(row["Reference"]);
                DirectDebitMandate directDebitMandate = new DirectDebitMandate(
                    directDebitReferenceNumber,
                    mandateCreationDate,
                    memberAccount,
                    clubMember.FullName);
                clubMember.AddDirectDebitMandate(directDebitMandate);
                membersCollection.Add(clubMember.MemberID, clubMember);
            }
            ScenarioContext.Current.Add("BICDictionary", bICDictionary);
            ScenarioContext.Current.Add("Members", membersCollection);
        }
        
        [Given(@"These bills")]
        public void GivenTheseBills(Table billsTable)
        {
            invoiceContextData.billDataManager.InvoiceSequenceNumber = 5000;
            Dictionary<string, ClubMember> membersCollection = (Dictionary<string, ClubMember>)ScenarioContext.Current["Members"];
            foreach (var row in billsTable.Rows)
            {
                string description = row["TransactionConcept"];
                double amount = double.Parse(row["Amount"]);
                List<Transaction> transaction = new List<Transaction>()
                {
                    new Transaction(description,1,amount,new Tax("NoTAX",0),0)
                };
                ClubMember clubMember = membersCollection[row["MemberID"]];
                InvoiceCustomerData invoiceCustomerData = new InvoiceCustomerData(clubMember);
                Invoice invoice = new Invoice(invoiceCustomerData, transaction, new DateTime(2013, 11, 11));
                invoicesManager.AddInvoiceToClubMember(invoice, clubMember);
            }
        }
        
        [Given(@"I have a I have a direct debit initiation contract")]
        public void GivenIHaveAIHaveADirectDebitInitiationContract()
        {
            DirectDebitInitiationContract directDebitInitiationContract = (DirectDebitInitiationContract)ScenarioContext.Current["DirectDebitInitiationContract"];
            Assert.AreEqual("777", directDebitInitiationContract.CreditorBussinessCode);
            Assert.AreEqual("ES90777G35008770", directDebitInitiationContract.CreditorID);
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

        [Given(@"I have a will send the payments using ""(.*)"" local instrument")]
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
            ScenarioContext.Current.Add("DirectDebitTransactionsGroupPayment", directDebitTransactionsGroupPayment);
        }

        [Then(@"An empty group of direct debit payments using ""(.*)"" is generated")]
        public void ThenAnEmptyGroupOfDirectDebitPaymentsUsingIsGenerated(string localInstrument)
        {
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment =
                (DirectDebitTransactionsGroupPayment)ScenarioContext.Current["DirectDebitTransactionsGroupPayment"];
            Assert.AreEqual(localInstrument, directDebitTransactionsGroupPayment.LocalInstrument);
            Assert.AreEqual(0, directDebitTransactionsGroupPayment.NumberOfDirectDebitTransactions);
        }

        [Given(@"I have a member")]
        public void GivenIHaveAMember()
        {
            ClubMember clubMember = ((Dictionary<string, ClubMember>)ScenarioContext.Current["Members"])["00001"];
            ScenarioContext.Current.Add("Member", clubMember);
        }

        [Given(@"The member has a bill")]
        public void GivenTheMemberHasABill()
        {
            ClubMember clubMember = (ClubMember)ScenarioContext.Current["Member"];
            Invoice invoice = clubMember.InvoicesList.Values.ElementAt(0);
            Bill bill = invoice.Bills.Values.ElementAt(0);
            ScenarioContext.Current.Add("Bill", bill);
        }

        [Given(@"The member has a Direct Debit Mandate")]
        public void GivenTheMemberHasADirectDebitMandate()
        {
            ClubMember clubMember = (ClubMember)ScenarioContext.Current["Member"];
            DirectDebitMandate directDebitmandate = clubMember.DirectDebitmandates.Values.ElementAt(0);
            ScenarioContext.Current.Add("DirectDebitMandate", directDebitmandate);
        }

        [When(@"I generate Direct Debit Transaction")]
        public void WhenIGenerateDirectDebitTransaction()
        {
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            DirectDebitMandate directDebitmandate = (DirectDebitMandate)ScenarioContext.Current["DirectDebitMandate"];
            DirectDebitTransaction directDebitTransaction = directDebitRemittancesManager.CreateANewDirectDebitTransactionFromAGroupOfBills(
                directDebitmandate,
                new List<Bill>() { bill });
            ScenarioContext.Current.Add("DirectDebitTransaction", directDebitTransaction);
        }

        [Then(@"The direct debit transaction is correctly created")]
        public void ThenTheDirectDebitTransactionIsCorrectlyCreated()
        {
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            DirectDebitTransaction directDebitTransaction = (DirectDebitTransaction)ScenarioContext.Current["DirectDebitTransaction"];
            Assert.AreEqual(1, directDebitTransaction.BillsInTransaction.Count);
            Assert.AreEqual(bill.Amount, directDebitTransaction.Amount);
        }

        [Given(@"I have a direct debit with (.*) bill and amount of (.*)")]
        public void GivenIHaveADirectDebitWithBillAndAmountOf(int numberOfBills, decimal amount)
        {
            ClubMember clubMember = ((Dictionary<string, ClubMember>)ScenarioContext.Current["Members"])["00002"];
            DirectDebitMandate directDebitmandate = clubMember.DirectDebitmandates.Values.ElementAt(0);
            Invoice invoice = clubMember.InvoicesList.Values.ElementAt(0);
            Bill bill = invoice.Bills.Values.ElementAt(0);
            DirectDebitTransaction directDebitTransaction = directDebitRemittancesManager.CreateANewDirectDebitTransactionFromAGroupOfBills(
                directDebitmandate,
                new List<Bill>() { bill });
            Assert.AreEqual(numberOfBills, directDebitTransaction.NumberOfBills);
            Assert.AreEqual(amount, directDebitTransaction.Amount);
            ScenarioContext.Current.Add("DirectDebitTransaction", directDebitTransaction);
        }

        [When(@"I add a new bill with amount of (.*)")]
        public void WhenIAddANewBillWithAmountOf(decimal amount)
        {
            ClubMember clubMember = ((Dictionary<string, ClubMember>)ScenarioContext.Current["Members"])["00002"];
            Invoice invoice = clubMember.InvoicesList.Values.ElementAt(1);
            Bill bill = invoice.Bills.Values.ElementAt(0);
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
            ClubMember clubMember = ((Dictionary<string, ClubMember>)ScenarioContext.Current["Members"])["00001"];
            DirectDebitMandate directDebitmandate = clubMember.DirectDebitmandates.Values.ElementAt(0);
            Invoice invoice = clubMember.InvoicesList.Values.ElementAt(0);
            Bill bill = invoice.Bills.Values.ElementAt(0);
            DirectDebitTransaction directDebitTransaction = directDebitRemittancesManager.CreateANewDirectDebitTransactionFromAGroupOfBills(
                directDebitmandate,
                new List<Bill>() { bill });
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
            ClubMember clubMember = ((Dictionary<string, ClubMember>)ScenarioContext.Current["Members"])["00002"];
            DirectDebitMandate directDebitmandate = clubMember.DirectDebitmandates.Values.ElementAt(0);
            Invoice invoice = clubMember.InvoicesList.Values.ElementAt(0);
            Bill bill = invoice.Bills.Values.ElementAt(0);
            DirectDebitTransaction directDebitTransaction = directDebitRemittancesManager.CreateANewDirectDebitTransactionFromAGroupOfBills(
                directDebitmandate,
                new List<Bill>() { bill });
            Assert.AreEqual(amount, directDebitTransaction.Amount);
            DirectDebitTransactionsGroupPayment directDebitTransactionsGroupPayment =
                (DirectDebitTransactionsGroupPayment)ScenarioContext.Current["DirectDebitTransactionsGroupPayment"];
            directDebitRemittancesManager.AddDirectDebitTransactionToGroupPayment(directDebitTransaction, directDebitTransactionsGroupPayment);
        }

        [Given(@"I have an empty direct debit remmitance")]
        public void GivenIHaveAnEmptyDirectDebitRemmitance()
        {
            DateTime creationDate = new DateTime(2013, 11, 11);
            DateTime requestedCollectionDate = new DateTime(2013, 11, 12);
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
            
            List<ClubMember> clubMembers = ((Dictionary<string, ClubMember>)ScenarioContext.Current["Members"]).Values.ToList();
            int transactionsCounter = 0;
            foreach (ClubMember clubMember in clubMembers)
            {
                DirectDebitMandate directDebitmandate = clubMember.DirectDebitmandates.Values.ElementAt(0);
                DirectDebitTransaction directDebitTransaction = directDebitRemittancesManager.CreateANewEmptyDirectDebitTransaction(directDebitmandate);
                transactionsCounter++;
                directDebitTransaction.GenerateDirectDebitTransactionInternalReference(transactionsCounter);
                directDebitTransaction.GenerateMandateID(directDebitInitiationContract.CreditorBussinessCode);
                foreach (Invoice invoice in clubMember.InvoicesList.Values)
                {
                    Bill bill = invoice.Bills.Values.ElementAt(0);
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
