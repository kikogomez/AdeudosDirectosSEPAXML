using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementAppLogic;
using RCNGCMembersManagementAppLogic.Billing;
using RCNGCMembersManagementAppLogic.MembersManaging;
using RCNGCMembersManagementAppLogic.ClubServices;
using RCNGCMembersManagementAppLogic.ClubStore;
using RCNGCMembersManagementMocks;
using ExtensionMethods;

namespace RCNGCMembersManagementUnitTests.Billing
{
    [TestClass]
    public class InvoiceUnitTests
    {
        static BillingDataManager billDataManager;
        List<Transaction> transactionsList;
        Dictionary<string, Tax> taxesDictionary;
        ClubMember clubMember;
        InvoiceCustomerData invoiceCustomerData;
        InvoicesManager invoicesManager;


        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            BillingSequenceNumbersMock billingSequenceNumbersMock = new BillingSequenceNumbersMock();
            billDataManager = BillingDataManager.Instance;
            billDataManager.SetBillingSequenceNumberCollaborator(billingSequenceNumbersMock);  
        }
        
        [TestInitialize]
        public void Setup()
        {
            taxesDictionary = new Dictionary<string, Tax>{
                {"No IGIC", new Tax("No IGIC",0)},
                {"IGIC Reducido 1", new Tax("IGIC Reducido 1",2.75)},
                {"IGIC Reducido 2", new Tax("IGIC Reducido 2",3)},
                {"IGIC General", new Tax("IGIC General",7)},
                {"IGIC Incrementado 1", new Tax("IGIC Incrementado 1",9.50)},
                {"IGIC Incrementado 2", new Tax("IGIC Incrementado 2",13.50)},
                {"IGIC Especial", new Tax("IGIC Especial",20)}};

            transactionsList = new List<Transaction>()
            {
                {new Transaction("Monthly Fee",1,80,taxesDictionary["No IGIC"],0)},
                {new Transaction("Renting a Kajak",1,50,taxesDictionary["No IGIC"],0)},
                {new Transaction("Blue cup",2,10,taxesDictionary["No IGIC"],0)},
                {new Transaction("BIG Mouring",1,500,taxesDictionary["No IGIC"],0)}
            };

            clubMember = new ClubMember("0002", "Francisco", "Gomez", "");
            invoiceCustomerData= new InvoiceCustomerData(clubMember);
            invoicesManager = new InvoicesManager();
        }

