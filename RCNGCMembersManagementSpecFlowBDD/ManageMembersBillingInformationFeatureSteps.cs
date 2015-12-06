using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementAppLogic.MembersManaging;
using RCNGCMembersManagementAppLogic.Billing;
using RCNGCMembersManagementAppLogic.Billing.DirectDebit;
using RCNGCMembersManagementMocks;

namespace RCNGCMembersManagementSpecFlowBDD
{
    [Binding, Scope(Feature = "Manage Members Billing Information")]
    public class ManageMembersBillingInformationFeatureSteps
    {
        private readonly MembersManagementContextData membersManagementContextData;
        private readonly DirectDebitContextData directDebitContextData;

        public ManageMembersBillingInformationFeatureSteps(
            MembersManagementContextData membersManagementContextData, 
            DirectDebitContextData directDebitContextData)
        {
            this.membersManagementContextData = membersManagementContextData;
            this.directDebitContextData = directDebitContextData;
        }

        [BeforeScenario]
        public void InitializeScenario()
        {
            directDebitContextData.billDataManager = BillingDataManager.Instance;
            BillingSequenceNumbersMock billingSequenceNumbersMock = new BillingSequenceNumbersMock();
            directDebitContextData.billDataManager.SetBillingSequenceNumberCollaborator(billingSequenceNumbersMock);

        }

        [Given(@"A Club Member")]
        public void GivenAClubMember(Table clientsTable)
        {
            membersManagementContextData.clubMember = new ClubMember(clientsTable.Rows[0]["MemberID"], clientsTable.Rows[0]["Name"], clientsTable.Rows[0]["FirstSurname"], clientsTable.Rows[0]["SecondSurname"]);
        }
        
        [Given(@"These Direct Debit Mandates")]
        public void GivenTheseDirectDebitMandates(Table directDebits)
        {
            directDebitContextData.directDebitMandates = new Dictionary<int, DirectDebitMandate>();
            foreach (var row in directDebits.Rows)
            {
                int internalReferenceNumber = int.Parse(row["DirectDebitInternalReferenceNumber"]);
                string iBAN = (string)row["IBAN"];
                DateTime creationDate = DateTime.Parse((string)row["RegisterDate"]).Date;
                BankAccount bankAccount = new BankAccount(new InternationalAccountBankNumberIBAN(iBAN));
                string debtorName = membersManagementContextData.clubMember.FullName;
                DirectDebitMandate directDebitmandate = new DirectDebitMandate(internalReferenceNumber, creationDate, bankAccount, debtorName);
                directDebitContextData.directDebitMandates.Add(internalReferenceNumber, directDebitmandate);
            }
        }

        [Given(@"These Account Numbers")]
        public void GivenTheseAccountNumbers(Table accountNumbers)
        {
            directDebitContextData.bankAccounts = new Dictionary<string, BankAccount>();
            foreach (var row in accountNumbers.Rows)
            {;
                string iBAN = (string)row["IBAN"];
                BankAccount bankAccount = new BankAccount(new InternationalAccountBankNumberIBAN(iBAN));
                directDebitContextData.bankAccounts.Add(iBAN, bankAccount);
            }
        }
        
        [Given(@"I have a member")]
        public void GivenIHaveAMember()
        {
            ScenarioContext.Current.Add("Member1", membersManagementContextData.clubMember);
        }

        [Given(@"The member has associated cash as payment method")]
        public void GivenTheMemberHasAssociatedCashAsPaymentMethod()
        {
            ClubMember clubMember = (ClubMember)ScenarioContext.Current["Member1"];
            CashPaymentMethod cashPaymnentMethod = new CashPaymentMethod();
            clubMember.SetDefaultPaymentMethod(cashPaymnentMethod);
        }

        [When(@"I set direct debit as new payment method")]
        public void WhenISetDirectDebitAsNewPaymentMethod()
        {
            ClubMember clubMember = (ClubMember)ScenarioContext.Current["Member1"];
            DirectDebitMandate directDebitMandate = directDebitContextData.directDebitMandates[2345];
            DirectDebitPaymentMethod directDebitPaymentMethod = new DirectDebitPaymentMethod(directDebitMandate, null);
            ScenarioContext.Current.Add("DirectDebitPaymentMethod", directDebitPaymentMethod);
            clubMember.SetDefaultPaymentMethod(directDebitPaymentMethod);
        }

