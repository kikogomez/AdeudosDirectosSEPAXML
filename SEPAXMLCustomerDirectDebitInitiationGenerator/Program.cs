using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace SEPAXMLCustomerDirectDebitInitiationGenerator
{
    class Program
    {
        static void Main(string[] args)
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
