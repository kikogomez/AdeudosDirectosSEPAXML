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
        [Option('i', "dbaseName", Required = true, HelpText = "Source Database full path.")]
        public string SourceDataBase { get; set; }

        [Option('o', "XMLFileName", Required = true, HelpText = "Output XML file name.")]
        public int OutputXMLFile { get; set; }

        [Option('v', null, HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            var usage = new StringBuilder();
            usage.AppendLine("Quickstart Application 1.0");
            usage.AppendLine("Read user manual for usage instructions...");
            return usage.ToString();
        }
    }
}
