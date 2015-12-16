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
    public partial class PostalAddress6
    {

        private AddressType2Code adrTpField;

        private bool adrTpFieldSpecified;

        private string deptField;

        private string subDeptField;

        private string strtNmField;

        private string bldgNbField;

        private string pstCdField;

        private string twnNmField;

        private string ctrySubDvsnField;

        private string ctryField;

        private string[] adrLineField;

        /// <comentarios/>
        public AddressType2Code AdrTp
        {
            get
            {
                return this.adrTpField;
            }
            set
            {
                this.adrTpField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AdrTpSpecified
        {
            get
            {
                return this.adrTpFieldSpecified;
            }
            set
            {
                this.adrTpFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public string Dept
        {
            get
            {
                return this.deptField;
            }
            set
            {
                this.deptField = value;
            }
        }

        /// <comentarios/>
        public string SubDept
        {
            get
            {
                return this.subDeptField;
            }
            set
            {
                this.subDeptField = value;
            }
        }

        /// <comentarios/>
        public string StrtNm
        {
            get
            {
                return this.strtNmField;
            }
            set
            {
                this.strtNmField = value;
            }
        }

        /// <comentarios/>
        public string BldgNb
        {
            get
            {
                return this.bldgNbField;
            }
            set
            {
                this.bldgNbField = value;
            }
        }

        /// <comentarios/>
        public string PstCd
        {
            get
            {
                return this.pstCdField;
            }
            set
            {
                this.pstCdField = value;
            }
        }

        /// <comentarios/>
        public string TwnNm
        {
            get
            {
                return this.twnNmField;
            }
            set
            {
                this.twnNmField = value;
            }
        }

        /// <comentarios/>
        public string CtrySubDvsn
        {
            get
            {
                return this.ctrySubDvsnField;
            }
            set
            {
                this.ctrySubDvsnField = value;
            }
        }

        /// <comentarios/>
        public string Ctry
        {
            get
            {
                return this.ctryField;
            }
            set
            {
                this.ctryField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("AdrLine")]
        public string[] AdrLine
        {
            get
            {
                return this.adrLineField;
            }
            set
            {
                this.adrLineField = value;
            }
        }
    }
}
