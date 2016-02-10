using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using CommandLine;
using System.IO;
using Billing;
using DirectDebitElements;
using ReferencesAndTools;

namespace SEPAXMLCustomerDirectDebitInitiationGenerator
{
    public class MainInstance
    {
        bool verboseExecution;

        public MainInstance()
        {
        }

        public void Run(string[] args)
        {
            string parseErrorString;
            string sourceDatabaseFullPath;
            string xMLCDDFilename;

            if (!ParseArguments(args, out parseErrorString, out sourceDatabaseFullPath, out xMLCDDFilename, out verboseExecution))
            {
                Console.WriteLine(parseErrorString);
                Environment.Exit((int)ExitCodes.InvalidArguments);
            }

            if (verboseExecution) Console.WriteLine("Locating source database...");
            if (!File.Exists(sourceDatabaseFullPath))
            {
                Console.WriteLine("{0} not found!", sourceDatabaseFullPath);
                Environment.Exit((int)ExitCodes.InvalidDataBasePath);
            }

            string dataBaseConnectionString = CreateDatabaseConnectionString(sourceDatabaseFullPath);

            GenerateSEPAXMLCustomerDirectDebitInitiationFromDatabase(dataBaseConnectionString, xMLCDDFilename);

            if (verboseExecution)
            {
                Console.WriteLine("Completed!");
                Console.WriteLine("Generated File: XMLOutputFiles\\{0}", xMLCDDFilename);
                Console.WriteLine("Press any key to close program...");
                Console.ReadKey();
            }
            Environment.Exit((int)ExitCodes.Success);
        }

        public bool ParseArguments(
            string[] arguments,
            out string parseErrorString,
            out string sourceDatabaseFullPath,
            out string xMLCDDFilename,
            out bool verboseExecution)
        {
            bool argumentsParseOk;
            ArgumentOptions argumentOptions = new ArgumentOptions();
            Parser parser = new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.HelpWriter = new StringWriter();    //Redirect error output to an StringWriter, instead of console
            });

