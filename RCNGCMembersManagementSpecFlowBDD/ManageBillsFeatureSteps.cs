using System;
using System.Linq;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementAppLogic;
using RCNGCMembersManagementAppLogic.MembersManaging;
using RCNGCMembersManagementAppLogic.ClubServices;
using RCNGCMembersManagementAppLogic.ClubStore;
using RCNGCMembersManagementAppLogic.Billing;
using RCNGCMembersManagementAppLogic.Billing.DirectDebit;
using RCNGCMembersManagementMocks;

namespace RCNGCMembersManagementSpecFlowBDD
{
    [Binding, Scope(Feature = "Manage bills")]
    public class ManageBillsFeatureSteps
    {
        private readonly MembersManagementContextData membersManagementContextData;
        private readonly InvoiceContextData invoiceContextData;
        InvoicesManager invoicesManager;
        BillsManager billsManager;

        public ManageBillsFeatureSteps(
            MembersManagementContextData membersManagementContextData,
            InvoiceContextData invoiceContextData)
        {
            this.membersManagementContextData = membersManagementContextData;
            this.invoiceContextData = invoiceContextData;
            invoicesManager = new InvoicesManager();
            billsManager = new BillsManager();
        }

        [Given(@"Last generated InvoiceID is ""(.*)""")]
        public void GivenLastGeneratedInvoiceIDIs(string lastInvoiceID)
        {
            invoiceContextData.lastInvoiceID = lastInvoiceID;
            BillingSequenceNumbersMock invoiceDataManagerMock = new BillingSequenceNumbersMock();
            invoiceContextData.billDataManager.SetBillingSequenceNumberCollaborator(invoiceDataManagerMock);
            invoiceContextData.billDataManager.InvoiceSequenceNumber = uint.Parse(lastInvoiceID.Substring(7));
        }

        [Given(@"A Club Member with a default Payment method")]
        public void GivenAClubMemberWithADefaultPaymentMethod(Table clientsTable)
        {
            membersManagementContextData.clubMember = new ClubMember(clientsTable.Rows[0]["MemberID"], clientsTable.Rows[0]["Name"], clientsTable.Rows[0]["FirstSurname"], clientsTable.Rows[0]["SecondSurname"]);
            string electronicIBANString = clientsTable.Rows[0]["Spanish IBAN Bank Account"].Replace(" ","").Substring(4);
            InternationalAccountBankNumberIBAN iban = new InternationalAccountBankNumberIBAN(electronicIBANString);
            BankAccount bankAccount = new BankAccount(iban);
            string debtorName = membersManagementContextData.clubMember.FullName;
            DirectDebitMandate directDebitmandate = new DirectDebitMandate(2345, DateTime.Now.Date, bankAccount, debtorName);
            PaymentMethod paymentMethod = new DirectDebitPaymentMethod(directDebitmandate, null);
            membersManagementContextData.clubMember.AddDirectDebitMandate(directDebitmandate);
            membersManagementContextData.clubMember.SetDefaultPaymentMethod(paymentMethod);
        }

