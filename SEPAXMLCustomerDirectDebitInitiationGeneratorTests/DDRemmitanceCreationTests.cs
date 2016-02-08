using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.OleDb;
using DirectDebitElements;
using SEPAXMLCustomerDirectDebitInitiationGenerator;

namespace SEPAXMLCustomerDirectDebitInitiationGeneratorTests
{
    [TestClass]
    public class DDRemmitanceCreationTests
    {
        [TestMethod]
        public void AnEmptyDirectDebitRemmitanceIsCorrectlyCreatedFromADatabase()
        {
            string relativePathToTestDatabase = @"TestFiles\TestMDB.mdb";
            string oleDBConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;" + "data source=" + relativePathToTestDatabase;
            OleDbConnection connection = new OleDbConnection(oleDBConnectionString);

            DirectDebitRemittance directDebitRemittance;
            string creditorName;
            MainInstance mainInstance = new MainInstance();
            mainInstance.GetRemmmitanceBaseInformation(connection, out creditorName, out directDebitRemittance);

            Assert.AreEqual("NOMBRE ACREEDOR PRUEBAS", creditorName);
            Assert.AreEqual("PREG1234567815011007:15:00", directDebitRemittance.MessageID);
            Assert.AreEqual(new DateTime(2015, 01, 10, 7, 15, 00), directDebitRemittance.CreationDate);
            Assert.AreEqual(new DateTime(2015, 01, 15), directDebitRemittance.RequestedCollectionDate);
            Assert.AreEqual(0, directDebitRemittance.ControlSum);
            Assert.AreEqual(0, directDebitRemittance.NumberOfTransactions);
            Assert.AreEqual("ES5621001111301111111111", directDebitRemittance.DirectDebitInitiationContract.CreditorAcount.IBAN.IBAN);
            Assert.AreEqual("CAIXESBBXXX", directDebitRemittance.DirectDebitInitiationContract.CreditorAgent.BankBIC);
            Assert.AreEqual("CaixaBank", directDebitRemittance.DirectDebitInitiationContract.CreditorAgent.BankName);
            Assert.AreEqual("011", directDebitRemittance.DirectDebitInitiationContract.CreditorBussinessCode);
            Assert.AreEqual("ES26011G12345678", directDebitRemittance.DirectDebitInitiationContract.CreditorID);
            CollectionAssert.AreEqual(new List<DirectDebitPaymentInstruction>() { }, directDebitRemittance.DirectDebitPaymentInstructions);
        }

        [TestMethod]
        public void TheRCURPaymentIsCorrectlyCreatedFromADataBase()
        {
            string relativePathToTestDatabase = @"TestFiles\TestMDB.mdb";
            string oleDBConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;" + "data source=" + relativePathToTestDatabase;
            OleDbConnection connection = new OleDbConnection(oleDBConnectionString);

            //DirectDebitRemittance directDebitRemittance;
            //string creditorName;

            MainInstance mainInstance = new MainInstance();
            DirectDebitPaymentInstruction directDebitPaymentInstruction = mainInstance.CreatePaymentInformationWithRCURTransactions(connection, "PREG1234567815011007:15:00-01");

            Assert.AreEqual(3, directDebitPaymentInstruction.NumberOfDirectDebitTransactions);
            Assert.AreEqual(316M, directDebitPaymentInstruction.TotalAmount);


            //Assert.AreEqual("NOMBRE ACREEDOR PRUEBAS", creditorName);
            //Assert.AreEqual("PREG1234567815011007:15:00", directDebitRemittance.MessageID);
            //Assert.AreEqual(new DateTime(2015, 01, 10, 7, 15, 00), directDebitRemittance.CreationDate);
            //Assert.AreEqual(new DateTime(2015, 01, 15), directDebitRemittance.RequestedCollectionDate);
            //Assert.AreEqual(0, directDebitRemittance.ControlSum);
            //Assert.AreEqual(0, directDebitRemittance.NumberOfTransactions);
            //Assert.AreEqual("ES5621001111301111111111", directDebitRemittance.DirectDebitInitiationContract.CreditorAcount.IBAN.IBAN);
            //Assert.AreEqual("CAIXESBBXXX", directDebitRemittance.DirectDebitInitiationContract.CreditorAgent.BankBIC);
            //Assert.AreEqual("CaixaBank", directDebitRemittance.DirectDebitInitiationContract.CreditorAgent.BankName);
            //Assert.AreEqual("011", directDebitRemittance.DirectDebitInitiationContract.CreditorBussinessCode);
            //Assert.AreEqual("ES26011G12345678", directDebitRemittance.DirectDebitInitiationContract.CreditorID);
            //CollectionAssert.AreEqual(new List<DirectDebitPaymentInstruction>() { }, directDebitRemittance.DirectDebitPaymentInstructions);
        }
    }
}
