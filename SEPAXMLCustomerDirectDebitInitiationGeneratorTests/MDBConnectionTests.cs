using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.OleDb;
using SEPAXMLCustomerDirectDebitInitiationGenerator;

namespace SEPAXMLCustomerDirectDebitInitiationGeneratorTests
{
    [TestClass]
    public class MDBConnectionTests
    {
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


    }
}
