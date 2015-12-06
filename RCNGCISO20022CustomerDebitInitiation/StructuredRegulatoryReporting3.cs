namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class StructuredRegulatoryReporting3
    {
        private string tpField;
        private System.DateTime dtField;
        private bool dtFieldSpecified;
        private string ctryField;
        private string cdField;
        private ActiveOrHistoricCurrencyAndAmount amtField;
        private string[] infField;

        //Parameterless constructor for Serialization purpose
        private StructuredRegulatoryReporting3() { }

        public StructuredRegulatoryReporting3(
            string type,
            System.DateTime date,
            bool dateSpecified,
            string country,
            string code,
            ActiveOrHistoricCurrencyAndAmount amount,
            string[] information)
        {
            this.tpField = type;
            this.dtField = date;
            this.dtFieldSpecified = dateSpecified;
            this.ctryField = country;
            this.cdField = code;
            this.amtField = amount;
            this.infField = (string[])information.Clone();
        }

        /// <comentarios/>
        public string Tp
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
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Dt
        {
            get
            {
                return this.dtField;
            }
            set
            {
                this.dtField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DtSpecified
        {
            get
            {
                return this.dtFieldSpecified;
            }
            set
            {
                this.dtFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public string Ctry
        {
            get
            {
                return this.ctryField;
            }
            set
            {
                this.ctryField = value;
            }
        }

        /// <comentarios/>
        public string Cd
        {
            get
            {
                return this.cdField;
            }
            set
            {
                this.cdField = value;
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

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("Inf")]
        public string[] Inf
        {
            get
            {
                return this.infField;
            }
            set
            {
                this.infField = value;
            }
        }
    }
}
