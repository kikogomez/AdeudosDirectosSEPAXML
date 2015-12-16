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
    public partial class AmendmentInformationDetails6
    {

        private string orgnlMndtIdField;

        private PartyIdentification32 orgnlCdtrSchmeIdField;

        private BranchAndFinancialInstitutionIdentification4 orgnlCdtrAgtField;

        private CashAccount16 orgnlCdtrAgtAcctField;

        private PartyIdentification32 orgnlDbtrField;

        private CashAccount16 orgnlDbtrAcctField;

        private BranchAndFinancialInstitutionIdentification4 orgnlDbtrAgtField;

        private CashAccount16 orgnlDbtrAgtAcctField;

        private System.DateTime orgnlFnlColltnDtField;

        private bool orgnlFnlColltnDtFieldSpecified;

        private Frequency1Code orgnlFrqcyField;

        private bool orgnlFrqcyFieldSpecified;

        /// <comentarios/>
        public string OrgnlMndtId
        {
            get
            {
                return this.orgnlMndtIdField;
            }
            set
            {
                this.orgnlMndtIdField = value;
            }
        }

        /// <comentarios/>
        public PartyIdentification32 OrgnlCdtrSchmeId
        {
            get
            {
                return this.orgnlCdtrSchmeIdField;
            }
            set
            {
                this.orgnlCdtrSchmeIdField = value;
            }
        }

        /// <comentarios/>
        public BranchAndFinancialInstitutionIdentification4 OrgnlCdtrAgt
        {
            get
            {
                return this.orgnlCdtrAgtField;
            }
            set
            {
                this.orgnlCdtrAgtField = value;
            }
        }

        /// <comentarios/>
        public CashAccount16 OrgnlCdtrAgtAcct
        {
            get
            {
                return this.orgnlCdtrAgtAcctField;
            }
            set
            {
                this.orgnlCdtrAgtAcctField = value;
            }
        }

        /// <comentarios/>
        public PartyIdentification32 OrgnlDbtr
        {
            get
            {
                return this.orgnlDbtrField;
            }
            set
            {
                this.orgnlDbtrField = value;
            }
        }

        /// <comentarios/>
        public CashAccount16 OrgnlDbtrAcct
        {
            get
            {
                return this.orgnlDbtrAcctField;
            }
            set
            {
                this.orgnlDbtrAcctField = value;
            }
        }

        /// <comentarios/>
        public BranchAndFinancialInstitutionIdentification4 OrgnlDbtrAgt
        {
            get
            {
                return this.orgnlDbtrAgtField;
            }
            set
            {
                this.orgnlDbtrAgtField = value;
            }
        }

        /// <comentarios/>
        public CashAccount16 OrgnlDbtrAgtAcct
        {
            get
            {
                return this.orgnlDbtrAgtAcctField;
            }
            set
            {
                this.orgnlDbtrAgtAcctField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime OrgnlFnlColltnDt
        {
            get
            {
                return this.orgnlFnlColltnDtField;
            }
            set
            {
                this.orgnlFnlColltnDtField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OrgnlFnlColltnDtSpecified
        {
            get
            {
                return this.orgnlFnlColltnDtFieldSpecified;
            }
            set
            {
                this.orgnlFnlColltnDtFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public Frequency1Code OrgnlFrqcy
        {
            get
            {
                return this.orgnlFrqcyField;
            }
            set
            {
                this.orgnlFrqcyField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OrgnlFrqcySpecified
        {
            get
            {
                return this.orgnlFrqcyFieldSpecified;
            }
            set
            {
                this.orgnlFrqcyFieldSpecified = value;
            }
        }
    }
}
