namespace ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03", IsNullable = false)]
    public class CustomerPaymentStatusReportDocument
    {
        private CustomerPaymentStatusReportV03 cstmrPmtStsRptField;

        //Parameterless constructor for Serialization purpose
        private CustomerPaymentStatusReportDocument() { }

        public CustomerPaymentStatusReportDocument(CustomerPaymentStatusReportV03 customerPaymentStatusReport)
        {
            this.cstmrPmtStsRptField = customerPaymentStatusReport;
        }

        /// The CustomerPaymentStatusReport message is exchanged between an agent and a non-financial institution customer
        /// to provide status information on instructions previously sent
        public CustomerPaymentStatusReportV03 CstmrPmtStsRpt
        {
            get
            {
                return this.cstmrPmtStsRptField;
            }
            set
            {
                this.cstmrPmtStsRptField = value;
            }
        }
    }
}
