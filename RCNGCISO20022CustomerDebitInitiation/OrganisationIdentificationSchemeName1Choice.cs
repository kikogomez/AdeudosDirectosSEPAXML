namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class OrganisationIdentificationSchemeName1Choice
    {
        private string itemField;
        private ItemChoiceType itemElementNameField;

        //Parameterless constructor for Serialization purpose
        private OrganisationIdentificationSchemeName1Choice() { }

        public OrganisationIdentificationSchemeName1Choice(string identificationSchemeName, ItemChoiceType codedOrPropietary)
        {
            this.itemField = identificationSchemeName;
            this.itemElementNameField = codedOrPropietary;
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("Cd", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("Prtry", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item
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

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }
    }
}
