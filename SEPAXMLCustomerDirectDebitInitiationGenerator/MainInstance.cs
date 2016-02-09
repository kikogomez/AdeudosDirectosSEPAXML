using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using CommandLine;
using System.IO;
using Billing;
using DirectDebitElements;
using ReferencesAndTools;

namespace SEPAXMLCustomerDirectDebitInitiationGenerator
{
    public class MainInstance
    {
        public MainInstance()
        {
        }

        public void Run(string[] args)
        {
            string parseErrorString;
            string sourceDatabaseFullPath;
            string xMLCDDFilename;
            bool verboseExecution;

            if (!ParseArguments(args, out parseErrorString, out sourceDatabaseFullPath, out xMLCDDFilename, out verboseExecution))
            {
                Console.WriteLine(parseErrorString);
                Environment.Exit((int)ExitCodes.InvalidArguments);
            }

            if (!File.Exists(sourceDatabaseFullPath))
            {
                Console.WriteLine("{0} not found!", sourceDatabaseFullPath);
                Environment.Exit((int)ExitCodes.InvalidDataBasePath);
            }

            string dataBaseConnectionString = CreateDatabaseConnectionString(sourceDatabaseFullPath);

            GenerateSEPAXMLCustomerDirectDebitInitiationFromDatabase(dataBaseConnectionString, xMLCDDFilename);          
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

            OleDbConnection conection = new OleDbConnection(oleDBConnectionString);

            RetrieveRemmitanceInformationFromDataBase(conection, out creditorNIF, out creditorName, out directDebitRemmitance);

            bool singleUnstructuredConcepts = true;
            GenerateXMLCustomerDirectDebitInitiationFileMessage(creditorNIF, creditorName, directDebitRemmitance, singleUnstructuredConcepts, outputFileName);
        }

        public void RetrieveRemmitanceInformationFromDataBase(
            OleDbConnection conection,
            out string creditorNIF,
            out string creditorName,
            out DirectDebitRemittance directDebitRemittance)
        {
            GetRemmmitanceBaseInformation(conection, out creditorNIF, out creditorName, out directDebitRemittance);

            string rCURTransactionsPaymentInstructionID = directDebitRemittance.MessageID + "-01";
            DirectDebitPaymentInstruction rCURDirectDebitPaymentInstruction = CreatePaymentInstructionWithRCURTransactions(conection, rCURTransactionsPaymentInstructionID, "CORE");
            directDebitRemittance.AddDirectDebitPaymentInstruction(rCURDirectDebitPaymentInstruction);
            string fRSTTransactionsPaymentInstructionID = directDebitRemittance.MessageID + "-02";
            DirectDebitPaymentInstruction fRSTDirectDebitPaymentInstruction = CreatePaymentInstructionWithFRSTTransactions(conection, fRSTTransactionsPaymentInstructionID, "CORE");
            directDebitRemittance.AddDirectDebitPaymentInstruction(fRSTDirectDebitPaymentInstruction);
        }

        public void GetRemmmitanceBaseInformation(OleDbConnection conection, out string creditorNIF, out string creditorName, out DirectDebitRemittance directDebitRemittance)
        {
            string messageID;
            DateTime generationDate;
            DateTime requestedCollectionDate;
            string creditorID;
            string creditorBussinesCode;
            string creditorAgentBIC;
            string creditorIBAN;
            string creditorAgentName;

            using (conection)
            {
                conection.Open();
                string query = "Select * From SEPAXMLDatosEnvio";
                OleDbCommand command = new OleDbCommand(query, conection);
                OleDbDataReader reader = command.ExecuteReader();

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
            }
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
            string transactionID;
            string mandateID;
            string oldMandateID;
            DateTime mandateCreationDate;
            string debtorFullName;
            string iBAN;
            string oldIBAN;
            //string debtorAgentBIC;
            double amount;
            string concept;
            bool fIRST;

            List<DirectDebitTransaction> directDebitTransactions = new List<DirectDebitTransaction>();
            using (connection)
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    transactionID = reader["TransactionID"] as string;
                    mandateID = reader["MandateID"] as string;
                    oldMandateID = reader["OldMandateID"] as string;
                    mandateCreationDate = reader["MandateCreationDate"] as DateTime? ?? new DateTime(2009, 10, 31);
                    debtorFullName = reader["DebtorFullName"] as string;
                    iBAN = reader["IBAN"] as string;
                    oldIBAN = reader["OldIBAN"] as string;
                    //debtorAgentBIC = (string)reader["DebtorAgentBIC"];
                    //amount = (decimal)reader["Amount"];
                    amount = reader["Amount"] as double? ?? default(double);
                    concept = reader["Concept"] as string;
                    fIRST = (bool)reader["FIRST"];

                    BankAccount oldAccount = null;
                    if (oldIBAN != null) oldAccount = new BankAccount(new InternationalAccountBankNumberIBAN(oldIBAN));
                    DirectDebitAmendmentInformation amendmentInformation = new DirectDebitAmendmentInformation(oldMandateID, oldAccount);
                    SimplifiedBill bill = new SimplifiedBill("tansactionID", concept, (decimal)amount, DateTime.MinValue, DateTime.MaxValue);
                    DirectDebitTransaction directDebitTransaction = new DirectDebitTransaction(
                        new List<SimplifiedBill>() { bill },
                        transactionID,
                        mandateID,
                        mandateCreationDate,
                        new BankAccount(new InternationalAccountBankNumberIBAN(iBAN)),
                        debtorFullName,
                        amendmentInformation,
                        fIRST);

                    directDebitTransactions.Add(directDebitTransaction);
                }
            }

            DirectDebitPaymentInstruction directDebitPaymentInstruction = new DirectDebitPaymentInstruction(
                paymentInstructionID,
                localInstrument,
                firstDebits,
                directDebitTransactions);

            return directDebitPaymentInstruction;
        }

        public void GenerateXMLCustomerDirectDebitInitiationFileMessage(
            string creditorNIF,
            string creditorName,
            DirectDebitRemittance directDebitRemmitance,
            bool singleUnstructuredConcepts,
            string outputFileName)
        {
            SEPAMessagesManager sEPAMessagesManager = new SEPAMessagesManager();
            sEPAMessagesManager.GenerateISO20022CustomerDirectDebitInitiationFileMessage(
                new Creditor(creditorNIF, creditorName),
                directDebitRemmitance.DirectDebitInitiationContract.CreditorAgent,
                directDebitRemmitance,
                singleUnstructuredConcepts,
                outputFileName);
        }
    }
}

