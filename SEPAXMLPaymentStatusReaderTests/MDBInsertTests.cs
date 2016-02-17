using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using System.Data.OleDb;
using SEPAXMLPaymentStatusReportReader;
using DirectDebitElements;


namespace SEPAXMLPaymentStatusReportReaderTests
{
    [TestClass]
    public class MDBInsertTests
    {
        static PaymentStatusReport paymentStatusReport;

        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            // La inicializacion de las transacciones no se hace con variables estáticas
            // pues la suscripcion a eventos interacciona entre los tests

            DirectDebitTransactionReject directDebitTransactionReject1 = new DirectDebitTransactionReject(
                "0123456788",
                "2015120100124",
                DateTime.Parse("2015-12-01"),
                80,
                "000001102564",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES6812345678061234567890")),
                "MS02");

            DirectDebitTransactionReject directDebitTransactionReject2 = new DirectDebitTransactionReject(
                "0123456789",
                "2015120100312",
                DateTime.Parse("2015-12-01"),
                70,
                "00000110421",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES3011112222003333333333")),
                "MS01");

            DirectDebitTransactionReject directDebitTransactionReject3 = new DirectDebitTransactionReject(
                "0123456790",
                "2015150100124",
                DateTime.Parse("2015-11-15"),
                80,
                "000001102564",
                new BankAccount(new InternationalAccountBankNumberIBAN("ES6812345678061234567890")),
                "MS02");

            List<DirectDebitTransactionReject> directDebitTransactionsRejectsList1 =
                new List<DirectDebitTransactionReject>()
                { directDebitTransactionReject1, directDebitTransactionReject2 };

            List<DirectDebitTransactionReject> directDebitTransactionsRejectsList2 =
                new List<DirectDebitTransactionReject>()
                { directDebitTransactionReject3};

            DirectDebitPaymentInstructionReject directDebitPaymentInstruction1 = new DirectDebitPaymentInstructionReject(
                "PaymentInstruction1",
                directDebitTransactionsRejectsList1);

            DirectDebitPaymentInstructionReject directDebitPaymentInstruction2 = new DirectDebitPaymentInstructionReject(
                "PaymentInstruction2",
                directDebitTransactionsRejectsList2);

            List<DirectDebitPaymentInstructionReject> directDebitPaymentInstructionRejects = new List<DirectDebitPaymentInstructionReject>()
            { directDebitPaymentInstruction1, directDebitPaymentInstruction2};

            paymentStatusReport = new PaymentStatusReport(
                "PSRMesssageID",
                new DateTime(2016,02, 12),
                new DateTime(2016, 02, 15),
                directDebitPaymentInstructionRejects);
        }

        [TestMethod]
        public void TestFileExistsWhileAccesingWithRelativePath()
        {
            string relativePathToTestDatabase = @"TestFiles\TestMDB.mdb";
            Assert.IsTrue(File.Exists(relativePathToTestDatabase));
        }

        [TestMethod]
        public void TestFileExistsWhileAccesingWithAbsolutePath()
        {
            //string applicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relativePathToTestDatabase = @"TestFiles\TestMDB.mdb";
            string testDatabaseFullPath = baseDirectory + @"\" + relativePathToTestDatabase;
            Assert.IsTrue(File.Exists(testDatabaseFullPath));
        }

        [TestMethod]
        public void TheConnectionStringIsValidWithRelativePath()
        {
            string relativePathToTestDatabase = @"TestFiles\TestMDB.mdb";

            MainInstance mainInstance = new MainInstance();
            string connectionString = mainInstance.CreateDatabaseConnectionString(relativePathToTestDatabase);

            OleDbConnection connection = new OleDbConnection(connectionString);
            connection.Open();
            Assert.IsTrue(connection.State == System.Data.ConnectionState.Open);
            connection.Close();
        }

        [TestMethod]
        public void TheConnectionStringIsValidWithAbsolutePath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relativePathToTestDatabase = @"TestFiles\TestMDB.mdb";
            string testDatabaseFullPath = baseDirectory + @"\" + relativePathToTestDatabase;

            MainInstance mainInstance = new MainInstance();
            string connectionString = mainInstance.CreateDatabaseConnectionString(testDatabaseFullPath);

            OleDbConnection connection = new OleDbConnection(connectionString);
            connection.Open();
            Assert.IsTrue(connection.State == System.Data.ConnectionState.Open);
            connection.Close();
        }

        [TestMethod]
        public void TheTransactionsRejectsAreCorrectlyInserted()
        {
            //To ensure no interference in DataBase, we can
            // - Make a TRANSACTION and rollback at the end
            // - Make a copy of the database for each test
            //    string thisTestDataBasename = "TestMDB" + testContextInstance.TestName + ".MDB";
            //    File.Copy(@"TestFiles\TestMDB.mdb", @"TestFiles\" + thisTestDataBasename);

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relativePathToTestDatabase = @"TestFiles\TestMDB.mdb";
            string testDatabaseFullPath = baseDirectory + @"\" + relativePathToTestDatabase;
            MainInstance mainInstance = new MainInstance();
            string connectionString = mainInstance.CreateDatabaseConnectionString(testDatabaseFullPath);
            OleDbConnection connection = new OleDbConnection(connectionString);
            connection.Open();
            
            int numberOfInsertedTransactionRejects = mainInstance.InsertTransactionRejectsIntoDatabase(connection, paymentStatusReport);

            Assert.AreEqual(paymentStatusReport.NumberOfTransactions, numberOfInsertedTransactionRejects);
        }

    }
}
