namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class NameAndAddress10
    {
        private string nmField;
        private PostalAddress6 adrField;

        //Parameterless constructor for Serialization purpose
        private NameAndAddress10() { }

        public NameAndAddress10(string name, PostalAddress6 postalAddress)
        {
            this.nmField = name;
            this.adrField = postalAddress;
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
        public PostalAddress6 Adr
        {
            get
            {
                return this.adrField;
            }
            set
            {
                this.adrField = value;
            }
        }
    }
}
