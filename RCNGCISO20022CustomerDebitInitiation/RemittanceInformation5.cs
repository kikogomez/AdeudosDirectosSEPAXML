namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class RemittanceInformation5
    {
        private string[] ustrdField;
        private StructuredRemittanceInformation7[] strdField;

        //Parameterless constructor for Serialization purpose
        private RemittanceInformation5() { }

        public RemittanceInformation5(
            string[] unstructuredRemitanceInformation,
            StructuredRemittanceInformation7[] structuredRemitanceInformation)
        {
            this.ustrdField = (string[])unstructuredRemitanceInformation.Clone();
            this.strdField = (StructuredRemittanceInformation7[])structuredRemitanceInformation.Clone();
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("Ustrd")]
        public string[] Ustrd
        {
            get
            {
                return this.ustrdField;
            }
            set
            {
                this.ustrdField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("Strd")]
        public StructuredRemittanceInformation7[] Strd
        {
            get
            {
                return this.strdField;
            }
            set
            {
                this.strdField = value;
            }
        }
    }
}
