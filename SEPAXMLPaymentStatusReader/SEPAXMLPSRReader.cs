using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEPAXMLPaymentStatusReportReader
{
    class SEPAXMLPSRReader
    {
        static void Main(string[] args)
        {
            MainInstance mainInstance = new MainInstance();
            mainInstance.Run(args);
        }
    }
}
