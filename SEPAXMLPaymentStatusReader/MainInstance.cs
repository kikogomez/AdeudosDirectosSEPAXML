using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using System.IO;
using DirectDebitElements;

namespace SEPAXMLPaymentStatusReportReader
{
    public class MainInstance
    {
        bool verboseExecution;

        public MainInstance()
        { }

        public void Run(string[] args)
        {
            string parseErrorString;
            string errorsInPath;
            string sourcePaymentStatusReportPath;
            string dataBasePath;
            int exitCode;

            if (!ParseArguments(args, out parseErrorString, out sourcePaymentStatusReportPath, out dataBasePath, out verboseExecution))
            {
                Console.WriteLine(parseErrorString);
                Console.WriteLine("Press any key to close program...");
                Console.ReadKey();
                Environment.Exit((int)ExitCodes.InvalidArguments);
            }

            if (!PathsToFilesAreCorrect(sourcePaymentStatusReportPath, dataBasePath, out errorsInPath, out exitCode))
            {
                Console.WriteLine(errorsInPath);
                Console.WriteLine("Press any key to close program...");
                Console.ReadKey();
                Environment.Exit(exitCode);
            }

            string oleDBConnectionString = CreateDatabaseConnectionString(dataBasePath);

            ReadPaymentStatusReportIntoDataBase(oleDBConnectionString, sourcePaymentStatusReportPath);

            if (verboseExecution)
            {
                Console.WriteLine("Completed!");
                Console.WriteLine("Payment Status report read: {0}", sourcePaymentStatusReportPath);
                Console.WriteLine("Press any key to close program...");
                Console.ReadKey();
            }
            Environment.Exit((int)ExitCodes.Success);
        }

        public bool ParseArguments(
            string[] arguments,
            out string parseErrorString,
            out string sourcePaymentStatusReportPath,
            out string dataBasePath,
            out bool verboseExecution)
        {
            bool argumentsParseOk;
            ArgumentOptions argumentOptions = new ArgumentOptions();
            Parser parser = new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.HelpWriter = new StringWriter();    //Redirect error output to an StringWriter, instead of console
            });

