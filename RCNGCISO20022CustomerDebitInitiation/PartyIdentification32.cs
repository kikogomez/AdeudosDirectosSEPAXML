namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class PartyIdentification32
    {
        private string nmField;
        private PostalAddress6 pstlAdrField;
        private Party6Choice idField;
        private string ctryOfResField;
        private ContactDetails2 ctctDtlsField;

        //Parameterless constructor for Serialization purpose
        private PartyIdentification32() { }

        public PartyIdentification32(
            string name,
            PostalAddress6 postalAddress,
            Party6Choice identification,
            string countryOfResidence,
            ContactDetails2 contactDetails)
        {
            this.nmField = name;
            this.pstlAdrField = postalAddress;
            this.idField = identification;
            this.ctryOfResField = countryOfResidence;
            this.ctctDtlsField = contactDetails;
        }

        /// <comentarios/>
        public string Nm
        {
            get
            {
                return this.nmField;
            }
            set
            {
                this.nmField = value;
            }
        }

        /// <comentarios/>
        public PostalAddress6 PstlAdr
        {
            get
            {
                return this.pstlAdrField;
            }
            set
            {
                this.pstlAdrField = value;
            }
        }

        /// <comentarios/>
        public Party6Choice Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <comentarios/>
        public string CtryOfRes
        {
            get
            {
                return this.ctryOfResField;
            }
            set
            {
                this.ctryOfResField = value;
            }
        }

        /// <comentarios/>
        public ContactDetails2 CtctDtls
        {
            get
            {
                return this.ctctDtlsField;
            }
            set
            {
                this.ctctDtlsField = value;
            }
        }
    }
}
