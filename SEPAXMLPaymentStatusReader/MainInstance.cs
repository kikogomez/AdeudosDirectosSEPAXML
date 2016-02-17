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
                try
                {
                    connection.Open();
                }
                catch (OleDbException connectionException)
                {
                    foreach (OleDbError error in connectionException.Errors)
                    {
                        Console.WriteLine("Connection error!");
                        Console.WriteLine(error.Message);
                    }
                    Console.WriteLine("Press any key to close program...");
                    Console.ReadKey();
                    Environment.Exit((int)ExitCodes.DataBaseConnectionError);
                }
                catch (InvalidOperationException conectionException)
                {
                    Console.WriteLine("The database connection is already open. Trying to continue.");
                    Console.WriteLine(conectionException.Message);
                }

                InsertTransactionRejectsIntoDatabase(connection, paymentStatusReport);

                //OleDbCommand command = connection.CreateCommand();
                //command.CommandText = "INSERT INTO SEPAXMLRecibosTemporalDevolucion ([MandateId], [OrgnlEndtoEndID], [Reason], [CCC])" + " VALUES (@MandateID, @OrgnlEndToEndID, @Reason, @CCC)";
                //command.Parameters.Add("@MandateID", OleDbType.VarChar);
                //command.Parameters.Add("@OrgnlEndToEndID", OleDbType.VarChar);
                //command.Parameters.Add("@Reason", OleDbType.VarChar);
                //command.Parameters.Add("@CCC", OleDbType.VarChar);
                //foreach (DirectDebitPaymentInstructionReject paymentInstructionReject in paymentStatusReport.DirectDebitPaymentInstructionRejects)
                //{
                //    foreach (DirectDebitTransactionReject directDebitTransacionReject in paymentInstructionReject.DirectDebitTransactionsRejects)
                //    {
                //        command.Parameters["@MandateID"].Value = directDebitTransacionReject.MandateID;
                //        command.Parameters["@OrgnlEndToEndID"].Value = directDebitTransacionReject.OriginalEndtoEndTransactionIdentification;
                //        command.Parameters["@Reason"].Value = directDebitTransacionReject.RejectReason;
                //        command.Parameters["@CCC"].Value = directDebitTransacionReject.DebtorAccount.CCC.CCC;
                //        command.ExecuteNonQuery();
                //    }
                //}
                //command.Parameters.Clear();
            }


            //SOLUCION 1 CON OLEDB
            //using (OleDbConnection conn = new OleDbConnection(connString))
            //{
            //    conn.Open();
            //    OleDbCommand cmd = conn.CreateCommand();

            //    for (int i = 0; i < Customers.Count; i++)
            //    {
            //        cmd.Parameters.Add(new OleDbParameter("@var1", Customer[i].Name))
            //         cmd.Parameters.Add(new OleDbParameter("@var2", Customer[i].PhoneNum))
            //         cmd.Parameters.Add(new OleDbParameter("@var3", Customer[i].ID))
            //         cmd.Parameters.Add(new OleDbParameter("@var4", Customer[i].Name))
            //         cmd.Parameters.Add(new OleDbParameter("@var5", Customer[i].PhoneNum))

            //cmd.CommandText = "UPDATE Customers SET Name=@var1, Phone=@var2" +
            //                  "WHERE ID=@var3 AND (Name<>@var4 OR Phone<>@var5)";
            //        cmd.ExecuteNonQuery();
            //        cmd.Parameters.Clear();
            //    }
            //}

            //SOLUCION 2 CON OLEDB
            //OleDbConnection dbConnection = new OleDbConnection(CONNECTION_STRING);

            //string commandString =
            //"INSERT INTO MeetingEntries (Subject, Location, [Start Date], [End Date], [Enable Alarm], [Repeat Alarm], Reminder, [Repetition Type])" + " VALUES (@Subject, @Location, @StartDate, @EndDate, @EnableAlarm, @RepeatAlarm, @Reminder, @RepetitionType)";
            ////"INSERT INTO MEETINGENTRIES (SUBJECT, LOCATION, START DATE, END DATE, ENABLE ALARM, REPEAT ALARM, REMINDER, REPETITION TYPE)" + " VALUES (?, ?, ?, ?, ?, ?, ?, ?)";

            //OleDbCommand commandStatement = new OleDbCommand(commandString, dbConnection);

            //commandStatement.Parameters.Add("Subject", OleDbType.VarWChar, 30).Value = currentEntry.Subject;
            //commandStatement.Parameters.Add("Location", OleDbType.VarWChar, 50).Value = currentEntry.Location;
            //commandStatement.Parameters.Add("StartDate", OleDbType.Date, 40).Value = currentEntry.StartDateTime.Date;
            //commandStatement.Parameters.Add("EndDate", OleDbType.Date, 40).Value = currentEntry.EndDateTime.Date;
            //commandStatement.Parameters.Add("EnableAlarm", OleDbType.Boolean, 1).Value = currentEntry.IsAlarmEnabled;
            //commandStatement.Parameters.Add("RepeatAlarm", OleDbType.Boolean, 1).Value = currentEntry.IsAlarmRepeated;
            //commandStatement.Parameters.Add("Reminder", OleDbType.Integer, 2).Value = currentEntry.Reminder;
            //commandStatement.Parameters.Add("RepetitionType", OleDbType.VarWChar, 10).Value = currentEntry.Repetition.ToString();


            //dbConnection.Open();
            //commandStatement.CommandText = commandString;
            //commandStatement.ExecuteNonQuery();




            //SOLUCION CON sql
            //string insertStatement = "INSERT INTO dbo.REPORT_MARJORIE_ROLE(REPORT_ID, ROLE_ID, CREATED_BY, CREATED) " +
            //                    "VALUES(@ReportID, @RoleID, 'SYSTEM', CURRENT_TIMESTAMP)";

            //// set up connection and command objects in ADO.NET
            //SqlConnection connection = new SqlConnection(connectionString);
            //SqlCommand command = new SqlCommand(insertStatement, connection);
            //using (command)
            //{
            //    command.Parameters.Add("@ReportID", SqlDbType.Int);

            //    using (connection)
            //    {
            //        try
            //        {
            //            connection.Open();
            //        }
            //        catch (SqlException connectionException)
            //        {
            //            foreach (SqlError error in connectionException.Errors)
            //            {
            //                Console.WriteLine("Connection error!");
            //                Console.WriteLine(error.Message);
            //            }
            //            Console.WriteLine("Press any key to close program...");
            //            Console.ReadKey();
            //            Environment.Exit((int)ExitCodes.DataBaseConnectionError);
            //        }
            //        catch (InvalidOperationException conectionException)
            //        {
            //            Console.WriteLine("The database connection is already open. Trying to continue.");
            //            Console.WriteLine(conectionException.Message);
            //        }

            //        foreach (DirectDebitPaymentInstructionReject paymentInstructionReject in paymentStatusReport.DirectDebitPaymentInstructionRejects)
            //        {
            //            foreach (DirectDebitTransactionReject directDebitTransacionReject in paymentInstructionReject.DirectDebitTransactionsRejects)
            //            {
            //                command.Parameters["@RoleID"].Value = directDebitTransacionReject.MandateID;
            //            }
            //        }
            //        int rowsUpdated = command.ExecuteNonQuery();
            //        if (rowsUpdated != paymentStatusReport.NumberOfTransactions)
            //        {
            //            //error
            //            // - Si es rowsUpdated es -1 ha habido un error de escritura y se ha hecho un 'rollback'
            //            // - Si es otro valor... algo ha pasado, poruqe no se han insertado todas las transacciones esperadas
            //            //    * Puede ser que el 'PaymentReportStatus' no se ha creado bien y el numero de TransactionRejects es erroneo
            //        }
            //    }
            //}
        }

        public int InsertTransactionRejectsIntoDatabase(OleDbConnection connection, PaymentStatusReport paymentStatusReport)
        {
            OleDbCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO SEPAXMLRecibosTemporalDevolucion ([MandateId], [OrgnlEndtoEndID], [Reason], [CCC])" + " VALUES (@MandateID, @OrgnlEndToEndID, @Reason, @CCC)";
            command.Parameters.Add("@MandateID", OleDbType.VarChar);
            command.Parameters.Add("@OrgnlEndToEndID", OleDbType.VarChar);
            command.Parameters.Add("@Reason", OleDbType.VarChar);
            command.Parameters.Add("@CCC", OleDbType.VarChar);
            int insertedRowsCount = 0;
            foreach (DirectDebitPaymentInstructionReject paymentInstructionReject in paymentStatusReport.DirectDebitPaymentInstructionRejects)
            {
                foreach (DirectDebitTransactionReject directDebitTransacionReject in paymentInstructionReject.DirectDebitTransactionsRejects)
                {
                    command.Parameters["@MandateID"].Value = directDebitTransacionReject.MandateID;
                    command.Parameters["@OrgnlEndToEndID"].Value = directDebitTransacionReject.OriginalEndtoEndTransactionIdentification;
                    command.Parameters["@Reason"].Value = directDebitTransacionReject.RejectReason;
                    command.Parameters["@CCC"].Value = directDebitTransacionReject.DebtorAccount.CCC.CCC;
                    command.ExecuteNonQuery();
                    insertedRowsCount++;
                }
            }
            command.Parameters.Clear();
            return insertedRowsCount;
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
