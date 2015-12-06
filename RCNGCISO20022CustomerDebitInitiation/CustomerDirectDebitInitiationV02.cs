namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class CustomerDirectDebitInitiationV02
    {
        private GroupHeader39 grpHdrField;
        private PaymentInstructionInformation4[] pmtInfField;

        //Parameterless constructor for Serialization purpose
        private CustomerDirectDebitInitiationV02() { }

        public CustomerDirectDebitInitiationV02(GroupHeader39 groupHeader, PaymentInstructionInformation4[] paymentInstructionInformation)
        {
            this.grpHdrField = groupHeader;
            pmtInfField = (PaymentInstructionInformation4[])paymentInstructionInformation.Clone();
        }

        /// <comentarios/>
        public GroupHeader39 GrpHdr
        {
            get
            {
                return this.grpHdrField;
            }
            set
            {
                this.grpHdrField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("PmtInf")]
        public PaymentInstructionInformation4[] PmtInf
        {
            get
            {
                return this.pmtInfField;
            }
            set
            {
                this.pmtInfField = value;
            }
        }
    }
}
