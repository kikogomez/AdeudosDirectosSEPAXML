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
    public partial class RemittanceInformation5
    {

        private string[] ustrdField;

        private StructuredRemittanceInformation7[] strdField;

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("Ustrd")]
        public string[] Ustrd
        {
            get
            {
                return this.ustrdField;
            }
            set
            {
                this.ustrdField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("Strd")]
        public StructuredRemittanceInformation7[] Strd
        {
            get
            {
                return this.strdField;
            }
            set
            {
                this.strdField = value;
            }
        }
    }
}
