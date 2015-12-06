using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using RCNGCMembersManagementAppLogic;
using RCNGCMembersManagementAppLogic.MembersManaging;
using RCNGCMembersManagementAppLogic.Billing;
using RCNGCMembersManagementMocks;


namespace RCNGCMembersManagementUnitTests
{
    [TestClass]
    public class InvoicesManagerUnitTests
    {
        static ClubMemberDataManager clubMemberDataManager;
        static BillingDataManager billDataManager;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            MembersSequenceNumberMock membersSequenceNumbersMock = new MembersSequenceNumberMock();
            clubMemberDataManager = ClubMemberDataManager.Instance;
            clubMemberDataManager.SetMembersSequenceNumberCollaborator(membersSequenceNumbersMock);

            BillingSequenceNumbersMock billingSequenceNumbersMock = new BillingSequenceNumbersMock();
            billDataManager = BillingDataManager.Instance;
            billDataManager.SetBillingSequenceNumberCollaborator(billingSequenceNumbersMock);
        }

        [TestMethod]
        public void AnInvoiceIsCorrectlyAdded()
        {
            clubMemberDataManager.MemberIDSequenceNumber = 5;
            ClubMember clubMember = new ClubMember("Francisco", "Gomez-Caldito", "Viseas");
            Assert.AreEqual(0, clubMember.InvoicesList.Count);

            InvoiceCustomerData invoiceCustomerData = new InvoiceCustomerData(clubMember);
            Invoice invoice = new Invoice(
                new InvoiceCustomerData(clubMember),
                new List<Transaction>() { new Transaction("Kajak Rent", 1, 50, new Tax("No Tax", 0), 0) },
                DateTime.Now);
            InvoicesManager invoicesManager = new InvoicesManager();
            invoicesManager.AddInvoiceToClubMember(invoice, clubMember);
            Assert.IsNotNull(clubMember.InvoicesList[invoice.InvoiceID]);
        }

        [TestMethod]
        public void AnInvoiceIsCorrectlyCancelled()
        {
            clubMemberDataManager.MemberIDSequenceNumber = 5;
            ClubMember clubMember = new ClubMember("Francisco", "Gomez-Caldito", "Viseas");
            InvoiceCustomerData invoiceCustomerData = new InvoiceCustomerData(clubMember);
            Invoice invoice = new Invoice(
                new InvoiceCustomerData(clubMember),
                new List<Transaction>() { new Transaction("Kajak Rent", 1, 50, new Tax("No Tax", 0), 0) },
                DateTime.Now);
            InvoicesManager invoicesManager = new InvoicesManager();
            invoicesManager.AddInvoiceToClubMember(invoice, clubMember);
            invoicesManager.CancelInvoice(invoice, clubMember);
            Assert.AreEqual(Invoice.InvoicePaymentState.Cancelled, invoice.InvoiceState);
        }

        [TestMethod]
        public void WhenCancellingAnInvoiceAnAmendingInvoiceIsCreatedAndAssignedToTheDebtor()
        {
            clubMemberDataManager.MemberIDSequenceNumber = 5;
            billDataManager.InvoiceSequenceNumber = 5;
            ClubMember clubMember = new ClubMember("Francisco", "Gomez-Caldito", "Viseas");
            InvoiceCustomerData invoiceCustomerData = new InvoiceCustomerData(clubMember);
            Invoice invoice = new Invoice(
                new InvoiceCustomerData(clubMember),
                new List<Transaction>() { new Transaction("Kajak Rent", 1, 50, new Tax("No Tax", 0), 0) },
                DateTime.Now);
            InvoicesManager invoicesManager = new InvoicesManager();
            invoicesManager.AddInvoiceToClubMember(invoice, clubMember);
            Assert.AreEqual(0, clubMember.AmendingInvoicesList.Count);
            invoicesManager.CancelInvoice(invoice, clubMember);
            Assert.IsNotNull(clubMember.AmendingInvoicesList["AMN2013000005"]);
        }


    }
}
