using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEPAXMLPaymentStatusReportReader
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

        /// Not Valid XML File Error
        NotValidXMLFile = -6,

        /// File is Compilant to pain.002.001.03 Error
        NotCompilantToSchemaFile = -7,

        /// Data Reading error
        DataBaseWritingError = -8,
    }
}
