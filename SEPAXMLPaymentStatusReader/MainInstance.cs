using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using System.IO;
using DirectDebitElements;

namespace SEPAXMLPaymentStatusReader
{
    public class MainInstance
    {
        bool verboseExecution;

        public MainInstance()
        { }

        public void Run(string[] args)
        {
            string parseErrorString;
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
            if (!PathIsValid(sourcePaymentStatusReportPath))
            {
                Console.WriteLine("Press any key to close program...");
                Console.ReadKey();
                Environment.Exit((int)ExitCodes.InvalidPaymentStatusFilePath);
            }

            //if (!File.Exists(sourcePaymentStatusReportPath))
            //{
            //    Console.WriteLine("{0} not found!", Path.GetFileName(sourcePaymentStatusReportPath));
            //    Console.WriteLine("Press any key to close program...");
            //    Console.ReadKey();
            //    Environment.Exit((int)ExitCodes.InvalidPaymentStatusFilePath);
            //}

            if (verboseExecution) Console.WriteLine("Locating database to write to...");
            if (!PathIsValid(dataBasePath))
            {
                Console.WriteLine("Press any key to close program...");
                Console.ReadKey();
                Environment.Exit((int)ExitCodes.InvalidDataBasePath);
            }

            //if (!File.Exists(dataBasePath))
            //{
            //    Console.WriteLine("{0} not found!", dataBasePath);
            //    Console.WriteLine("Press any key to close program...");
            //    Console.ReadKey();
            //    Environment.Exit((int)ExitCodes.InvalidDataBasePath);
            //}

            string oleDBConnectionString = CreateDatabaseConnectionString(dataBasePath);

            ReadPaymentStatusReportIntoDataBase(oleDBConnectionString, sourcePaymentStatusReportPath);

            if (verboseExecution)
            {
                Console.WriteLine("Completed!");
                Console.WriteLine("Payment Status report readd: {0}", sourcePaymentStatusReportPath);
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
            try
            {
                paymentStatusReport = sEPAMessagesManager.ReadISO20022PaymentStatusReportStringMessage(xmlStringMessage);
            }

            catch (Exception ex) when (ex is InvalidOperationException || ex is System.Xml.XmlException)
            {
                ///Nota: Posibles errores
                /// -> El fichero no es un XML valido (errores con la construccion de los nodos, etiqueta incompletas....)
                /// -> El fichero no valida frente al esquema XSD
                ///Hay que conseguir que a traves de la excepcion podamos distinguir todas las posibilidades
                ///Por lo pronto falta discernir los errores de lectura generales
                //if (xMLFileErrorException.InnerException != null)
                //{
                //    Console.WriteLine("The source file is not a valid XML file");

                //}
                //else
                //{

                //}
            }

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

        private bool PathIsValid(string fullPathToCheck)
        {
            string fileName = "";
            try
            {
                fileName = Path.GetFileName(fullPathToCheck);
            }
            catch (ArgumentException filePathErrorException)
            {
                Console.WriteLine("Path to file contains invalid characters");
                Console.WriteLine(filePathErrorException.Message);
                return false;

            }
            if (fileName == "")
            {
                Console.WriteLine("No file specified in path");
                return false;
            }
            if (!File.Exists(fullPathToCheck))
            {
                Console.WriteLine("File not found!");
                return false;
            }
            return true;
        }

    }
}
