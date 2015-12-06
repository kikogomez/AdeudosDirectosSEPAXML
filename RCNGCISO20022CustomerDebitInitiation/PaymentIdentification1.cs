namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class PaymentIdentification1
    {
        private string instrIdField;
        private string endToEndIdField;

        //Parameterless constructor for Serialization purpose
        private PaymentIdentification1() { }

        public PaymentIdentification1(string instructionIdentification, string endToEndIdentification)
        {
            this.instrIdField = instructionIdentification;
            this.endToEndIdField = endToEndIdentification;
        }

        /// <comentarios/>
        public string InstrId
        {
            get
            {
                return this.instrIdField;
            }
            set
            {
                this.instrIdField = value;
            }
        }

        /// <comentarios/>
        public string EndToEndId
        {
            get
            {
                return this.endToEndIdField;
            }
            set
            {
                this.endToEndIdField = value;
            }
        }
    }
}
