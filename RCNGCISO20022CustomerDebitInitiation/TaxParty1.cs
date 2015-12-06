namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class TaxParty1
    {
        private string taxIdField;
        private string regnIdField;
        private string taxTpField;

        //Parameterless constructor for Serialization purpose
        private TaxParty1() { }

        public TaxParty1(
            string taxIdentification,
            string registrationIdentification,
            string taxType)
        {
            this.TaxId = taxIdentification;
            this.regnIdField = registrationIdentification;
            this.taxTpField = taxType;
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
    }
}
