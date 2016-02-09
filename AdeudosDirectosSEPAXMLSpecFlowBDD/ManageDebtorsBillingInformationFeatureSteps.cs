using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing;
using DirectDebitElements;

namespace AdeudosDirectosSEPAXMLSpecFlowBDD
{
    [Binding, Scope(Feature = "Manage Debtors Billing Information")]
    public class ManageDebtorsBillingInformationFeatureSteps
    {
        private readonly DebtorsManagementContextData membersManagementContextData;
        private readonly DirectDebitContextData directDebitContextData;

        public ManageDebtorsBillingInformationFeatureSteps(
            DebtorsManagementContextData membersManagementContextData, 
            DirectDebitContextData directDebitContextData)
        {
            this.membersManagementContextData = membersManagementContextData;
            this.directDebitContextData = directDebitContextData;
        }

        [Given(@"A debtor")]
        public void GivenAClubMember(Table clientsTable)
        {
            membersManagementContextData.debtor = new Debtor(clientsTable.Rows[0]["DebtorID"], clientsTable.Rows[0]["Name"], clientsTable.Rows[0]["FirstSurname"], clientsTable.Rows[0]["SecondSurname"]);
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
                string debtorName = membersManagementContextData.debtor.FullName;
                DirectDebitMandate directDebitmandate = new DirectDebitMandate(internalReferenceNumber, creationDate, bankAccount, null, debtorName);
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
        
        [Given(@"I have a debtor")]
        public void GivenIHaveADebtor()
        {
            ScenarioContext.Current.Add("Debtor1", membersManagementContextData.debtor);
        }

        [When(@"I add a new direct debit mandate to the debtor")]
        public void WhenIAddANewDirectDebitMandateToTheMember()
        {
            Debtor debtor = (Debtor)ScenarioContext.Current["Debtor1"];
            DirectDebitMandate directDebitMandate = directDebitContextData.directDebitMandates[2345];
            ScenarioContext.Current.Add("DirectDebitMandate", directDebitMandate);
            debtor.AddDirectDebitMandate(directDebitMandate);
        }

        [Then(@"The new direct debit mandate is correctly assigned")]
        public void ThenTheNewDirectDebitMandateIsCorrectlyAssigned()
        {
            Debtor debtor = (Debtor)ScenarioContext.Current["Debtor1"];
            DirectDebitMandate directDebitMandate = (DirectDebitMandate)ScenarioContext.Current["DirectDebitMandate"];
            Assert.AreEqual(directDebitMandate, debtor.DirectDebitmandates[2345]);
        }

        [Given(@"The debtor has associated cash as payment method")]
        public void GivenTheDebtorHasAssociatedCashAsPaymentMethod()
        {
            Debtor debtor = (Debtor)ScenarioContext.Current["Debtor1"];
            CashPaymentMethod cashPaymnentMethod = new CashPaymentMethod();
            debtor.SetDefaultPaymentMethod(cashPaymnentMethod);
        }

        [When(@"I set direct debit as new payment method")]
        public void WhenISetDirectDebitAsNewPaymentMethod()
        {
            Debtor debtor = (Debtor)ScenarioContext.Current["Debtor1"];
            DirectDebitMandate directDebitMandate = directDebitContextData.directDebitMandates[2345];
            DirectDebitPaymentMethod directDebitPaymentMethod = new DirectDebitPaymentMethod(directDebitMandate, null);
            ScenarioContext.Current.Add("DirectDebitPaymentMethod", directDebitPaymentMethod);
            debtor.SetDefaultPaymentMethod(directDebitPaymentMethod);
        }

        [Then(@"The new payment method is correctly updated")]
        public void ThenTheNewPaymentMethodIsCorrectlyUpdated()
        {
            Debtor debtor = (Debtor)ScenarioContext.Current["Debtor1"];
            DirectDebitPaymentMethod directDebitPaymentMethod = (DirectDebitPaymentMethod)ScenarioContext.Current["DirectDebitPaymentMethod"];
            Assert.AreEqual(directDebitPaymentMethod, (DirectDebitPaymentMethod)debtor.DefaultPaymentMethod);
        }
       


        [Given(@"I have a direct debit associated to the debtor")]
        public void GivenIHaveADirectDebitAssociatedToTheMember()
        {
            Debtor debtor = (Debtor)ScenarioContext.Current["Debtor1"];
            DirectDebitMandate directDebitMandate = directDebitContextData.directDebitMandates[2345];
            debtor.AddDirectDebitMandate(directDebitMandate);
        }

        [When(@"I change the account number of the direct debit")]
        public void WhenIChangeTheAccountNumberOfTheDirectDebit()
        {
            Debtor debtor = (Debtor)ScenarioContext.Current["Debtor1"];
            DateTime changingDate = new DateTime(2013, 12, 01);
            BankAccount originalBankAccount = debtor.DirectDebitmandates[2345].BankAccount;
            ScenarioContext.Current.Add("ChangingBankDate", changingDate);
            ScenarioContext.Current.Add("OriginalBankAccount", originalBankAccount);
            BankAccount bankAccount = directDebitContextData.bankAccounts["ES3011112222003333333333"];
            debtor.DirectDebitmandates[2345].ChangeBankAccount(bankAccount,null,  changingDate);
        }

        [Then(@"The account number is correctly changed")]
        public void ThenTheAccountNumberIsCorrectlyChanged()
        {
            Debtor debtor = (Debtor)ScenarioContext.Current["Debtor1"];
            BankAccount bankAccount = directDebitContextData.bankAccounts["ES3011112222003333333333"];
            Assert.AreEqual(bankAccount, debtor.DirectDebitmandates[2345].BankAccount);
        }

        [Then(@"The old account number is stored in the account numbers history")]
        public void ThenTheOldAccountNumberIsStoredInTheAccountNumbersHistory()
        {
            Debtor debtor = (Debtor)ScenarioContext.Current["Debtor1"];
            BankAccount originalBankAccount = (BankAccount)ScenarioContext.Current["OriginalBankAccount"];
            DateTime changingBankDate = (DateTime)ScenarioContext.Current["ChangingBankDate"];
            Assert.AreEqual(originalBankAccount, debtor.DirectDebitmandates[2345].BankAccountHistory[changingBankDate].BankAccount);
        }
    }
}
