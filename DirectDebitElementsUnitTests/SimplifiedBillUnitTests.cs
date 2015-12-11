using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing;
using DirectDebitElements;

namespace DirectDebitElementsUnitTests
{
    [TestClass]
    public class SimplifiedBillUnitTests
    {

        [TestMethod]
        public void ABillIsCorrectlyCreated()
        {
            DateTime issueDate = new DateTime(2013, 11, 11);
            DateTime dueDate = issueDate.AddYears(10);
            SimplifiedBill bill = new SimplifiedBill("MMM201300015/001", "An easy to pay bill", 1, issueDate, dueDate);
            Assert.AreEqual("MMM201300015/001", bill.BillID);
            Assert.AreEqual("An easy to pay bill", bill.Description);
            Assert.AreEqual((decimal)1, bill.Amount);
            Assert.AreEqual(issueDate, bill.IssueDate);
            Assert.AreEqual(dueDate, bill.DueDate);
        }

        [TestMethod]
        public void ABillShouldHaveABillID()
        {
            SimplifiedBill bill = new SimplifiedBill("MMM201300015/001", "An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            Assert.AreEqual("MMM201300015/001", bill.BillID);
        }

        [TestMethod]
        public void ByDefaultABillIsGeneratedWithoutAPaymentMethod()
        {
            SimplifiedBill bill = new SimplifiedBill("MMM201300015/001","An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            Assert.IsNull(bill.AssignedPaymentMethod);
        }

        [TestMethod]
        public void AsigningAPaymentMethodToANewlyCreatedBill()
        {
            SimplifiedBill bill = new SimplifiedBill("MMM201300015/001", "An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            int internalReferenceNumber = 2645;
            ClientAccountCodeCCC clientCodeCCC = new ClientAccountCodeCCC("21001668120200186862");
            BankAccount bankAccount = new BankAccount(clientCodeCCC);
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(internalReferenceNumber, DateTime.Today, bankAccount, "Miguel Fierro");
            bill.AssignedPaymentMethod = new DirectDebitPaymentMethod(directDebitMandate,"000000000");
            Assert.AreEqual(typeof(DirectDebitPaymentMethod),bill.AssignedPaymentMethod.GetType());
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
    }
}
