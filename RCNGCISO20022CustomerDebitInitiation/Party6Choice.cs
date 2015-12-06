namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class Party6Choice
    {
        private object itemField;

        //Parameterless constructor for Serialization purpose
        private Party6Choice() { }

        public Party6Choice(OrganisationIdentification4 organisationIdentification)
        {
            this.itemField = organisationIdentification;
        }

        public Party6Choice(PersonIdentification5 personIdentification)
        {
            this.itemField = personIdentification;
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("OrgId", typeof(OrganisationIdentification4))]
        [System.Xml.Serialization.XmlElementAttribute("PrvtId", typeof(PersonIdentification5))]
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
