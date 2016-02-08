using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEPAXMLCustomerDirectDebitInitiationGenerator
{
    public enum ExitCodes
    {

        /// Normal Exit
        Success = 0,

        /// Invalid command line options
        InvalidArguments = -1,

        /// DataBaseNotFound
        InvalidDataBasePath = -2
    }
}
