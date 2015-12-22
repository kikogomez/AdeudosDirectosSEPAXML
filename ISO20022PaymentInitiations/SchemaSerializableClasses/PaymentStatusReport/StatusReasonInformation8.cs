namespace ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public class StatusReasonInformation8
    {
        private PartyIdentification32 orgtrField;
        private StatusReason6Choice rsnField;
        private string[] addtlInfField;

        //Parameterless constructor for Serialization purposes
        public StatusReasonInformation8() { }

        public StatusReasonInformation8(
            PartyIdentification32 originator,
            StatusReason6Choice reason,
            string[] additionalInformation)
        {
            this.orgtrField = originator;
            this.rsnField = reason;
            this.addtlInfField = (string[])additionalInformation.Clone();
        }

        /// <comentarios/>
        public PartyIdentification32 Orgtr
        {
            get
            {
                return this.orgtrField;
            }
            set
            {
                this.orgtrField = value;
            }
        }

        /// <comentarios/>
        public StatusReason6Choice Rsn
        {
            get
            {
                return this.rsnField;
            }
            set
            {
                this.rsnField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("AddtlInf")]
        public string[] AddtlInf
        {
            get
            {
                return this.addtlInfField;
            }
            set
            {
                this.addtlInfField = value;
            }
        }
    }
}
