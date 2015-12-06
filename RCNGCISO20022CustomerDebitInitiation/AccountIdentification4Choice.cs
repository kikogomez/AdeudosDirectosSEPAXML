namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class AccountIdentification4Choice
    {
        private object itemField;

        //Parameterless constructor for Serialization purposes
        private AccountIdentification4Choice() { }

        public AccountIdentification4Choice(string iBAN)
        {
            this.itemField = iBAN;
        }

        public AccountIdentification4Choice(GenericAccountIdentification1 other)
        {
            this.itemField = other;
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("IBAN", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("Othr", typeof(GenericAccountIdentification1))]
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
