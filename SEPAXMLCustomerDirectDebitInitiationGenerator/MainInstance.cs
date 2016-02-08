using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using CommandLine;
using System.IO;

namespace SEPAXMLCustomerDirectDebitInitiationGenerator
{
    public class MainInstance
    {
        string dataBaseConnectionString;

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

            dataBaseConnectionString = CreateDatabaseConnectionString(sourceDatabaseFullPath);

            OleDbDataReader sourceData = GetReaderFormDatabase(sourceDatabaseFullPath);

            GenerateXML(sourceData, xMLCDDFilename);
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

        public OleDbDataReader GetReaderFormDatabase (string sourceDatabaseFullPath)
        {
            OleDbConnection conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;" + "data source=" + sourceDatabaseFullPath);

            conection.Open();
            var query = "Select * From SEPAXMLRecibosTemporalSoporte";
            var command = new OleDbCommand(query, conection);
            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                return reader;
            }
            else
            {
                return null;
            }
            conection.Close();

            //using (conection)
            //{
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
            //}
        }

        public void GenerateXML(OleDbDataReader reader, string outputFileName)
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine("{0}\t{1}", reader.GetInt32(0),
                        reader.GetString(1));
                }
            }
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
    }
}

