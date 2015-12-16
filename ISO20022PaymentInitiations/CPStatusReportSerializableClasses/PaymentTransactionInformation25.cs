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
    public partial class PaymentTransactionInformation25
    {

        private string stsIdField;

        private string orgnlInstrIdField;

        private string orgnlEndToEndIdField;

        private TransactionIndividualStatus3Code txStsField;

        private bool txStsFieldSpecified;

        private StatusReasonInformation8[] stsRsnInfField;

        private ChargesInformation5[] chrgsInfField;

        private System.DateTime accptncDtTmField;

        private bool accptncDtTmFieldSpecified;

        private string acctSvcrRefField;

        private string clrSysRefField;

        private OriginalTransactionReference13 orgnlTxRefField;

        /// <comentarios/>
        public string StsId
        {
            get
            {
                return this.stsIdField;
            }
            set
            {
                this.stsIdField = value;
            }
        }

        /// <comentarios/>
        public string OrgnlInstrId
        {
            get
            {
                return this.orgnlInstrIdField;
            }
            set
            {
                this.orgnlInstrIdField = value;
            }
        }

        /// <comentarios/>
        public string OrgnlEndToEndId
        {
            get
            {
                return this.orgnlEndToEndIdField;
            }
            set
            {
                this.orgnlEndToEndIdField = value;
            }
        }

        /// <comentarios/>
        public TransactionIndividualStatus3Code TxSts
        {
            get
            {
                return this.txStsField;
            }
            set
            {
                this.txStsField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TxStsSpecified
        {
            get
            {
                return this.txStsFieldSpecified;
            }
            set
            {
                this.txStsFieldSpecified = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("StsRsnInf")]
        public StatusReasonInformation8[] StsRsnInf
        {
            get
            {
                return this.stsRsnInfField;
            }
            set
            {
                this.stsRsnInfField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("ChrgsInf")]
        public ChargesInformation5[] ChrgsInf
        {
            get
            {
                return this.chrgsInfField;
            }
            set
            {
                this.chrgsInfField = value;
            }
        }

        /// <comentarios/>
        public System.DateTime AccptncDtTm
        {
            get
            {
                return this.accptncDtTmField;
            }
            set
            {
                this.accptncDtTmField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AccptncDtTmSpecified
        {
            get
            {
                return this.accptncDtTmFieldSpecified;
            }
            set
            {
                this.accptncDtTmFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public string AcctSvcrRef
        {
            get
            {
                return this.acctSvcrRefField;
            }
            set
            {
                this.acctSvcrRefField = value;
            }
        }

        /// <comentarios/>
        public string ClrSysRef
        {
            get
            {
                return this.clrSysRefField;
            }
            set
            {
                this.clrSysRefField = value;
            }
        }

        /// <comentarios/>
        public OriginalTransactionReference13 OrgnlTxRef
        {
            get
            {
                return this.orgnlTxRefField;
            }
            set
            {
                this.orgnlTxRefField = value;
            }
        }
    }
}
