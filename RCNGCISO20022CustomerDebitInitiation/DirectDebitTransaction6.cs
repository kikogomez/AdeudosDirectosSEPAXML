namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class DirectDebitTransaction6
    {
        private MandateRelatedInformation6 mndtRltdInfField;
        private PartyIdentification32 cdtrSchmeIdField;
        private string preNtfctnIdField;
        private System.DateTime preNtfctnDtField;
        private bool preNtfctnDtFieldSpecified;

        //Parameterless constructor for Serialization purpose
        private DirectDebitTransaction6() { }

        public DirectDebitTransaction6(
            MandateRelatedInformation6 mandateRelatedInformation,
            PartyIdentification32 creditorSchemeIdentification,
            string preNotificationIdentification,
            System.DateTime preNotificationDate,
            bool preNotificationDateSpecified)
        {
            this.mndtRltdInfField = mandateRelatedInformation;
            this.cdtrSchmeIdField = creditorSchemeIdentification;
            this.preNtfctnIdField = preNotificationIdentification;
            this.preNtfctnDtField = preNotificationDate;
            this.preNtfctnDtFieldSpecified = preNotificationDateSpecified;
        }

        /// <comentarios/>
        public MandateRelatedInformation6 MndtRltdInf
        {
            get
            {
                return this.mndtRltdInfField;
            }
            set
            {
                this.mndtRltdInfField = value;
            }
        }

        /// <comentarios/>
        public PartyIdentification32 CdtrSchmeId
        {
            get
            {
                return this.cdtrSchmeIdField;
            }
            set
            {
                this.cdtrSchmeIdField = value;
            }
        }

        /// <comentarios/>
        public string PreNtfctnId
        {
            get
            {
                return this.preNtfctnIdField;
            }
            set
            {
                this.preNtfctnIdField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime PreNtfctnDt
        {
            get
            {
                return this.preNtfctnDtField;
            }
            set
            {
                this.preNtfctnDtField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PreNtfctnDtSpecified
        {
            get
            {
                return this.preNtfctnDtFieldSpecified;
            }
            set
            {
                this.preNtfctnDtFieldSpecified = value;
            }
        }
    }
}
