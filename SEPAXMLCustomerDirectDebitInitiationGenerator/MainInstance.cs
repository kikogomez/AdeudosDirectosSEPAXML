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
        //string sourceDatabaseFullPath;
        //string xMLCDDFilename;
        //bool verboseExecution;

        public MainInstance()
        {
        }

        public void Run(string[] args)
        {
            string sourceDatabaseFullPath;
            string xMLCDDFilename;
            bool verboseExecution;

            bool parseResult = ParseArguments(args, out sourceDatabaseFullPath, out xMLCDDFilename, out verboseExecution);
        }

        public bool ParseArguments(
            string[] arguments,
            out string sourceDatabaseFullPath,
            out string xMLCDDFilename,
            out bool verboseExecution)
        {
            ArgumentOptions argumentOptions = new ArgumentOptions();
            Parser parser = new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.HelpWriter = new StringWriter();    //Redirect error output to an StringWriter, instead of console
            });

            if (parser.ParseArguments(arguments, argumentOptions))
            {
                sourceDatabaseFullPath = argumentOptions.SourceDataBase;
                xMLCDDFilename = argumentOptions.OutputXMLFile;
                verboseExecution = argumentOptions.Verbose;
                return true;
            }
            else
            {
                sourceDatabaseFullPath = null;
                xMLCDDFilename = null;
                verboseExecution = false;
                return false;
            }
        }

        private static void Run(ArgumentOptions argumentOptions)
        {
            var myDataTable = new DataTable();
            using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;" + "data source=C:\\menus\\newmenus\\menu.mdb;Password=****"))
            {
                conection.Open();
                var query = "Select siteid From n_user";
                var command = new OleDbCommand(query, conection);
                var reader = command.ExecuteReader();
            }

            using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;" + "data source=C:\\menus\\newmenus\\menu.mdb;Password=****"))
            {
                conection.Open();
                var query = "Select siteid From n_user";
                var adapter = new OleDbDataAdapter(query, conection);
                adapter.Fill(myDataTable);
                string text = myDataTable.Rows[0][0].ToString();
            }
        }
    }
}

