using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectDebitElements
{
    public class DirectDebitPropietaryCodesGenerator
    {
        DirectDebitInitiationContract directDebitInitiationContract;

        public DirectDebitPropietaryCodesGenerator(DirectDebitInitiationContract directDebitInitiationContract)
        {
            this.directDebitInitiationContract = directDebitInitiationContract;
        }

        public DirectDebitInitiationContract DirectDebitInitiationContract
        {
            get
            {
                return directDebitInitiationContract;
            }
        }

        public string CalculateMyOldCSB19Code(int mandateInternalReferenceNumber)
        {
            return "0000" + directDebitInitiationContract.CreditorBussinessCode + mandateInternalReferenceNumber.ToString("00000");
        }
    }
}
