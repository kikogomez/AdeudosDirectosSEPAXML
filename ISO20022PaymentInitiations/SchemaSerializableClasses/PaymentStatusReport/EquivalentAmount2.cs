namespace ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public class EquivalentAmount2
    {
        private ActiveOrHistoricCurrencyAndAmount amtField;
        private string ccyOfTrfField;

        //Parameterless constructor for Serialization purpose
        public EquivalentAmount2() { }

        public EquivalentAmount2(ActiveOrHistoricCurrencyAndAmount amount, string currencyOfTransfer)
        {
            this.amtField = amount;
            this.ccyOfTrfField = currencyOfTransfer;
        }

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount Amt
        {
            get
            {
                return this.amtField;
            }
            set
            {
                this.amtField = value;
            }
        }

        /// <comentarios/>
        public string CcyOfTrf
        {
            get
            {
                return this.ccyOfTrfField;
            }
            set
            {
                this.ccyOfTrfField = value;
            }
        }
    }
}
