using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementAppLogic.Billing;
using RCNGCMembersManagementAppLogic.Billing.DirectDebit;
using RCNGCMembersManagementMocks;
using ExtensionMethods;

namespace RCNGCMembersManagementUnitTests
{
    [TestClass]
    public class DirectDebitUnitTests
    {
        static BillingDataManager billDataManager;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            BillingSequenceNumbersMock billingSequenceNumbersMock = new BillingSequenceNumbersMock();
            billDataManager = BillingDataManager.Instance;
            billDataManager.SetBillingSequenceNumberCollaborator(billingSequenceNumbersMock);
        }


        [TestMethod]
        public void GivenAReferenceNumberADirectDebitMandateIsCorrectlyCreated()
        {
            int internalReferenceNumber = 2645;
            BankAccount bankAccount = new BankAccount(new ClientAccountCodeCCC("12345678061234567890"));
            DateTime directDebitMandateCreationDate = new DateTime(2013, 11, 11);
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(internalReferenceNumber, directDebitMandateCreationDate, bankAccount, "NoName");
            Assert.AreEqual(DirectDebitMandate.DirectDebitmandateStatus.Active, directDebitMandate.Status);
            Assert.AreEqual(internalReferenceNumber, directDebitMandate.InternalReferenceNumber);
            Assert.AreEqual(directDebitMandateCreationDate, directDebitMandate.DirectDebitMandateCreationDate);
            Assert.AreEqual(bankAccount, directDebitMandate.BankAccount);
            Assert.AreEqual(directDebitMandateCreationDate, directDebitMandate.BankAccountActivationDate);
        }

        [TestMethod]
        public void IfNoReferenceNumberIsProvidedASequenceNumberIsAssigned()
        {
            billDataManager.DirectDebitSequenceNumber = 5000;
            BankAccount bankAccount = new BankAccount(new ClientAccountCodeCCC("12345678061234567890"));
            DateTime directDebitMandateCreationDate = new DateTime(2013, 11, 11);
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(directDebitMandateCreationDate, bankAccount, "NoName");
            Assert.AreEqual(5000, directDebitMandate.InternalReferenceNumber);
        }

