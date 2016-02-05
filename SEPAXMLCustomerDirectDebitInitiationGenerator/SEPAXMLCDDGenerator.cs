using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using CommandLine;

namespace SEPAXMLCustomerDirectDebitInitiationGenerator
{
    class SEPAXMLCDDGenerator
    {
        string sourceDatabaseFullPath;
        string xMLCDDFilename;

        static void Main(string[] args)
        {

            ArgumentOptions argumentOptions = new ArgumentOptions();
            CommandLine.Parser parser = new CommandLine.Parser(with => with.HelpWriter = Console.Error);

            if (parser.ParseArgumentsStrict(args, argumentOptions, () => Environment.Exit(-2)))
            {
                Run(argumentOptions);
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
