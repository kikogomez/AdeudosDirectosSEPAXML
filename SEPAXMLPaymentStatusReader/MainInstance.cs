using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using System.IO;
using DirectDebitElements;
using System.Data.OleDb;
//using System.Data;
//using System.Data.SqlClient;

namespace SEPAXMLPaymentStatusReportReader
{
    public class MainInstance
    {
        bool verboseExecution;
        bool pauseBeforeExit;

        public MainInstance()
        { }

        public void Run(string[] args)
        {
            string usageHelpString;
            string errorsInPath;
            string sourcePaymentStatusReportPath;
            string dataBasePath;
            int exitCode;

            if (!ParseArguments(args, out usageHelpString, out sourcePaymentStatusReportPath, out dataBasePath, out verboseExecution, out pauseBeforeExit))
            {
                Console.WriteLine("Arguments error!");
                Console.WriteLine(usageHelpString);
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
                Console.WriteLine("Payment Status report file: {0}", sourcePaymentStatusReportPath);
                if (pauseBeforeExit)
                {
                    Console.WriteLine("Press any key to close program...");
                    Console.ReadKey();
                }
            }
            Environment.Exit((int)ExitCodes.Success);
        }

        public bool ParseArguments(
            string[] arguments,
            out string usageHelpString,
            out string sourcePaymentStatusReportPath,
            out string dataBasePath,
            out bool verboseExecution,
            out bool pauseBeforeExit)
        {
            bool argumentsParseOk;
            ArgumentOptions argumentOptions = new ArgumentOptions();
            Parser parser = new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.HelpWriter = new StringWriter();    //Redirect error output to an StringWriter, instead of console
            });

            argumentsParseOk = (parser.ParseArguments(arguments, argumentOptions) ? true : false);
            usageHelpString = parser.Settings.HelpWriter.ToString();
            sourcePaymentStatusReportPath = argumentOptions.PaymentStatusReportFilePath;
            dataBasePath = argumentOptions.DataBaseFullPath;
            verboseExecution = argumentOptions.Verbose;
            pauseBeforeExit = argumentOptions.Pause;
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

        public void ReadPaymentStatusReportIntoDataBase(string connectionString, string sourcePaymentStatusReportPath)
        {
            PaymentStatusReport paymentStatusReport = ReadPaymentStatusReportXMLFile(sourcePaymentStatusReportPath);

            ProcessPaymentStatusReportIntoDatabase(connectionString, paymentStatusReport);
        }

        public PaymentStatusReport ReadPaymentStatusReportXMLFile(string sourcePaymentStatusReportPath)
        {
            if (verboseExecution) Console.WriteLine("Reading file {0}", Path.GetFileName(sourcePaymentStatusReportPath));
            string xmlStringMessage = null;
            try
            {
                xmlStringMessage = ReadXMLSourceFileToString(sourcePaymentStatusReportPath);
            }
            catch (Exception fileReadException) when (fileReadException is IOException || fileReadException is UnauthorizedAccessException || fileReadException is System.Security.SecurityException)
            {
                ProcessFileReadToStringException(fileReadException);
            }

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

        public void ProcessPaymentStatusReportIntoDatabase(string connectionString, PaymentStatusReport paymentStatusReport)
        {
            if (verboseExecution) Console.WriteLine("Connecting to database...");

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                int paymentStatusReportInfoRegisters = 0;
                int rejectedTransactionsRegisters = 0;

                ConnectToDataBase(connection);
                if (verboseExecution) Console.WriteLine("Writing payment reject basic info to database...");
                paymentStatusReportInfoRegisters = InsertPaymentStatusRejectInfoIntoDataBase(connection, paymentStatusReport);
                if (verboseExecution) Console.WriteLine("Writing writing transaction rejects to database...");
                rejectedTransactionsRegisters = InsertTransactionRejectsIntoDatabase(connection, paymentStatusReport);
                if (paymentStatusReportInfoRegisters != 1 || rejectedTransactionsRegisters != paymentStatusReport.NumberOfTransactions)
                {
                    connection.Close();
                    Console.WriteLine("A problem occurred when trying to write into database");
                    Console.WriteLine("Some info couldn't be written");
                    Console.WriteLine("Press any key to close program...");
                    Console.ReadKey();
                    Environment.Exit((int)ExitCodes.DataBaseWritingError);
                }
                connection.Close();
            }
        }

        private void ConnectToDataBase(OleDbConnection connection)
        {
            try
            {
                connection.Open();
            }
            catch (Exception connectionException) when (connectionException is OleDbException || connectionException is InvalidOperationException)
            {
                ProcessConnectionException(connectionException);
            }
            catch (Exception unexpectedException)
            {
                Console.WriteLine("Uncontrolled exception while opening connection");
                Console.WriteLine(unexpectedException.Message);
                Console.WriteLine("Press any key to close program...");
                Console.ReadKey();
                Environment.Exit((int)ExitCodes.UndefinedError);
            }
        }

