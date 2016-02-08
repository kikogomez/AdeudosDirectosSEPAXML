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
    class SEPAXMLCDDGenerator
    {
        static void Main(string[] args)
        {
            MainInstance mainInstance = new MainInstance();
            mainInstance.Run(args);
        }
    }
}
