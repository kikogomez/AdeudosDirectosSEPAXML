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
    public partial class OriginalGroupInformation20
    {

        private string orgnlMsgIdField;

        private string orgnlMsgNmIdField;

        private System.DateTime orgnlCreDtTmField;

        private bool orgnlCreDtTmFieldSpecified;

        private string orgnlNbOfTxsField;

        private decimal orgnlCtrlSumField;

        private bool orgnlCtrlSumFieldSpecified;

        private TransactionGroupStatus3Code grpStsField;

        private bool grpStsFieldSpecified;

        private StatusReasonInformation8[] stsRsnInfField;

        private NumberOfTransactionsPerStatus3[] nbOfTxsPerStsField;

        /// <comentarios/>
        public string OrgnlMsgId
        {
            get
            {
                return this.orgnlMsgIdField;
            }
            set
            {
                this.orgnlMsgIdField = value;
            }
        }

        /// <comentarios/>
        public string OrgnlMsgNmId
        {
            get
            {
                return this.orgnlMsgNmIdField;
            }
            set
            {
                this.orgnlMsgNmIdField = value;
            }
        }

        /// <comentarios/>
        public System.DateTime OrgnlCreDtTm
        {
            get
            {
                return this.orgnlCreDtTmField;
            }
            set
            {
                this.orgnlCreDtTmField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OrgnlCreDtTmSpecified
        {
            get
            {
                return this.orgnlCreDtTmFieldSpecified;
            }
            set
            {
                this.orgnlCreDtTmFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public string OrgnlNbOfTxs
        {
            get
            {
                return this.orgnlNbOfTxsField;
            }
            set
            {
                this.orgnlNbOfTxsField = value;
            }
        }

        /// <comentarios/>
        public decimal OrgnlCtrlSum
        {
            get
            {
                return this.orgnlCtrlSumField;
            }
            set
            {
                this.orgnlCtrlSumField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OrgnlCtrlSumSpecified
        {
            get
            {
                return this.orgnlCtrlSumFieldSpecified;
            }
            set
            {
                this.orgnlCtrlSumFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public TransactionGroupStatus3Code GrpSts
        {
            get
            {
                return this.grpStsField;
            }
            set
            {
                this.grpStsField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool GrpStsSpecified
        {
            get
            {
                return this.grpStsFieldSpecified;
            }
            set
            {
                this.grpStsFieldSpecified = value;
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
        [System.Xml.Serialization.XmlElementAttribute("NbOfTxsPerSts")]
        public NumberOfTransactionsPerStatus3[] NbOfTxsPerSts
        {
            get
            {
                return this.nbOfTxsPerStsField;
            }
            set
            {
                this.nbOfTxsPerStsField = value;
            }
        }
    }
}
