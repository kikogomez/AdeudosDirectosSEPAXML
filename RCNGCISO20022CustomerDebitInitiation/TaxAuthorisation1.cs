namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class TaxAuthorisation1
    {
        private string titlField;
        private string nmField;

        //Parameterless constructor for Serialization purpose
        private TaxAuthorisation1() { }

        public TaxAuthorisation1(string title, string name)
        {
            this.titlField = title;
            this.nmField = name;
        }

        /// <comentarios/>
        public string Titl
        {
            get
            {
                return this.titlField;
            }
            set
            {
                this.titlField = value;
            }
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
    }
}
