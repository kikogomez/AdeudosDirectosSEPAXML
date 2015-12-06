namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class Authorisation1Choice
    {
        private object itemField;

        //Parameterless constructor for Serialization purpose
        private Authorisation1Choice() { }

        public Authorisation1Choice(Authorisation1Code authorisationCode)
        {
            this.itemField = authorisationCode;
        }

        public Authorisation1Choice(string propietaryFormatAuthorisation)
        {
            this.itemField = propietaryFormatAuthorisation;
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("Cd", typeof(Authorisation1Code))]
        [System.Xml.Serialization.XmlElementAttribute("Prtry", typeof(string))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

}
