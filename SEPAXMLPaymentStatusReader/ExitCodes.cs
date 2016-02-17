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

        /// Undefined error
        UndefinedError = -1,

        /// Invalid command line options
        InvalidArguments = -2,

        /// DataBase Not Found
        InvalidDataBasePath = -3,

        /// Source data file not found
        InvalidPaymentStatusFilePath = -4,

        /// DataBase connection error
        DataBaseConnectionError = -5,

        /// File Reading Error
        FileReadingError = -6,

        /// Not Valid XML File Error
        NotValidXMLFile = -7,

        /// File is Compilant to pain.002.001.03 Error
        NotCompilantToSchemaFile = -8,

        /// Data Reading error
        DataBaseWritingError = -9
    }
}