            argumentsParseOk = (parser.ParseArguments(arguments, argumentOptions) ? true : false);
            parseErrorString = parser.Settings.HelpWriter.ToString();
            sourcePaymentStatusReportPath = argumentOptions.PaymentStatusReportFilePath;
            dataBasePath = argumentOptions.DataBaseFullPath;
            verboseExecution = argumentOptions.Verbose;
            return argumentsParseOk;
        }

        public bool PathsToFilesAreCorrect(
            string sourcePaymentStatusReportPath,
            string dataBasePath,
            out string errorsInPathString,
            out int exitCode)
        {

            if (verboseExecution) Console.WriteLine("Locating sorce XML Payment Status Report...");
            errorsInPathString = ErrorsInPath(sourcePaymentStatusReportPath);
            if (errorsInPathString != "")
            {
                errorsInPathString = sourcePaymentStatusReportPath + Environment.NewLine + errorsInPathString;
                exitCode = ((int)ExitCodes.InvalidPaymentStatusFilePath);
                return false;
            }

            if (verboseExecution) Console.WriteLine("Locating database to write to...");
            errorsInPathString = ErrorsInPath(dataBasePath);
            if (errorsInPathString != "")
            {
                errorsInPathString = dataBasePath + Environment.NewLine + errorsInPathString;
                exitCode = ((int)ExitCodes.InvalidDataBasePath);
                return false;
            }
            exitCode = 0;
            return true;
        }

        public string CreateDatabaseConnectionString(string pathToDataBase)
        {
            return "Provider=Microsoft.JET.OLEDB.4.0;" + "data source=" + pathToDataBase;
        }

        public void ReadPaymentStatusReportIntoDataBase(string oleDBConnectionString, string sourcePaymentStatusReportPath)
        {
            PaymentStatusReport paymentStatusReport = ReadPaymentStatusReportXMLFile(sourcePaymentStatusReportPath);

            //Next, connect to database and write data into it

        }

        public PaymentStatusReport ReadPaymentStatusReportXMLFile(string sourcePaymentStatusReportPath)
        {
            if (verboseExecution) Console.WriteLine("Reading file {0}", Path.GetFileName(sourcePaymentStatusReportPath));
            string xmlStringMessage = ReadXMLSourceFileToString(sourcePaymentStatusReportPath);

            if (verboseExecution) Console.WriteLine("Parsing xML Message...", Path.GetFileName(sourcePaymentStatusReportPath));
            PaymentStatusReport paymentStatusReport = null;
            try
            {
                paymentStatusReport = ParsePaymentStatusReportString(xmlStringMessage);
            }
            catch (Exception paymentStatusReadException)
            when (paymentStatusReadException is System.Xml.XmlException || paymentStatusReadException is System.Xml.Schema.XmlSchemaValidationException)
            {
                ProcessPaymentStatusReportParsingException(paymentStatusReadException);
            }
            return paymentStatusReport;
        }

        public string ReadXMLSourceFileToString(string sourcePaymentStatusReportPath)
        {
            string xmlMessage = null;
            try
            {
                xmlMessage = File.ReadAllText(sourcePaymentStatusReportPath);
            }
            catch (Exception fileReadException) when (fileReadException is IOException || fileReadException is UnauthorizedAccessException || fileReadException is System.Security.SecurityException)
            {
                ProcessFileReadToStringException(fileReadException);
            }
            return xmlMessage;
        }

        public PaymentStatusReport ParsePaymentStatusReportString(string xmlStringMessage)
        {
            SEPAMessagesManager sEPAMessagesManager = new SEPAMessagesManager();
            return sEPAMessagesManager.ReadISO20022PaymentStatusReportStringMessage(xmlStringMessage);
        }

        private string ErrorsInPath(string fullPathToCheck)
        {
            string fileName = "";
            try
            {
                fileName = Path.GetFileName(fullPathToCheck);
            }
            catch (ArgumentException filePathErrorException)
            {
                return "Path to file contains invalid characters";
            }
            if (fileName == "")
            {
                return "No file specified in path";
            }
            if (!File.Exists(fullPathToCheck))
            {
                return "File not found!";
            }
            return "";
        }

        private void ProcessFileReadToStringException(Exception fileReadException)
        {
            string errorMessage = "";
            int exitCode = 0;
            switch (fileReadException.GetType().ToString())
            {
                case "System.IO.IOException":
                    errorMessage = "File read error!" + Environment.NewLine + ((IOException)fileReadException).Message;
                    exitCode = (int)ExitCodes.FileReadingError;
                    break;
                case "System.UnauthorizedAccessException":
                case "System.Security.SecurityException":
                    errorMessage = "You don't have permision to read file or directory" + Environment.NewLine + fileReadException.Message;
                    exitCode = (int)ExitCodes.FileReadingError;
                    break;
            }
            Console.WriteLine(errorMessage);
            Environment.Exit(exitCode);
        }

        private void ProcessPaymentStatusReportParsingException(Exception paymentStatusReadException)
        {
            string errorMessage;
            switch (paymentStatusReadException.GetType().ToString())
            {
                case "System.Xml.XmlException":
                    errorMessage = "The source file is not a valid XML"
                        + Environment.NewLine + ((System.Xml.XmlException)paymentStatusReadException).Message;
                    Console.WriteLine(errorMessage);
                    Console.WriteLine("Press any key to close program...");
                    Console.ReadKey();
                    Environment.Exit((int)ExitCodes.NotValidXMLFile);
                    break;
                case "System.Xml.Schema.XmlSchemaValidationException":
                    errorMessage = "The source file is not compilant to pain.002.001.03"
                        + Environment.NewLine + ((System.Xml.Schema.XmlSchemaValidationException)paymentStatusReadException).Message;
                    Console.WriteLine("Press any key to close program...");
                    Console.ReadKey();
                    Environment.Exit((int)ExitCodes.NotCompilantToSchemaFile);
                    break;
            }
        }
    }
}
