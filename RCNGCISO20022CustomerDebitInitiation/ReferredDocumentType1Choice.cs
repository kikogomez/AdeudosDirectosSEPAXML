namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class ReferredDocumentType1Choice
    {
        private object itemField;

        //Parameterless constructor for Serialization purpose
        private ReferredDocumentType1Choice() { }

        public ReferredDocumentType1Choice(DocumentType5Code codedReferredDocumentType)
        {
            this.itemField = codedReferredDocumentType;
        }

        public ReferredDocumentType1Choice(string propietaryFormatReferredDocumentType)
        {
            this.itemField = propietaryFormatReferredDocumentType;
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("Cd", typeof(DocumentType5Code))]
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
