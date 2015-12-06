namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class CashAccountType2
    {
        private object itemField;

        //Parameterless constructor for Serialization purpose
        private CashAccountType2() { }

        public CashAccountType2(CashAccountType4Code codedCashAccountType)
        {
            this.itemField = codedCashAccountType;
        }

        public CashAccountType2(string propietaryCashAccountType)
        {
            this.itemField = propietaryCashAccountType;
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("Cd", typeof(CashAccountType4Code))]
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
