using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISO20022PaymentInitiations.CPStatusReportSerializableClasses
{
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public partial class BranchAndFinancialInstitutionIdentification4
    {

        private FinancialInstitutionIdentification7 finInstnIdField;

        private BranchData2 brnchIdField;

        /// <comentarios/>
        public FinancialInstitutionIdentification7 FinInstnId
        {
            get
            {
                return this.finInstnIdField;
            }
            set
            {
                this.finInstnIdField = value;
            }
        }

        /// <comentarios/>
        public BranchData2 BrnchId
        {
            get
            {
                return this.brnchIdField;
            }
            set
            {
                this.brnchIdField = value;
            }
        }
    }
}