        [Then(@"The new payment method is correctly updated")]
        public void ThenTheNewPaymentMethodIsCorrectlyUpdated()
        {
            ClubMember clubMember = (ClubMember)ScenarioContext.Current["Member1"];
            DirectDebitPaymentMethod directDebitPaymentMethod = (DirectDebitPaymentMethod)ScenarioContext.Current["DirectDebitPaymentMethod"];
            Assert.AreEqual(directDebitPaymentMethod, (DirectDebitPaymentMethod)clubMember.DefaultPaymentMethod);
        }

        [Given(@"The direct debit reference sequence number is (.*)")]
        public void GivenTheDirectDebitReferenceSequenceNumberIs(int directDebitSequenceNumber)
        {
            directDebitContextData.billDataManager.DirectDebitSequenceNumber = (uint)directDebitSequenceNumber;
            
        }
       
        [When(@"I add a new direct debit mandate to the member")]
        public void WhenIAddANewDirectDebitMandateToTheMember()
        {
            ClubMember clubMember = (ClubMember)ScenarioContext.Current["Member1"];
            DateTime creationDate = new DateTime(2013, 11, 11);
            BankAccount bankAccount = directDebitContextData.bankAccounts["ES6812345678061234567890"];
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(creationDate, bankAccount, clubMember.FullName);
            ScenarioContext.Current.Add("DirectDebitMandate", directDebitMandate);
            clubMember.AddDirectDebitMandate(directDebitMandate);
        }

        [Then(@"The new direct debit mandate is correctly assigned")]
        public void ThenTheNewDirectDebitMandateIsCorrectlyAssigned()
        {
            ClubMember clubMember = (ClubMember)ScenarioContext.Current["Member1"];
            DirectDebitMandate directDebitMandate = (DirectDebitMandate)ScenarioContext.Current["DirectDebitMandate"];
            Assert.AreEqual(directDebitMandate, clubMember.DirectDebitmandates[5000]);
        }

        [Then(@"The new direct debit reference sequence number is (.*)")]
        public void ThenTheNewDirectDebitReferenceSequenceNumberIs(int directDebitInternalSequenceNumber)
        {
            Assert.AreEqual((uint)directDebitInternalSequenceNumber, directDebitContextData.billDataManager.DirectDebitSequenceNumber);
        }

        [Given(@"I have a direct debit associated to the member")]
        public void GivenIHaveADirectDebitAssociatedToTheMember()
        {
            ClubMember clubMember = (ClubMember)ScenarioContext.Current["Member1"];
            DirectDebitMandate directDebitMandate = directDebitContextData.directDebitMandates[2345];
            clubMember.AddDirectDebitMandate(directDebitMandate);
        }

        [When(@"I change the account number of the direct debit")]
        public void WhenIChangeTheAccountNumberOfTheDirectDebit()
        {
            ClubMember clubMember = (ClubMember)ScenarioContext.Current["Member1"];
            DateTime changingDate = new DateTime(2013, 12, 01);
            BankAccount originalBankAccount = clubMember.DirectDebitmandates[2345].BankAccount;
            ScenarioContext.Current.Add("ChangingBankDate", changingDate);
            ScenarioContext.Current.Add("OriginalBankAccount", originalBankAccount);
            BankAccount bankAccount = directDebitContextData.bankAccounts["ES3011112222003333333333"];
            clubMember.DirectDebitmandates[2345].ChangeBankAccount(bankAccount, changingDate);
        }

        [Then(@"The account number is correctly changed")]
        public void ThenTheAccountNumberIsCorrectlyChanged()
        {
            ClubMember clubMember = (ClubMember)ScenarioContext.Current["Member1"];
            BankAccount bankAccount = directDebitContextData.bankAccounts["ES3011112222003333333333"];
            Assert.AreEqual(bankAccount, clubMember.DirectDebitmandates[2345].BankAccount);
        }

        [Then(@"The old account number is stored in the account numbers history")]
        public void ThenTheOldAccountNumberIsStoredInTheAccountNumbersHistory()
        {
            ClubMember clubMember = (ClubMember)ScenarioContext.Current["Member1"];
            BankAccount originalBankAccount = (BankAccount)ScenarioContext.Current["OriginalBankAccount"];
            DateTime changingBankDate = (DateTime)ScenarioContext.Current["ChangingBankDate"];
            Assert.AreEqual(originalBankAccount, clubMember.DirectDebitmandates[2345].BankAccountHistory[changingBankDate].BankAccount);
        }
    }
}
