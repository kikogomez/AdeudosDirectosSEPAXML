namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class CreditorReferenceInformation2
    {
        private CreditorReferenceType2 tpField;
        private string refField;

        //Parameterless constructor for Serialization purpose
        private CreditorReferenceInformation2() { }

        public CreditorReferenceInformation2(CreditorReferenceType2 creditorReferenceType, string reference)
        {
            this.tpField = creditorReferenceType;
            this.refField = reference;
        }

        /// <comentarios/>
        public CreditorReferenceType2 Tp
        {
            get
            {
                return this.tpField;
            }
            set
            {
                this.tpField = value;
            }
        }

        /// <comentarios/>
        public string Ref
        {
            get
            {
                return this.refField;
            }
            set
            {
                this.refField = value;
            }
        }
    }
}
