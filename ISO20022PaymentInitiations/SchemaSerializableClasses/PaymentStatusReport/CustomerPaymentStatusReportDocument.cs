using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport
{
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03", IsNullable = false)]
    public partial class CustomerPaymentStatusReportDocument
    {

        private CustomerPaymentStatusReportV03 cstmrPmtStsRptField;

        /// <comentarios/>
        public CustomerPaymentStatusReportV03 CstmrPmtStsRpt
        {
            get
            {
                return this.cstmrPmtStsRptField;
            }
            set
            {
                this.cstmrPmtStsRptField = value;
            }
        }
    }
}
