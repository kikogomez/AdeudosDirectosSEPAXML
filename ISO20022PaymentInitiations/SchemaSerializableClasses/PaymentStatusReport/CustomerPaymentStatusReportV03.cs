namespace ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public class CustomerPaymentStatusReportV03
    {
        private GroupHeader36 grpHdrField;
        private OriginalGroupInformation20 orgnlGrpInfAndStsField;
        private OriginalPaymentInformation1[] orgnlPmtInfAndStsField;

        //Parameterless constructor for Serialization purpose
        public CustomerPaymentStatusReportV03() { }

        public CustomerPaymentStatusReportV03(
            GroupHeader36 groupHeader,
            OriginalGroupInformation20 originalGroupInformation,
            OriginalPaymentInformation1[] originalPaymentInformation)
        {
            this.grpHdrField = groupHeader;
            this.orgnlGrpInfAndStsField = originalGroupInformation;
            orgnlPmtInfAndStsField = (OriginalPaymentInformation1[]) originalPaymentInformation.Clone();
        }

        /// <comentarios/>
        public GroupHeader36 GrpHdr
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
        public OriginalGroupInformation20 OrgnlGrpInfAndSts
        {
            get
            {
                return this.orgnlGrpInfAndStsField;
            }
            set
            {
                this.orgnlGrpInfAndStsField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("OrgnlPmtInfAndSts")]
        public OriginalPaymentInformation1[] OrgnlPmtInfAndSts
        {
            get
            {
                return this.orgnlPmtInfAndStsField;
            }
            set
            {
                this.orgnlPmtInfAndStsField = value;
            }
        }
    }
}
