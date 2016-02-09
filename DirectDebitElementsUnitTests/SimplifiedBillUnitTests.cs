using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Billing;
using DirectDebitElements;
using ExtensionMethods;

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
        [ExpectedException(typeof(System.ArgumentException))]
        public void ABillShouldHaveABillID()
        {
            try
            {
                SimplifiedBill bill = new SimplifiedBill(null, "An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("El ID del recibo no puede ser nulo", e.GetMessageWithoutParamName());
                Assert.AreEqual("billID", e.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ABillIDCantBeEnEmptyString()
        {
            try
            {
                SimplifiedBill bill = new SimplifiedBill("", "An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("El ID del recibo no puede ser una cadena vacía o espacios", e.GetMessageWithoutParamName());
                Assert.AreEqual("billID", e.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ABillIDCantBeSpaces()
        {
            try
            {
                SimplifiedBill bill = new SimplifiedBill("   ", "An easy to pay bill", 1, DateTime.Now, DateTime.Now.AddYears(10));
            }

            catch (System.ArgumentException e)
            {
                Assert.AreEqual("El ID del recibo no puede ser una cadena vacía o espacios", e.GetMessageWithoutParamName());
                Assert.AreEqual("billID", e.ParamName);
                throw;
            }
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
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(internalReferenceNumber, DateTime.Today, bankAccount, "CAIXESBBXXX", "Miguel Fierro");
            bill.AssignedPaymentMethod = new DirectDebitPaymentMethod(directDebitMandate,"000000000");
            Assert.AreEqual(typeof(DirectDebitPaymentMethod),bill.AssignedPaymentMethod.GetType());
        }

        [TestMethod]
        public void ADirectDebitPaymentMethodIsCorrectlyCreated()
        {
            int internalReferenceNumber = 2645;
            BankAccount bankAccount = new BankAccount(new ClientAccountCodeCCC("12345678061234567890"));
            DateTime directDebitMandateCreationDate = new DateTime(2013, 11, 11);
            DirectDebitMandate directDebitMandate = new DirectDebitMandate(internalReferenceNumber, directDebitMandateCreationDate, bankAccount, null, "NoName");
            string directDebitTransactionPaymentIdentification = "201311110000123456";
            DirectDebitPaymentMethod directDebitPaymentMethod = new DirectDebitPaymentMethod(directDebitMandate, directDebitTransactionPaymentIdentification);
            Assert.AreEqual(directDebitMandate, directDebitPaymentMethod.DirectDebitMandate);
            Assert.AreEqual("201311110000123456", directDebitPaymentMethod.DDTXPaymentIdentification);
        }
    }
}
