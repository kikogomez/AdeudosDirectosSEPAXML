namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class RemittanceLocation2
    {
        private string rmtIdField;
        private RemittanceLocationMethod2Code rmtLctnMtdField;
        private bool rmtLctnMtdFieldSpecified;
        private string rmtLctnElctrncAdrField;
        private NameAndAddress10 rmtLctnPstlAdrField;

        //Parameterless constructor for Serialization purpose
        private RemittanceLocation2() { }

        public RemittanceLocation2(
            string remittanceIdentification,
            RemittanceLocationMethod2Code remittanceLocationMethod,
            bool remittanceLocationMethodSpecified,
            string remittanceLocationElectronicAddress,
            NameAndAddress10 remittanceLocationPostalAddress)
        {
            this.rmtIdField = remittanceIdentification;
            this.rmtLctnMtdField = remittanceLocationMethod;
            this.rmtLctnMtdFieldSpecified = remittanceLocationMethodSpecified;
            this.rmtLctnElctrncAdrField = remittanceLocationElectronicAddress;
            this.rmtLctnPstlAdrField = remittanceLocationPostalAddress;
        }

        /// <comentarios/>
        public string RmtId
        {
            get
            {
                return this.rmtIdField;
            }
            set
            {
                this.rmtIdField = value;
            }
        }

        /// <comentarios/>
        public RemittanceLocationMethod2Code RmtLctnMtd
        {
            get
            {
                return this.rmtLctnMtdField;
            }
            set
            {
                this.rmtLctnMtdField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RmtLctnMtdSpecified
        {
            get
            {
                return this.rmtLctnMtdFieldSpecified;
            }
            set
            {
                this.rmtLctnMtdFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public string RmtLctnElctrncAdr
        {
            get
            {
                return this.rmtLctnElctrncAdrField;
            }
            set
            {
                this.rmtLctnElctrncAdrField = value;
            }
        }

        /// <comentarios/>
        public NameAndAddress10 RmtLctnPstlAdr
        {
            get
            {
                return this.rmtLctnPstlAdrField;
            }
            set
            {
                this.rmtLctnPstlAdrField = value;
            }
        }
    }
}