        [TestMethod]
        public void CreatingANewInvoiceForASetOfTransactions()
        {
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            Assert.IsNotNull(invoice);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void AnInvoiceDetailCantBeEmpty()
        {
            DateTime issueDate = DateTime.Now;
            List<Transaction> transactionsList = new List<Transaction>();
            try
            {
                Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            }
            catch (ArgumentNullException exception)
            {
                Assert.AreEqual("The invoice detail can't be empty", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        public void AFreshlyCreatedInvoiceIsSetToBePaid()
        {
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            Assert.AreEqual(Invoice.InvoicePaymentState.ToBePaid, invoice.InvoiceState);
        }

        [TestMethod]
        public void InvoiceCustomerDataIsWellStoredAndReadable()
        {
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            Assert.AreEqual("Francisco Gomez", invoice.CustomerData.FullName);
        }

        [TestMethod]
        public void IssueDateIsWellStoredAndReadable()
        {
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            Assert.AreEqual(issueDate, invoice.IssueDate);
        }

        [TestMethod]
        public void WhenInstantiatingANewInvoiceANewInvoiceIDIsAssigned()
        {
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            Assert.IsNotNull(invoice.InvoiceID);
        }

        [TestMethod]
        public void TheLastSixCharactersOfAnInvoiceAreANumber()
        {
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            string invoiceIDLastSixCharacters = invoice.InvoiceID.Substring(invoice.InvoiceID.Length - 6);
            int number;
            Assert.IsTrue(int.TryParse(invoiceIDLastSixCharacters, out number));
        }

        [TestMethod]
        public void TheLastSixCharactersOfAnInvoiceAreASequenceNumber()
        {
            DateTime issueDate = DateTime.Now;
            Invoice firstInvoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            int firstSequenceNumber = int.Parse(firstInvoice.InvoiceID.Substring(firstInvoice.InvoiceID.Length - 6));
            Invoice secondInvoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            int secondSequenceNumber = int.Parse(secondInvoice.InvoiceID.Substring(secondInvoice.InvoiceID.Length - 6));
            Assert.AreEqual(secondSequenceNumber, firstSequenceNumber + 1);
        }

        [TestMethod]
        public void WeCanSetTheInvoiceSequenceNumberValue()
        {
            billDataManager.InvoiceSequenceNumber=5000;
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            int invoiceSequenceNumber = int.Parse(invoice.InvoiceID.Substring(invoice.InvoiceID.Length - 6));
            Assert.AreEqual(5000, invoiceSequenceNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CantSetInvoiceSequenceNumberOver999999()
        {
            try
            {
                billDataManager.InvoiceSequenceNumber=1000000;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.AreEqual("Invoice ID out of range (1-999999)", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CantSetInvoiceSequenceNumberTo0()
        {
            try
            {
                billDataManager.InvoiceSequenceNumber = 0;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.AreEqual("Invoice ID out of range (1-999999)", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        public void TheInvoiceIDIsINVplusYEARpulsInvoiceSequencenumber()
        {
            DateTime issueDate = DateTime.Parse("01/01/2013");
            billDataManager.InvoiceSequenceNumber=5000;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            Assert.AreEqual("INV2013005000", invoice.InvoiceID);
        }

        [TestMethod]
        public void ICanInstantiateAnInvoiceWithAGivenInvoiceID()
        {
            string invoiceID = "INV2013012345";
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceID, invoiceCustomerData, transactionsList, issueDate);
            Assert.AreEqual(invoiceID, invoice.InvoiceID);
        }

        [TestMethod]
        public void InstantiatingAnInvoiceWithAGivenInvoiceIDDoesntChangeTheInvoiceIDSequenceNumber()
        {
            billDataManager.InvoiceSequenceNumber = (5000);
            string invoiceID = "INV2013012345";
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceID, invoiceCustomerData, transactionsList, issueDate);
            Assert.AreEqual((uint)5000, billDataManager.InvoiceSequenceNumber);
        }

        [TestMethod]
        public void TheInvoiceTransactionsCanHaveEachDifferentTaxes()
        {
            DateTime issueDate = DateTime.Now;
            List<Transaction> transactionsList = new List<Transaction>{
                new Transaction("Nice Blue Cap", 1,10,new Tax("5% Tax",5),0),
                new Transaction("Nice Blue T-Shirt", 1,20,new Tax("10% Tax",10),0)};
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            Assert.AreEqual((decimal)32.5, invoice.NetAmount);
        }

        [TestMethod]
        public void TheInvoiceCanMixSalesAndServiceCharges()
        {
            Product cap = new Product("Cap", 10, new Tax("5% Tax", 5));
            ClubService membership = new ClubService("Club Full Membership", 80, new Tax("5% Tax", 5));
            DateTime issueDate = DateTime.Now;
            List<Transaction> transactionsList = new List<Transaction>{
                new Sale(cap, "Nice Blue Cap", 1,0),
                new ServiceCharge(membership, "June Membership Fee", 1,0)};
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            Assert.AreEqual((decimal)94.5, invoice.NetAmount);
        }

        [TestMethod]
        public void AnInvoiceCanMixTransactionsWithDifferentTaxes()
        {
            DateTime issueDate = DateTime.Now;
            List<Transaction> transactionsList = new List<Transaction>{
                new Transaction("Nice Blue Cap", 1,10,new Tax("5% Tax", 5),0),
                new Transaction("June Membership Fee", 1,40,new Tax("No Tax", 0),0)};
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            Assert.AreEqual((decimal)50.5, invoice.NetAmount);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void TransactionsOnInvoicesCantHaveZeroOrLessUnits()
        {
            Product cap = new Product("Cap", 5, taxesDictionary["IGIC General"]);
            ClubService membership = new ClubService("Club Full Membership", 50, taxesDictionary["IGIC General"]);
            DateTime issueDate = DateTime.Now;
            List<Transaction> transactionsList = new List<Transaction>{
                new ServiceCharge(membership, "June Membership Fee", 0,79,taxesDictionary["IGIC General"],0)};
            try
            {
                Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.AreEqual("Invoice transactions must have at least one element to transact", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void TransactionsOnProFormaInvoicesCantHaveZeroOrLessUnits()
        {
            Product cap = new Product("Cap", 5, taxesDictionary["IGIC General"]);
            ClubService membership = new ClubService("Club Full Membership", 50, taxesDictionary["IGIC General"]);
            DateTime issueDate = DateTime.Now;
            List<Transaction> transactionsList = new List<Transaction>{
                new ServiceCharge(membership, "June Membership Fee", 0,79,taxesDictionary["IGIC General"],0)};
            try
            {
                ProFormaInvoice proFormaInvoice = new ProFormaInvoice(invoiceCustomerData, transactionsList, issueDate);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.AreEqual("Pro Forma Invoice transactions must have at least one element to transact", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }


        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void TransactionsCantHaveNegativeCost()
        {
            DateTime issueDate = DateTime.Now;
            try
            {
                Transaction transaction= new Transaction("June Membership Fee", 1,-79,taxesDictionary["IGIC General"],0);
            
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.AreEqual("Transactions units cost can't be negative", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        public void InvoicesAcceptTransactionsWithZeroCost()
        {
            Product cap = new Product("Cap", 5, taxesDictionary["IGIC General"]);
            ClubService membership = new ClubService("Club Full Membership", 50, taxesDictionary["IGIC General"]);
            DateTime issueDate = DateTime.Now;
            List<Transaction> transactionsList = new List<Transaction>{
                new Sale(cap, "Nice Blue Cap", 1,0,taxesDictionary["IGIC Reducido 2"],0),
                new ServiceCharge(membership, "June Membership Fee", 1,0,taxesDictionary["IGIC General"],0)};
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            Assert.AreEqual((decimal)0, invoice.NetAmount);
        }

        [TestMethod]
        public void ProFormaInvoicesAcceptTransactionsWithZeroCost()
        {
            Product cap = new Product("Cap", 5, taxesDictionary["IGIC General"]);
            ClubService membership = new ClubService("Club Full Membership", 50, taxesDictionary["IGIC General"]);
            DateTime issueDate = DateTime.Now;
            List<Transaction> transactionsList = new List<Transaction>{
                new Sale(cap, "Nice Blue Cap", 1,0,taxesDictionary["IGIC Reducido 2"],0),
                new ServiceCharge(membership, "June Membership Fee", 1,0,taxesDictionary["IGIC General"],0)};
            ProFormaInvoice proFormaInvoice = new ProFormaInvoice(invoiceCustomerData, transactionsList, issueDate);
            Assert.AreEqual((decimal)0, proFormaInvoice.NetAmount);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void SalesCanNotBeNegative()
        {
            Product cap = new Product("Cap", 5, taxesDictionary["IGIC General"]);
            DateTime issueDate = DateTime.Now;
            try
            {
                Transaction transaction = new Sale(cap,"Return a cap", 1, -10, taxesDictionary["IGIC General"], 0);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.AreEqual("Transactions units cost can't be negative", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void ServiceChargesCanNotBeNegative()
        {
            ClubService membership = new ClubService("Club Full Membership", 50, taxesDictionary["IGIC General"]);
            DateTime issueDate = DateTime.Now;
            try
            {
                Transaction transaction = new ServiceCharge(membership, "Return Member Fee", 1, -79, taxesDictionary["IGIC General"], 0);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.AreEqual("Transactions units cost can't be negative", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        public void TransactionsMustAcceptNegativeUnitCostsForTheCaseOfAmendingInvoices()
        {
            DateTime issueDate = DateTime.Now;
            Transaction transaction;
            try
            {
                transaction = new Transaction("AmmendingInvoice", -1, 79, taxesDictionary["IGIC General"], 0);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                throw exception;
            }
            Assert.AreEqual((decimal)-84.53, transaction.NetAmount);
        }

        [TestMethod]
        public void AnAmendingInvoiceHasTheSameGrossAmountThanTheAmendedInvoiceButNegative()
        {
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            AmendingInvoice amendingInvoice = new AmendingInvoice(invoice);
            Assert.AreEqual(-invoice.GrossAmount, amendingInvoice.GrossAmount);
        }

        [TestMethod]
        public void AnAmendingInvoiceHasTheSameNetAmountThanTheAmendedInvoiceButNegative()
        {
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            AmendingInvoice amendingInvoice = new AmendingInvoice(invoice);
            Assert.AreEqual(-invoice.NetAmount, amendingInvoice.NetAmount);
        }

        [TestMethod]
        public void AmendingInvoiceHasTheSameInvoiceIDButWithDifferentPrefix()
        {
            DateTime issueDate = DateTime.Parse("01/01/2013");
            billDataManager.InvoiceSequenceNumber = 5000;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            AmendingInvoice amendingInvoice = new AmendingInvoice(invoice);
            Assert.AreEqual("INV2013005000", invoice.InvoiceID);
            Assert.AreEqual("AMN2013005000", amendingInvoice.InvoiceID);
        }

        [TestMethod]
        public void TheCustomerDataInfoOfdAnAmendingInvoiceIsTheSameThanTheOriginalInvoice()
        {
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            AmendingInvoice amendingInvoice = new AmendingInvoice(invoice);
            Assert.AreEqual(invoiceCustomerData, amendingInvoice.CustomerData);
        }

        [TestMethod]
        public void AnAmendingInvoiceHasTheSameNumberOfTransactionsThanTheAmendedInvoicePlusOne()
        {
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            AmendingInvoice amendingInvoice = new AmendingInvoice(invoice);
            List<Transaction> invoiceDetail = invoice.InvoiceDetail;
            List<Transaction> amendingInvoiceDetail = amendingInvoice.InvoiceDetail;
            Assert.AreEqual(invoiceDetail.Count, amendingInvoiceDetail.Count - 1);
        }

        [TestMethod]
        public void FirstTransactionInAmendingInvoiceIsANoValueReferenceToOriginalInvoice()
        {
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            Transaction originalInvoiceReference = new Transaction("Amending invoice " + invoice.InvoiceID + "as detailed", 1, 0, new Tax("VoidTax", 0), 0);
            AmendingInvoice amendingInvoice = new AmendingInvoice(invoice);
            Transaction firstTransactionFromAmendingInvoice = amendingInvoice.InvoiceDetail[0];
            Assert.AreEqual(true, firstTransactionFromAmendingInvoice.CompareTo(originalInvoiceReference));
        }

        [TestMethod]
        public void AfterTheFirstReferenceAnAmendingInvoiceHasTheSameTransactionsThanOriginalInvoiceButWithNegativeUnits()
        {
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            AmendingInvoice amendingInvoice = new AmendingInvoice(invoice);
            List<Transaction> invoiceDetail = invoice.InvoiceDetail;
            List<Transaction> amendingInvoiceDetail = amendingInvoice.InvoiceDetail;
            bool bothDetailsAreComplementary = true;
            for (int index = 0; index < invoiceDetail.Count; index++)
            {
                Transaction currentLine= invoiceDetail[index];
                Transaction amendingTransaction = new Transaction(
                    "Amending " + currentLine.Description, -currentLine.Units, currentLine.UnitCost, currentLine.Tax, currentLine.Discount);
                if (!amendingTransaction.CompareTo(amendingInvoiceDetail[index + 1]))
                {
                    bothDetailsAreComplementary = false;
                    break;
                }
            }
            Assert.AreEqual(true, bothDetailsAreComplementary);
        }

        [TestMethod]
        public void WhenCancellingAnInvoiceTheInvoiceIsMarkedAsCancelled()
        {
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            invoice.Cancel();
            Assert.AreEqual(Invoice.InvoicePaymentState.Cancelled, invoice.InvoiceState);
        }

        [TestMethod]
        public void WhenCancellingAnInvoiceAllThePendingBillsAreMarkedAsCancelled()
        {
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            List<Bill> pendingBills = invoice.Bills
                .Select(billsDictionayElement => billsDictionayElement.Value)
                .Where(bill => bill.PaymentResult == Bill.BillPaymentResult.ToCollect || bill.PaymentResult == Bill.BillPaymentResult.Unpaid).ToList();
            invoice.Cancel();
            foreach (Bill bill in pendingBills) Assert.AreEqual(Bill.BillPaymentResult.CancelledOut, bill.PaymentResult);
        }

        [TestMethod]
        public void WhenCancellingAnInvoiceTheBillTotalAmountToBePaidIs0()
        {
            DateTime issueDate = DateTime.Now;
            Invoice invoice = new Invoice(invoiceCustomerData, transactionsList, issueDate);
            List<Bill> pendingBills = invoice.Bills
                .Select(billsDictionayElement => billsDictionayElement.Value)
                .Where(bill => bill.PaymentResult == Bill.BillPaymentResult.ToCollect || bill.PaymentResult == Bill.BillPaymentResult.Unpaid).ToList();
            invoice.Cancel();
            foreach (Bill bill in pendingBills) Assert.AreEqual(Bill.BillPaymentResult.CancelledOut, bill.PaymentResult);
        }

        [TestMethod]
        public void WhenPayingABillIfThereAreNoMoreBillsToCollectTheInvoiceIsMarkedAsPaid()
        {
            BillsManager billsManager = new BillsManager();
            string invoiceID = "MMM2013005001";
            List<Bill> billsList = new List<Bill>()
            {
                {new Bill("MMM2013005001/001", "First Instalment", 200, DateTime.Now, DateTime.Now.AddDays(30))},
                {new Bill("MMM2013005001/002", "Second Instalment", 200, DateTime.Now, DateTime.Now.AddDays(60))},
                {new Bill("MMM2013005001/003", "Third Instalment", 250, DateTime.Now, DateTime.Now.AddDays(90))}
            };
            Invoice invoice = new Invoice(invoiceID, invoiceCustomerData, transactionsList, billsList, DateTime.Now);
            Assert.AreEqual(Invoice.InvoicePaymentState.ToBePaid, invoice.InvoiceState);
            CashPaymentMethod cashPayment = new CashPaymentMethod();
            DateTime paymentDate = new DateTime(2013, 11, 11);
            Payment payment200 = new Payment((decimal)200, paymentDate, cashPayment);
            Payment payment250 = new Payment((decimal)250, paymentDate, cashPayment);
            billsManager.PayBill(invoice, invoice.Bills["MMM2013005001/001"], payment200);
            Assert.AreEqual(Invoice.InvoicePaymentState.ToBePaid, invoice.InvoiceState);
            billsManager.PayBill(invoice, invoice.Bills["MMM2013005001/002"], payment200);
            Assert.AreEqual(Invoice.InvoicePaymentState.ToBePaid, invoice.InvoiceState);
            billsManager.PayBill(invoice, invoice.Bills["MMM2013005001/003"], payment250);
            Assert.AreEqual(Invoice.InvoicePaymentState.Paid, invoice.InvoiceState);
        }

        [TestMethod]
        public void WhenABillIsUnpaidTheInvoiceIsSetAsUnpaid()
        {
            BillsManager billsManager = new BillsManager();
            string invoiceID = "MMM2013005001";
            List<Bill> billsList = new List<Bill>()
            {
                {new Bill("MMM2013005001/001", "First Instalment", 200, new DateTime(2013,10,1), new DateTime(2013,11,1))},
                {new Bill("MMM2013005001/002", "Second Instalment", 200, new DateTime(2013,10,1), new DateTime(2013,12,1))},
                {new Bill("MMM2013005001/003", "Third Instalment", 250, new DateTime(2013,10,1), new DateTime(2014,1,1))}
            };
            Invoice invoice = new Invoice(invoiceID, invoiceCustomerData, transactionsList, billsList, DateTime.Now);
            Bill billtoCheck = invoice.Bills["MMM2013005001/001"];
            Assert.AreEqual(Invoice.InvoicePaymentState.ToBePaid, invoice.InvoiceState);
            Assert.AreEqual(Bill.BillPaymentResult.ToCollect, billtoCheck.PaymentResult);
            billsManager.CheckDueDate(invoice, billtoCheck, new DateTime(2013, 11, 11));
            Assert.AreEqual(Invoice.InvoicePaymentState.Unpaid, invoice.InvoiceState);
            Assert.AreEqual(Bill.BillPaymentResult.Unpaid, billtoCheck.PaymentResult);
        }

        [TestMethod]
        public void WhenABillIsUnpaidTheInvoiceIsSetAsUnpaidButTheRestOfTheBillsRemailToCollect()
        {
            BillsManager billsManager = new BillsManager();
            string invoiceID = "MMM2013005001";
            List<Bill> billsList = new List<Bill>()
            {
                {new Bill("MMM2013005001/001", "First Instalment", 200, new DateTime(2013,10,1), new DateTime(2013,11,1))},
                {new Bill("MMM2013005001/002", "Second Instalment", 200, new DateTime(2013,10,1), new DateTime(2013,12,1))},
                {new Bill("MMM2013005001/003", "Third Instalment", 250, new DateTime(2013,10,1), new DateTime(2014,1,1))}
            };
            Invoice invoice = new Invoice(invoiceID, invoiceCustomerData, transactionsList, billsList, DateTime.Now);
            foreach (Bill bill in invoice.Bills.Values)
            {
                billsManager.CheckDueDate(invoice, bill, new DateTime(2013, 11, 11));
            }
            Assert.AreEqual(Invoice.InvoicePaymentState.Unpaid, invoice.InvoiceState);
            Assert.AreEqual(Bill.BillPaymentResult.Unpaid, invoice.Bills["MMM2013005001/001"].PaymentResult);
            Assert.AreEqual(Bill.BillPaymentResult.ToCollect, invoice.Bills["MMM2013005001/002"].PaymentResult);
            Assert.AreEqual(Bill.BillPaymentResult.ToCollect, invoice.Bills["MMM2013005001/003"].PaymentResult);
        }

        [TestMethod]
        public void WhenTheDueDateOfABillInAnUnpaidInvoiceisRenewedTheBillIsSetAgainToBePaid()
        {
            BillsManager billsManager = new BillsManager();
            string invoiceID = "MMM2013005001";
            List<Bill> billsList = new List<Bill>()
            {
                {new Bill("MMM2013005001/001", "First Instalment", 200, new DateTime(2013,10,1), new DateTime(2013,11,1))},
                {new Bill("MMM2013005001/002", "Second Instalment", 200, new DateTime(2013,10,1), new DateTime(2013,12,1))},
                {new Bill("MMM2013005001/003", "Third Instalment", 250, new DateTime(2013,10,1), new DateTime(2014,1,1))}
            };
            Invoice invoice = new Invoice(invoiceID, invoiceCustomerData, transactionsList, billsList, DateTime.Now);
            Assert.AreEqual(Invoice.InvoicePaymentState.ToBePaid, invoice.InvoiceState);
            foreach (Bill bill in invoice.Bills.Values)
            {
                billsManager.CheckDueDate(invoice, bill, new DateTime(2013, 11, 11));
            }
            Assert.AreEqual(Invoice.InvoicePaymentState.Unpaid, invoice.InvoiceState);
            Assert.AreEqual(Bill.BillPaymentResult.Unpaid, invoice.Bills["MMM2013005001/001"].PaymentResult);
            Assert.AreEqual(Bill.BillPaymentResult.ToCollect, invoice.Bills["MMM2013005001/002"].PaymentResult);
            Assert.AreEqual(Bill.BillPaymentResult.ToCollect, invoice.Bills["MMM2013005001/003"].PaymentResult);
            DateTime renewDate = new DateTime(2013, 11, 30);
            DateTime todayDate = new DateTime(2013, 11, 20);
            billsManager.RenewBillDueDate(invoice, invoice.Bills["MMM2013005001/001"], renewDate, todayDate);
            Assert.AreEqual(Bill.BillPaymentResult.ToCollect, invoice.Bills["MMM2013005001/001"].PaymentResult);
            Assert.AreEqual(Invoice.InvoicePaymentState.ToBePaid, invoice.InvoiceState);
        }
    }
}
