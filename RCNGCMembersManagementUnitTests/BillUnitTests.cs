using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementAppLogic;
using RCNGCMembersManagementAppLogic.Billing;
using RCNGCMembersManagementAppLogic.Billing.DirectDebit;
using RCNGCMembersManagementAppLogic.MembersManaging;
using RCNGCMembersManagementMocks;
using ExtensionMethods;

namespace RCNGCMembersManagementUnitTests.Billing
{
    [TestClass]
    public class BillUnitTests
    {
        static List<Transaction> transactionList;        
        static ClubMember clubMember;
        static InvoiceCustomerData invoiceCustomerData;
        
        BillingDataManager billingDataManager;
        List<Bill> unassignedBillsList;


        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            BillingSequenceNumbersMock invoiceDataManagerMock = new BillingSequenceNumbersMock();
            BillingDataManager.Instance.SetBillingSequenceNumberCollaborator(invoiceDataManagerMock);

            transactionList = new List<Transaction>()
            {
                {new Transaction("Monthly Fee",1,80,new Tax("NOIGIC",0),0)},
                {new Transaction("Renting a Kajak",1,50,new Tax("NOIGIC",0),0)},
                {new Transaction("Blue cup",2,10,new Tax("NOIGIC",0),0)},
                {new Transaction("BIG Mouring",1,500,new Tax("NOIGIC",0),0)}
            };

            clubMember = new ClubMember("0002", "Francisco", "Gomez", "");
            invoiceCustomerData = new InvoiceCustomerData(clubMember);
        }

        [TestInitialize]
        public void IntializeTests()
        {
            billingDataManager = BillingDataManager.Instance;
            billingDataManager.InvoiceSequenceNumber = 5000;

            unassignedBillsList = new List<Bill>()
            {
                {new Bill("First Instalment", 200, DateTime.Now, DateTime.Now.AddDays(30))},
                {new Bill("Second Instalment", 200, DateTime.Now, DateTime.Now.AddDays(60))},
                {new Bill("Third Instalment", 250, DateTime.Now, DateTime.Now.AddDays(90))}
            };
        }

        [TestMethod]
        public void ABillIsCorrectlyCreated()
        {
            DateTime issueDate = new DateTime(2013, 11, 11);
            DateTime dueDate = issueDate.AddYears(10);
            Bill bill = new Bill("MMM201300015/001", "An easy to pay bill", 1, issueDate, dueDate);
            Assert.AreEqual("MMM201300015/001", bill.BillID);
            Assert.AreEqual("An easy to pay bill", bill.Description);
            Assert.AreEqual((decimal)1, bill.Amount);
            Assert.AreEqual(issueDate, bill.IssueDate);
            Assert.AreEqual(dueDate, bill.DueDate);
        }