            argumentsParseOk = (parser.ParseArguments(arguments, argumentOptions)? true : false);
            parseErrorString = parser.Settings.HelpWriter.ToString();
            sourceDatabaseFullPath = argumentOptions.SourceDataBase;
            xMLCDDFilename = argumentOptions.OutputXMLFile;
            verboseExecution = argumentOptions.Verbose;
            return argumentsParseOk;
        }

        public string CreateDatabaseConnectionString(string pathToDataBase)
        {
            return "Provider=Microsoft.JET.OLEDB.4.0;" + "data source=" + pathToDataBase;
        }

        public void GenerateSEPAXMLCustomerDirectDebitInitiationFromDatabase(string oleDBConnectionString, string outputFileName)
        {
            string creditorNIF;
            string creditorName;
            DirectDebitRemittance directDebitRemmitance;

            OleDbConnection connection = new OleDbConnection(oleDBConnectionString);

            RetrieveRemmitanceInformationFromDataBase(connection, out creditorNIF, out creditorName, out directDebitRemmitance);

            bool singleUnstructuredConcepts = true;
            GenerateXMLCustomerDirectDebitInitiationFileMessage(creditorNIF, creditorName, directDebitRemmitance, singleUnstructuredConcepts, outputFileName);
        }

        public void RetrieveRemmitanceInformationFromDataBase(
            OleDbConnection connection,
            out string creditorNIF,
            out string creditorName,
            out DirectDebitRemittance directDebitRemittance)
        {
            if (verboseExecution) Console.WriteLine("Connecting to database...");
            using (connection)
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
                    Environment.Exit((int)ExitCodes.DataBaseConnectionError);
                }
                catch (InvalidOperationException conectionException)
                {
                    Console.WriteLine("The database connection is already open. Trying to continue.");
                    Console.WriteLine(conectionException.Message);
                }
                //connection.Open(); 
                if (verboseExecution) Console.WriteLine("Reading remittance base information...");
                GetRemmmitanceBaseInformation(connection, out creditorNIF, out creditorName, out directDebitRemittance);

                if (verboseExecution) Console.WriteLine("Reading recurrent direct debits...");
                string rCURTransactionsPaymentInstructionID = directDebitRemittance.MessageID + "-RC";
                DirectDebitPaymentInstruction rCURDirectDebitPaymentInstruction = CreatePaymentInstructionWithRCURTransactions(connection, rCURTransactionsPaymentInstructionID, "CORE");
                directDebitRemittance.AddDirectDebitPaymentInstruction(rCURDirectDebitPaymentInstruction);
                if (verboseExecution) Console.WriteLine("Reading first time direct debits...");
                string fRSTTransactionsPaymentInstructionID = directDebitRemittance.MessageID + "-FR";
                DirectDebitPaymentInstruction fRSTDirectDebitPaymentInstruction = CreatePaymentInstructionWithFRSTTransactions(connection, fRSTTransactionsPaymentInstructionID, "CORE");
                directDebitRemittance.AddDirectDebitPaymentInstruction(fRSTDirectDebitPaymentInstruction);
            }
        }

        public void GetRemmmitanceBaseInformation(OleDbConnection connection, out string creditorNIF, out string creditorName, out DirectDebitRemittance directDebitRemittance)
        {
            string messageID;
            DateTime generationDate;
            DateTime requestedCollectionDate;
            string creditorID;
            string creditorBussinesCode;
            string creditorAgentBIC;
            string creditorIBAN;
            string creditorAgentName;

            //connection.Open();
            string query = "Select * From SEPAXMLDatosEnvio";
            OleDbCommand command = new OleDbCommand(query, connection);
            OleDbDataReader reader = null;
            try
            {
                reader = command.ExecuteReader();
            }
            catch (System.Exception exception ) 
            when(exception is SqlException || exception is InvalidOperationException || exception is IOException || exception is ObjectDisposedException)
            {
                string errorMessage = ComposeErrorMessageForException(exception);
                Console.WriteLine("Error while reading remmitance base information.");
                Console.WriteLine(errorMessage);
                Environment.Exit((int)ExitCodes.DataBaseReadingError);
            }

            reader.Read();

            //messageID = reader.GetString(0);
            //generationDate = reader.GetDateTime(1);
            //requestedCollectionDate = reader.GetDateTime(2);
            //creditorName = reader.GetString(3);
            //creditorID = reader.GetString(4);
            //creditorBussinesCode = reader.GetString(5);
            //creditorAgent = reader.GetString(6);
            //creditorBankAccount = reader.GetString(7);

            messageID = reader["MessageID"] as string;
            generationDate = (DateTime)reader["GenerationDate"];
            requestedCollectionDate = (DateTime)reader["RequestedCollectionDate"];
            creditorName = reader["CreditorName"] as string;
            creditorID = reader["CreditorID"] as string;
            creditorBussinesCode = reader["CreditorBusinessCode"] as string;
            creditorAgentBIC = reader["CreditorAgent"] as string;
            creditorIBAN = reader["CreditorBankAccount"] as string;
            creditorAgentName = reader["CreditorAgentName"] as string;
            creditorNIF = creditorID.Substring(7, 9);
            BankAccount creditorBankAccount = new BankAccount(new InternationalAccountBankNumberIBAN(creditorIBAN));
            CreditorAgent creditorAgent = new CreditorAgent(new BankCode(creditorBankAccount.BankAccountFieldCodes.BankCode, creditorAgentName, creditorAgentBIC));
            DirectDebitInitiationContract directDebitInitiationContract = new DirectDebitInitiationContract(
                new BankAccount(new InternationalAccountBankNumberIBAN(creditorIBAN)),
                creditorNIF,
                creditorBussinesCode,
                creditorAgent);
            directDebitRemittance = new DirectDebitRemittance(messageID, generationDate, requestedCollectionDate, directDebitInitiationContract);
        }

        public DirectDebitPaymentInstruction CreatePaymentInstructionWithRCURTransactions(OleDbConnection connection, string paymentInstructionID, string localInstrument)
        {
            string query = "Select * From SEPAXMLRecibosTemporalSoporte Where ([FIRST] = false)";
            DirectDebitPaymentInstruction DirectDebitPaymentInstruction = CreatePaymentInstruction(
                connection,
                query,
                paymentInstructionID,
                localInstrument,
                false);
            return DirectDebitPaymentInstruction;
        }

        public DirectDebitPaymentInstruction CreatePaymentInstructionWithFRSTTransactions(OleDbConnection connection, string paymentInstructionID, string localInstrument)
        {
            string query = "Select * From SEPAXMLRecibosTemporalSoporte Where ([FIRST] = true)";
            DirectDebitPaymentInstruction DirectDebitPaymentInstruction = CreatePaymentInstruction(
                connection,
                query,
                paymentInstructionID,
                localInstrument,
                true);
            return DirectDebitPaymentInstruction;
        }

        private DirectDebitPaymentInstruction CreatePaymentInstruction(
            OleDbConnection connection,
            string query,
            string paymentInstructionID,
            string localInstrument,
            bool firstDebits)
        {
            List<DirectDebitTransaction> directDebitTransactions = new List<DirectDebitTransaction>();
            //connection.Open();
            OleDbCommand command = new OleDbCommand(query, connection);
            //OleDbDataReader reader = command.ExecuteReader();
            OleDbDataReader reader = null;
            try
            {
                reader = command.ExecuteReader();
            }
            catch (System.Exception exception)
            when (exception is SqlException || exception is InvalidOperationException || exception is IOException || exception is ObjectDisposedException)
            {
                string errorMessage = ComposeErrorMessageForException(exception);
                Console.WriteLine("Error while reading direct debits data.");
                Console.WriteLine(errorMessage);
                Environment.Exit((int)ExitCodes.DataBaseReadingError);
            }
            while (reader.Read())
            {
                DirectDebitTransaction directDebitTransaction = ReadRecordIntoDirectDebitTransaction((IDataRecord)reader);
                directDebitTransactions.Add(directDebitTransaction);
            }

            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                paymentInstructionID,
                localInstrument,
                firstDebits,
                directDebitTransactions);

            return directDebitPaymentInstruction;
        }

        private DirectDebitTransaction ReadRecordIntoDirectDebitTransaction(IDataRecord record)
        {
            string transactionID = record["TransactionID"] as string;
            string mandateID = record["MandateID"] as string;
            string oldMandateID = record["OldMandateID"] as string;
            DateTime mandateCreationDate = record["MandateCreationDate"] as DateTime? ?? new DateTime(2009, 10, 31);
            string debtorFullName = record["DebtorFullName"] as string;
            string iBAN = record["IBAN"] as string;
            string oldIBAN = record["OldIBAN"] as string;
            string debtorAgentBIC = (string)record["DebtorAgentBIC"];
            double amount = record["Amount"] as double? ?? default(double);
            string concept = record["Concept"] as string;
            bool fIRST = (bool)record["FIRST"];

            BankAccount oldAccount = null;
            if (oldIBAN != null) oldAccount = new BankAccount(new InternationalAccountBankNumberIBAN(oldIBAN));
            DirectDebitAmendmentInformation amendmentInformation = new DirectDebitAmendmentInformation(oldMandateID, oldAccount);
            SimplifiedBill bill = new SimplifiedBill(transactionID, concept, (decimal)amount, DateTime.MinValue, DateTime.MaxValue);
            DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                new List<SimplifiedBill>() { bill },
                transactionID,
                mandateID,
                mandateCreationDate,
                new BankAccount(new InternationalAccountBankNumberIBAN(iBAN)),
                debtorAgentBIC,
                debtorFullName,
                amendmentInformation,
                fIRST);
            return directDebitTransaction;
        }

        private void GenerateXMLCustomerDirectDebitInitiationFileMessage(
            string creditorNIF,
            string creditorName,
            DirectDebitRemittance directDebitRemmitance,
            bool singleUnstructuredConcepts,
            string outputFileName)
        {
            if (verboseExecution) Console.WriteLine("Generating XML File...");
            string relativeOutputPath = @"XMLOutputFiles\" + outputFileName;
            File.Delete(relativeOutputPath);
            SEPAMessagesManager sEPAMessagesManager = new SEPAMessagesManager();
            sEPAMessagesManager.GenerateISO20022CustomerDirectDebitInitiationFileMessage(
                new Creditor(creditorNIF, creditorName),
                directDebitRemmitance.DirectDebitInitiationContract.CreditorAgent,
                directDebitRemmitance,
                singleUnstructuredConcepts,
                relativeOutputPath);
            if(!File.Exists(relativeOutputPath))
            {
                Console.WriteLine("Error while creating XML File");
                Environment.Exit((int)ExitCodes.FileCreatingError);
            }
        }

        private string ComposeErrorMessageForException(Exception exception)
        {
            string errorMessage = "";
            if (exception is SqlException)
            {
                errorMessage = "Locked row or Timeout." + Environment.NewLine;
                errorMessage += ((SqlException)exception).Message + Environment.NewLine;
                foreach (SqlError error in ((SqlException)exception).Errors)
                {
                    errorMessage += error.Message + Environment.NewLine;
                }
            }
            if (exception is InvalidOperationException)
            {
                errorMessage = "Connection is closed or was closed while reading data." + Environment.NewLine;
                errorMessage += ((InvalidOperationException)exception).Message + Environment.NewLine;
            }
            if (exception is IOException)
            {
                errorMessage = "Error while reading data." + Environment.NewLine;
                errorMessage += ((IOException)exception).Message + Environment.NewLine;
            }
            if (exception is ObjectDisposedException)
            {
                errorMessage = "Error while reading data." + Environment.NewLine + " DataReader was closed while reading." + Environment.NewLine;
                errorMessage += ((ObjectDisposedException)exception).Message + Environment.NewLine;
            }
            return errorMessage;
        }
    }
}

