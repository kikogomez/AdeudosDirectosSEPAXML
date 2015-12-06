namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class TaxParty2
    {
        private string taxIdField;
        private string regnIdField;
        private string taxTpField;
        private TaxAuthorisation1 authstnField;

        //Parameterless constructor for Serialization purpose
        private TaxParty2() { }

        public TaxParty2(
            string taxIdentification,
            string registrationIdentification,
            string taxType,
            TaxAuthorisation1 authorisation)
        {
            this.taxIdField = taxIdentification;
            this.regnIdField = registrationIdentification;
            this.taxIdField = taxType;
            this.authstnField = authorisation;
        }

        /// <comentarios/>
        public string TaxId
        {
            get
            {
                return this.taxIdField;
            }
            set
            {
                this.taxIdField = value;
            }
        }

        /// <comentarios/>
        public string RegnId
        {
            get
            {
                return this.regnIdField;
            }
            set
            {
                this.regnIdField = value;
            }
        }

        /// <comentarios/>
        public string TaxTp
        {
            get
            {
                return this.taxTpField;
            }
            set
            {
                this.taxTpField = value;
            }
        }

        /// <comentarios/>
        public TaxAuthorisation1 Authstn
        {
            get
            {
                return this.authstnField;
            }
            set
            {
                this.authstnField = value;
            }
        }
    }
}
