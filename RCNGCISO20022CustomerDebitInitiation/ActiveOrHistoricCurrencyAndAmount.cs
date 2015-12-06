namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class ActiveOrHistoricCurrencyAndAmount
    {
        private string ccyField;
        private decimal valueField;

        //Parameterless constructor for Serialization purposes
        private ActiveOrHistoricCurrencyAndAmount() { }

        public ActiveOrHistoricCurrencyAndAmount(string currencyCode, decimal amount)
        {
            this.ccyField = currencyCode;
            this.valueField = amount;
        }

        /// CurrencyCode
        /// [A-Z]{3,3} - EUR
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Ccy
        {
            get
            {
                return this.ccyField;
            }
            set
            {
                this.ccyField = value;
            }
        }

        /// Amount
        /// fractionDigits: 2
        /// minInclusive: 0
        /// totalDigits: 11
        /// decimalSeparator: '.'
        /// minAmount = 0.01
        /// maxAmount = "999999999.99"
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}
