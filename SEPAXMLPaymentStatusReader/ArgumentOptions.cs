using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;
using System.Threading.Tasks;

namespace SEPAXMLPaymentStatusReportReader
{
   public class ArgumentOptions
    {
        [Option('i', "inputPaymentStatusReportFullPath", Required = true, HelpText = "Payment Status Report full path.")]
        public string PaymentStatusReportFilePath { get; set; }

        [Option('o', "outputDataBaseFullPath", Required = true, HelpText = "Output Data Base Full Path.")]
        public string DataBaseFullPath { get; set; }

        [Option('v', null, HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }

        [Option('p', null, HelpText = "Pauses before exit program")]
        public bool Pause { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            var usage = new StringBuilder();
            usage.AppendLine("SEPA XML Payment Status Report (pain.002.001.03) Reader");
            usage.AppendLine("Required arguments:");
            usage.AppendLine("   -i InputPaymentStatusReportFullPath  --->(Input XML File Path)");
            usage.AppendLine("   -o OutputDataBaseFullPath   --->(Batabase Path)");
            usage.AppendLine("Optional arguments:");
            usage.AppendLine("   -v   --->(Verbose execution)");
            usage.AppendLine("   -p   --->(Pause before exit)");
            return usage.ToString();
        }
    }
}