        [TestMethod]
        public void ABillShouldHaveABillID()
        {
            Bill bill = new Bill("MMM201300015/001", "An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            Assert.AreEqual("MMM201300015/001", bill.BillID);
        }

        [TestMethod]
        public void BillIDCanBeNullForLaterInitializationWhenAssignedToInvoices()
        {
            Bill bill = new Bill("An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            Assert.IsNull(bill.BillID);
        }

        [TestMethod]
        public void WhenCreatingANewInvoiceASingleBillIsCreated()
        {
            Invoice invoice = new Invoice(invoiceCustomerData, transactionList, DateTime.Now);
            Assert.AreEqual(1, invoice.Bills.Count);
        }

        [TestMethod]
        public void ABillOf650IsAutomaticallyCreatedForAnInvoiceOf650NetAmount()
        {
            Invoice invoice = new Invoice(invoiceCustomerData, transactionList, DateTime.Now);
            Assert.AreEqual(650, invoice.Bills.Values.ElementAt(0).Amount);
        }

        [TestMethod]
        public void WhenCreatingAnInvoiceItProvidesTheBillIDtoItsAssociatedBill()
        {
            Invoice invoice = new Invoice(invoiceCustomerData, transactionList, DateTime.Now);
            Assert.IsNotNull(invoice.Bills.Values.ElementAt(0).BillID);
        }

        [TestMethod]
        public void IfTheInvoiceIDIsDINV2013005000ThenTheAutomaticallyCreatedBillIDIsINV2013005000_001()
        {
            Invoice invoice = new Invoice(invoiceCustomerData, transactionList, DateTime.Now);
            Assert.AreEqual("INV2013005000/001", invoice.Bills.Values.ElementAt(0).BillID);
        }

        [TestMethod]
        public void ICanLoadAListOfExistingBillsWhenCreatingAnInvoice()
        {
            string invoiceID = "MMM2013005001";
            List<Bill> assignedBillsList = new List<Bill>(unassignedBillsList);
            assignedBillsList[0].BillID = "MMM2013005001/001";
            assignedBillsList[1].BillID = "MMM2013005001/002";
            assignedBillsList[2].BillID = "MMM2013005001/003";
            Invoice invoice = new Invoice(invoiceID, invoiceCustomerData, transactionList, assignedBillsList, DateTime.Now);
            Assert.AreEqual(3, invoice.Bills.Count);
        }

        [TestMethod]
        public void TheBillsListTotalMustEqualTheTheInvoiceNetAmoutWhenProvided()
        {  
            string invoiceID = "MMM2013005001";
            List<Bill> assignedBillsList = new List<Bill>(unassignedBillsList);
            assignedBillsList[0].BillID = "MMM2013005001/001";
            assignedBillsList[1].BillID = "MMM2013005001/002";
            assignedBillsList[2].BillID = "MMM2013005001/003";
            decimal billsTotalAmount = assignedBillsList.Select(bill => bill.Amount).Sum();
            Invoice invoice = new Invoice(invoiceID, invoiceCustomerData, transactionList, assignedBillsList, DateTime.Now);
            decimal invoiceInitialAmount = invoice.NetAmount;
            Assert.AreEqual(billsTotalAmount, invoiceInitialAmount);
        }

        [TestMethod]
        public void APaymentAgreementIsCorrectlyCreated()
        {
            PaymentAgreement paymentAgreement = new PaymentAgreement("Club President", "New Payment Agreement", new DateTime(2013, 11, 11));
            Assert.AreEqual("Club President", paymentAgreement.AuthorizingPerson);
            Assert.AreEqual("New Payment Agreement", paymentAgreement.AgreementTerms);
            Assert.AreEqual(new DateTime(2013,11,11), paymentAgreement.AgreementDate);
        }

        [TestMethod]
        public void ICanReplaceASetOfBillsInAnInvoiceWithASetOfNewBillsThatAddTheSameAmountByAddingABillPaymentAgreement()
        {
            decimal invoiceInitialAmount;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionList, DateTime.Now);
            invoiceInitialAmount = invoice.NetAmount;
            string authorizingPerson = "Club President";
            string agreementTerms = "New Payment Agreement";
            DateTime agreementDate = DateTime.Now.Date;
            PaymentAgreement paymentAgreement = new PaymentAgreement(authorizingPerson, agreementTerms, agreementDate);
            List<Bill> billsToRenegotiate = new List<Bill>() {invoice.Bills["INV2013005000/001"]};
            List<Bill> billsToAdd = new List<Bill>(unassignedBillsList);
            invoice.RenegotiateBillsIntoInstalments(paymentAgreement, billsToRenegotiate, billsToAdd);
            Assert.AreEqual(invoiceInitialAmount, invoice.NetAmount);
        }

        [TestMethod]
        public void WhenReplacingBillsTheOldBillsAreMarkedAsRenegotiated()
        {
            Invoice invoice = new Invoice(invoiceCustomerData, transactionList, DateTime.Now);
            string authorizingPerson = "Club President";
            string agreementTerms = "New Payment Agreement";
            DateTime agreementDate = DateTime.Now;
            PaymentAgreement paymentAgreement = new PaymentAgreement(authorizingPerson, agreementTerms, agreementDate);
            List<Bill> billsToRenegotiate = new List<Bill>() { invoice.Bills["INV2013005000/001"] };
            List<Bill> billsToAdd = new List<Bill>(unassignedBillsList);
            invoice.RenegotiateBillsIntoInstalments(paymentAgreement, billsToRenegotiate, billsToAdd);
            billsToRenegotiate.ForEach(bill => Assert.AreEqual(Bill.BillPaymentResult.Renegotiated, bill.PaymentResult));
        }

        [TestMethod]
        public void WhenReplacingBillsThePaymentAgreementIsAssociatedToReplacedAndReplacingBills()
        {
            Invoice invoice = new Invoice(invoiceCustomerData, transactionList, DateTime.Now);
            string authorizingPerson = "Club President";
            string agreementTerms = "New Payment Agreement";
            DateTime agreementDate = DateTime.Now.Date;
            PaymentAgreement paymentAgreement = new PaymentAgreement(authorizingPerson, agreementTerms, agreementDate);
            List<Bill> billsToRenegotiate = new List<Bill>() { invoice.Bills["INV2013005000/001"] };
            List<Bill> billsToAdd = new List<Bill>(unassignedBillsList);
            invoice.RenegotiateBillsIntoInstalments(paymentAgreement, billsToRenegotiate, billsToAdd);
            billsToRenegotiate.ForEach(bill => Assert.AreEqual(Bill.BillPaymentResult.Renegotiated, bill.PaymentResult));
            billsToRenegotiate.ForEach(bill => Assert.AreEqual("New Payment Agreement", bill.RenegotiationAgreement.AgreementTerms));
            billsToAdd.ForEach(bill => Assert.AreEqual("New Payment Agreement", bill.PaymentAgreements[agreementDate].AgreementTerms));
        }

        [TestMethod]
        public void TheBillIDOfTheReplpacingBillsAreCalculatedByTheInvoiceAndHaveConsecutiveNumbers()
        {
            Invoice invoice = new Invoice(invoiceCustomerData, transactionList, DateTime.Now);
            string authorizingPerson = "Club President";
            string agreementTerms = "New Payment Agreement";
            DateTime agreementDate = DateTime.Now;
            PaymentAgreement paymentAgreement = new PaymentAgreement(authorizingPerson, agreementTerms, agreementDate);
            List<Bill> billsToRenegotiate = new List<Bill>() { invoice.Bills["INV2013005000/001"] };
            List<Bill> billsToAdd = new List<Bill>(unassignedBillsList);
            invoice.RenegotiateBillsIntoInstalments(paymentAgreement, billsToRenegotiate, billsToAdd);
            Assert.AreEqual("INV2013005000/002", billsToAdd[0].BillID);
            Assert.AreEqual("INV2013005000/003", billsToAdd[1].BillID);
            Assert.AreEqual("INV2013005000/004", billsToAdd[2].BillID);
        }

        [TestMethod]
        public void ByDefaultABillIsGeneratedWithoutAPaymentMethod()
        {
            Bill bill = new Bill("MMM201300015/001","An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            Assert.IsNull(bill.AssignedPaymentMethod);
        }

        [TestMethod]
        public void AsigningAPaymentMethodToANewlyCreatedBill()
        {
            Bill bill = new Bill("MMM201300015/001", "An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            bill.AssignedPaymentMethod = new CashPaymentMethod();
            Assert.AreEqual(typeof(CashPaymentMethod),bill.AssignedPaymentMethod.GetType());
        }

        [TestMethod]
        public void APaymentIsCorrectlyCreated()
        {
            CashPaymentMethod cashPayment = new CashPaymentMethod();
            DateTime paymentDate = new DateTime(2013,11,11);
            Payment payment= new Payment((decimal)100, paymentDate , cashPayment);
            Assert.AreEqual((decimal)100, payment.PaymentAmount);
            Assert.AreEqual(paymentDate, payment.PaymentDate);
            Assert.AreEqual(cashPayment, payment.PaymentMethod);        
        }

        [TestMethod]
        public void WhenPayingABillTheBillIsSetAsPaid()
        {
            Bill bill = new Bill("MMM201300015/001", "An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            CashPaymentMethod cashPayment = new CashPaymentMethod();
            DateTime paymentDate = new DateTime(2013, 11, 11);
            Payment payment = new Payment(bill.Amount, paymentDate, cashPayment);
            bill.PayBill(payment);
            Assert.AreEqual(Bill.BillPaymentResult.Paid, bill.PaymentResult);
        }

        [TestMethod]
        public void WhenPayingABillTheBillAPaymentIsAssignedToTheBill()
        {
            Bill bill = new Bill("MMM201300015/001", "An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            CashPaymentMethod cashPayment = new CashPaymentMethod();
            DateTime paymentDate = new DateTime(2013, 11, 11);
            Payment payment = new Payment(bill.Amount, paymentDate, cashPayment);
            bill.PayBill(payment);
            Assert.AreEqual(payment, bill.Payment);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void OnlyPaymentsForTheTotalBillAmoutAreAccepted()
        {
            Bill bill = new Bill("MMM201300015/001", "An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            CashPaymentMethod cashPaymentMethod = new CashPaymentMethod();
            DateTime paymentDate = new DateTime(2013, 11, 11);
            Payment payment = new Payment((decimal)2, paymentDate, cashPaymentMethod);
            try
            {
                bill.PayBill(payment);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Only payments for the bill total amount are accepted", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        public void WhenPayingABillTheBillPaymentDateIsStored()
        {
            Bill bill = new Bill("MMM201300015/001", "An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            CashPaymentMethod cashPayment = new CashPaymentMethod();
            DateTime paymentDate = new DateTime(2013, 11, 11);
            Payment payment = new Payment(bill.Amount, paymentDate, cashPayment);
            bill.PayBill(payment);
            Assert.AreEqual(paymentDate, bill.Payment.PaymentDate);
        }

        [TestMethod]
        public void WhenPayingABillTheInvoiceTotalAmountToCollectIsCorrectlyUpdates()
        {
            string invoiceID = "MMM2013005001";
            List<Bill> assignedBillsList = new List<Bill>(unassignedBillsList);
            assignedBillsList[0].BillID = "MMM2013005001/001";
            assignedBillsList[1].BillID = "MMM2013005001/002";
            assignedBillsList[2].BillID = "MMM2013005001/003";
            Invoice invoice = new Invoice(invoiceID, invoiceCustomerData, transactionList, assignedBillsList, DateTime.Now);
            Assert.AreEqual((decimal)650, invoice.BillsTotalAmountToCollect);
            CashPaymentMethod cashPayment = new CashPaymentMethod();
            DateTime paymentDate = new DateTime(2013, 11, 11);
            Payment payment = new Payment((decimal)200, paymentDate, cashPayment);
            invoice.Bills["MMM2013005001/001"].PayBill(payment);
            Assert.AreEqual((decimal)450, invoice.BillsTotalAmountToCollect);            
        }

        [TestMethod]
        public void WhenABillIsPaidOnCashTheBillPaymentMethodIsSetAsCash()
        {
            Bill bill = new Bill("MMM201300015/001", "An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            CashPaymentMethod cashPayment = new CashPaymentMethod();
            DateTime paymentDate = new DateTime(2013, 11, 11);
            Payment payment = new Payment(bill.Amount, paymentDate, cashPayment);
            bill.PayBill(payment);
            Assert.AreEqual(cashPayment, bill.Payment.PaymentMethod);
        }

        [TestMethod]
        public void ABankTransferPaymentMethodIsCorrectlyCreated()
        {
            BankAccount transferorAccount = new BankAccount(new ClientAccountCodeCCC("20381111401111111111"));
            BankAccount transfereeAccount = new BankAccount(new ClientAccountCodeCCC("21001111301111111111"));
            BankTransferPaymentMethod bankTransferPaymentMethod = new BankTransferPaymentMethod(transferorAccount, transfereeAccount);
            Assert.AreEqual(transferorAccount, bankTransferPaymentMethod.TransferorAccount);
            Assert.AreEqual(transfereeAccount, bankTransferPaymentMethod.TransfereeAccount);
        }

        [TestMethod]
        public void WhenABillIsPaidByBankTransferTheBillPaymentMethodIsSetAsPaidByBankTransfer()
        {
            Bill bill = new Bill("MMM201300015/001", "An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            BankAccount transferorAccount = new BankAccount(new ClientAccountCodeCCC("20381111401111111111"));
            BankAccount transfereeAccount = new BankAccount(new ClientAccountCodeCCC("21001111301111111111"));
            BankTransferPaymentMethod bankTransferPaymentMethod = new BankTransferPaymentMethod(transferorAccount, transfereeAccount);
            DateTime paymentDate = new DateTime(2013, 11, 11);
            Payment payment = new Payment(bill.Amount, paymentDate, bankTransferPaymentMethod);
            bill.PayBill(payment);
            Assert.AreEqual(bankTransferPaymentMethod, bill.Payment.PaymentMethod);
        }

        [TestMethod]
        public void WhenABillIsPaidByBankTransferTheTransferorAndTheTransfereeAccountsAreStored()
        {
            Bill bill = new Bill("MMM201300015/001", "An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            BankAccount transferorAccount = new BankAccount(new ClientAccountCodeCCC("20381111401111111111"));
            BankAccount transfereeAccount = new BankAccount(new ClientAccountCodeCCC("21001111301111111111"));
            BankTransferPaymentMethod bankTransferPaymentMethod = new BankTransferPaymentMethod(transferorAccount, transfereeAccount);
            DateTime paymentDate = new DateTime(2013, 11, 11);
            Payment payment = new Payment(bill.Amount, paymentDate, bankTransferPaymentMethod);
            bill.PayBill(payment);
            Assert.AreEqual(transferorAccount, ((BankTransferPaymentMethod)bill.Payment.PaymentMethod).TransferorAccount);
            Assert.AreEqual(transfereeAccount, ((BankTransferPaymentMethod)bill.Payment.PaymentMethod).TransfereeAccount);
        }

        [TestMethod]
        public void ADirectDebitPaymentMethodIsCorrectlyCreated()
        {
            int internalReferenceNumber = 2645;
            BankAccount bankAccount = new BankAccount(new ClientAccountCodeCCC("12345678061234567890"));
            DateTime directDebitMandateCreationDate = new DateTime(2013, 11, 11);
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(internalReferenceNumber, directDebitMandateCreationDate, bankAccount, "NoName");
            string directDebitTransactionPaymentIdentification = "201311110000123456";
            DirectDebitPaymentMethod directDebitPaymentMethod = new DirectDebitPaymentMethod(directDebitMandate, directDebitTransactionPaymentIdentification);
            Assert.AreEqual(directDebitMandate, directDebitPaymentMethod.DirectDebitMandate);
            Assert.AreEqual("201311110000123456", directDebitPaymentMethod.DDTXPaymentIdentification);
        }

        [TestMethod]
        public void WhenABillIsPaidByDirectDebitTheBillPaymentMethodIsSetAsPaidByDirectDebit()
        {
            Bill bill = new Bill("MMM201300015/001", "An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            int internalReferenceNumber = 2645;
            BankAccount bankAccount = new BankAccount(new ClientAccountCodeCCC("12345678061234567890"));
            DateTime directDebitMandateCreationDate = new DateTime(2013, 11, 11);
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(internalReferenceNumber, directDebitMandateCreationDate, bankAccount, "NoName");
            string directDebitTransactionPaymentIdentification = "201311110000123456";
            DirectDebitPaymentMethod directDebitPaymentMethod = new DirectDebitPaymentMethod(directDebitMandate, directDebitTransactionPaymentIdentification);
            DateTime paymentDate = new DateTime(2013, 11, 11);
            Payment payment = new Payment(bill.Amount, paymentDate, directDebitPaymentMethod);
            bill.PayBill(payment);
            Assert.AreEqual(directDebitPaymentMethod, bill.Payment.PaymentMethod);
        }

        [TestMethod]
        public void WhenABillIsPaidByDirectDebitTheDebtorAccountAndTheDirectDebitTransactionPaymentIdentificartionAreStored()
        {
            Bill bill = new Bill("MMM201300015/001", "An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            int internalReferenceNumber = 2645;
            BankAccount bankAccount = new BankAccount(new ClientAccountCodeCCC("12345678061234567890"));
            DateTime directDebitMandateCreationDate = new DateTime(2013, 11, 11);
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(internalReferenceNumber, directDebitMandateCreationDate, bankAccount, "NoName");
            string directDebitTransactionPaymentIdentification = "201311110000123456";
            DirectDebitPaymentMethod directDebitTransfermethod = new DirectDebitPaymentMethod(directDebitMandate, directDebitTransactionPaymentIdentification);
            DateTime paymentDate = new DateTime(2013, 11, 11);
            Payment payment = new Payment(bill.Amount, paymentDate, directDebitTransfermethod);
            bill.PayBill(payment);
            Assert.AreEqual("12345678061234567890", ((DirectDebitPaymentMethod)bill.Payment.PaymentMethod).DirectDebitMandate.BankAccount.CCC.CCC);
            Assert.AreEqual("201311110000123456", ((DirectDebitPaymentMethod)bill.Payment.PaymentMethod).DDTXPaymentIdentification);
        }

        [TestMethod]
        public void ABillPastDueDateIsMarkedAsUnpaid()
        {
            Bill bill = new Bill("MMM201300015/001", "This bill is past due date", 1, new DateTime(2013, 11, 11), new DateTime(2013, 11, 15));
            Assert.AreEqual(Bill.BillPaymentResult.ToCollect, bill.PaymentResult);
            bill.CheckDueDate(new DateTime(2013,11,30));
            Assert.AreEqual(Bill.BillPaymentResult.Unpaid, bill.PaymentResult);
        }

        [TestMethod]
        public void IfTheBillIsNotPastDueDateItKeepsToCollect()
        {
            Bill bill = new Bill("MMM201300015/001", "This bill is not past due date", 1, new DateTime(2013, 11, 01), new DateTime(2013, 12, 01));
            Assert.AreEqual(Bill.BillPaymentResult.ToCollect, bill.PaymentResult);
            bill.CheckDueDate(new DateTime(2013, 11, 30));
            Assert.AreEqual(Bill.BillPaymentResult.ToCollect, bill.PaymentResult);
        }

        [TestMethod]
        public void WhenABillIsUnpaidIfThereIsAnAgreementAssociatedToItTheAgreementIsCancelled()
        {
            Invoice invoice = new Invoice(invoiceCustomerData, transactionList, DateTime.Now);
            string authorizingPerson = "Club President";
            string agreementTerms = "New Payment Agreement";
            DateTime agreementDate = new DateTime(2013,10,1);
            PaymentAgreement paymentAgreement = new PaymentAgreement(authorizingPerson, agreementTerms, agreementDate);
            List<Bill> billsToRenegotiate = new List<Bill>() { invoice.Bills["INV2013005000/001"] };
            List<Bill> billsToAdd = new List<Bill>()
            {
                {new Bill("MMM2013005001/002", "First Instalment", 200, new DateTime(2013,10,1), new DateTime(2013,11,1))},
                {new Bill("MMM2013005001/003", "Second Instalment", 200, new DateTime(2013,10,1), new DateTime(2013,12,1))},
                {new Bill("MMM2013005001/004", "Third Instalment", 250, new DateTime(2013,10,1), new DateTime(2014,1,1))}
            };
            invoice.RenegotiateBillsIntoInstalments(paymentAgreement, billsToRenegotiate, billsToAdd);
            Bill bill = invoice.Bills["MMM2013005001/002"];
            Assert.AreEqual(PaymentAgreement.PaymentAgreementStatus.Active, bill.PaymentAgreements[new DateTime(2013, 10, 1)].PaymentAgreementActualStatus);
            bill.CheckDueDate(new DateTime(2013, 11, 30));
            Assert.AreEqual(PaymentAgreement.PaymentAgreementStatus.NotAcomplished, bill.PaymentAgreements[new DateTime(2013, 10, 1)].PaymentAgreementActualStatus);
        }

        [TestMethod]
        public void IfTheLastBillOfAnAgreementIsPaidTheAgreementIsConsideredAccompllished()
        {
            Invoice invoice = new Invoice(invoiceCustomerData, transactionList, DateTime.Now);
            string authorizingPerson = "Club President";
            string agreementTerms = "New Payment Agreement";
            DateTime agreementDate = new DateTime(2013, 10, 1);
            PaymentAgreement paymentAgreement = new PaymentAgreement(authorizingPerson, agreementTerms, agreementDate);
            List<Bill> billsToRenegotiate = new List<Bill>() { invoice.Bills["INV2013005000/001"] };
            List<Bill> billsToAdd = new List<Bill>()
            {
                {new Bill("MMM2013005001/002", "First Instalment", 200, new DateTime(2013,10,1), new DateTime(2013,11,1))},
                {new Bill("MMM2013005001/003", "Second Instalment", 200, new DateTime(2013,10,1), new DateTime(2013,12,1))},
                {new Bill("MMM2013005001/004", "Third Instalment", 250, new DateTime(2013,10,1), new DateTime(2014,1,1))}
            };
            invoice.RenegotiateBillsIntoInstalments(paymentAgreement, billsToRenegotiate, billsToAdd);
            BillsManager billsManager = new BillsManager();
            CashPaymentMethod cashPayment = new CashPaymentMethod();
            DateTime paymentDate = new DateTime(2013, 11, 11);
            Payment payment200 = new Payment((decimal)200, paymentDate, cashPayment);
            Payment payment250 = new Payment((decimal)250, paymentDate, cashPayment);
            billsManager.PayBill(invoice, invoice.Bills["MMM2013005001/002"], payment200);
            Assert.AreEqual(PaymentAgreement.PaymentAgreementStatus.Active, invoice.Bills["MMM2013005001/002"].PaymentAgreements[agreementDate].PaymentAgreementActualStatus);
            billsManager.PayBill(invoice, invoice.Bills["MMM2013005001/003"], payment200);
            Assert.AreEqual(PaymentAgreement.PaymentAgreementStatus.Active, invoice.Bills["MMM2013005001/003"].PaymentAgreements[agreementDate].PaymentAgreementActualStatus);
            billsManager.PayBill(invoice, invoice.Bills["MMM2013005001/004"], payment250);
            Assert.AreEqual(PaymentAgreement.PaymentAgreementStatus.Accomplished, invoice.Bills["MMM2013005001/004"].PaymentAgreements[agreementDate].PaymentAgreementActualStatus);
            Assert.AreEqual(PaymentAgreement.PaymentAgreementStatus.Accomplished, invoice.Bills["MMM2013005001/002"].PaymentAgreements[agreementDate].PaymentAgreementActualStatus);
            Assert.AreEqual(PaymentAgreement.PaymentAgreementStatus.Accomplished, invoice.Bills["MMM2013005001/003"].PaymentAgreements[agreementDate].PaymentAgreementActualStatus);
        }

        [TestMethod]
        public void WhenIRenewTheDueDateOfABillItIsCorrectlyUpdated()
        {
            Bill bill = new Bill("MMM201300015/001", "This bill is past due date", 1, new DateTime(2013, 11, 11), new DateTime(2013, 11, 15));
            DateTime newDueDate = new DateTime(2013, 11, 30);
            DateTime todayDate = new DateTime(2013, 11, 20);
            bill.RenewDueDate(newDueDate, todayDate);
            Assert.AreEqual(newDueDate, bill.DueDate);
        }

        [TestMethod]
        public void WhenIRenewTheDueDateOfAPastDueDateBillTheBillIsSetAgainToToCollect()
        {
            Bill bill = new Bill("MMM201300015/001", "This bill is past due date", 1, new DateTime(2013, 11, 11), new DateTime(2013, 11, 15));
            DateTime newDueDate = new DateTime(2013, 11, 30);
            DateTime todayDate = new DateTime(2013, 11, 20);
            bill.CheckDueDate(todayDate);
            Assert.AreEqual(Bill.BillPaymentResult.Unpaid, bill.PaymentResult);
            bill.RenewDueDate(newDueDate, todayDate);
            Assert.AreEqual(Bill.BillPaymentResult.ToCollect, bill.PaymentResult);
        }
    }
}
