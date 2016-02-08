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

            DataSet sourceDataset = GetDataSetFromDB(sourceDatabaseFullPath);
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

        public DataSet GetDataSetFromDB (string sourceDatabaseFullPath)
        {
            var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;" + "data source=" + sourceDatabaseFullPath);
            var myDataTable = new DataTable();
            using (conection)
            {
                conection.Open();
                var query = "Select * From sepa";
                var command = new OleDbCommand(query, conection);
                var reader = command.ExecuteReader();
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