        [TestMethod]
        public void WeCanSetTheDirectDebitSequenceNumberValue()
        {
            BankAccount bankAccount = new BankAccount(new ClientAccountCodeCCC("12345678061234567890"));
            DateTime directDebitMandateCreationDate = new DateTime(2013, 11, 11);
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(2345, directDebitMandateCreationDate, bankAccount, "NoName");
            Assert.AreEqual(2345, directDebitMandate.InternalReferenceNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CantSetDirectDebitSequenceNumberOver99999()
        {
            try
            {
                billDataManager.DirectDebitSequenceNumber = 100000;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.AreEqual("Direct Debit Sequence Number out of range (1-99999)", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CantSetDirectDebitSequenceNumberTo0()
        {
            try
            {
                billDataManager.DirectDebitSequenceNumber = 0;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.AreEqual("Direct Debit Sequence Number out of range (1-99999)", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CantInitializeADirectDebitWithAnOutOfRangeSequenceNumber()
        {
            BankAccount bankAccount = new BankAccount(new ClientAccountCodeCCC("12345678061234567890"));
            DateTime directDebitMandateCreationDate = new DateTime(2013, 11, 11);
            try
            {
                DirectDebitMandate directDebitMandate = new DirectDebitMandate(100000, directDebitMandateCreationDate, bankAccount, "NoName");
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.AreEqual("Direct Debit Sequence Number out of range (1-99999)", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        public void ProvidingTheLastDirectDebitReferenceNumberWas100TheNextAssignedMustBe101()
        {
            billDataManager.DirectDebitSequenceNumber = 100;
            ClientAccountCodeCCC ccc = new ClientAccountCodeCCC("12345678061234567890");
            BankAccount bankAccount = new BankAccount(ccc);
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(DateTime.Now.Date, bankAccount, "NoName");
            Assert.AreEqual((uint)101, billDataManager.DirectDebitSequenceNumber);
        }

        [TestMethod]
        public void ADirectDebitMandateCanBeDeactivated()
        {
            ClientAccountCodeCCC ccc = new ClientAccountCodeCCC("12345678061234567890");
            BankAccount bankAccount = new BankAccount(ccc);
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(1, DateTime.Now.Date, bankAccount, "NoName");
            Assert.AreEqual(DirectDebitMandate.DirectDebitmandateStatus.Active, directDebitMandate.Status);
            directDebitMandate.DeactivateMandate();
            Assert.AreEqual(DirectDebitMandate.DirectDebitmandateStatus.Inactive, directDebitMandate.Status);
        }

        [TestMethod]
        public void ADirectDebitMandateCanBeReactivated()
        {
            ClientAccountCodeCCC ccc = new ClientAccountCodeCCC("12345678061234567890");
            BankAccount bankAccount = new BankAccount(ccc);
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(1, DateTime.Now.Date, bankAccount, "NoName");
            Assert.AreEqual(DirectDebitMandate.DirectDebitmandateStatus.Active, directDebitMandate.Status);
            directDebitMandate.DeactivateMandate();
            Assert.AreEqual(DirectDebitMandate.DirectDebitmandateStatus.Inactive, directDebitMandate.Status);
            directDebitMandate.ActivateMandate();
            Assert.AreEqual(DirectDebitMandate.DirectDebitmandateStatus.Active, directDebitMandate.Status);
        }

        [TestMethod]
        public void ABankAccountHistoricalDataIsCorrectlyCreated()
        {
            ClientAccountCodeCCC ccc = new ClientAccountCodeCCC("12345678061234567890");
            BankAccount bankAccount = new BankAccount(ccc);
            DateTime activationDate = new DateTime(2013, 11, 11);
            DateTime deactivationDate = new DateTime(2013, 11, 30);
            BankAccountHistoricalData ephemeralBankAcount = new BankAccountHistoricalData(bankAccount, activationDate, deactivationDate);
            Assert.AreEqual(bankAccount, ephemeralBankAcount.BankAccount);
            Assert.AreEqual(activationDate, ephemeralBankAcount.AccountActivationDate);
            Assert.AreEqual(deactivationDate, ephemeralBankAcount.AccountDeactivationDate);
        }

        [TestMethod]
        public void TheBankAccountOfADirectDebitOrderCanBeChanged()
        {
            ClientAccountCodeCCC ccc = new ClientAccountCodeCCC("12345678061234567890");
            BankAccount ephemeralBankAcount = new BankAccount(ccc);
            DateTime ephemeralBankAcountActivationDate = new DateTime(2013, 11, 11);
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(1, ephemeralBankAcountActivationDate, ephemeralBankAcount, "NoName");
            Assert.AreEqual(ephemeralBankAcount, directDebitMandate.BankAccount);
            InternationalAccountBankNumberIBAN iBAN = new InternationalAccountBankNumberIBAN("ES3011112222003333333333");
            BankAccount newBankAccount = new BankAccount(iBAN);
            DateTime dateOfChange = new DateTime(2013, 11, 30);
            directDebitMandate.ChangeBankAccount(newBankAccount, dateOfChange);
            Assert.AreEqual(newBankAccount, directDebitMandate.BankAccount);
            Assert.AreEqual(dateOfChange, directDebitMandate.BankAccountActivationDate);
            Assert.AreEqual(ephemeralBankAcount, directDebitMandate.BankAccountHistory[dateOfChange].BankAccount);
            Assert.AreEqual(ephemeralBankAcountActivationDate, directDebitMandate.BankAccountHistory[dateOfChange].AccountActivationDate);
            Assert.AreEqual(dateOfChange, directDebitMandate.BankAccountHistory[dateOfChange].AccountDeactivationDate);
        }
    }
}