        [Given(@"This set of taxes")]
        public void GivenThisSetOfTaxes(Table taxes)
        {
            invoiceContextData.taxesDictionary = new Dictionary<string, Tax>();
            foreach (var row in taxes.Rows)
            {
                string key = row["Tax Type"];
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

        [Given(@"The member uses the club service ""(.*)""")]
        public void GivenTheMemberUsesTheClubService(string serviceName)
        {
            ClubService clubService = invoiceContextData.servicesDictionary[serviceName];
            ScenarioContext.Current.Add("A_Club_Service", clubService);
        }

        [When(@"I generate an invoice for the service")]
        public void WhenIGenerateAnInvoiceForTheService()
        {
            DateTime issueDate = DateTime.Now;
            List<Transaction> serviceChargeList = new List<Transaction> { new ServiceCharge((ClubService)ScenarioContext.Current["A_Club_Service"]) };
            Invoice invoice = new Invoice(new InvoiceCustomerData(membersManagementContextData.clubMember), serviceChargeList, issueDate);
            invoicesManager.AddInvoiceToClubMember(invoice, membersManagementContextData.clubMember);
            ScenarioContext.Current.Add("Invoice", invoice);
        }

        [Then(@"An invoice is created for the cost of the service: (.*)")]
        public void ThenAnInvoiceIsCreatedForTheCostOfTheService(decimal cost)
        {
            Assert.AreEqual(cost, ((Invoice)ScenarioContext.Current["Invoice"]).NetAmount);
        }

        [Then(@"A single bill To Collect is generated for the total amount of the invoice: (.*)")]
        public void ThenASingleBillToCollectIsGeneratedForTheTotalAmountOfTheInvoice(decimal totalAmount)
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            Assert.AreEqual(1, invoice.Bills.Count);
            ScenarioContext.Current.Add("UniqueBillID", invoice.Bills.ElementAt(0).Key);
            Assert.AreEqual(totalAmount, ((Invoice)ScenarioContext.Current["Invoice"]).BillsTotalAmountToCollect);
        }

        [Then(@"The bill ID is ""(.*)""")]
        public void ThenTheBillIDIs(string billID)
        {
            Assert.AreEqual(billID, ScenarioContext.Current["UniqueBillID"].ToString());
        }

        [Then(@"By default no payment method is associated to bill")]
        public void ThenByDefaultNoPaymentMethodIsAssociatedToBill()
        {
            Assert.IsNull(((Invoice)ScenarioContext.Current["Invoice"]).Bills.Values.ElementAt(0).AssignedPaymentMethod);
        }


        [When(@"I generate an pro-forma invoice for the service")]
        public void WhenIGenerateAnPro_FormaInvoiceForTheService()
        {
            DateTime issueDate = DateTime.Now;
            List<Transaction> serviceChargeList = new List<Transaction> { new ServiceCharge((ClubService)ScenarioContext.Current["A_Club_Service"]) };
            ProFormaInvoice proFormaInvoice = new ProFormaInvoice(new InvoiceCustomerData(membersManagementContextData.clubMember), serviceChargeList, issueDate);
            invoicesManager.AddProFormaInvoiceToClubMember(proFormaInvoice, membersManagementContextData.clubMember);
            ScenarioContext.Current.Add("ProFormaInvoice", proFormaInvoice);
        }

        [Then(@"A pro-forma invoice is created for the cost of the service: (.*)")]
        public void ThenAPro_FormaInvoiceIsCreatedForTheCostOfTheService(decimal cost)
        {
            Assert.AreEqual(cost, ((ProFormaInvoice)ScenarioContext.Current["ProFormaInvoice"]).NetAmount);
        }

        [Then(@"No bills are created for a pro-forma invoice")]
        public void ThenNoBillsAreCreatedForAPro_FormaInvoice()
        {
            Assert.AreEqual(0, ((ProFormaInvoice)ScenarioContext.Current["ProFormaInvoice"]).BillsTotalAmountToCollect);
        }


        [Given(@"I have an invoice with cost (.*) with a single bill with ID ""(.*)""")]
        public void GivenIHaveAnInvoiceWithCostWithASingleBillWithID(int invoiceNetAmout, string billID)
        {
            List<Transaction> transactionList =
                new List<Transaction>() { new Transaction("Big Payment", 1, invoiceNetAmout, new Tax("NOIGIC", 0), 0) };
            DateTime issueDate = DateTime.Now.Date;
            Invoice invoice = new Invoice(new InvoiceCustomerData(membersManagementContextData.clubMember), transactionList, issueDate);
            invoicesManager.AddInvoiceToClubMember(invoice, membersManagementContextData.clubMember);
            ScenarioContext.Current.Add("Invoice", invoice);
            ScenarioContext.Current.Add("InvoiceNetAmount", invoiceNetAmout);
            ScenarioContext.Current.Add("BillID",billID);
            ScenarioContext.Current.Add("IssueDate", issueDate);
        }

        [When(@"I renegotiate the bill ""(.*)"" into three instalments: (.*), (.*), (.*) to pay in (.*), (.*) and (.*) days with agreement terms ""(.*)""")]
        public void WhenIRenegotiateTheBillIntoThreeInstalmentsToPayInAndDaysWithAgreementTerms(
            string billID,
            decimal firstInstalmentAmount,
            decimal secondInstalmentAmount,
            decimal thirdInstalmentAmount,
            int firstInstalmentDueDays,
            int secondInstalmentDueDays,
            int thirdInstalmentDueDays,
            string agreementTerms)
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            List<Bill> billsToRenegotiate = new List<Bill>() { invoice.Bills[ScenarioContext.Current["BillID"].ToString()] };
            List<Bill> billsToAdd = new List<Bill>()
            {
                {new Bill("First Instalment", 200, DateTime.Now, DateTime.Now.AddDays(firstInstalmentDueDays))},
                {new Bill("Second Instalment", 200, DateTime.Now, DateTime.Now.AddDays(secondInstalmentDueDays))},
                {new Bill("Third Instalment", 250, DateTime.Now, DateTime.Now.AddDays(thirdInstalmentDueDays))}
            };
            string authorizingPerson = "Club President";
            DateTime agreementDate = DateTime.Now;
            ScenarioContext.Current.Add("AgreementDate", agreementDate);
            PaymentAgreement paymentAgreement = new PaymentAgreement(authorizingPerson, agreementTerms, agreementDate);
            invoicesManager.RenegotiateBillsOnInvoice(invoice, paymentAgreement, billsToRenegotiate, billsToAdd);
        }

