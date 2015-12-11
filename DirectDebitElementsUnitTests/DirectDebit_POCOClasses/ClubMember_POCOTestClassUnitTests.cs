using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing;
using DirectDebitElements;
//using RCNGCMembersManagementAppLogic.MembersManaging;
//using RCNGCMembersManagementAppLogic.Billing;
//using RCNGCMembersManagementMocks;
using ExtensionMethods;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class ClubMember_POCOTestClassUnitTests
    {

        [TestMethod]
        public void InstantiatingASimpleClubMember()
        {
            ClubMember_POCOTestClass clubMember = new ClubMember_POCOTestClass("0002", "Francisco", "Gómez-Caldito", "Viseas");
            Assert.IsNotNull(clubMember);
        }

        [TestMethod]
        public void NamesAndSurnamesAreTrimmed()
        {
            ClubMember_POCOTestClass clubMember = new ClubMember_POCOTestClass("0002", "Francisco   ", "     Gómez-Caldito  ", "        ");
            Assert.AreEqual("Francisco", clubMember.Name);
            Assert.AreEqual("Gómez-Caldito", clubMember.FirstSurname);
            Assert.AreEqual("", clubMember.SecondSurname);
        }

        [TestMethod]
        public void FullNameIsWellCalculatedGivenNameAndBothSurnames()
        {
            ClubMember_POCOTestClass clubMember = new ClubMember_POCOTestClass("0002", "Francisco", "Gómez-Caldito", "Viseas");
            string expectedFullname = "Francisco Gómez-Caldito Viseas";
            Assert.AreEqual(expectedFullname, clubMember.FullName);
        }

        [TestMethod]
        public void FullNameIsWellCalculatedGivenEmptySecondSurname()
        {
            ClubMember_POCOTestClass clubMember = new ClubMember_POCOTestClass("0002", "Francisco", "Gómez-Caldito", "");
            string expectedFullname = "Francisco Gómez-Caldito";
            Assert.AreEqual(expectedFullname, clubMember.FullName);
        }

        [TestMethod]
        public void FullNameIsWellCalculatedGivenNullSecondSurname()
        {
            ClubMember_POCOTestClass clubMember = new ClubMember_POCOTestClass("0002", "Francisco", "Gómez-Caldito", null);
            string expectedFullname = "Francisco Gómez-Caldito";
            Assert.AreEqual(expectedFullname, clubMember.FullName);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void EmptyNameThrowsException()
        {
            try
            {
                ClubMember_POCOTestClass clubMebber = new ClubMember_POCOTestClass("0002", "", "Gómez-Caldito", "Viseas");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Club Member name cannot be empty", exception.GetMessageWithoutParamName());
                Assert.AreEqual("name", exception.ParamName);
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void NameWithOnlySpacesThrowsException()
        {
            try
            {
                ClubMember_POCOTestClass clubMebber = new ClubMember_POCOTestClass("0002", "    ", "Gómez-Caldito", "Viseas");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Club Member name cannot be empty", exception.GetMessageWithoutParamName());
                Assert.AreEqual("name", exception.ParamName);
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void NullNameThrowsException()
        {
            try
            {
                ClubMember_POCOTestClass clubMebber = new ClubMember_POCOTestClass("0002", null, "Gómez-Caldito", "Viseas");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Club Member name cannot be empty", exception.GetMessageWithoutParamName());
                Assert.AreEqual("name", exception.ParamName);
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void EmptyFirstSurnameThrowsException()
        {
            try
            {
                ClubMember_POCOTestClass clubMebber = new ClubMember_POCOTestClass("0002", "Francisco", "", "Viseas");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Club Member first surname cannot be empty", exception.GetMessageWithoutParamName());
                Assert.AreEqual("firstSurname", exception.ParamName);
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void FirstSurnameWithOnlySpacesThrowsException()
        {
            try
            {
                ClubMember_POCOTestClass clubMebber = new ClubMember_POCOTestClass("0002", "Francisco", "     ", "Viseas");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Club Member first surname cannot be empty", exception.GetMessageWithoutParamName());
                Assert.AreEqual("firstSurname", exception.ParamName);
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void NullFirstSurnameThrowsException()
        {
            try
            {
                ClubMember_POCOTestClass clubMember = new ClubMember_POCOTestClass("0002", "Francisco", null, "Viseas");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Club Member first surname cannot be empty", exception.GetMessageWithoutParamName());
                Assert.AreEqual("firstSurname", exception.ParamName);
                throw exception;
            }
        }

        [TestMethod]
        public void TheClubMemberIntitialDefaultPaymentMathodIsCashPayment()
        {
            ClubMember_POCOTestClass clubMember = new ClubMember_POCOTestClass("0001", "Francisco", "Gomez-Caldito", "Viseas");
            Assert.AreEqual(typeof(CashPaymentMethod), clubMember.DefaultPaymentMethod.GetType());
        }

        [TestMethod]
        public void InitiallyAClubMemberHasNoAssociatedDirectDebitMandates()
        {
            ClubMember_POCOTestClass clubMember = new ClubMember_POCOTestClass("0001", "Francisco", "Gomez-Caldito", "Viseas");
            Assert.AreEqual(0, clubMember.DirectDebitmandates.Count);
        }

        [TestMethod]
        public void ADirectDebitMandateCanBeAddedToClubMember()
        {
            ClubMember_POCOTestClass clubMember = new ClubMember_POCOTestClass("0001", "Francisco", "Gomez-Caldito", "Viseas");
            ClientAccountCodeCCC ccc = new ClientAccountCodeCCC("12345678061234567890");
            BankAccount bankAccount = new BankAccount(ccc);
            int internalReferenceNumber = 1111;
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(internalReferenceNumber, DateTime.Today.AddMonths(-1), bankAccount, clubMember.FullName);

            clubMember.AddDirectDebitMandate(directDebitMandate);
            Assert.IsNotNull(clubMember.DirectDebitmandates[internalReferenceNumber]);
        }

        [TestMethod]
        public void DuplicatedDirectDebitMandatesAreOverwriten()
        {
            ClubMember_POCOTestClass clubMember = new ClubMember_POCOTestClass("0001", "Francisco", "Gomez-Caldito", "Viseas");
            ClientAccountCodeCCC ccc = new ClientAccountCodeCCC("12345678061234567890");
            BankAccount bankAccount = new BankAccount(ccc);
            int internalReferenceNumber = 1111;
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(internalReferenceNumber, DateTime.Today.AddMonths(-1), bankAccount, clubMember.FullName);
            clubMember.AddDirectDebitMandate(directDebitMandate);
            Assert.IsNotNull(clubMember.DirectDebitmandates[internalReferenceNumber]);

            ClientAccountCodeCCC newCCC = new ClientAccountCodeCCC("11112222003333333333");
            BankAccount newBankAccount = new BankAccount(newCCC);
            int newInternalReferenceNumber = 1111;
            DirectDebitMandate newDirectDebitMandate = new DirectDebitMandate(newInternalReferenceNumber, DateTime.Today.AddMonths(-1), bankAccount, clubMember.FullName);
            clubMember.AddDirectDebitMandate(directDebitMandate);
            Assert.IsNotNull("11112222003333333333", clubMember.DirectDebitmandates[1111].BankAccount.CCC.CCC);
        }

        [TestMethod]
        public void TheClubMemberDefaultPaymentMathodCanBeChanged()
        {
            ClubMember_POCOTestClass clubMember = new ClubMember_POCOTestClass("0001", "Francisco", "Gomez-Caldito", "Viseas");
            ClientAccountCodeCCC ccc = new ClientAccountCodeCCC("12345678061234567890");
            BankAccount bankAccount = new BankAccount(ccc);
            int internalReferenceNumber = 1111;
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(internalReferenceNumber, DateTime.Today.AddMonths(-1), bankAccount, clubMember.FullName);
            PaymentMethod defaultPaymentMethod = new DirectDebitPaymentMethod(directDebitMandate, "000001");

            clubMember.SetDefaultPaymentMethod(defaultPaymentMethod);

            Assert.AreEqual(typeof(DirectDebitPaymentMethod), clubMember.DefaultPaymentMethod.GetType());
        }

        [TestMethod]
        public void WhenChangingDefaultPaymentMethodToDirectDebitTheIncludedMandateIsAddedToMandatesList()
        {
            ClubMember_POCOTestClass clubMember = new ClubMember_POCOTestClass("0001", "Francisco", "Gomez-Caldito", "Viseas");
            ClientAccountCodeCCC ccc = new ClientAccountCodeCCC("12345678061234567890");
            BankAccount bankAccount = new BankAccount(ccc);
            int internalReferenceNumber = 1111;
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(internalReferenceNumber, DateTime.Today.AddMonths(-1), bankAccount, clubMember.FullName);
            PaymentMethod defaultPaymentMethod = new DirectDebitPaymentMethod(directDebitMandate, "000001");

            clubMember.SetDefaultPaymentMethod(defaultPaymentMethod);

            Assert.IsNotNull(clubMember.DirectDebitmandates[internalReferenceNumber]);
        }

        [TestMethod]
        public void InitiallyAClubMemberHasNoBillsAssociated()
        {
            ClubMember_POCOTestClass clubMember = new ClubMember_POCOTestClass("0001", "Francisco", "Gomez-Caldito", "Viseas");
            Assert.AreEqual(0, clubMember.SimplifiedBills.Count);
        }

        [TestMethod]
        public void ASimpleBillIsCorrectlyAddedToClubMember()
        {
            ClubMember_POCOTestClass clubMember = new ClubMember_POCOTestClass("0001", "Francisco", "Gomez-Caldito", "Viseas");
            string billID = "00001";
            SimplifiedBill bill = new SimplifiedBill(billID, "Cuota Mesual Noviembre 2015", 79, DateTime.Today, DateTime.Today.AddMonths(1));
            clubMember.AddSimplifiedBill(bill);
            Assert.IsNotNull(clubMember.SimplifiedBills[billID]);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void DuplicatedBillIDsCauseExceptionRaise()
        {
            ClubMember_POCOTestClass clubMember = new ClubMember_POCOTestClass("0001", "Francisco", "Gomez-Caldito", "Viseas");
            string billID = "00001";
            SimplifiedBill bill = new SimplifiedBill(billID, "Cuota Mesual Noviembre 2015", 79, DateTime.Today, DateTime.Today.AddMonths(1));
            clubMember.AddSimplifiedBill(bill);
            try
            {
                clubMember.AddSimplifiedBill(bill);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("The billID already exists", exception.GetMessageWithoutParamName());
                Assert.AreEqual("billID", exception.ParamName);
                throw exception;
            }
        }
    }
}
