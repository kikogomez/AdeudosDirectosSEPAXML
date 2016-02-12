using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEPAXMLPaymentStatusReader
{
    public enum ExitCodes
    {

        /// Normal Exit
        Success = 0,

        /// Invalid command line options
        InvalidArguments = -1,

        /// DataBase Not Found
        InvalidDataBasePath = -2,

        /// Source data file not found
        InvalidPaymentStatusFilePath = -3,

        /// DataBase connection error
        DataBaseConnectionError = -4,

        /// File Reading Error
        FileReadingError = -5,

        /// Data Reading error
        DataBaseWritingError = -6,
    }
}
