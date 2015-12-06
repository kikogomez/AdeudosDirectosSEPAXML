namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class OrganisationIdentification4
    {
        private string bICOrBEIField;
        private GenericOrganisationIdentification1[] othrField;

        //Parameterless constructor for Serialization purpose
        private OrganisationIdentification4() { }

        public OrganisationIdentification4(string bICOrBEI, GenericOrganisationIdentification1[] otherIdentification)
        {
            this.bICOrBEIField = bICOrBEI;
            this.othrField = (GenericOrganisationIdentification1[])otherIdentification;
        }

        /// <comentarios/>
        public string BICOrBEI
        {
            get
            {
                return this.bICOrBEIField;
            }
            set
            {
                this.bICOrBEIField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("Othr")]
        public GenericOrganisationIdentification1[] Othr
        {
            get
            {
                return this.othrField;
            }
            set
            {
                this.othrField = value;
            }
        }
    }
}
