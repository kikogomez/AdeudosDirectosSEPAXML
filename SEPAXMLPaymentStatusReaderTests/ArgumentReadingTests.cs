using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandLine;
using SEPAXMLPaymentStatusReportReader;

namespace SEPAXMLPaymentStatusReaderTests
{
    [TestClass]
    public class ArgumentReadingTests
    {

        static string errorString;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            var expectedErrorStringBuilder = new StringBuilder();
            expectedErrorStringBuilder.AppendLine("SEPA XML Payment Status Report (pain.002.001.03) Reader");
            expectedErrorStringBuilder.AppendLine("Arguments error.");
            expectedErrorStringBuilder.AppendLine("Required arguments:");
            expectedErrorStringBuilder.AppendLine("   -i InputPaymentStatusReportFullPath");
            expectedErrorStringBuilder.AppendLine("   -o OutputDataBaseFullPath");
            errorString = expectedErrorStringBuilder.ToString();
        }

        [TestMethod]
        public void PorvidingValidArgumentsReturnsTrueAndArgumentsAreCorrectlyStored()
        {
            string[] arguments = { "-i", "PaymentStatusReport.xml", "-o", "TestMDB.mdb", "-v" };
            ArgumentOptions argumentOptions = new ArgumentOptions();
            Parser parser= new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.HelpWriter = new StringWriter();    //Redirect error output to an StringWriter, instead of console
            });

            bool parseResult = parser.ParseArguments(arguments, argumentOptions);

            Assert.IsTrue(parseResult);
            Assert.AreEqual("", parser.Settings.HelpWriter.ToString());
            Assert.AreEqual("PaymentStatusReport.xml", argumentOptions.PaymentStatusReportFilePath);
            Assert.AreEqual("TestMDB.mdb", argumentOptions.DataBaseFullPath);
            Assert.AreEqual(true, argumentOptions.Verbose);
        }

        [TestMethod]
        public void PorvidingInvalidArgumentsReturnsTrueAndArgumentsArenull()
        {
            string[] arguments = { "-h", "dasdad", "r", "-k", "dasd" };
            ArgumentOptions argumentOptions = new ArgumentOptions();
            Parser parser = new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.HelpWriter = new StringWriter();    //Redirect error output to an StringWriter, instead of console
            });

            bool parseResult = parser.ParseArguments(arguments, argumentOptions);

            Assert.IsFalse(parseResult);
            Assert.AreEqual(errorString, parser.Settings.HelpWriter.ToString());
            Assert.AreEqual(null, argumentOptions.PaymentStatusReportFilePath);
            Assert.AreEqual(null, argumentOptions.DataBaseFullPath);
            Assert.AreEqual(false, argumentOptions.Verbose);
        }

        [TestMethod]
        public void InvalidCharactersAraParsedNormallyAInvalidArguments()
        {
            string[] arguments = { "-h", ",_$%", "r", "-k", "dasd" };
            ArgumentOptions argumentOptions = new ArgumentOptions();
            Parser parser = new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.HelpWriter = new StringWriter();    //Redirect error output to an StringWriter, instead of console
            });

            bool parseResult = parser.ParseArguments(arguments, argumentOptions);

            Assert.IsFalse(parseResult);
            Assert.AreEqual(errorString, parser.Settings.HelpWriter.ToString());
            Assert.AreEqual(null, argumentOptions.PaymentStatusReportFilePath);
            Assert.AreEqual(null, argumentOptions.DataBaseFullPath);
            Assert.AreEqual(false, argumentOptions.Verbose);
        }

        [TestMethod]
        public void ParsingIsCorrectlyInvokedFromProgram()
        {
            string[] arguments = { "-i", "PaymentStatusReport.xml", "-o", "TestMDB.mdb", "-v" };
            string parseErrorString;
            string xMLPSRFullPath;
            string databaseFullPath;
            bool verboseExecution;

            MainInstance mainInstane = new MainInstance();
            bool parseResult = mainInstane.ParseArguments(
                arguments,
                out parseErrorString,
                out xMLPSRFullPath,
                out databaseFullPath,
                out verboseExecution);

            Assert.IsTrue(parseResult);
            Assert.AreEqual("", parseErrorString);
            Assert.AreEqual("PaymentStatusReport.xml", xMLPSRFullPath);
            Assert.AreEqual("TestMDB.mdb", databaseFullPath);
            Assert.AreEqual(true, verboseExecution);
        }

        [TestMethod]
        public void ArgumentsAreNotCaseSensitive()
        {
            string[] arguments = { "-I", "PaymentStatusReport.xml", "-o", "TestMDB.mdb", "-V" };
            string parseErrorString;
            string xMLPSRFullPath;
            string databaseFullPath;
            bool verboseExecution;

            MainInstance mainInstane = new MainInstance();
            bool parseResult = mainInstane.ParseArguments(
                arguments,
                out parseErrorString,
                out xMLPSRFullPath,
                out databaseFullPath,
                out verboseExecution);

            Assert.IsTrue(parseResult);
            Assert.AreEqual("", parseErrorString);
            Assert.AreEqual("PaymentStatusReport.xml", xMLPSRFullPath);
            Assert.AreEqual("TestMDB.mdb", databaseFullPath);
            Assert.AreEqual(true, verboseExecution);
        }

        [TestMethod]
        public void OmittingVerboseParameterIsCorrectlParsed()
        {
            string[] arguments = { "-i", "PaymentStatusReport.xml", "-o", "TestMDB.mdb" };
            string parseErrorString;
            string xMLPSRFullPath;
            string databaseFullPath;
            bool verboseExecution;

            MainInstance mainInstane = new MainInstance();
            bool parseResult = mainInstane.ParseArguments(
                arguments,
                out parseErrorString,
                out xMLPSRFullPath,
                out databaseFullPath,
                out verboseExecution);

            Assert.IsTrue(parseResult);
            Assert.AreEqual("", parseErrorString);
            Assert.AreEqual("PaymentStatusReport.xml", xMLPSRFullPath);
            Assert.AreEqual("TestMDB.mdb", databaseFullPath);
            Assert.AreEqual(false, verboseExecution);
        }

        [TestMethod]
        public void OmitingRequiredArgument_PaymentStatusReportFullPath_InCommandLineReturnsFalseAndArgumentIsNull()
        {
            string[] arguments = { "-o", "TestMDB.mdb" };
            string parseErrorString;
            string dataBaseFullPath;
            string xMLPSRFullPath;
            bool verboseExecution;

            MainInstance mainInstane = new MainInstance();
            bool parseResult = mainInstane.ParseArguments(
                arguments,
                out parseErrorString,
                out xMLPSRFullPath,
                out dataBaseFullPath,
                out verboseExecution);

            Assert.IsFalse(parseResult);
            Assert.AreEqual(errorString, parseErrorString);
            Assert.AreEqual(null, xMLPSRFullPath);
            Assert.AreEqual("TestMDB.mdb", dataBaseFullPath);
            Assert.AreEqual(false, verboseExecution);
        }

        [TestMethod]
        public void OmitingRequiredArgument_DataBaseFullPath_InCommandLineReturnsFalseAndArgumentIsNull()
        {
            string[] arguments = { "-i", "PaymentStatusReport.xml" };
            string parseErrorString;
            string dataBaseFullPath;
            string xMLPSRFullPath;
            bool verboseExecution;

            MainInstance mainInstane = new MainInstance();
            bool parseResult = mainInstane.ParseArguments(
                arguments,
                out parseErrorString,
                out xMLPSRFullPath,
                out dataBaseFullPath,
                out verboseExecution);

            Assert.IsFalse(parseResult);
            Assert.AreEqual(errorString, parseErrorString);
            Assert.AreEqual("PaymentStatusReport.xml", xMLPSRFullPath);
            Assert.AreEqual(null, dataBaseFullPath);
            Assert.AreEqual(false, verboseExecution);
        }

        [TestMethod]
        public void NoArgumentsAreParsedInvalidAndArgumentsAreNull()
        {
            string[] arguments = { };
            string parseErrorString;
            string dataBaseFullPath;
            string xMLPSRFullPath;
            bool verboseExecution;

            MainInstance mainInstane = new MainInstance();
            bool parseResult = mainInstane.ParseArguments(
                arguments,
                out parseErrorString,
                out xMLPSRFullPath,
                out dataBaseFullPath,
                out verboseExecution);

            Assert.IsFalse(parseResult);
            Assert.AreEqual(errorString, parseErrorString);
            Assert.AreEqual(null, xMLPSRFullPath);
            Assert.AreEqual(null, dataBaseFullPath);
            Assert.AreEqual(false, verboseExecution);
        }

        [TestMethod]
        public void BothIncorrectArgumentsInCommandLineReturnsFalseAndArgumentsAreNull()
        {
            string[] arguments = { "-i", "-o"};
            string parseErrorString;
            string dataBaseFullPath;
            string xMLPSRFullPath;
            bool verboseExecution;

            MainInstance mainInstane = new MainInstance();
            bool parseResult = mainInstane.ParseArguments(
                arguments,
                out parseErrorString,
                out xMLPSRFullPath,
                out dataBaseFullPath,
                out verboseExecution);

            Assert.IsFalse(parseResult);
            Assert.AreEqual(errorString, parseErrorString);
            Assert.AreEqual(null, xMLPSRFullPath);
            Assert.AreEqual(null, dataBaseFullPath);
            Assert.AreEqual(false, verboseExecution);
        }

        [TestMethod]
        public void ExtraArgumentsReturnParseErrorButValidArgumentsAreCorrectlyParsed()
        {
            string[] arguments = { "-i", "PaymentStatusReport.xml", "-o", "TestMDB.mdb", "-v", "-d", "dads" };
            string parseErrorString;
            string xMLPSRFullPath;
            string databaseFullPath;
            bool verboseExecution;

            MainInstance mainInstane = new MainInstance();
            bool parseResult = mainInstane.ParseArguments(
                arguments,
                out parseErrorString,
                out xMLPSRFullPath,
                out databaseFullPath,
                out verboseExecution);

            Assert.IsFalse(parseResult);
            Assert.AreEqual(errorString, parseErrorString);
            Assert.AreEqual("PaymentStatusReport.xml", xMLPSRFullPath);
            Assert.AreEqual("TestMDB.mdb", databaseFullPath);
            Assert.AreEqual(true, verboseExecution);
        }

        [TestMethod]
        public void ValidOPathIsNotCheckedDuringParsing()
        {
            string[] arguments = { "-i", "ThisFileDoesNotExists.foo", "-o", "NeitherThisOne.foo" };
            string parseErrorString;
            string xMLPSRFullPath;
            string dataBaseFullPath;
            bool verboseExecution;

            MainInstance mainInstane = new MainInstance();
            bool parseResult = mainInstane.ParseArguments(
                arguments,
                out parseErrorString,
                out xMLPSRFullPath,
                out dataBaseFullPath,
                out verboseExecution);

            Assert.IsTrue(parseResult);
            Assert.AreEqual("", parseErrorString);
            Assert.AreEqual("ThisFileDoesNotExists.foo", xMLPSRFullPath);
            Assert.AreEqual("NeitherThisOne.foo", dataBaseFullPath);
            Assert.AreEqual(false, verboseExecution);
            Assert.IsFalse(File.Exists("ThisFileDoesNotExists.foo"));
            Assert.IsFalse(File.Exists("NeitherThisOne.foo"));
        }
    }
}
