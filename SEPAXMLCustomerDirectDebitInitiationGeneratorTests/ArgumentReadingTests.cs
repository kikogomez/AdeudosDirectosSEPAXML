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

        [TestMethod]
        public void PorvidingValidArgumentsReturnsTrueAndArgumentsAreCorrectlyStored()
        {
            string[] arguments = { "-i", "TestMDB.mdb" , "-o", "outputFile.xml", "-v" };
            ArgumentOptions argumentOptions = new ArgumentOptions();
            Parser parser= new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.HelpWriter = new StringWriter();    //Redirect error output to an StringWriter, instead of console
            });

            bool parseResult = parser.ParseArguments(arguments, argumentOptions);

            Assert.IsTrue(parseResult);
            Assert.AreEqual("", parser.Settings.HelpWriter.ToString());
            Assert.AreEqual("TestMDB.mdb", argumentOptions.SourceDataBase);
            Assert.AreEqual("outputFile.xml", argumentOptions.OutputXMLFile);
            Assert.AreEqual(true, argumentOptions.Verbose);
        }

        [TestMethod]
        public void ParsingIsCorrectlyInvokedFromProgram()
        {
            string[] arguments = { "-i", "TestMDB.mdb", "-o", "outputFile.xml", "-v" };
            string parseErrorString;
            string sourceDatabaseFullPath;
            string xMLCDDFilename;
            bool verboseExecution;

            MainInstance mainInstane = new MainInstance();
            bool parseResult = mainInstane.ParseArguments(
                arguments,
                out parseErrorString,
                out sourceDatabaseFullPath,
                out xMLCDDFilename,
                out verboseExecution);

            Assert.IsTrue(parseResult);
            Assert.AreEqual("", parseErrorString);
            Assert.AreEqual("TestMDB.mdb", sourceDatabaseFullPath);
            Assert.AreEqual("outputFile.xml", xMLCDDFilename);
            Assert.AreEqual(true, verboseExecution);
        }

        [TestMethod]
        public void ArgumentsAreNotCaseSensitive()
        {
            string[] arguments = { "-I", "TestMDB.mdb", "-o", "outputFile.xml", "-V" };
            string parseErrorString;
            string sourceDatabaseFullPath;
            string xMLCDDFilename;
            bool verboseExecution;

            MainInstance mainInstane = new MainInstance();
            bool parseResult = mainInstane.ParseArguments(
                arguments,
                out parseErrorString,
                out sourceDatabaseFullPath,
                out xMLCDDFilename,
                out verboseExecution);

            Assert.IsTrue(parseResult);
            Assert.AreEqual("", parseErrorString);
            Assert.AreEqual("TestMDB.mdb", sourceDatabaseFullPath);
            Assert.AreEqual("outputFile.xml", xMLCDDFilename);
            Assert.AreEqual(true, verboseExecution);
        }

        [TestMethod]
        public void OmittingVerboseParameterIsCorrectlParsed()
        {
            string[] arguments = { "-i", "TestMDB.mdb", "-o", "outputFile.xml" };
            string parseErrorString;
            string sourceDatabaseFullPath;
            string xMLCDDFilename;
            bool verboseExecution;

            MainInstance mainInstane = new MainInstance();
            bool parseResult = mainInstane.ParseArguments(
                arguments,
                out parseErrorString,
                out sourceDatabaseFullPath,
                out xMLCDDFilename,
                out verboseExecution);

            Assert.IsTrue(parseResult);
            Assert.AreEqual("", parseErrorString);
            Assert.AreEqual("TestMDB.mdb", sourceDatabaseFullPath);
            Assert.AreEqual("outputFile.xml", xMLCDDFilename);
            Assert.AreEqual(false, verboseExecution);
        }

        [TestMethod]
        public void IncorrectDataBaseFullPathArgumentInCommandLineReturnsFalseAndArgumentIsNull()
        {
            string[] arguments = { "-i", "TestMDB.mdb" };
            string parseErrorString;
            string sourceDatabaseFullPath;
            string xMLCDDFilename;
            bool verboseExecution;

            MainInstance mainInstane = new MainInstance();
            bool parseResult = mainInstane.ParseArguments(
                arguments,
                out parseErrorString,
                out sourceDatabaseFullPath,
                out xMLCDDFilename,
                out verboseExecution);

            Assert.IsFalse(parseResult);
            Assert.AreEqual(errorString, parseErrorString);
            Assert.AreEqual("TestMDB.mdb", sourceDatabaseFullPath);
            Assert.AreEqual(null, xMLCDDFilename);
            Assert.AreEqual(false, verboseExecution);
        }

        [TestMethod]
        public void ValidOPathIsNotCheckedDuringParsing()
        {
            string[] arguments = { "-i", "TestMDB.mdb", "-o", "outputFile.xml" };
            string parseErrorString;
            string sourceDatabaseFullPath;
            string xMLCDDFilename;
            bool verboseExecution;

            MainInstance mainInstane = new MainInstance();
            bool parseResult = mainInstane.ParseArguments(
                arguments,
                out parseErrorString,
                out sourceDatabaseFullPath,
                out xMLCDDFilename,
                out verboseExecution);

            Assert.IsTrue(parseResult);
            Assert.AreEqual("", parseErrorString);
            Assert.AreEqual("TestMDB.mdb", sourceDatabaseFullPath);
            Assert.AreEqual("outputFile.xml", xMLCDDFilename);
            Assert.AreEqual(false, verboseExecution);
        }

        [TestMethod]
        public void IncorrectOutputXMLFileNameArgumentInCommandLineReturnsFalseAndArgumentIsNull()
        {
            string[] arguments = { "-o", "outputFile.xml"};
            string parseErrorString;
            string sourceDatabaseFullPath;
            string xMLCDDFilename;
            bool verboseExecution;

            MainInstance mainInstane = new MainInstance();
            bool parseResult = mainInstane.ParseArguments(
                arguments,
                out parseErrorString,
                out sourceDatabaseFullPath,
                out xMLCDDFilename,
                out verboseExecution);

            Assert.IsFalse(parseResult);
            Assert.AreEqual(errorString, parseErrorString);
            Assert.AreEqual(null, sourceDatabaseFullPath);
            Assert.AreEqual("outputFile.xml", xMLCDDFilename);
            Assert.AreEqual(false, verboseExecution);
        }

        [TestMethod]
        public void BothIncorrectArgumentsInCommandLineReturnsFalseAndArgumentsAreNull()
        {
            string[] arguments = { "-i", "-o" };
            string parseErrorString;
            string sourceDatabaseFullPath;
            string xMLCDDFilename;
            bool verboseExecution;

            MainInstance mainInstane = new MainInstance();
            bool parseResult = mainInstane.ParseArguments(
                arguments,
                out parseErrorString,
                out sourceDatabaseFullPath,
                out xMLCDDFilename,
                out verboseExecution);

            Assert.IsFalse(parseResult);
            Assert.AreEqual(errorString, parseErrorString);
            Assert.AreEqual(null, sourceDatabaseFullPath);
            Assert.AreEqual(null, xMLCDDFilename);
            Assert.AreEqual(false, verboseExecution);
        }

        [TestMethod]
        public void NoArgumentsAreParsedInvalid()
        {
            string[] arguments = { };
            string parseErrorString;
            string sourceDatabaseFullPath;
            string xMLCDDFilename;
            bool verboseExecution;

            MainInstance mainInstane = new MainInstance();
            bool parseResult = mainInstane.ParseArguments(
                arguments,
                out parseErrorString,
                out sourceDatabaseFullPath,
                out xMLCDDFilename,
                out verboseExecution);

            Assert.IsFalse(parseResult);
            Assert.AreEqual(errorString, parseErrorString);
            Assert.AreEqual(null, sourceDatabaseFullPath);
            Assert.AreEqual(null, xMLCDDFilename);
            Assert.AreEqual(false, verboseExecution);
        }
    }
}
