using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;
using System.Threading.Tasks;

namespace SEPAXMLCustomerDirectDebitInitiationGenerator
{
    class ArgumentOptions
    {
        [Option('i', "inputDataBaseFullPath", Required = true, HelpText = "Source Database full path.")]
        public string SourceDataBase { get; set; }

        [Option('o', "outputXMLFilename", Required = true, HelpText = "Output XML file name.")]
        public int OutputXMLFile { get; set; }

        [Option('v', null, HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            var usage = new StringBuilder();
            usage.AppendLine("SEPA XML Customer Direct Debit Initiation Message (pain.008.001.02) Generator");
            usage.AppendLine("-i SourceDatabaseFullPath");
            usage.AppendLine("-o OutputXMLFileName");
            return usage.ToString();
        }
    }
}