        public int InsertPaymentStatusRejectInfoIntoDataBase(OleDbConnection connection, PaymentStatusReport paymentStatusReport)
        {
            OleDbCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO SEPAXMLDatosDevolucion ([MessageId], [CreationDate], [NumberOfTransactions], [TotalAmount])" + " VALUES (@MessageID, @CreationDate, @NumberOfTransactions, @TotalAmount)";
            command.Parameters.Add("@MessageID", OleDbType.WChar, 35);
            command.Parameters.Add("@CreationDate", OleDbType.Date);
            command.Parameters.Add("@NumberOfTransactions", OleDbType.SmallInt);
            command.Parameters.Add("@TotalAmount", OleDbType.Double);
            //command.Parameters.Add("@TotalAmount", OleDbType.Decimal);
            //command.Parameters["@TotalAmount"].Precision = 2;
            command.Parameters["@MessageID"].Value = paymentStatusReport.MessageID;
            command.Parameters["@CreationDate"].Value = paymentStatusReport.MessageCreationDateTime;
            command.Parameters["@NumberOfTransactions"].Value = paymentStatusReport.NumberOfTransactions;
            command.Parameters["@TotalAmount"].Value = paymentStatusReport.ControlSum;
            int insertedRowsCount = 0;
            using (OleDbTransaction transaction = connection.BeginTransaction())
            {
                command.Transaction = transaction;
                try
                {
                    insertedRowsCount = command.ExecuteNonQuery();
                }
                catch (InvalidOperationException queryExecutionException)
                {
                    transaction.Rollback();
                    ProcessQueryExecutionExceptions(queryExecutionException);
                }
                if (insertedRowsCount == 1)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
            }
            command.Parameters.Clear();
            return insertedRowsCount;
        }

        public int InsertTransactionRejectsIntoDatabase(OleDbConnection connection, PaymentStatusReport paymentStatusReport)
        {
            OleDbCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO SEPAXMLRecibosTemporalDevolucion ([MandateId], [OrgnlEndtoEndID], [Reason], [CCC])" + " VALUES (@MandateID, @OrgnlEndToEndID, @Reason, @CCC)";
            command.Parameters.Add("@MandateID", OleDbType.VarChar, 50);
            command.Parameters.Add("@OrgnlEndToEndID", OleDbType.VarChar, 35);
            command.Parameters.Add("@Reason", OleDbType.VarChar, 50);
            command.Parameters.Add("@CCC", OleDbType.VarChar, 50);
            int insertedRowsCount = 0;
            using (OleDbTransaction transaction = connection.BeginTransaction())
            {
                command.Transaction = transaction;
                foreach (DirectDebitPaymentInstructionReject paymentInstructionReject in paymentStatusReport.DirectDebitPaymentInstructionRejects)
                {
                    foreach (DirectDebitTransactionReject directDebitTransacionReject in paymentInstructionReject.DirectDebitTransactionsRejects)
                    {
                        command.Parameters["@MandateID"].Value = directDebitTransacionReject.MandateID;
                        command.Parameters["@OrgnlEndToEndID"].Value = directDebitTransacionReject.OriginalEndtoEndTransactionIdentification;
                        command.Parameters["@Reason"].Value = directDebitTransacionReject.RejectReason;
                        command.Parameters["@CCC"].Value = directDebitTransacionReject.DebtorAccount.CCC.CCC;
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (InvalidOperationException queryExecutionException)
                        {
                            transaction.Rollback();
                            ProcessQueryExecutionExceptions(queryExecutionException);
                        }
                        
                        insertedRowsCount++;
                    }
                }
                if (insertedRowsCount == paymentStatusReport.NumberOfTransactions)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
                command.Parameters.Clear();
                return insertedRowsCount;
            }
        }

        private string ErrorsInPath(string fullPathToCheck)
        {
            string fileName = "";
            try
            {
                fileName = Path.GetFileName(fullPathToCheck);
            }
            catch (ArgumentException)
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
            string errorMessage = "Undefined Error";
            int exitCode = (int)ExitCodes.UndefinedError;
            switch (paymentStatusReadException.GetType().ToString())
            {
                case "System.Xml.XmlException":
                    errorMessage = "The source file is not a valid XML"
                        + Environment.NewLine + ((System.Xml.XmlException)paymentStatusReadException).Message;
                    exitCode = (int)ExitCodes.NotValidXMLFile;
                    break;
                case "System.Xml.Schema.XmlSchemaValidationException":
                    errorMessage = "The source file is not compilant to pain.002.001.03"
                        + Environment.NewLine + ((System.Xml.Schema.XmlSchemaValidationException)paymentStatusReadException).Message;
                    exitCode = (int)ExitCodes.NotCompilantToSchemaFile;
                    break;
            }
            Console.WriteLine(errorMessage);
            Console.WriteLine("Press any key to close program...");
            Console.ReadKey();
            Environment.Exit(exitCode);
        }

        private void ProcessConnectionException(Exception connectionException)
        {
            string errorMessage = "Undefined Error";
            int exitCode = (int)ExitCodes.UndefinedError;
            switch (connectionException.GetType().ToString())
            {
                case "System.Data.OleDb.OleDbException":
                    errorMessage = "Connection error!";
                    foreach (OleDbError error in ((OleDbException)connectionException).Errors)
                    {
                        errorMessage+= Environment.NewLine + error.Message;
                    }
                    Console.WriteLine("Press any key to close program...");
                    Console.ReadKey();
                    Environment.Exit((int)ExitCodes.DataBaseConnectionError);
                    break;
                case "System.InvalidOperationException":
                    Console.WriteLine("The database connection is already open. Trying to continue.");
                    Console.WriteLine(((InvalidOperationException)connectionException).Message);
                    break;
            }
            Console.WriteLine(errorMessage);
            Console.WriteLine("Press any key to close program...");
            Console.ReadKey();
            Environment.Exit(exitCode);
        }

        private void ProcessQueryExecutionExceptions(InvalidOperationException queryExecutionException)
        {
            Console.WriteLine("A problem occurred when trying to write into database");
            Console.WriteLine(queryExecutionException.Message);
            Console.WriteLine("Press any key to close program...");
            Console.ReadKey();
            Environment.Exit((int)ExitCodes.DataBaseWritingError);
        }
    }
}
