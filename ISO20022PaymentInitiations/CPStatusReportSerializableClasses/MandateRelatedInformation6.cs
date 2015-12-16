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
    public partial class MandateRelatedInformation6
    {

        private string mndtIdField;

        private System.DateTime dtOfSgntrField;

        private bool dtOfSgntrFieldSpecified;

        private bool amdmntIndField;

        private bool amdmntIndFieldSpecified;

        private AmendmentInformationDetails6 amdmntInfDtlsField;

        private string elctrncSgntrField;

        private System.DateTime frstColltnDtField;

        private bool frstColltnDtFieldSpecified;

        private System.DateTime fnlColltnDtField;

        private bool fnlColltnDtFieldSpecified;

        private Frequency1Code frqcyField;

        private bool frqcyFieldSpecified;

        /// <comentarios/>
        public string MndtId
        {
            get
            {
                return this.mndtIdField;
            }
            set
            {
                this.mndtIdField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime DtOfSgntr
        {
            get
            {
                return this.dtOfSgntrField;
            }
            set
            {
                this.dtOfSgntrField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DtOfSgntrSpecified
        {
            get
            {
                return this.dtOfSgntrFieldSpecified;
            }
            set
            {
                this.dtOfSgntrFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public bool AmdmntInd
        {
            get
            {
                return this.amdmntIndField;
            }
            set
            {
                this.amdmntIndField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AmdmntIndSpecified
        {
            get
            {
                return this.amdmntIndFieldSpecified;
            }
            set
            {
                this.amdmntIndFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public AmendmentInformationDetails6 AmdmntInfDtls
        {
            get
            {
                return this.amdmntInfDtlsField;
            }
            set
            {
                this.amdmntInfDtlsField = value;
            }
        }

        /// <comentarios/>
        public string ElctrncSgntr
        {
            get
            {
                return this.elctrncSgntrField;
            }
            set
            {
                this.elctrncSgntrField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime FrstColltnDt
        {
            get
            {
                return this.frstColltnDtField;
            }
            set
            {
                this.frstColltnDtField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FrstColltnDtSpecified
        {
            get
            {
                return this.frstColltnDtFieldSpecified;
            }
            set
            {
                this.frstColltnDtFieldSpecified = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime FnlColltnDt
        {
            get
            {
                return this.fnlColltnDtField;
            }
            set
            {
                this.fnlColltnDtField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FnlColltnDtSpecified
        {
            get
            {
                return this.fnlColltnDtFieldSpecified;
            }
            set
            {
                this.fnlColltnDtFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public Frequency1Code Frqcy
        {
            get
            {
                return this.frqcyField;
            }
            set
            {
                this.frqcyField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FrqcySpecified
        {
            get
            {
                return this.frqcyFieldSpecified;
            }
            set
            {
                this.frqcyFieldSpecified = value;
            }
        }
    }
}
