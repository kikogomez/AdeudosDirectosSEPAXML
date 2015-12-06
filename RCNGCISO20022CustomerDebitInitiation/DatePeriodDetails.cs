namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class DatePeriodDetails
    {
        private System.DateTime frDtField;
        private System.DateTime toDtField;

        //Parameterless constructor for Serialization purpose
        private DatePeriodDetails() { }

        public DatePeriodDetails(System.DateTime fromDate, System.DateTime toDate)
        {
            this.frDtField = fromDate;
            this.toDtField = toDate;
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime FrDt
        {
            get
            {
                return this.frDtField;
            }
            set
            {
                this.frDtField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime ToDt
        {
            get
            {
                return this.toDtField;
            }
            set
            {
                this.toDtField = value;
            }
        }
    }
}
