namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class TaxPeriod1
    {
        private System.DateTime yrField;
        private bool yrFieldSpecified;
        private TaxRecordPeriod1Code tpField;
        private bool tpFieldSpecified;
        private DatePeriodDetails frToDtField;

        //Parameterless constructor for Serialization purpose
        private TaxPeriod1() { }

        public TaxPeriod1(
            System.DateTime year,
            bool yearSpecified,
            TaxRecordPeriod1Code taxRecordPeriodType,
            bool taxRecordPeriodTypeSpecified,
            DatePeriodDetails fromToDate)
        {
            this.yrField = year;
            this.yrFieldSpecified = yearSpecified;
            this.tpField = taxRecordPeriodType;
            this.tpFieldSpecified = taxRecordPeriodTypeSpecified;
            this.frToDtField = fromToDate;
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Yr
        {
            get
            {
                return this.yrField;
            }
            set
            {
                this.yrField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool YrSpecified
        {
            get
            {
                return this.yrFieldSpecified;
            }
            set
            {
                this.yrFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public TaxRecordPeriod1Code Tp
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TpSpecified
        {
            get
            {
                return this.tpFieldSpecified;
            }
            set
            {
                this.tpFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public DatePeriodDetails FrToDt
        {
            get
            {
                return this.frToDtField;
            }
            set
            {
                this.frToDtField = value;
            }
        }
    }
}
