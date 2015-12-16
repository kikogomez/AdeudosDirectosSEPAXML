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
    public partial class ChargesInformation5
    {

        private ActiveOrHistoricCurrencyAndAmount amtField;

        private BranchAndFinancialInstitutionIdentification4 ptyField;

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount Amt
        {
            get
            {
                return this.amtField;
            }
            set
            {
                this.amtField = value;
            }
        }

        /// <comentarios/>
        public BranchAndFinancialInstitutionIdentification4 Pty
        {
            get
            {
                return this.ptyField;
            }
            set
            {
                this.ptyField = value;
            }
        }
    }
}
