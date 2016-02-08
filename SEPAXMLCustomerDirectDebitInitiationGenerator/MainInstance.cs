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
            string creditorName;
            DirectDebitRemittance directDebitRemmitance;

            OleDbConnection conection = new OleDbConnection(oleDBConnectionString);

            GetRemmmitanceBaseInformation(conection, out creditorName, out directDebitRemmitance);

            string rCURTransactionsPaymentInstructionID = directDebitRemmitance.MessageID + "-01";
            DirectDebitPaymentInstruction directDebitPaymentInstruction = CreatePaymentInformationWithRCURTransactions(conection, rCURTransactionsPaymentInstructionID);
        }

        public void GetRemmmitanceBaseInformation(OleDbConnection conection, out string creditorName, out DirectDebitRemittance directDebitRemittance)
        {
            string messageID;
            DateTime generationDate;
            DateTime requestedCollectionDate;
            string creditorID;
            string creditorBussinesCode;
            string creditorAgentBIC;
            string creditorIBAN;

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

                messageID = (string)reader["MessageID"];
                generationDate = (DateTime)reader["GenerationDate"];
                requestedCollectionDate = (DateTime)reader["RequestedCollectionDate"];
                creditorName = (string)reader["CreditorName"];
                creditorID = (string)reader["CreditorID"];
                creditorBussinesCode = (string)reader["CreditorBusinessCode"];
                creditorAgentBIC = (string)reader["CreditorAgent"];
                creditorIBAN = (string)reader["CreditorBankAccount"];
            }
            BankAccount creditorBankAccount = new BankAccount(new InternationalAccountBankNumberIBAN(creditorIBAN));
            CreditorAgent creditorAgent = new CreditorAgent(new BankCode(creditorBankAccount.BankAccountFieldCodes.BankCode, "CaixaBank", creditorAgentBIC));
            DirectDebitInitiationContract directDebitInitiationContract = new DirectDebitInitiationContract(
                new BankAccount(new InternationalAccountBankNumberIBAN(creditorIBAN)),
                creditorID.Substring(7, 9),
                creditorBussinesCode,
                creditorAgent);
            directDebitRemittance = new DirectDebitRemittance(messageID, generationDate, requestedCollectionDate, directDebitInitiationContract);
        }

        public DirectDebitPaymentInstruction CreatePaymentInformationWithRCURTransactions(OleDbConnection conection, string paymentInstructionID)
        {
            string transactionID;
            string mandateID;
            string oldMandateID;
            DateTime mandateCreationDate;
            string debtorFullName;
            string iBAN;
            string oldIBAN;
            //string debtorAgentBIC;
            decimal amount;
            string concept;
            bool fIRST;


            List<DirectDebitTransaction> directDebitTransactions = new List<DirectDebitTransaction>();
            using (conection)
            {
                conection.Open();
                string query = "SELECT * FROM SEPAXMLRecibosTemporalSoporte WHERE ([FIRST] = false)";
                OleDbCommand command = new OleDbCommand(query, conection);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    transactionID = (string)reader["TransactionID"];
                    mandateID = (string)reader["MandateID"];
                    oldMandateID = (string)reader["OldMandateID"];
                    mandateCreationDate = (DateTime)reader["MandateCreationDate"];
                    debtorFullName = (string)reader["DebtorFullName"];
                    iBAN = (string)reader["IBAN"];
                    oldIBAN = (string)reader["OldIBAN"];
                    //debtorAgentBIC = (string)reader["DebtorAgentBIC"];
                    amount = (decimal)reader["Amount"];
                    concept = (string)reader["Concept"];
                    fIRST = (bool)reader["FIRST"];

                    BankAccount oldAccount = null;
                    if (oldIBAN != null) oldAccount = new BankAccount(new InternationalAccountBankNumberIBAN(oldIBAN));
                    DirectDebitAmendmentInformation amendmentInformation = new DirectDebitAmendmentInformation(oldMandateID, oldAccount);
                    SimplifiedBill bill = new SimplifiedBill("tansactionID", concept, amount, DateTime.MinValue, DateTime.MaxValue);
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
                "CORE",
                false,
                directDebitTransactions);

            return directDebitPaymentInstruction;
        }

        //private static void Run(ArgumentOptions argumentOptions)
        //{
        //    var myDataTable = new DataTable();
        //    using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;" + "data source=C:\\menus\\newmenus\\menu.mdb;Password=****"))
        //    {
        //        conection.Open();
        //        var query = "Select siteid From n_user";
        //        var command = new OleDbCommand(query, conection);
        //        var reader = command.ExecuteReader();
        //    }

        //    using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;" + "data source=C:\\menus\\newmenus\\menu.mdb;Password=****"))
        //    {
        //        conection.Open();
        //        var query = "Select siteid From n_user";
        //        var adapter = new OleDbDataAdapter(query, conection);
        //        adapter.Fill(myDataTable);
        //        string text = myDataTable.Rows[0][0].ToString();
        //    }
        //}

        //public OleDbDataReader GetReaderFormDatabase (string sourceDatabaseFullPath)
        //{
        //    OleDbConnection conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;" + "data source=" + sourceDatabaseFullPath);

        //    conection.Open();
        //    var query = "Select * From SEPAXMLRecibosTemporalSoporte";
        //    var command = new OleDbCommand(query, conection);
        //    var reader = command.ExecuteReader();

        //    if (reader.HasRows)
        //    {
        //        return reader;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //    conection.Close();

        //    //using (conection)
        //    //{
        //    //    conection.Open();
        //    //    var query = "Select * From SEPAXMLRecibosTemporalSoporte";
        //    //    var command = new OleDbCommand(query, conection);
        //    //    var reader = command.ExecuteReader();

        //    //    if (reader.HasRows)
        //    //    {
        //    //        return reader;
        //    //    }
        //    //    else
        //    //    {
        //    //        return null;
        //    //    }
        //    //}
        //}
    }
}

