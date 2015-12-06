namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class CreditorReferenceType1Choice
    {
        private object itemField;

        //Parameterless constructor for Serialization purpose
        private CreditorReferenceType1Choice() { }

        public CreditorReferenceType1Choice(DocumentType3Code codedCreditorReferenceDocumentType)
        {
            this.itemField = codedCreditorReferenceDocumentType;
        }

        public CreditorReferenceType1Choice(string propietaryFormatCreditorReference)
        {
            this.itemField = propietaryFormatCreditorReference;
        }

        ///
        [System.Xml.Serialization.XmlElementAttribute("Cd", typeof(DocumentType3Code))]
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
