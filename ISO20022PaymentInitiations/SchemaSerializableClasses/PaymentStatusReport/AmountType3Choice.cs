namespace ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public class AmountType3Choice
    {
        private object itemField;

        //Parameterless constructor for Serialization purpose
        public AmountType3Choice() { }

        public AmountType3Choice(EquivalentAmount2 equivalentAmount)
        {
            this.itemField = equivalentAmount;
        }

        public AmountType3Choice(ActiveOrHistoricCurrencyAndAmount instructedAmount)
        {
            this.itemField = instructedAmount;
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("EqvtAmt", typeof(EquivalentAmount2))]
        [System.Xml.Serialization.XmlElementAttribute("InstdAmt", typeof(ActiveOrHistoricCurrencyAndAmount))]
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
