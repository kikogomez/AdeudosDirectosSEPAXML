using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing;
using DirectDebitElements;
using ExtensionMethods;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class DebtorUnitTests
    {

        [TestMethod]
        public void InstantiatingASimpleDebtor()
        {
            Debtor debtor = new Debtor("0002", "Francisco", "Gómez-Caldito", "Viseas");
            Assert.IsNotNull(debtor);
        }

        [TestMethod]
        public void NamesAndSurnamesAreTrimmed()
        {
            Debtor debtor = new Debtor("0002", "Francisco   ", "     Gómez-Caldito  ", "        ");
            Assert.AreEqual("Francisco", debtor.Name);
            Assert.AreEqual("Gómez-Caldito", debtor.FirstSurname);
            Assert.AreEqual("", debtor.SecondSurname);
        }

        [TestMethod]
        public void FullNameIsWellCalculatedGivenNameAndBothSurnames()
        {
            Debtor debtor = new Debtor("0002", "Francisco", "Gómez-Caldito", "Viseas");
            string expectedFullname = "Francisco Gómez-Caldito Viseas";
            Assert.AreEqual(expectedFullname, debtor.FullName);
        }

        [TestMethod]
        public void FullNameIsWellCalculatedGivenEmptySecondSurname()
        {
            Debtor debtor = new Debtor("0002", "Francisco", "Gómez-Caldito", "");
            string expectedFullname = "Francisco Gómez-Caldito";
            Assert.AreEqual(expectedFullname, debtor.FullName);
        }

        [TestMethod]
        public void FullNameIsWellCalculatedGivenNullSecondSurname()
        {
            Debtor debtor = new Debtor("0002", "Francisco", "Gómez-Caldito", null);
            string expectedFullname = "Francisco Gómez-Caldito";
            Assert.AreEqual(expectedFullname, debtor.FullName);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void EmptyNameThrowsException()
        {
            try
            {
                Debtor debtor = new Debtor("0002", "", "Gómez-Caldito", "Viseas");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Debtor name cannot be empty", exception.GetMessageWithoutParamName());
                Assert.AreEqual("name", exception.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void NameWithOnlySpacesThrowsException()
        {
            try
            {
                Debtor debtor = new Debtor("0002", "    ", "Gómez-Caldito", "Viseas");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Debtor name cannot be empty", exception.GetMessageWithoutParamName());
                Assert.AreEqual("name", exception.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void NullNameThrowsException()
        {
            try
            {
                Debtor debtor = new Debtor("0002", null, "Gómez-Caldito", "Viseas");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Debtor name cannot be empty", exception.GetMessageWithoutParamName());
                Assert.AreEqual("name", exception.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void EmptyFirstSurnameThrowsException()
        {
            try
            {
                Debtor debtor = new Debtor("0002", "Francisco", "", "Viseas");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Debtor first surname cannot be empty", exception.GetMessageWithoutParamName());
                Assert.AreEqual("firstSurname", exception.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void FirstSurnameWithOnlySpacesThrowsException()
        {
            try
            {
                Debtor debtor = new Debtor("0002", "Francisco", "     ", "Viseas");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Debtor first surname cannot be empty", exception.GetMessageWithoutParamName());
                Assert.AreEqual("firstSurname", exception.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void NullFirstSurnameThrowsException()
        {
            try
            {
                Debtor debtor = new Debtor("0002", "Francisco", null, "Viseas");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Debtor first surname cannot be empty", exception.GetMessageWithoutParamName());
                Assert.AreEqual("firstSurname", exception.ParamName);
                throw;
            }
        }

        [TestMethod]
        public void TheDebtorIntitialDefaultPaymentMathodIsCashPayment()
        {
            Debtor debtor = new Debtor("0001", "Francisco", "Gomez-Caldito", "Viseas");
            Assert.AreEqual(typeof(CashPaymentMethod), debtor.DefaultPaymentMethod.GetType());
        }

        [TestMethod]
        public void InitiallyADebtorHasNoAssociatedDirectDebitMandates()
        {
            Debtor debtor = new Debtor("0001", "Francisco", "Gomez-Caldito", "Viseas");
            Assert.AreEqual(0, debtor.DirectDebitmandates.Count);
        }

        [TestMethod]
        public void ADirectDebitMandateCanBeAddedToDebtor()
        {
            Debtor debtor = new Debtor("0001", "Francisco", "Gomez-Caldito", "Viseas");
            ClientAccountCodeCCC ccc = new ClientAccountCodeCCC("12345678061234567890");
            BankAccount bankAccount = new BankAccount(ccc);
            int internalReferenceNumber = 1111;
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(internalReferenceNumber, DateTime.Today.AddMonths(-1), bankAccount, debtor.FullName);

            debtor.AddDirectDebitMandate(directDebitMandate);
            Assert.IsNotNull(debtor.DirectDebitmandates[internalReferenceNumber]);
        }

        [TestMethod]
        public void DuplicatedDirectDebitMandatesAreOverwriten()
        {
            Debtor debtor = new Debtor("0001", "Francisco", "Gomez-Caldito", "Viseas");
            ClientAccountCodeCCC ccc = new ClientAccountCodeCCC("12345678061234567890");
            BankAccount bankAccount = new BankAccount(ccc);
            int internalReferenceNumber = 1111;
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(internalReferenceNumber, DateTime.Today.AddMonths(-1), bankAccount, debtor.FullName);
            debtor.AddDirectDebitMandate(directDebitMandate);
            Assert.IsNotNull(debtor.DirectDebitmandates[internalReferenceNumber]);

            ClientAccountCodeCCC newCCC = new ClientAccountCodeCCC("11112222003333333333");
            BankAccount newBankAccount = new BankAccount(newCCC);
            int newInternalReferenceNumber = 1111;
            DirectDebitMandate newDirectDebitMandate = new DirectDebitMandate(newInternalReferenceNumber, DateTime.Today.AddMonths(-1), bankAccount, debtor.FullName);
            debtor.AddDirectDebitMandate(directDebitMandate);
            Assert.IsNotNull("11112222003333333333", debtor.DirectDebitmandates[1111].BankAccount.CCC.CCC);
        }

        [TestMethod]
        public void TheDebtorDefaultPaymentMathodCanBeChanged()
        {
            Debtor debtor = new Debtor("0001", "Francisco", "Gomez-Caldito", "Viseas");
            ClientAccountCodeCCC ccc = new ClientAccountCodeCCC("12345678061234567890");
            BankAccount bankAccount = new BankAccount(ccc);
            int internalReferenceNumber = 1111;
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(internalReferenceNumber, DateTime.Today.AddMonths(-1), bankAccount, debtor.FullName);
            PaymentMethod defaultPaymentMethod = new DirectDebitPaymentMethod(directDebitMandate, "000001");

            debtor.SetDefaultPaymentMethod(defaultPaymentMethod);

            Assert.AreEqual(typeof(DirectDebitPaymentMethod), debtor.DefaultPaymentMethod.GetType());
        }

        [TestMethod]
        public void WhenChangingDefaultPaymentMethodToDirectDebitTheIncludedMandateIsAddedToMandatesList()
        {
            Debtor debtor = new Debtor("0001", "Francisco", "Gomez-Caldito", "Viseas");
            ClientAccountCodeCCC ccc = new ClientAccountCodeCCC("12345678061234567890");
            BankAccount bankAccount = new BankAccount(ccc);
            int internalReferenceNumber = 1111;
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(internalReferenceNumber, DateTime.Today.AddMonths(-1), bankAccount, debtor.FullName);
            PaymentMethod defaultPaymentMethod = new DirectDebitPaymentMethod(directDebitMandate, "000001");

            debtor.SetDefaultPaymentMethod(defaultPaymentMethod);

            Assert.IsNotNull(debtor.DirectDebitmandates[internalReferenceNumber]);
        }

        [TestMethod]
        public void InitiallyADebtorHasNoBillsAssociated()
        {
            Debtor debtor = new Debtor("0001", "Francisco", "Gomez-Caldito", "Viseas");
            Assert.AreEqual(0, debtor.SimplifiedBills.Count);
        }

        [TestMethod]
        public void ASimpleBillIsCorrectlyAddedToDebtor()
        {
            Debtor debtor = new Debtor("0001", "Francisco", "Gomez-Caldito", "Viseas");
            string billID = "00001";
            SimplifiedBill bill = new SimplifiedBill(billID, "Cuota Mesual Noviembre 2015", 79, DateTime.Today, DateTime.Today.AddMonths(1));
            debtor.AddSimplifiedBill(bill);
            Assert.IsNotNull(debtor.SimplifiedBills[billID]);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void DuplicatedBillIDsCauseExceptionRaise()
        {
            Debtor debtor = new Debtor("0001", "Francisco", "Gomez-Caldito", "Viseas");
            string billID = "00001";
            SimplifiedBill bill = new SimplifiedBill(billID, "Cuota Mesual Noviembre 2015", 79, DateTime.Today, DateTime.Today.AddMonths(1));
            debtor.AddSimplifiedBill(bill);
            try
            {
                debtor.AddSimplifiedBill(bill);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("The billID already exists", exception.GetMessageWithoutParamName());
                Assert.AreEqual("billID", exception.ParamName);
                throw;
            }
        }
    }
}
