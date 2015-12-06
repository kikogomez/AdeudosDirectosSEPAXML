using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementAppLogic.MembersManaging;
using RCNGCMembersManagementAppLogic.ClubServices;
using RCNGCMembersManagementAppLogic.ClubStore;
using RCNGCMembersManagementAppLogic.Billing;
using RCNGCMembersManagementMocks;

namespace RCNGCMembersManagementSpecFlowBDD
{
    [Binding, Scope(Feature = "Generating pro forma invoices")]
    class GeneratingProFormaInvoicesFeatureSteps
    {
        private readonly MembersManagementContextData membersManagementContextData;
        private readonly InvoiceContextData invoiceContextData;

        public GeneratingProFormaInvoicesFeatureSteps(
            MembersManagementContextData membersManagementContextData,
            InvoiceContextData invoiceContextData)
        {
            this.membersManagementContextData = membersManagementContextData;
            this.invoiceContextData = invoiceContextData;
        }

        [BeforeScenario]
        public void InitializeTransactionsList()
        {
            List<Transaction> transactionsList= new List<Transaction>();
            ScenarioContext.Current.Add("Transactions_List", transactionsList);
        }

        [Given(@"Last generated pro forma invoice ID is ""(.*)""")]
        public void GivenLastGeneratedProFormaInvoiceIDIs(string lastProFormaInvoiceID)
        {

            invoiceContextData.lastProFormaInvoiceID = lastProFormaInvoiceID;
            BillingSequenceNumbersMock invoiceDataManagerMock = new BillingSequenceNumbersMock();
            invoiceContextData.billDataManager.SetBillingSequenceNumberCollaborator(invoiceDataManagerMock);
            invoiceContextData.billDataManager.ProFormaInvoiceSequenceNumber = uint.Parse(lastProFormaInvoiceID.Substring(7));
        }

        [Given(@"A Club Member")]
        public void GivenAClubMember(Table clientsTable)
        {
            membersManagementContextData.clubMember= new ClubMember(clientsTable.Rows[0]["MemberID"], clientsTable.Rows[0]["Name"], clientsTable.Rows[0]["FirstSurname"], clientsTable.Rows[0]["SecondSurname"]);
        }

        [Given(@"This set of taxes")]
        public void GivenThisSetOfTaxes(Table taxes)
        {
            invoiceContextData.taxesDictionary = new Dictionary<string, Tax>();
            foreach (var row in taxes.Rows)
            {
                string key=row["Tax Type"];
                Tax tax = new Tax((string)row["Tax Type"], double.Parse(row["Tax Value"]));
                invoiceContextData.taxesDictionary.Add(key, tax);
            }
        }

        [Given(@"These services")]
        public void GivenTheseServices(Table services)
        {
            invoiceContextData.servicesDictionary = new Dictionary<string, ClubService>();
            foreach (var row in services.Rows)
            {
                string serviceName = row["Service Name"];
                double defaultCost = double.Parse(row["Default Cost"]);
                string defaultTax = row["Default Tax"];
                ClubService clubService = new ClubService(serviceName, defaultCost, invoiceContextData.taxesDictionary[defaultTax]);
                invoiceContextData.servicesDictionary.Add(serviceName, clubService);    
            }
        }

        [Given(@"These products")]
        public void GivenTheseProducts(Table products)
        {

            invoiceContextData.productsDictionary = new Dictionary<string, Product>();
            foreach (var row in products.Rows)
            {
                string productName = row["Product Name"];
                double defaultCost = double.Parse(row["Default Cost"]);
                string defaultTax = row["Default Tax"];
                Product product = new Product(productName, defaultCost, invoiceContextData.taxesDictionary[defaultTax]);
                invoiceContextData.productsDictionary.Add(productName, product);
            }
        }

        [Given(@"This set of service charge transactions")]
        public void GivenThisSetOfServiceChargeTransactions(Table transactions)
        {
            AddTransactionsToTransactionList((List<Transaction>)ScenarioContext.Current["Transactions_List"], transactions);
        }

        [Given(@"This set of sale transactions")]
        public void GivenThisSetOfSaleTransactions(Table transactions)
        {
            AddTransactionsToTransactionList((List<Transaction>)ScenarioContext.Current["Transactions_List"], transactions);
        }

        [Given(@"I generate a pro forma invoice for this/these transaction/s")]
        public void GivenIGenerateAProFormaInvoiceForThisTheseTransactionS(Table transactions)
        {
            List<Transaction> transactionsList = new List<Transaction>();
            AddTransactionsToTransactionList(transactionsList, transactions);
            ProFormaInvoice proFormaInvoice = new ProFormaInvoice(new InvoiceCustomerData(membersManagementContextData.clubMember), transactionsList, DateTime.Now);
            ScenarioContext.Current.Add("ProFormaInvoice", proFormaInvoice);
        }

        [When(@"I generate a pro forma invoice for this/these transaction/s")]
        public void WhenIGenerateAProFormaInvoiceForThisTheseTransactionS()
        {
            ProFormaInvoice proFormaInvoice = new ProFormaInvoice(new InvoiceCustomerData(membersManagementContextData.clubMember), (List<Transaction>)ScenarioContext.Current["Transactions_List"], DateTime.Now);
            ScenarioContext.Current.Add("ProFormaInvoice", proFormaInvoice);
        }

        [When(@"I change the invoice detail to these values")]
        public void WhenIChangeTheInvoiceDetailToTheseValues(Table transactions)
        {
            List<Transaction> transactionsList = new List<Transaction>();
            AddTransactionsToTransactionList(transactionsList, transactions);
            ((ProFormaInvoice)ScenarioContext.Current["ProFormaInvoice"]).SetNewInvoiceDetail(transactionsList);
        }

        [Then(@"A pro forma invoice is created for the cost of the service: (.*)")]
        public void ThenAProFormaInvoiceIsCreatedForTheCostOfTheService(Decimal cost)
        {
            Assert.AreEqual(cost, ((ProFormaInvoice)ScenarioContext.Current["ProFormaInvoice"]).NetAmount);
        }

        [Then(@"The pro forma invoice is modified reflecting the new value: (.*)")]
        public void ThenTheProFormaInvoiceIsModifiedReflectingTheNewValue(Decimal amount)
        {
            Assert.AreEqual(amount, ((ProFormaInvoice)ScenarioContext.Current["ProFormaInvoice"]).NetAmount);
        }

        [Then(@"No bills are created for a pro forma invoice")]
        public void ThenNoBillsAreCreatedForAProFormaInvoice()
        {
            Assert.AreEqual(0, ((ProFormaInvoice)ScenarioContext.Current["ProFormaInvoice"]).BillsTotalAmountToCollect);
        }

        private void AddTransactionsToTransactionList(List<Transaction> currentTransactionsList, Table newTransactions)
        {
            foreach (var row in newTransactions.Rows)
            {
                Transaction transaction;
                int units = int.Parse(row["Units"]);
                string elementName = row[1];
                string description = row["Description"];
                double unitCost = double.Parse(row["Unit Cost"]);
                Tax tax = invoiceContextData.taxesDictionary[row["Tax"]];
                double discount = double.Parse(row["Discount"]);
                if (newTransactions.Header.Contains("Service Name"))
                {
                    ClubService clubService = invoiceContextData.servicesDictionary[elementName];
                    transaction = new ServiceCharge(clubService, description, units, unitCost, tax, discount);
                }
                else
                {
                    Product product = invoiceContextData.productsDictionary[elementName];
                    transaction = new Sale(product, description, units, unitCost, tax, discount);
                }
                currentTransactionsList.Add(transaction);
            }
        }
    }
}
