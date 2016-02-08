using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandLine;
using SEPAXMLCustomerDirectDebitInitiationGenerator;

namespace SEPAXMLCustomerDirectDebitInitiationGeneratorTests
{
    [TestClass]
    public class ArgumentReadingTests
    {

        static string errorString;
        ArgumentOptions argumentOptions;
        Parser parser;

        string parth = @"XML Test Files\TestMDB.mdb";

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            var expectedErrorStringBuilder = new StringBuilder();
            expectedErrorStringBuilder.AppendLine("SEPA XML Customer Direct Debit Initiation Message (pain.008.001.02) Generator");
            expectedErrorStringBuilder.AppendLine("Arguments error.");
            expectedErrorStringBuilder.AppendLine("Required arguments:");
            expectedErrorStringBuilder.AppendLine("   -i SourceDatabaseFullPath");
            expectedErrorStringBuilder.AppendLine("   -o OutputXMLFileName");
            errorString = expectedErrorStringBuilder.ToString();
        }

        [TestInitialize]
        public void TestInit()
        {
            argumentOptions = new ArgumentOptions();
            parser = new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.HelpWriter = new StringWriter();    //Redirect error output to an StringWriter, instead of console
            });
        }

        [TestMethod]
        public void PorvidingValidArgumentsReturnsTrueAndArgumentsAreCorrectlyStored()
        {
            string[] arguments = { "-i", "TestMDB.mdb" , "-o", "outputFile.xml", "-v" };

            bool parseResult = parser.ParseArguments(arguments, argumentOptions);

            Assert.IsTrue(parseResult);
            Assert.AreEqual("", parser.Settings.HelpWriter.ToString());
            Assert.AreEqual("TestMDB.mdb", argumentOptions.SourceDataBase);
            Assert.AreEqual("outputFile.xml", argumentOptions.OutputXMLFile);
            Assert.AreEqual(true, argumentOptions.Verbose);
        }

        [TestMethod]
        public void ArgumentsAreNotCaseSensitive()
        {
            string[] arguments = { "-I", "TestMDB.mdb", "-o", "outputFile.xml", "-V" };

            bool parseResult = parser.ParseArguments(arguments, argumentOptions);

            Assert.IsTrue(parseResult);
            Assert.AreEqual("", parser.Settings.HelpWriter.ToString());
            Assert.AreEqual("TestMDB.mdb", argumentOptions.SourceDataBase);
            Assert.AreEqual("outputFile.xml", argumentOptions.OutputXMLFile);
            Assert.AreEqual(true, argumentOptions.Verbose);
        }

        [TestMethod]
        public void IncorrectDataBaseFullPathArgumentInCommandLineReturnsFalseAndArgumentIsNull()
        {
            string[] arguments = { "-i", "TestMDB.mdb" };

            bool parseResult = parser.ParseArguments(arguments, argumentOptions);

            Assert.IsFalse(parseResult);
            Assert.AreEqual(errorString, parser.Settings.HelpWriter.ToString());
            Assert.AreEqual("TestMDB.mdb", argumentOptions.SourceDataBase);
            Assert.AreEqual(null, argumentOptions.OutputXMLFile);
        }

        [TestMethod]
        public void ValidOPathIsNotCheckedDuringParsing()
        {
            string[] arguments = { "-i", "TestMDB.mdb", "-o", "outputFile.xml" };

            bool parseResult = parser.ParseArguments(arguments, argumentOptions);

            Assert.IsTrue(parseResult);
            Assert.AreEqual("", parser.Settings.HelpWriter.ToString());
            Assert.AreEqual("TestMDB.mdb", argumentOptions.SourceDataBase);
            Assert.IsFalse(File.Exists(argumentOptions.SourceDataBase));
        }

        [TestMethod]
        public void IncorrectOutputXMLFileNameArgumentInCommandLineReturnsFalseAndArgumentIsNull()
        {
            string[] arguments = { "-o", "outputFile.xml"};

            bool parseResult = parser.ParseArguments(arguments, argumentOptions);

            Assert.IsFalse(parseResult);
            Assert.AreEqual(errorString, parser.Settings.HelpWriter.ToString());
            Assert.AreEqual(null, argumentOptions.SourceDataBase);
            Assert.AreEqual("outputFile.xml", argumentOptions.OutputXMLFile);
        }

        [TestMethod]
        public void BothIncorrectArgumentsInCommandLineReturnsFalseAndArgumentsAreNull()
        {
            string[] arguments = { "-i", "-o" };

            bool parseResult = parser.ParseArguments(arguments, argumentOptions);

            Assert.IsFalse(parseResult);
            Assert.AreEqual(errorString, parser.Settings.HelpWriter.ToString());
            Assert.AreEqual(null, argumentOptions.SourceDataBase);
            Assert.AreEqual(null, argumentOptions.OutputXMLFile);
        }

        [TestMethod]
        public void NoArgumentsAreParsedInvalid()
        {
            string[] arguments = { };

            bool parseResult = parser.ParseArguments(arguments, argumentOptions);

            Assert.IsFalse(parseResult);
            Assert.AreEqual(errorString, parser.Settings.HelpWriter.ToString());
            Assert.AreEqual(null, argumentOptions.SourceDataBase);
            Assert.AreEqual(null, argumentOptions.OutputXMLFile);
        }

        //[TestMethod]
        //public static void dsa()
        //{
        //    //static string sourceDatabaseFullPath;
        //    //static string xMLCDDFilename;
        //    //static bool verboseExecution;

            

        //    string[] arguments = { "-i", "TestMDB.mdb", "-o", "outputFile.xml" };

        //    bool parseResult = parser.ParseArguments(arguments, argumentOptions);

        //    Assert.IsFalse(parseResult);
        //    Assert.AreEqual(errorString, parser.Settings.HelpWriter.ToString());
        //    Assert.AreEqual(null, argumentOptions.SourceDataBase);
        //    Assert.AreEqual(null, argumentOptions.OutputXMLFile);
        //}
    }
}
