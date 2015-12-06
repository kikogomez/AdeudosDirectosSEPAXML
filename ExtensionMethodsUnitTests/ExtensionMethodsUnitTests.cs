using System;
using ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtensionMethodsUnitTests
{
    [TestClass]
    public class ExtensionMethodsUnitTests
    {
        [TestMethod]
        public void ArgumentExceptionMessageWhenEmpty()
        {
            ArgumentException exception = new ArgumentException("");
            Assert.AreEqual("", exception.GetMessageWithoutParamName());
        }

        [TestMethod]
        public void ArgumentExceptionMessageWhenNoParamNameIsGiven()
        {
            ArgumentException exception = new ArgumentException("Only this messasge");
            Assert.AreEqual("Only this messasge", exception.GetMessageWithoutParamName());
        }

        [TestMethod]
        public void ArgumentExceptionMessageWhenParamNameIsGiven()
        {
            ArgumentException exception = new ArgumentException("This is the message", "ParameterName");
            Assert.AreEqual("This is the message\r\nNombre del parámetro: ParameterName", exception.Message);
            Assert.AreEqual("This is the message", exception.GetMessageWithoutParamName());
        }

        [TestMethod]
        public void ArgumentNullExceptionMessageWhenParamNameIsGiven()
        {
            ArgumentNullException exception = new ArgumentNullException("ParameterName", "This is the message");
            Assert.AreEqual("This is the message\r\nNombre del parámetro: ParameterName", exception.Message);
            Assert.AreEqual("This is the message", exception.GetMessageWithoutParamName());
        }

        [TestMethod]
        public void ArgumentOutOfRangeExceptionMessageWhenParamNameIsGiven()
        {
            ArgumentOutOfRangeException exception = new ArgumentOutOfRangeException("ParameterName", "This is the message");
            Assert.AreEqual("This is the message\r\nNombre del parámetro: ParameterName", exception.Message);
            Assert.AreEqual("This is the message", exception.GetMessageWithoutParamName());
        }

        [TestMethod]
        public void DateIdCorrectlyTruncatedToSeconds()
        {
            DateTime dateTime = new DateTime(2013, 11, 9, 10, 10, 10, 10);
            DateTime truncatedDateTime = dateTime.Truncate(TimeSpan.FromSeconds(1));
            Assert.AreEqual(new DateTime(2013, 11, 9, 10, 10, 10), truncatedDateTime);
        }

        [TestMethod]
        public void DateIdCorrectlyTruncatedToMinutes()
        {
            DateTime dateTime = new DateTime(2013, 11, 9, 10, 10, 10, 10);
            DateTime truncatedDateTime = dateTime.Truncate(TimeSpan.FromMinutes(1));
            Assert.AreEqual(new DateTime(2013, 11, 9, 10, 10, 0), truncatedDateTime);
        }

        [TestMethod]
        public void DateIdCorrectlyTruncatedToHours()
        {
            DateTime dateTime = new DateTime(2013, 11, 9, 10, 10, 10, 10);
            DateTime truncatedDateTime = dateTime.Truncate(TimeSpan.FromHours(1));
            Assert.AreEqual(new DateTime(2013, 11, 9, 10, 0, 0), truncatedDateTime);
        }

        [TestMethod]
        public void DateIdCorrectlyTruncatedToStarOfTheDay()
        {
            DateTime dateTime = new DateTime(2013, 11, 9, 10, 10, 10, 10);
            DateTime truncatedDateTime = dateTime.Truncate(TimeSpan.FromDays(1));
            Assert.AreEqual(new DateTime(2013, 11, 9, 0, 0, 0), truncatedDateTime);
        }
    }
}
