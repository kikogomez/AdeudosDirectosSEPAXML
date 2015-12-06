namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class TaxAmount1
    {
        private decimal rateField;
        private bool rateFieldSpecified;
        private ActiveOrHistoricCurrencyAndAmount taxblBaseAmtField;
        private ActiveOrHistoricCurrencyAndAmount ttlAmtField;
        private TaxRecordDetails1[] dtlsField;

        //Parameterless constructor for Serialization purpose
        private TaxAmount1() { }

        public TaxAmount1(
            decimal rate,
            bool rateSpecified,
            ActiveOrHistoricCurrencyAndAmount taxableBase,
            ActiveOrHistoricCurrencyAndAmount totalAmount,
            TaxRecordDetails1[] details)
        {
            this.rateField = rate;
            this.rateFieldSpecified = rateSpecified;
            this.taxblBaseAmtField = taxableBase;
            this.ttlAmtField = totalAmount;
            this.dtlsField = (TaxRecordDetails1[])details.Clone();
        }

        /// <comentarios/>
        public decimal Rate
        {
            get
            {
                return this.rateField;
            }
            set
            {
                this.rateField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RateSpecified
        {
            get
            {
                return this.rateFieldSpecified;
            }
            set
            {
                this.rateFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount TaxblBaseAmt
        {
            get
            {
                return this.taxblBaseAmtField;
            }
            set
            {
                this.taxblBaseAmtField = value;
            }
        }

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount TtlAmt
        {
            get
            {
                return this.ttlAmtField;
            }
            set
            {
                this.ttlAmtField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("Dtls")]
        public TaxRecordDetails1[] Dtls
        {
            get
            {
                return this.dtlsField;
            }
            set
            {
                this.dtlsField = value;
            }
        }
    }
}
