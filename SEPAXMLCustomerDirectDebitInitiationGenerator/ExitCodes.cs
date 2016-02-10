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

        /// DataBase Not Found
        InvalidDataBasePath = -2,

        ///DataBase connection error
        DataBaseConnectionError = -3,

        ///Data Reading error
        DataBaseReadingError = -4
    }
}
