using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementAppLogic.ClubStore;
using RCNGCMembersManagementAppLogic.Billing;
using ExtensionMethods;

namespace RCNGCMembersManagementUnitTests
{
    [TestClass]
    public class ProductUnitTests
    {
        [TestMethod]
        public void InstantiatingASimpleProductThatHasADiscriptionCostAndAplicapleTax()
        {
            Tax defaultTax = new Tax("5% Tax", 5);
            Product simpleProduct = new Product("A Cap", 10, defaultTax);
            Assert.AreEqual("A Cap",simpleProduct.Description);
            Assert.AreEqual(10,simpleProduct.Cost);
            Assert.AreEqual(defaultTax, simpleProduct.Tax);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ProductCostCantBeNegative()
        {
            Tax defaultTax = new Tax("5% Tax", 5);
            try
            {
                Product simpleService = new Product(null, -1, defaultTax);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Product cost can't be negative", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ProductDescriptionCanNotBeNull()
        {
            Tax defaultTax = new Tax("5% Tax", 5);
            try
            {
                Product simpleService = new Product(null, 10, defaultTax);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Product name can't be empty or null", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ProductDescriptionCanNotBeEmpty()
        {
            Tax defaultTax = new Tax("5% Tax", 5);
            try
            {
                Product simpleService = new Product("", 10, defaultTax);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Product name can't be empty or null", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ProductDescriptionCanNotBeJustSpaces()
        {
            Tax defaultTax = new Tax("5% Tax", 5);
            try
            {
                Product simpleService = new Product("   ", 10, defaultTax);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Product name can't be empty or null", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }
    }
}
