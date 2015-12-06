using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementAppLogic.Billing;
using ExtensionMethods;

namespace RCNGCMembersManagementUnitTests.Billing
{
    [TestClass]
    public class TaxTestUnits
    {
        [TestMethod]
        public void CreatingATax()
        {
            Tax tax = new Tax("Canarian IGIC", 7);
            Assert.AreEqual(7, tax.TaxPercentage);
        }

        [TestMethod]
        public void TaxPercentageCanBeZero()
        {
            Tax tax = new Tax("No Tax", 0);
            Assert.AreEqual(0, tax.TaxPercentage);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void TaxPercentagesCanNotBeNegative()
        {
            try
            {
                Tax tax = new Tax("Negative TAX", -5);

            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.AreEqual("Tax percentages can't be negative", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TaxNameCanNotBeNull()
        {
            try
            {
                Tax tax = new Tax(null, 5);

            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Tax name can't be empty or null", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TaxNameCanNotBeEmpty()
        {
            try
            {
                Tax tax = new Tax("", 5);

            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Tax name can't be empty or null", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TaxNameCanNotBeJustSpaces()
        {
            try
            {
                Tax tax = new Tax("  ", 5);

            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Tax name can't be empty or null", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        public void Special_VoidTax_CanBeZeroAndEmptyDescription()
        {
            Tax tax = new Tax("", 0);
            Assert.AreEqual(0, tax.TaxPercentage);
        }

        [TestMethod]
        public void Special_VoidTax_CanBeZeroAndNullDescription()
        {
            Tax tax = new Tax(null, 0);
            Assert.AreEqual(0, tax.TaxPercentage);
        }
    }
}