        [Then(@"The bill ""(.*)"" is marked as renegotiated")]
        public void ThenTheBillIsMarkedAsRenegotiated(string renegotiatedBillID)
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            Assert.AreEqual(Bill.BillPaymentResult.Renegotiated, invoice.Bills[renegotiatedBillID].PaymentResult);
        }

        [Then(@"The renegotiated bill ""(.*)"" has associated the agreement terms ""(.*)"" to it")]
        public void ThenTheRenegotiatedBillHasAssociatedTheAgreementTermsToIt(string renegotiatedBillID, string agreemetTerms)
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            Assert.AreEqual(agreemetTerms, invoice.Bills[renegotiatedBillID].RenegotiationAgreement.AgreementTerms);
        }

        [Then(@"A bill with ID ""(.*)"" and cost of (.*) to be paid in (.*) days is created")]
        public void ThenABillWithIDAndCostOfToBePaidInDaysIsCreated(string createdBillID, decimal billAmount, int daysToDue)
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            Assert.AreEqual(billAmount, invoice.Bills[createdBillID].Amount);
            Assert.AreEqual(((DateTime)ScenarioContext.Current["IssueDate"]).AddDays(daysToDue), invoice.Bills[createdBillID].DueDate);
        }

        [Then(@"The new bill ""(.*)"" has associated the agreement terms ""(.*)"" to it")]
        public void ThenTheNewBillHasAssociatedTheAgreementTermsToIt(string newBillID, string agreementTerms)
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            DateTime agreementDate = (DateTime)ScenarioContext.Current["AgreementDate"];
            Assert.AreEqual(agreementTerms, invoice.Bills[newBillID].PaymentAgreements[DateTime.Now.Date].AgreementTerms);
        }

        [When(@"I assign to be paid with a direct debit")]
        public void WhenIAssignToBePaidWithADirectDebit()
        {
            int internalReferenceNumber = 2645;
            BankAccount bankAccount = new BankAccount(new ClientAccountCodeCCC("12345678061234567890"));
            DateTime directDebitMandateCreationDate = new DateTime(2013, 11, 11);
            string debtorname = membersManagementContextData.clubMember.FullName;
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(internalReferenceNumber, directDebitMandateCreationDate, bankAccount, debtorname);
            DirectDebitPaymentMethod directDebitPaymentMethod = new DirectDebitPaymentMethod(directDebitMandate, null);
            ScenarioContext.Current.Add("DirectDebitpaymentMethod", directDebitPaymentMethod);
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            string billID= (string)ScenarioContext.Current["BillID"];
            Bill bill = invoice.Bills[billID];
            bill.AssignPaymentMethod(directDebitPaymentMethod);
        }

        [Then(@"The new payment method is correctly assigned")]
        public void ThenTheNewPaymentMethodIsCorrectlyAssigned()
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            string billID = (string)ScenarioContext.Current["BillID"];
            DirectDebitPaymentMethod directDebitPaymentMethod = (DirectDebitPaymentMethod)ScenarioContext.Current["DirectDebitpaymentMethod"];
            Bill bill = invoice.Bills[billID];
            Assert.AreEqual(directDebitPaymentMethod, (DirectDebitPaymentMethod)bill.AssignedPaymentMethod);
        }

        [Given(@"I have an invoice with some bills")]
        public void GivenIHaveAnInvoiceWithSomeBills()
        {
            string invoiceID = "MMM2013005001";
            List<Transaction> transactionList = new List<Transaction>()
                {new Transaction("Big Payment",1,650,new Tax("NOIGIC",0),0)};
            List<Bill> unassignedBillsList = new List<Bill>()
            {
                {new Bill("MMM2013005001/001", "First Instalment", 200, new DateTime(2013,11,01), new DateTime(2013,12,01))},
                {new Bill("MMM2013005001/002", "Second Instalment", 200, new DateTime(2013,11,01), new DateTime(2014,01,01))},
                {new Bill("MMM2013005001/003", "Third Instalment", 250, new DateTime(2013,11,01), new DateTime(2014,02,01))}
            };
            Invoice invoice = new Invoice(
                invoiceID,
                new InvoiceCustomerData(membersManagementContextData.clubMember),
                transactionList,
                unassignedBillsList,
                DateTime.Now);
            ScenarioContext.Current.Add("Invoice", invoice);
        }

        [Given(@"I have a bill to collect in the invoice")]
        public void GivenIHaveABillToCollectInTheInvoice()
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            Bill bill = invoice.Bills["MMM2013005001/001"];
            Assert.AreEqual(Bill.BillPaymentResult.ToCollect, bill.PaymentResult);
            ScenarioContext.Current.Add("Bill", bill);
        }

        [When(@"The bill is paid in cash")]
        public void WhenTheBillIsPaidInCash()
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            CashPaymentMethod cashPaymentMethod = new CashPaymentMethod();
            Payment payment = new Payment(bill.Amount, new DateTime(2013, 11, 11), cashPaymentMethod);
            billsManager.PayBill(invoice, bill, payment);
        }

        [Then(@"The bill state is set to ""(.*)""")]
        public void ThenTheBillStateIsSetTo(string billState)
        {
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            Assert.AreEqual(billState, bill.PaymentResult.ToString());
        }

        [Then(@"The bill payment method is set to ""(.*)""")]
        public void ThenTheBillPaymentMethodIsSetTo(string paymentMethod)
        {
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            PaymentMethod billPaymentMethod = bill.Payment.PaymentMethod;
            Type paymentMethodType = billPaymentMethod.GetType();
            switch (paymentMethod)
            {
                case "Cash":
                    Assert.AreEqual(paymentMethodType, typeof(CashPaymentMethod));
                    break;
                case "Bank Transfer":
                    Assert.AreEqual(paymentMethodType, typeof(BankTransferPaymentMethod));
                    break;
                case "Direct Debit":
                    Assert.AreEqual(paymentMethodType, typeof(DirectDebitPaymentMethod));
                    break;
            }
        }

        [Then(@"The bill payment date is stored")]
        public void ThenTheBillPaymentDateIsStored()
        {
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            Assert.AreEqual(new DateTime(2013, 11, 11), bill.Payment.PaymentDate);
        }

        [Then(@"The bill amount is deduced form the invoice total amount")]
        public void ThenTheBillAmountIsDeducedFormTheInvoiceTotalAmount()
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            decimal restToPay = invoice.NetAmount - bill.Amount;
            Assert.AreEqual(restToPay, invoice.BillsTotalAmountToCollect);
        }

        [When(@"The bill is paid by bank transfer")]
        public void WhenTheBillIsPaidByBankTransfer()
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            BankAccount transferorAccount = new BankAccount(new ClientAccountCodeCCC("20381111401111111111"));
            BankAccount transfereeAccount = new BankAccount(new ClientAccountCodeCCC("21001111301111111111"));
            BankTransferPaymentMethod bankTransferPaymentMethod = new BankTransferPaymentMethod(transferorAccount, transfereeAccount);
            Payment payment = new Payment(bill.Amount, new DateTime(2013, 11, 11), bankTransferPaymentMethod);
            billsManager.PayBill(invoice, bill, payment);
        }

        [Then(@"The transferor account is stored")]
        public void ThenTheTransferorAccountIsStored()
        {
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            Assert.AreEqual("20381111401111111111", ((BankTransferPaymentMethod)bill.Payment.PaymentMethod).TransferorAccount.CCC.CCC);
        }

        [Then(@"The transferee account is stored")]
        public void ThenTheTransfereeAccountIsStored()
        {
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            Assert.AreEqual("21001111301111111111", ((BankTransferPaymentMethod)bill.Payment.PaymentMethod).TransfereeAccount.CCC.CCC);
        }

        [When(@"The bill is paid by direct debit")]
        public void WhenTheBillIsPaidByDirectDebit()
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            int internalReferenceNumber = 2645;
            BankAccount bankAccount = new BankAccount(new ClientAccountCodeCCC("12345678061234567890"));
            DateTime directDebitMandateCreationDate = new DateTime(2013, 11, 11);
            string debtorname = membersManagementContextData.clubMember.FullName;
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(internalReferenceNumber, directDebitMandateCreationDate, bankAccount, debtorname);
            string directDebitTransactionPaymentIdentification = "201311110000123456";
            DirectDebitPaymentMethod directDebitPaymentMethod = new DirectDebitPaymentMethod(directDebitMandate, directDebitTransactionPaymentIdentification);
            DateTime paymentDate = new DateTime(2013, 11, 11);
            Payment payment = new Payment(bill.Amount, paymentDate, directDebitPaymentMethod);
            billsManager.PayBill(invoice, bill, payment);
        }

        [Then(@"The direct debit initiation ID is stored")]
        public void ThenTheDirectDebitInitiationIDIsStored()
        {
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            Assert.AreEqual("201311110000123456", ((DirectDebitPaymentMethod)bill.Payment.PaymentMethod).DDTXPaymentIdentification);
        }

        [When(@"All the bills are paid")]
        public void WhenAllTheBillsArePaid()
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            CashPaymentMethod cashPayment = new CashPaymentMethod();
            DateTime paymentDate = new DateTime(2013, 11, 11);
            Payment payment200 = new Payment((decimal)200, paymentDate, cashPayment);
            Payment payment250 = new Payment((decimal)250, paymentDate, cashPayment);
            billsManager.PayBill(invoice, invoice.Bills["MMM2013005001/001"], payment200);
            billsManager.PayBill(invoice, invoice.Bills["MMM2013005001/002"], payment200);
            billsManager.PayBill(invoice, invoice.Bills["MMM2013005001/003"], payment250);
        }

        [Then(@"The invoice state is set as ""(.*)""")]
        public void ThenTheInvoiceStateIsSetAs(string paymentStatus)
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            Assert.AreEqual(paymentStatus, invoice.InvoiceState.ToString());
        }

        [When(@"The bill is past due date")]
        public void WhenTheBillIsPastDueDate()
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            billsManager.CheckDueDate(invoice, bill, new DateTime(2013, 12, 31));
        }

        [Then(@"The bill is marked as ""(.*)""")]
        public void ThenTheBillIsMarkedAs(string billStatus)
        {
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            Assert.AreEqual(billStatus, bill.PaymentResult.ToString());
        }

        [Then(@"The invoice containig the bill is marked as ""(.*)""")]
        public void ThenTheInvoiceContainigTheBillIsMarkedAs(string invoiceStatus)
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            Assert.AreEqual(invoiceStatus, invoice.InvoiceState.ToString());
        }

        [Given(@"I have an invoice with some bills with agreements")]
        public void GivenIHaveAnInvoiceWithSomeBillsWithAgreements()
        {
            string invoiceID = "MMM2013005001";
            List<Transaction> transactionList = new List<Transaction>() { new Transaction("Big Payment", 1, 650, new Tax("NOIGIC", 0), 0) };
            DateTime issueDate = new DateTime(2013,10,1);
            Invoice invoice = new Invoice(invoiceID, new InvoiceCustomerData(membersManagementContextData.clubMember), transactionList, issueDate);
            string authorizingPerson = "Club President";
            string agreementTerms = "New Payment Agreement";
            DateTime agreementDate = new DateTime(2013, 10, 1);
            PaymentAgreement paymentAgreement = new PaymentAgreement(authorizingPerson, agreementTerms, agreementDate);
            List<Bill> billsToRenegotiate = new List<Bill>() { invoice.Bills["MMM2013005001/001"] };
            List<Bill> billsToAdd = new List<Bill>()
            {
                {new Bill("MMM2013005001/002", "First Instalment", 200, new DateTime(2013,10,1), new DateTime(2013,11,1))},
                {new Bill("MMM2013005001/003", "Second Instalment", 200, new DateTime(2013,10,1), new DateTime(2013,12,1))},
                {new Bill("MMM2013005001/004", "Third Instalment", 250, new DateTime(2013,10,1), new DateTime(2014,1,1))}
            };
            invoice.RenegotiateBillsIntoInstalments(paymentAgreement, billsToRenegotiate, billsToAdd);
            ScenarioContext.Current.Add("Invoice", invoice);
            ScenarioContext.Current.Add("AgreementDate", agreementDate);
            ScenarioContext.Current.Add("Bill", invoice.Bills["MMM2013005001/002"]);
        }

        [Given(@"I have a bill to collect in the invoice with a payment agreement")]
        public void GivenIHaveABillToCollectInTheInvoiceWithAPaymentAgreement()
        {
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            DateTime agreementDate = (DateTime)ScenarioContext.Current["AgreementDate"];
            Assert.AreEqual(Bill.BillPaymentResult.ToCollect, bill.PaymentResult);
            Assert.IsTrue(bill.PaymentAgreements.ContainsKey(agreementDate));
        }

        [Then(@"The associated payment agreement is set to ""(.*)"" for all bills involved on the agreement")]
        public void ThenTheAssociatedPaymentAgreementIsSetToForAllBillsInvolvedOnTheAgreement(string paymentAgreementStatus)
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            DateTime agreementDate = (DateTime)ScenarioContext.Current["AgreementDate"];
            var billsIncludedInTheAgreement = invoice.Bills.Values.Where(bill => bill.PaymentAgreements.ContainsKey(agreementDate));
            foreach (Bill bill in billsIncludedInTheAgreement)
                Assert.AreEqual(paymentAgreementStatus, bill.PaymentAgreements[agreementDate].PaymentAgreementActualStatus.ToString());
        }

        [Then(@"The associated payment agreement is set to ""(.*)"" for the invoice")]
        public void ThenTheAssociatedPaymentAgreementIsSetToForTheInvoice(string invoiceAgreementState)
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            DateTime agreementDate = (DateTime)ScenarioContext.Current["AgreementDate"];
            Assert.AreEqual(invoiceAgreementState, invoice.PaymentAgreements[agreementDate].PaymentAgreementActualStatus.ToString());
        }

        [When(@"I renew the due date")]
        public void WhenIRenewTheDueDate()
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            DateTime newDueDate = new DateTime(2013, 12, 15);
            DateTime todayDate = new DateTime(2013, 11, 11);
            billsManager.RenewBillDueDate(invoice, bill, newDueDate, todayDate);
            ScenarioContext.Current.Add("RenewDueDate", newDueDate);
        }

        [Then(@"The new due date is assigned to the bill")]
        public void ThenTheNewDueDateIsAssignedToTheBill()
        {
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            DateTime renewedDueDate = (DateTime)ScenarioContext.Current["RenewDueDate"];
            Assert.AreEqual(renewedDueDate, bill.DueDate);
        }

        [Given(@"The bill is past due date")]
        public void GivenTheBillIsPastDueDate()
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            Bill bill = (Bill)ScenarioContext.Current["Bill"];
            DateTime todayDate = new DateTime(2013, 12, 15);
            billsManager.CheckDueDate(invoice, bill, todayDate);
            Assert.AreEqual(Bill.BillPaymentResult.Unpaid, bill.PaymentResult);
        }

        [Then(@"If there are no other bills marked as ""(.*)"" the invoice is marked ""(.*)""")]
        public void ThenIfThereAreNoOtherBillsMarkedAsTheInvoiceIsMarked(string billState, string invoiceState)
        {
            Invoice invoice = (Invoice)ScenarioContext.Current["Invoice"];
            var billsMarkedAsUnpaid = invoice.Bills.Values.Where(bill => bill.PaymentResult.ToString() == billState);
            Assert.AreEqual(0, billsMarkedAsUnpaid.Count());
            Assert.AreEqual(invoiceState, invoice.InvoiceState.ToString());
        }
    }
}
