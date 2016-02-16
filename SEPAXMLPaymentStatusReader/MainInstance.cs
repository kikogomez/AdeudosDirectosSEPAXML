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
            string errorsInPathString;
            string sourcePaymentStatusReportPath;
            string dataBasePath;

            if (!ParseArguments(args, out parseErrorString, out sourcePaymentStatusReportPath, out dataBasePath, out verboseExecution))
            {
                Console.WriteLine(parseErrorString);
                Console.WriteLine("Press any key to close program...");
                Console.ReadKey();
                Environment.Exit((int)ExitCodes.InvalidArguments);
            }

            if (verboseExecution) Console.WriteLine("Locating sorce XML Payment Status Report...");
            errorsInPathString = ErrorsInPath(sourcePaymentStatusReportPath);
            if (errorsInPathString != "")
            {
                Console.WriteLine(errorsInPathString);
                Console.WriteLine("Press any key to close program...");
                Console.ReadKey();
                Environment.Exit((int)ExitCodes.InvalidPaymentStatusFilePath);
            }

            if (verboseExecution) Console.WriteLine("Locating database to write to...");
            errorsInPathString = ErrorsInPath(dataBasePath);
            if (errorsInPathString != "")
            {
                Console.WriteLine(errorsInPathString);
                Console.WriteLine("Press any key to close program...");
                Console.ReadKey();
                Environment.Exit((int)ExitCodes.InvalidDataBasePath);
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

        public string CreateDatabaseConnectionString(string pathToDataBase)
        {
            return "Provider=Microsoft.JET.OLEDB.4.0;" + "data source=" + pathToDataBase;
        }

        public void ReadPaymentStatusReportIntoDataBase(string oleDBConnectionString, string sourcePaymentStatusReportPath)
        {
            PaymentStatusReport paymentStatusReport = ReadPaymentStatusReportXMLFile(sourcePaymentStatusReportPath);

            //Next, connect to database and write data into it

        }

        private PaymentStatusReport ReadPaymentStatusReportXMLFile(string sourcePaymentStatusReportPath)
        {
            if (verboseExecution) Console.WriteLine("Reading file {0}", Path.GetFileName(sourcePaymentStatusReportPath));
            string xmlStringMessage = ReadXMLSourceFileToString(sourcePaymentStatusReportPath);

            if (verboseExecution) Console.WriteLine("Parsing xML Message...", Path.GetFileName(sourcePaymentStatusReportPath));
            SEPAMessagesManager sEPAMessagesManager = new SEPAMessagesManager();
            PaymentStatusReport paymentStatusReport = null;
            //try
            //{
            //    paymentStatusReport = sEPAMessagesManager.ReadISO20022PaymentStatusReportStringMessage(xmlStringMessage);
            //}

            //catch (ArgumentException validationException) when (validationException is InvalidOperationException || validationException is System.Xml.XmlException)
            //{
            //    string errorMessage = "";
            //    int exitCode = 0;

            //    //switch (validationException.GetType().ToString())
            //    //{
            //    //    case "System.InvalidOperationException":
            //    //        errorMessage = "The source file is not a valid XML" + Environment.NewLine + ((InvalidOperationException)validationException).Message;
            //    //        exitCode = (int)ExitCodes.NotValidXMLFile;
            //    //        break;
            //    //    case "System.Xml.XmlException":
            //    //        errorMessage = "The source file is not compilant to pain.002.001.03" + Environment.NewLine + ((System.Xml.XmlException)validationException).Message;
            //    //        exitCode = (int)ExitCodes.NotValidXMLFile;
            //    //        break;
            //    //}

            //    ///Nota: Posibles errores
            //    /// -> El fichero no es un XML valido (errores con la construccion de los nodos, etiqueta incompletas....)
            //    /// -> El fichero no valida frente al esquema XSD
            //    ///Hay que conseguir que a traves de la excepcion podamos distinguir todas las posibilidades
            //    ///Por lo pronto falta discernir los errores de lectura generales
            //    //if (xMLFileErrorException.InnerException != null)
            //    //{
            //    //    Console.WriteLine("The source file is not a valid XML file");

            //    //}
            //    //else
            //    //{

            //    //}
            //}

            return paymentStatusReport;
        }

        private string ReadXMLSourceFileToString(string sourcePaymentStatusReportPath)
        {
            string xmlMessage = null;
            try
            {
                xmlMessage = File.ReadAllText(sourcePaymentStatusReportPath);
            }
            catch (Exception fileReadException) when (fileReadException is IOException || fileReadException is UnauthorizedAccessException || fileReadException is System.Security.SecurityException)
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
            return xmlMessage;
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
                return "Path to file contains invalid characters" + Environment.NewLine + filePathErrorException.Message;
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

    }
}
