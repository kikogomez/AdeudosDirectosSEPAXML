namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class CreditorReferenceType2
    {
        private CreditorReferenceType1Choice cdOrPrtryField;
        private string issrField;

        //Parameterless constructor for Serialization purpose
        private CreditorReferenceType2() { }

        public CreditorReferenceType2(CreditorReferenceType1Choice codedOrPropietaryCreditorReferenceTypeChoice, string issuer)
        {
            this.cdOrPrtryField = codedOrPropietaryCreditorReferenceTypeChoice;
            this.issrField = issuer;
        }

        /// <comentarios/>
        public CreditorReferenceType1Choice CdOrPrtry
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
