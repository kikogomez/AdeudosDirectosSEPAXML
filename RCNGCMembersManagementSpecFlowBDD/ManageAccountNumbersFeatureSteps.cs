using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementAppLogic.Billing.DirectDebit;

namespace RCNGCMembersManagementSpecFlowBDD
{
    [Binding, Scope(Feature = "Manage account numbers")]
    class ManageAccountNumbersFeatureSteps
    {
        private readonly BankAccountContextData bankAccountContextData;

        public ManageAccountNumbersFeatureSteps(BankAccountContextData bankAccountContextData)
        {
            this.bankAccountContextData = bankAccountContextData;
        }

        [Given(@"This bank account ""(.*)"", ""(.*)"", ""(.*)"", ""(.*)""")]
        public void GivenThisBankAccount(string bank, string  office, string checkDigits, string accountNumber)
        {
            bankAccountContextData.bank = bank;
            bankAccountContextData.office = office;
            bankAccountContextData.checkDigits = checkDigits;
            bankAccountContextData.accountNumber = accountNumber;
        }

        [Given(@"This CCC ""(.*)""")]
        public void GivenThisCCC(string ccc)
        {
            bankAccountContextData.ccc = ccc;
        }

        [Given(@"This IBAN ""(.*)""")]
        public void GivenThisIBAN(string iban)
        {
            bankAccountContextData.iban = iban;
        }


        [When(@"I process the bank account")]
        public void WhenIProcessTheBankAccount()
        {
            BankAccount bankAccount;
            try
            {
                BankAccountFields bankAccountFields= new BankAccountFields(
                     bankAccountContextData.bank,
                     bankAccountContextData.office,
                     bankAccountContextData.checkDigits,
                     bankAccountContextData.accountNumber);
                bankAccount = new BankAccount(bankAccountFields);
                ScenarioContext.Current.Add("Bank_Account", bankAccount);
            }
            catch
            {
                ScenarioContext.Current.Add("Bank_Account", null);
            }           
        }

        [When(@"I process the CCC")]
        public void WhenIProcessTheCCC()
        {
            BankAccount bankAccount;
            try
            {
                ClientAccountCodeCCC ccc = new ClientAccountCodeCCC(bankAccountContextData.ccc);
                bankAccount = new BankAccount(ccc);
                ScenarioContext.Current.Add("Bank_Account", bankAccount);
            }
            catch
            {
                ScenarioContext.Current.Add("Bank_Account", null);
            }   
        }

        [When(@"I process the IBAN")]
        public void WhenIProcessTheIBAN()
        {
            BankAccount bankAccount;
            try
            {
                InternationalAccountBankNumberIBAN iban = new InternationalAccountBankNumberIBAN(bankAccountContextData.iban);
                bankAccount = new BankAccount(iban);
                ScenarioContext.Current.Add("Bank_Account", bankAccount);
            }
            catch
            {
                ScenarioContext.Current.Add("Bank_Account", null);
            }   
        }

        [Then(@"the bank account is considered ""(.*)""")]
        public void TheBankAccountIsConsidered(string validity)
        {
            bool valid = (validity == "valid" ? true : false);
            string ccc =
                bankAccountContextData.bank +
                bankAccountContextData.office +
                bankAccountContextData.checkDigits +
                bankAccountContextData.accountNumber;
            Assert.AreEqual(valid, BankAccount.IsValidCCC(ccc));
        }

        [Then(@"the CCC is considered ""(.*)""")]
        public void TheCCCIsConsidered(string validity)
        {
            bool valid = (validity == "valid" ? true : false);
            Assert.AreEqual(valid, BankAccount.IsValidCCC(bankAccountContextData.ccc));
        }

        [Then(@"the IBAN is considered ""(.*)""")]
        public void ThenTheIBANIsConsidered(string validity)
        {
            bool valid = (validity == "valid" ? true : false);
            Assert.AreEqual(valid, BankAccount.IsValidIBAN(bankAccountContextData.iban));
        }

        [Then(@"the bank account is ""(.*)""")]
        public void ThenTheBankAccountIs(string storage)
        {
            bool isStored = (storage == "stored" ? true : false);
            if (ScenarioContext.Current["Bank_Account"] == null)
            {
                Assert.AreEqual(isStored, false);
            }
            else
            {
                BankAccount storedBankAccount = (BankAccount)ScenarioContext.Current["Bank_Account"];
                Assert.AreEqual(isStored, bankAccountContextData.bank == storedBankAccount.BankAccountFieldCodes.BankCode);
                Assert.AreEqual(isStored, bankAccountContextData.office == storedBankAccount.BankAccountFieldCodes.OfficeCode);
                Assert.AreEqual(isStored, bankAccountContextData.checkDigits == storedBankAccount.BankAccountFieldCodes.CheckDigits);
                Assert.AreEqual(isStored, bankAccountContextData.accountNumber == storedBankAccount.BankAccountFieldCodes.AccountNumber);
            }
        }

        [Then(@"the CCC is ""(.*)""")]
        public void ThenTheCCCIs(string storage)
        {
            bool isStored = (storage == "stored" ? true : false);
            if (ScenarioContext.Current["Bank_Account"] == null)
            {
                Assert.AreEqual(isStored, false);
            }
            else
            {
                BankAccount storedBankAccount = (BankAccount)ScenarioContext.Current["Bank_Account"];
                Assert.AreEqual(isStored, bankAccountContextData.ccc == storedBankAccount.CCC.CCC);
            }
        }

        [Then(@"the IBAN is ""(.*)""")]
        public void ThenTheIBANIs(string storage)
        {
            bool isStored = (storage == "stored" ? true : false);
            if (ScenarioContext.Current["Bank_Account"] == null)
            {
                Assert.AreEqual(isStored, false);
            }
            else
            {
                BankAccount storedBankAccount = (BankAccount)ScenarioContext.Current["Bank_Account"];
                Assert.AreEqual(isStored, bankAccountContextData.iban == storedBankAccount.IBAN.IBAN);
            }
        }


        [Then(@"the bank account ""(.*)"", ""(.*)"", ""(.*)"", ""(.*)"" is created")]
        public void ThenTheBankAccountIsCreated(string bank, string office, string checkDigit, string accountNumber)
        {
            BankAccount bankAccount = (BankAccount)ScenarioContext.Current["Bank_Account"];
            Assert.AreEqual(bank, bankAccount.BankAccountFieldCodes.BankCode);
            Assert.AreEqual(office, bankAccount.BankAccountFieldCodes.OfficeCode);
            Assert.AreEqual(checkDigit, bankAccount.BankAccountFieldCodes.CheckDigits);
            Assert.AreEqual(accountNumber, bankAccount.BankAccountFieldCodes.AccountNumber);
        }


        [Then(@"The CCC ""(.*)"" is created")]
        public void ThenTheCCCIsCreated(string ccc)
        {
            if (ScenarioContext.Current["Bank_Account"] == null)
            {
                Assert.AreEqual("", ccc);
            }
            else
            {
                Assert.AreEqual(ccc, ((BankAccount)ScenarioContext.Current["Bank_Account"]).CCC.CCC ?? "");
            }
        }

        [Then(@"The spanish IBAN code ""(.*)"" is created")]
        public void ThenTheSpanishIBANCodeIsCreated(string iban)
        {
            if (ScenarioContext.Current["Bank_Account"] == null)
            {
                Assert.AreEqual("", iban);
            }
            else
            {
                Assert.AreEqual(iban, ((BankAccount)ScenarioContext.Current["Bank_Account"]).IBAN.IBAN ?? "");
            }
        }
    }
}
