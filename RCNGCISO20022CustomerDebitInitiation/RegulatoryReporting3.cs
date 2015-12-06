namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class RegulatoryReporting3
    {
        private RegulatoryReportingType1Code dbtCdtRptgIndField;
        private bool dbtCdtRptgIndFieldSpecified;
        private RegulatoryAuthority2 authrtyField;
        private StructuredRegulatoryReporting3[] dtlsField;

        //Parameterless constructor for Serialization purpose
        private RegulatoryReporting3() { }
        
        public RegulatoryReporting3(
            RegulatoryReportingType1Code debitCreditReportingIndicator,
            bool debitCreditReportingIndicatorSpecified,
            RegulatoryAuthority2 authority,
            StructuredRegulatoryReporting3[] details)
        {
            this.dbtCdtRptgIndField = debitCreditReportingIndicator;
            this.dbtCdtRptgIndFieldSpecified = debitCreditReportingIndicatorSpecified;
            this.authrtyField = authority;
            this.dtlsField = (StructuredRegulatoryReporting3[])details;
        }

        /// <comentarios/>
        public RegulatoryReportingType1Code DbtCdtRptgInd
        {
            get
            {
                return this.dbtCdtRptgIndField;
            }
            set
            {
                this.dbtCdtRptgIndField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DbtCdtRptgIndSpecified
        {
            get
            {
                return this.dbtCdtRptgIndFieldSpecified;
            }
            set
            {
                this.dbtCdtRptgIndFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public RegulatoryAuthority2 Authrty
        {
            get
            {
                return this.authrtyField;
            }
            set
            {
                this.authrtyField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("Dtls")]
        public StructuredRegulatoryReporting3[] Dtls
        {
            get
            {
                return this.dtlsField;
            }
            set
            {
                this.dtlsField = value;
            }
        }
    }

}
