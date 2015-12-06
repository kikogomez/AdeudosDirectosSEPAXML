namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class ReferredDocumentType2
    {
        private ReferredDocumentType1Choice cdOrPrtryField;
        private string issrField;

        //Parameterless constructor for Serialization purpose
        private ReferredDocumentType2() { }

        public ReferredDocumentType2(ReferredDocumentType1Choice codedOrPropietaryReferredDocumentTypeChoice, string issuer)
        {
            this.cdOrPrtryField = codedOrPropietaryReferredDocumentTypeChoice;
            this.issrField = issuer;
        }

        /// <comentarios/>
        public ReferredDocumentType1Choice CdOrPrtry
        {
            get
            {
                return this.cdOrPrtryField;
            }
            set
            {
                this.cdOrPrtryField = value;
            }
        }

        /// <comentarios/>
        public string Issr
        {
            get
            {
                return this.issrField;
            }
            set
            {
                this.issrField = value;
            }
        }
    }
}
