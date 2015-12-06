namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class PostalAddress6
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

        //Parameterless constructor for Serialization purpose
        private PostalAddress6() { }

        public PostalAddress6(
            AddressType2Code addressType,
            bool addressTypeSpecified,
            string department,
            string subDepartment,
            string streetName,
            string buildingNumber,
            string postCode,
            string townName,
            string countrySubDivision,
            string country,
            string[] addressLine)
        {
            this.adrTpField = addressType;
            this.adrTpFieldSpecified = addressTypeSpecified;
            this.deptField = department;
            this.subDeptField = subDepartment;
            this.strtNmField = streetName;
            this.bldgNbField = buildingNumber;
            this.pstCdField = postCode;
            this.twnNmField = townName;
            this.ctrySubDvsnField = countrySubDivision;
            this.ctryField = country;
            this.adrLineField = (string[])addressLine;
        }

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
