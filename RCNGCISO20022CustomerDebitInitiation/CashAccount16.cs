namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class CashAccount16
    {
        private AccountIdentification4Choice idField;
        private CashAccountType2 tpField;
        private string ccyField;
        private string nmField;

        //Parameterless constructor for Serialization purpose
        private CashAccount16() { }

        public CashAccount16(
            AccountIdentification4Choice identification,
            CashAccountType2 type,
            string currency,
            string name)
        {
            this.idField = identification;
            this.tpField = type;
            this.ccyField = currency;
            this.nmField = name;
        }

        /// <comentarios/>
        public AccountIdentification4Choice Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <comentarios/>
        public CashAccountType2 Tp
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

        /// <comentarios/>
        public string Nm
        {
            get
            {
                return this.nmField;
            }
            set
            {
                this.nmField = value;
            }
        }
    }

}
