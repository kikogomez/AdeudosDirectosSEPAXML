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
    public partial class StructuredRemittanceInformation7
    {

        private ReferredDocumentInformation3[] rfrdDocInfField;

        private RemittanceAmount1 rfrdDocAmtField;

        private CreditorReferenceInformation2 cdtrRefInfField;

        private PartyIdentification32 invcrField;

        private PartyIdentification32 invceeField;

        private string[] addtlRmtInfField;

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("RfrdDocInf")]
        public ReferredDocumentInformation3[] RfrdDocInf
        {
            get
            {
                return this.rfrdDocInfField;
            }
            set
            {
                this.rfrdDocInfField = value;
            }
        }

        /// <comentarios/>
        public RemittanceAmount1 RfrdDocAmt
        {
            get
            {
                return this.rfrdDocAmtField;
            }
            set
            {
                this.rfrdDocAmtField = value;
            }
        }

        /// <comentarios/>
        public CreditorReferenceInformation2 CdtrRefInf
        {
            get
            {
                return this.cdtrRefInfField;
            }
            set
            {
                this.cdtrRefInfField = value;
            }
        }

        /// <comentarios/>
        public PartyIdentification32 Invcr
        {
            get
            {
                return this.invcrField;
            }
            set
            {
                this.invcrField = value;
            }
        }

        /// <comentarios/>
        public PartyIdentification32 Invcee
        {
            get
            {
                return this.invceeField;
            }
            set
            {
                this.invceeField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("AddtlRmtInf")]
        public string[] AddtlRmtInf
        {
            get
            {
                return this.addtlRmtInfField;
            }
            set
            {
                this.addtlRmtInfField = value;
            }
        }
    }
}
