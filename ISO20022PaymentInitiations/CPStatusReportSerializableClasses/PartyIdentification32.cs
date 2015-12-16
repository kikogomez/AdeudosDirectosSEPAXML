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
    public partial class PartyIdentification32
    {

        private string nmField;

        private PostalAddress6 pstlAdrField;

        private Party6Choice idField;

        private string ctryOfResField;

        private ContactDetails2 ctctDtlsField;

        /// <comentarios/>
        public string Nm
        {
            get
            {
                return this.nmField;
            }
            set
            {
                this.nmField = value;
            }
        }

        /// <comentarios/>
        public PostalAddress6 PstlAdr
        {
            get
            {
                return this.pstlAdrField;
            }
            set
            {
                this.pstlAdrField = value;
            }
        }

        /// <comentarios/>
        public Party6Choice Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <comentarios/>
        public string CtryOfRes
        {
            get
            {
                return this.ctryOfResField;
            }
            set
            {
                this.ctryOfResField = value;
            }
        }

        /// <comentarios/>
        public ContactDetails2 CtctDtls
        {
            get
            {
                return this.ctctDtlsField;
            }
            set
            {
                this.ctctDtlsField = value;
            }
        }
    }
}
