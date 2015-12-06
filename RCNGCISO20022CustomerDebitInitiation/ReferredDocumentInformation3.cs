namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class ReferredDocumentInformation3
    {
        private ReferredDocumentType2 tpField;
        private string nbField;
        private System.DateTime rltdDtField;
        private bool rltdDtFieldSpecified;

        //Parameterless constructor for Serialization purpose
        private ReferredDocumentInformation3() { }

        public ReferredDocumentInformation3(
            ReferredDocumentType2 type,
            string number,
            System.DateTime relatedDate,
            bool relatedDateSpecified
            )
        {
            this.tpField = type;
            this.nbField = number;
            this.rltdDtField = relatedDate;
            this.rltdDtFieldSpecified = relatedDateSpecified;
        }

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
