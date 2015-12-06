using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementAppLogic.MembersManaging;
using RCNGCMembersManagementAppLogic.Billing;
using RCNGCMembersManagementMocks;
using ExtensionMethods;

namespace RCNGCMembersManagementUnitTests.MembersManaging
{
    [TestClass]
    public class ClubMemberUnitTests
    {

        static ClubMemberDataManager clubMemberDataManager;
        static BillingDataManager billDataManager;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            MembersSequenceNumberMock membersSequenceNumberMock = new MembersSequenceNumberMock();
            clubMemberDataManager = ClubMemberDataManager.Instance;
            clubMemberDataManager.SetMembersSequenceNumberCollaborator(membersSequenceNumberMock);

            BillingSequenceNumbersMock billingSequenceNumbersMock = new BillingSequenceNumbersMock();
            billDataManager = BillingDataManager.Instance;
            billDataManager.SetBillingSequenceNumberCollaborator(billingSequenceNumbersMock);
        }

        [TestMethod]
        public void InstantiatingASimpleClubMember()
        {
            ClubMember clubMember = new ClubMember("0002", "Francisco", "Gómez-Caldito", "Viseas");
            Assert.IsNotNull(clubMember);
        }

        [TestMethod]
        public void NamesAndSurnamesAreTrimmed()
        {
            ClubMember clubMember = new ClubMember("0002", "Francisco   ", "     Gómez-Caldito  ", "        ");
            Assert.AreEqual("Francisco", clubMember.Name);
            Assert.AreEqual("Gómez-Caldito", clubMember.FirstSurname);
            Assert.AreEqual("", clubMember.SecondSurname);
        }

        [TestMethod]
        public void FullNameIsWellCalculatedGivenNameAndBothSurnames()
        {
            ClubMember clubMember = new ClubMember("0002", "Francisco", "Gómez-Caldito", "Viseas");
            string expectedFullname = "Francisco Gómez-Caldito Viseas";
            Assert.AreEqual(expectedFullname, clubMember.FullName);
        }

        [TestMethod]
        public void FullNameIsWellCalculatedGivenEmptySecondSurname()
        {
            ClubMember clubMember = new ClubMember("0002", "Francisco", "Gómez-Caldito", "");
            string expectedFullname = "Francisco Gómez-Caldito";
            Assert.AreEqual(expectedFullname, clubMember.FullName);
        }

