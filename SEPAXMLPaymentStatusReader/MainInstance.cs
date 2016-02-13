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
            if (!File.Exists(sourcePaymentStatusReportPath))
            {
                Console.WriteLine("{0} not found!", sourcePaymentStatusReportPath);
                Console.WriteLine("Press any key to close program...");
                Console.ReadKey();
                Environment.Exit((int)ExitCodes.InvalidPaymentStatusFilePath);
            }

            if (verboseExecution) Console.WriteLine("Locating database to write to...");
            if (!File.Exists(dataBasePath))
            {
                Console.WriteLine("{0} not found!", dataBasePath);
                Console.WriteLine("Press any key to close program...");
                Console.ReadKey();
                Environment.Exit((int)ExitCodes.InvalidDataBasePath);
            }

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
            string fileName = Path.GetFileName(sourcePaymentStatusReportPath);
            if (verboseExecution) Console.WriteLine("Reading file {0}", fileName);
            SEPAMessagesManager sEPAMessagesManager = new SEPAMessagesManager();
            PaymentStatusReport paymentStatusReport=null;
            try
            {
                paymentStatusReport = sEPAMessagesManager.ReadISO20022PaymentStatusReportFile(sourcePaymentStatusReportPath);

                ///Nota: Posibles errores
                /// -> Error general de lectura (problemas de acceso a disco)
                /// -> El fichero no es un XML valido (errores con la construccion de los nodos, etiqueta incompletas....)
                /// -> El fichero no valida frente al esquema XSD
                ///Hay que conseguir que a traves de la excepcion podamos distinguir todas las posibilidades
                ///Por lo pronto falta discernir los errores de lectura generales

            }
            catch (ArgumentException xMLFileErrorException)
            {
                if (xMLFileErrorException.InnerException!=null)
                {
                    Console.WriteLine("The source file is not a valid XML file");
                    
                }
                else
                {

                }
            }

            return paymentStatusReport;
        }

    }
}
