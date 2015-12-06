using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCNGCMembersManagementAppLogic.ClubServices;
using RCNGCMembersManagementAppLogic.Billing;
using ExtensionMethods;

namespace RCNGCMembersManagementUnitTests
{
    [TestClass]
    public class ServiceUnitTests
    {
        [TestMethod]
        public void InstantiatingASimpleProductThatHasADescriptionCostAndAplicapleTax()
        {
            Tax defaultTax = new Tax("5% Tax", 5);
            ClubService simpleService = new ClubService("Optimist Rent", 10, defaultTax);
            Assert.AreEqual("Optimist Rent", simpleService.Description);
            Assert.AreEqual(10,simpleService.Cost);
            Assert.AreEqual(defaultTax, simpleService.Tax);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ServiceCostCantBeNegative()
        {
            Tax defaultTax = new Tax("5% Tax", 5);
            try
            {
                ClubService simpleService = new ClubService(null, -1, defaultTax);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Service cost can't be negative", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ServiceDescriptionCanNotBeNull()
        {
            Tax defaultTax = new Tax("5% Tax", 5);
            try
            {
                ClubService simpleService = new ClubService(null, 10, defaultTax);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Service name can't be empty or null", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ServiceDescriptionCanNotBeEmpty()
        {
            Tax defaultTax = new Tax("5% Tax", 5);
            try
            {
                ClubService simpleService = new ClubService("", 10, defaultTax);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Service name can't be empty or null", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ServiceDescriptionCanNotBeJustSpaces()
        {
            Tax defaultTax = new Tax("5% Tax", 5);
            try
            {
                ClubService simpleService = new ClubService("   ", 10, defaultTax);
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual("Service name can't be empty or null", exception.GetMessageWithoutParamName());
                throw exception;
            }
        }
    }
}