        [TestMethod]
        public void FullNameIsWellCalculatedGivenNullSecondSurname()
        {
            ClubMember clubMember = new ClubMember("0002", "Francisco", "Gómez-Caldito", null);
            string expectedFullname = "Francisco Gómez-Caldito";
            Assert.AreEqual(expectedFullname, clubMember.FullName);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void EmptyNameThrowsException()
        {
            try
            {
                ClubMember clubMebber = new ClubMember("0002", "", "Gómez-Caldito", "Viseas");
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
                ClubMember clubMebber = new ClubMember("0002", "    ", "Gómez-Caldito", "Viseas");
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
                ClubMember clubMebber = new ClubMember("0002", null, "Gómez-Caldito", "Viseas");
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
                ClubMember clubMebber = new ClubMember("0002", "Francisco", "", "Viseas");
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
                ClubMember clubMebber = new ClubMember("0002", "Francisco", "     ", "Viseas");
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
                ClubMember clubMebber = new ClubMember("0002", "Francisco", null, "Viseas");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Club Member first surname cannot be empty", exception.GetMessageWithoutParamName());
                Assert.AreEqual("firstSurname", exception.ParamName);
                throw exception;
            }
        }

        [TestMethod]
        public void ThereIsAMembersIDCounterWichICanSet()
        {
            clubMemberDataManager.MemberIDSequenceNumber = 2;
            Assert.AreEqual((uint)2, clubMemberDataManager.MemberIDSequenceNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void ICantSetMemberIDSequenceToZero()
        {
            try
            {
                clubMemberDataManager.MemberIDSequenceNumber = 0;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.AreEqual("Member ID out of range (1-99999)", exception.GetMessageWithoutParamName());
                Assert.AreEqual("memberIDSequenceNumber", exception.ParamName);
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void ICantSetMemberIDSequenceToNegative()
        {
            try
            {
                var value = -5;
                clubMemberDataManager.MemberIDSequenceNumber = (uint)value;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.AreEqual("Member ID out of range (1-99999)", exception.GetMessageWithoutParamName());
                Assert.AreEqual("memberIDSequenceNumber", exception.ParamName);
                throw exception;
            }
        }

        [TestMethod]
        public void WhenSettingMemberIDSequenceToUpperLimit_TopRangePlusOne_ThereAreNoMoreAvailableMembersID()
        {
            clubMemberDataManager.MemberIDSequenceNumber = 100000;
            Assert.AreEqual((uint)100000, clubMemberDataManager.MemberIDSequenceNumber);
            Assert.AreEqual(true, clubMemberDataManager.AvailableMembersIDAreExhausted);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CantSetMemberIDOvertheUpperLimit()
        {
            try
            {
                clubMemberDataManager.MemberIDSequenceNumber = 100001;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.AreEqual("Member ID out of range (1-99999)", exception.GetMessageWithoutParamName());
                Assert.AreEqual("memberIDSequenceNumber", exception.ParamName);
                throw exception;
            }
        }

        [TestMethod]
        public void WhenAddingANewMemberTheIDIsCreated()
        {
            clubMemberDataManager.MemberIDSequenceNumber = 5;
            ClubMember clubMember = new ClubMember("Francisco","Gomez-Caldito","Viseas");
            Assert.AreEqual("00005", clubMember.MemberID);
        }

        [TestMethod]
        public void WhenAddingANewMemberTheMemberIDSequenceNumberIsIncreasedByOne()
        {
            clubMemberDataManager.MemberIDSequenceNumber = 5;
            ClubMember clubMember = new ClubMember("Francisco", "Gomez-Caldito", "Viseas");
            Assert.AreEqual((uint)6, clubMemberDataManager.MemberIDSequenceNumber);
        }

        [TestMethod]
        public void WhenMember99999IsCreatedMemberIDSequenceNumberIsSetUpperLimit_ToMaxValuePlusOne_()
        {
            clubMemberDataManager.MemberIDSequenceNumber = 99999;
            ClubMember clubMember = new ClubMember("Francisco", "Gomez-Caldito", "Viseas");
            Assert.AreEqual("99999", clubMember.MemberID);
            Assert.AreEqual((uint)100000, clubMemberDataManager.MemberIDSequenceNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void IfITryToCreateMemeber100000AnExceptionIsThrown()
        {
            try
            {
                clubMemberDataManager.MemberIDSequenceNumber = 100000;
                ClubMember clubMember = new ClubMember("Francisco", "Gomez-Caldito", "Viseas");
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.AreEqual("Member ID out of range (1-99999)", exception.GetMessageWithoutParamName());
                Assert.AreEqual("memberIDSequenceNumber", exception.ParamName);
                throw exception;
            }
        }

        [TestMethod]
        public void IfITryToCreateMemeber100000TheMemberIsNotCreated()
        {
            ClubMember clubMember = null;
            try
            {
                clubMemberDataManager.MemberIDSequenceNumber = 100000;
                clubMember = new ClubMember("Francisco", "Gomez-Caldito", "Viseas");
            }
            catch 
            {
                Assert.IsNull(clubMember);
            }
        }

        [TestMethod]
        public void IfITryToCreateMemeber100000TheSequenceNumberIsNotIncreased()
        {
            try
            {
                clubMemberDataManager.MemberIDSequenceNumber = 100000;
                ClubMember clubMember = new ClubMember("Francisco", "Gomez-Caldito", "Viseas");
            }
            catch 
            {
                Assert.AreEqual((uint)100000, clubMemberDataManager.MemberIDSequenceNumber);
            }
        }

        [TestMethod]
        public void AnInvoiceIsCorrectlyAddedToClubMember()
        {
            clubMemberDataManager.MemberIDSequenceNumber = 5;
            ClubMember clubMember = new ClubMember("Francisco", "Gomez-Caldito", "Viseas");
            Assert.AreEqual(0, clubMember.InvoicesList.Count);

            InvoiceCustomerData invoiceCustomerData = new InvoiceCustomerData(clubMember);
            Invoice invoice = new Invoice(
                new InvoiceCustomerData(clubMember),
                new List<Transaction>() { new Transaction("Kajak Rent", 1, 50, new Tax("No Tax", 0), 0) },
                DateTime.Now);
            clubMember.AddInvoice(invoice);
            Assert.IsNotNull(clubMember.InvoicesList[invoice.InvoiceID]);
        }

        [TestMethod]
        public void AmendingInvoiceIsCorrectlyAddedToClubMember()
        {
            clubMemberDataManager.MemberIDSequenceNumber = 5;
            ClubMember clubMember = new ClubMember("Francisco", "Gomez-Caldito", "Viseas");
            Assert.AreEqual(0, clubMember.InvoicesList.Count);

            InvoiceCustomerData invoiceCustomerData = new InvoiceCustomerData(clubMember);
            Invoice invoice = new Invoice(
                new InvoiceCustomerData(clubMember),
                new List<Transaction>() { new Transaction("Kajak Rent", 1, 50, new Tax("No Tax", 0), 0) },
                DateTime.Now);
            AmendingInvoice amendingInvoice = new AmendingInvoice(invoice);
            clubMember.AddAmendingInvoice(amendingInvoice);
            Assert.IsNotNull(clubMember.AmendingInvoicesList[amendingInvoice.InvoiceID]);
        }
    }
}
