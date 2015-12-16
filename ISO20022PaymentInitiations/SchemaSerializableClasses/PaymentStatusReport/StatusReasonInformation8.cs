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
    public partial class StatusReasonInformation8
    {

        private PartyIdentification32 orgtrField;

        private StatusReason6Choice rsnField;

        private string[] addtlInfField;

        /// <comentarios/>
        public PartyIdentification32 Orgtr
        {
            get
            {
                return this.orgtrField;
            }
            set
            {
                this.orgtrField = value;
            }
        }

        /// <comentarios/>
        public StatusReason6Choice Rsn
        {
            get
            {
                return this.rsnField;
            }
            set
            {
                this.rsnField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("AddtlInf")]
        public string[] AddtlInf
        {
            get
            {
                return this.addtlInfField;
            }
            set
            {
                this.addtlInfField = value;
            }
        }
    }
}
