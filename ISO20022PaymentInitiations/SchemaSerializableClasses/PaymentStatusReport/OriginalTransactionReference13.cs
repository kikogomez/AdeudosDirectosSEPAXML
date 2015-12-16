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
    public partial class OriginalTransactionReference13
    {

        private ActiveOrHistoricCurrencyAndAmount intrBkSttlmAmtField;

        private AmountType3Choice amtField;

        private System.DateTime intrBkSttlmDtField;

        private bool intrBkSttlmDtFieldSpecified;

        private System.DateTime reqdColltnDtField;

        private bool reqdColltnDtFieldSpecified;

        private System.DateTime reqdExctnDtField;

        private bool reqdExctnDtFieldSpecified;

        private PartyIdentification32 cdtrSchmeIdField;

        private SettlementInformation13 sttlmInfField;

        private PaymentTypeInformation22 pmtTpInfField;

        private PaymentMethod4Code pmtMtdField;

        private bool pmtMtdFieldSpecified;

        private MandateRelatedInformation6 mndtRltdInfField;

        private RemittanceInformation5 rmtInfField;

        private PartyIdentification32 ultmtDbtrField;

        private PartyIdentification32 dbtrField;

        private CashAccount16 dbtrAcctField;

        private BranchAndFinancialInstitutionIdentification4 dbtrAgtField;

        private CashAccount16 dbtrAgtAcctField;

        private BranchAndFinancialInstitutionIdentification4 cdtrAgtField;

        private CashAccount16 cdtrAgtAcctField;

        private PartyIdentification32 cdtrField;

        private CashAccount16 cdtrAcctField;

        private PartyIdentification32 ultmtCdtrField;

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount IntrBkSttlmAmt
        {
            get
            {
                return this.intrBkSttlmAmtField;
            }
            set
            {
                this.intrBkSttlmAmtField = value;
            }
        }

        /// <comentarios/>
        public AmountType3Choice Amt
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
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime IntrBkSttlmDt
        {
            get
            {
                return this.intrBkSttlmDtField;
            }
            set
            {
                this.intrBkSttlmDtField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IntrBkSttlmDtSpecified
        {
            get
            {
                return this.intrBkSttlmDtFieldSpecified;
            }
            set
            {
                this.intrBkSttlmDtFieldSpecified = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime ReqdColltnDt
        {
            get
            {
                return this.reqdColltnDtField;
            }
            set
            {
                this.reqdColltnDtField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ReqdColltnDtSpecified
        {
            get
            {
                return this.reqdColltnDtFieldSpecified;
            }
            set
            {
                this.reqdColltnDtFieldSpecified = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime ReqdExctnDt
        {
            get
            {
                return this.reqdExctnDtField;
            }
            set
            {
                this.reqdExctnDtField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ReqdExctnDtSpecified
        {
            get
            {
                return this.reqdExctnDtFieldSpecified;
            }
            set
            {
                this.reqdExctnDtFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public PartyIdentification32 CdtrSchmeId
        {
            get
            {
                return this.cdtrSchmeIdField;
            }
            set
            {
                this.cdtrSchmeIdField = value;
            }
        }

        /// <comentarios/>
        public SettlementInformation13 SttlmInf
        {
            get
            {
                return this.sttlmInfField;
            }
            set
            {
                this.sttlmInfField = value;
            }
        }

        /// <comentarios/>
        public PaymentTypeInformation22 PmtTpInf
        {
            get
            {
                return this.pmtTpInfField;
            }
            set
            {
                this.pmtTpInfField = value;
            }
        }

        /// <comentarios/>
        public PaymentMethod4Code PmtMtd
        {
            get
            {
                return this.pmtMtdField;
            }
            set
            {
                this.pmtMtdField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PmtMtdSpecified
        {
            get
            {
                return this.pmtMtdFieldSpecified;
            }
            set
            {
                this.pmtMtdFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public MandateRelatedInformation6 MndtRltdInf
        {
            get
            {
                return this.mndtRltdInfField;
            }
            set
            {
                this.mndtRltdInfField = value;
            }
        }

        /// <comentarios/>
        public RemittanceInformation5 RmtInf
        {
            get
            {
                return this.rmtInfField;
            }
            set
            {
                this.rmtInfField = value;
            }
        }

        /// <comentarios/>
        public PartyIdentification32 UltmtDbtr
        {
            get
            {
                return this.ultmtDbtrField;
            }
            set
            {
                this.ultmtDbtrField = value;
            }
        }

        /// <comentarios/>
        public PartyIdentification32 Dbtr
        {
            get
            {
                return this.dbtrField;
            }
            set
            {
                this.dbtrField = value;
            }
        }

        /// <comentarios/>
        public CashAccount16 DbtrAcct
        {
            get
            {
                return this.dbtrAcctField;
            }
            set
            {
                this.dbtrAcctField = value;
            }
        }

        /// <comentarios/>
        public BranchAndFinancialInstitutionIdentification4 DbtrAgt
        {
            get
            {
                return this.dbtrAgtField;
            }
            set
            {
                this.dbtrAgtField = value;
            }
        }

        /// <comentarios/>
        public CashAccount16 DbtrAgtAcct
        {
            get
            {
                return this.dbtrAgtAcctField;
            }
            set
            {
                this.dbtrAgtAcctField = value;
            }
        }

        /// <comentarios/>
        public BranchAndFinancialInstitutionIdentification4 CdtrAgt
        {
            get
            {
                return this.cdtrAgtField;
            }
            set
            {
                this.cdtrAgtField = value;
            }
        }

        /// <comentarios/>
        public CashAccount16 CdtrAgtAcct
        {
            get
            {
                return this.cdtrAgtAcctField;
            }
            set
            {
                this.cdtrAgtAcctField = value;
            }
        }

        /// <comentarios/>
        public PartyIdentification32 Cdtr
        {
            get
            {
                return this.cdtrField;
            }
            set
            {
                this.cdtrField = value;
            }
        }

        /// <comentarios/>
        public CashAccount16 CdtrAcct
        {
            get
            {
                return this.cdtrAcctField;
            }
            set
            {
                this.cdtrAcctField = value;
            }
        }

        /// <comentarios/>
        public PartyIdentification32 UltmtCdtr
        {
            get
            {
                return this.ultmtCdtrField;
            }
            set
            {
                this.ultmtCdtrField = value;
            }
        }
    }
}
