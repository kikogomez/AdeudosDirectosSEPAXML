namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class TaxRecordDetails1
    {
        private TaxPeriod1 prdField;
        private ActiveOrHistoricCurrencyAndAmount amtField;

        //Parameterless constructor for Serialization purpose
        private TaxRecordDetails1() { }

        public TaxRecordDetails1(TaxPeriod1 period, ActiveOrHistoricCurrencyAndAmount amount)
        {
            this.prdField = period;
            this.amtField = amount;
        }

        /// <comentarios/>
        public TaxPeriod1 Prd
        {
            get
            {
                return this.prdField;
            }
            set
            {
                this.prdField = value;
            }
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
    }
}
