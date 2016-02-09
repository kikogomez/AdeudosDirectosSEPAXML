using System;
using System.IO;
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
        public void AnEmptyDirectDebitRemmitanceIsCorrectlyCreatedFromTheTestDatabase()
        {
            string relativePathToTestDatabase = @"TestFiles\TestMDB.mdb";
            string oleDBConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;" + "data source=" + relativePathToTestDatabase;
            OleDbConnection connection = new OleDbConnection(oleDBConnectionString);

            DirectDebitRemittance directDebitRemittance;
            string creditorNIF;
            string creditorName;
            MainInstance mainInstance = new MainInstance();
            using (connection)
            {
                connection.Open();
                mainInstance.GetRemmmitanceBaseInformation(connection, out creditorNIF, out creditorName, out directDebitRemittance);
            }

            Assert.AreEqual("G12345678", creditorNIF);
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
        public void TheRCURPaymentInstructionIsCorrectlyCreatedFromTheTestDataBase()
        {
            string relativePathToTestDatabase = @"TestFiles\TestMDB.mdb";
            string oleDBConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;" + "data source=" + relativePathToTestDatabase;
            OleDbConnection connection = new OleDbConnection(oleDBConnectionString);

            DirectDebitPaymentInstruction directDebitPaymentInstruction;
            using (connection)
            {
                connection.Open();
                MainInstance mainInstance = new MainInstance();
                directDebitPaymentInstruction = mainInstance.CreatePaymentInstructionWithRCURTransactions(connection, "PREG1234567815011007:15:00-01", "CORE");
            }

            Assert.AreEqual("PREG1234567815011007:15:00-01", directDebitPaymentInstruction.PaymentInformationID);
            Assert.AreEqual("CORE", directDebitPaymentInstruction.LocalInstrument);
            Assert.AreEqual(false, directDebitPaymentInstruction.FirstDebits);
            Assert.AreEqual(3, directDebitPaymentInstruction.DirectDebitTransactions.Count);
            Assert.AreEqual(3, directDebitPaymentInstruction.NumberOfDirectDebitTransactions);
            Assert.AreEqual(316M, directDebitPaymentInstruction.TotalAmount);

            DirectDebitTransaction directDebitTransaction;

            //FirstTransaction
            directDebitTransaction = directDebitPaymentInstruction.DirectDebitTransactions[0];
            Assert.AreEqual("PREG1234567815011007:15:00-0100001", directDebitTransaction.TransactionID);
            Assert.AreEqual("000001101234", directDebitTransaction.MandateID);
            Assert.AreEqual(new DateTime(2013, 11, 11), directDebitTransaction.MandateSigatureDate);
            Assert.AreEqual(1, directDebitTransaction.NumberOfBills);
            Assert.AreEqual(79M, directDebitTransaction.Amount);
            Assert.AreEqual("ES8721002222002222222222", directDebitTransaction.DebtorAccount.IBAN.IBAN);
            Assert.AreEqual("CAIXESBBXXX", directDebitTransaction.DebtorAgentBIC);
            Assert.IsNull(directDebitTransaction.AmendmentInformation.OldBankAccount);
            Assert.IsNull(directDebitTransaction.AmendmentInformation.OldMandateID);
            Assert.AreEqual("Francisco Gómez-Caldito Viseas", directDebitTransaction.AccountHolderName);
            Assert.AreEqual("Cuota Social Octubre 2014 --- 79,00", directDebitTransaction.BillsInTransaction[0].Description);
            Assert.AreEqual(false, directDebitTransaction.FirstDebit);

            //Second Transaction
            directDebitTransaction = directDebitPaymentInstruction.DirectDebitTransactions[1];
            Assert.AreEqual("PREG1234567815011007:15:00-0100002", directDebitTransaction.TransactionID);
            Assert.AreEqual("000001101235", directDebitTransaction.MandateID);
            Assert.AreEqual(new DateTime(2013, 11, 11), directDebitTransaction.MandateSigatureDate);
            Assert.AreEqual(1, directDebitTransaction.NumberOfBills);
            Assert.AreEqual(158M, directDebitTransaction.Amount);
            Assert.AreEqual("ES3821003333802222222222", directDebitTransaction.DebtorAccount.IBAN.IBAN);
            Assert.AreEqual("CAIXESBBXXX", directDebitTransaction.DebtorAgentBIC);
            Assert.IsNull(directDebitTransaction.AmendmentInformation.OldBankAccount);
            Assert.AreEqual("000001101000",directDebitTransaction.AmendmentInformation.OldMandateID);
            Assert.AreEqual("Pedro Pérez Gómez", directDebitTransaction.AccountHolderName);
            Assert.AreEqual("Cuota Social Octubre 2014 --- 79,00; Cuota Social Noviembre 2014 --- 79,00", directDebitTransaction.BillsInTransaction[0].Description);
            Assert.AreEqual(false, directDebitTransaction.FirstDebit);

            //ThirdTransaction
            directDebitTransaction = directDebitPaymentInstruction.DirectDebitTransactions[2];
            Assert.AreEqual("PREG1234567815011007:15:00-0100003", directDebitTransaction.TransactionID);
            Assert.AreEqual("000001101236", directDebitTransaction.MandateID);
            Assert.AreEqual(new DateTime(2013, 11, 11), directDebitTransaction.MandateSigatureDate);
            Assert.AreEqual(1, directDebitTransaction.NumberOfBills);
            Assert.AreEqual(79M, directDebitTransaction.Amount);
            Assert.AreEqual("ES1921004444502222222222", directDebitTransaction.DebtorAccount.IBAN.IBAN);
            Assert.AreEqual("CAIXESBBXXX", directDebitTransaction.DebtorAgentBIC);
            Assert.AreEqual("ES8421000000661234567890", directDebitTransaction.AmendmentInformation.OldBankAccount.IBAN.IBAN);
            Assert.IsNull(directDebitTransaction.AmendmentInformation.OldMandateID);
            Assert.AreEqual("Rodrigo Rodríguez Rodríguez", directDebitTransaction.AccountHolderName);
            Assert.AreEqual("Cuota Social Noviembre 2014 --- 79,00", directDebitTransaction.BillsInTransaction[0].Description);
            Assert.AreEqual(false, directDebitTransaction.FirstDebit);
        }

        [TestMethod]
        public void TheFRSTPaymentInstructionIsCorrectlyCreatedFromTheTestDataBase()
        {
            string relativePathToTestDatabase = @"TestFiles\TestMDB.mdb";
            string oleDBConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;" + "data source=" + relativePathToTestDatabase;
            OleDbConnection connection = new OleDbConnection(oleDBConnectionString);

            DirectDebitPaymentInstruction directDebitPaymentInstruction;
            using (connection)
            {
                connection.Open();
                MainInstance mainInstance = new MainInstance();
                directDebitPaymentInstruction = mainInstance.CreatePaymentInstructionWithFRSTTransactions(connection, "PREG1234567815011007:15:00-02", "CORE");
            }

            Assert.AreEqual("PREG1234567815011007:15:00-02", directDebitPaymentInstruction.PaymentInformationID);
            Assert.AreEqual("CORE", directDebitPaymentInstruction.LocalInstrument);
            Assert.AreEqual(true, directDebitPaymentInstruction.FirstDebits);
            Assert.AreEqual(1, directDebitPaymentInstruction.DirectDebitTransactions.Count);
            Assert.AreEqual(1, directDebitPaymentInstruction.NumberOfDirectDebitTransactions);
            Assert.AreEqual(79.5M, directDebitPaymentInstruction.TotalAmount);

            DirectDebitTransaction directDebitTransaction;

            //FirstTransaction
            directDebitTransaction = directDebitPaymentInstruction.DirectDebitTransactions[0];
            Assert.AreEqual("PREG1234567815011007:15:00-0200001", directDebitTransaction.TransactionID);
            Assert.AreEqual("000001101237", directDebitTransaction.MandateID);
            Assert.AreEqual(new DateTime(2013, 11, 11), directDebitTransaction.MandateSigatureDate);
            Assert.AreEqual(1, directDebitTransaction.NumberOfBills);
            Assert.AreEqual(79.5M, directDebitTransaction.Amount);
            Assert.AreEqual("ES5720385555302222222222", directDebitTransaction.DebtorAccount.IBAN.IBAN);
            Assert.AreEqual("CAHMESBBXXX", directDebitTransaction.DebtorAgentBIC);
            Assert.AreEqual("ES0201820000961234567890", directDebitTransaction.AmendmentInformation.OldBankAccount.IBAN.IBAN);
            Assert.IsNull(directDebitTransaction.AmendmentInformation.OldMandateID);
            Assert.AreEqual("Domingo Domínguez Domínguez", directDebitTransaction.AccountHolderName);
            Assert.AreEqual("Cuota Social Noviembre 2014 --- 79,50", directDebitTransaction.BillsInTransaction[0].Description);
            Assert.AreEqual(true, directDebitTransaction.FirstDebit);
        }

        [TestMethod]
        public void TheDirectdebitRemmitanceInformationIsCorrectlyRetrievedFromADataBase()
        {
            string relativePathToTestDatabase = @"TestFiles\TestMDB.mdb";
            string oleDBConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;" + "data source=" + relativePathToTestDatabase;
            OleDbConnection connection = new OleDbConnection(oleDBConnectionString);

            string creditorNIF;
            string creditorName;
            DirectDebitRemittance directDebitRemittance;
            MainInstance mainInstance = new MainInstance();
            mainInstance.RetrieveRemmitanceInformationFromDataBase(connection, out creditorNIF, out creditorName, out directDebitRemittance);

            Assert.AreEqual("G12345678", creditorNIF);
            Assert.AreEqual("ES26011G12345678", directDebitRemittance.DirectDebitInitiationContract.CreditorID);
            Assert.AreEqual("NOMBRE ACREEDOR PRUEBAS", creditorName);
            Assert.AreEqual("PREG1234567815011007:15:00", directDebitRemittance.MessageID);
            Assert.AreEqual(new DateTime(2015,1, 10,7,15,0), directDebitRemittance.CreationDate);
            Assert.AreEqual(new DateTime(2015, 1, 15), directDebitRemittance.RequestedCollectionDate);
            Assert.AreEqual("ES5621001111301111111111", directDebitRemittance.DirectDebitInitiationContract.CreditorAcount.IBAN.IBAN);
            Assert.AreEqual("CAIXESBBXXX", directDebitRemittance.DirectDebitInitiationContract.CreditorAgent.BankBIC);
            Assert.AreEqual("CaixaBank", directDebitRemittance.DirectDebitInitiationContract.CreditorAgent.BankName);
            Assert.AreEqual("011", directDebitRemittance.DirectDebitInitiationContract.CreditorBussinessCode);
            Assert.AreEqual(4, directDebitRemittance.NumberOfTransactions);
            Assert.AreEqual(395.5M, directDebitRemittance.ControlSum);
            Assert.IsFalse(directDebitRemittance.DirectDebitPaymentInstructions[0].FirstDebits);
            Assert.AreEqual("PREG1234567815011007:15:00-RC", directDebitRemittance.DirectDebitPaymentInstructions[0].PaymentInformationID);
            Assert.IsTrue(directDebitRemittance.DirectDebitPaymentInstructions[1].FirstDebits);
            Assert.AreEqual("PREG1234567815011007:15:00-FR", directDebitRemittance.DirectDebitPaymentInstructions[1].PaymentInformationID);
        }

        [TestMethod]
        public void TheXMLFileIsCorrectlyGeneratedFormTheTestDataBase()
        {
            string relativePathToTestDatabase = @"TestFiles\TestMDB.mdb";
            string oleDBConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;" + "data source=" + relativePathToTestDatabase;

            MainInstance mainInstance = new MainInstance();
            mainInstance.GenerateSEPAXMLCustomerDirectDebitInitiationFromDatabase(oleDBConnectionString, "Test.xml");

            Assert.IsTrue(File.Exists(@"XMLOutputFiles\Test.xml"));
        }
    }
}
