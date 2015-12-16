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
    public partial class ReferredDocumentInformation3
    {

        private ReferredDocumentType2 tpField;

        private string nbField;

        private System.DateTime rltdDtField;

        private bool rltdDtFieldSpecified;

        /// <comentarios/>
        public ReferredDocumentType2 Tp
        {
            get
            {
                return this.tpField;
            }
            set
            {
                this.tpField = value;
            }
        }

        /// <comentarios/>
        public string Nb
        {
            get
            {
                return this.nbField;
            }
            set
            {
                this.nbField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime RltdDt
        {
            get
            {
                return this.rltdDtField;
            }
            set
            {
                this.rltdDtField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RltdDtSpecified
        {
            get
            {
                return this.rltdDtFieldSpecified;
            }
            set
            {
                this.rltdDtFieldSpecified = value;
            }
        }
    }
}
