namespace ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public class NumberOfTransactionsPerStatus3
    {
        private string dtldNbOfTxsField;
        private TransactionIndividualStatus3Code dtldStsField;
        private decimal dtldCtrlSumField;
        private bool dtldCtrlSumFieldSpecified;

        public NumberOfTransactionsPerStatus3() { }

        public NumberOfTransactionsPerStatus3(
            string detailedNumberOfTransactions,
            TransactionIndividualStatus3Code detailedStatus,
            decimal detailedControlSum,
            bool detailedControlSumSpecified)
        {
            this.dtldNbOfTxsField = detailedNumberOfTransactions;
            this.dtldStsField = detailedStatus;
            this.dtldCtrlSumField = detailedControlSum;
            this.dtldCtrlSumFieldSpecified = detailedControlSumSpecified;
        }


        /// <comentarios/>
        public string DtldNbOfTxs
        {
            get
            {
                return this.dtldNbOfTxsField;
            }
            set
            {
                this.dtldNbOfTxsField = value;
            }
        }

        /// <comentarios/>
        public TransactionIndividualStatus3Code DtldSts
        {
            get
            {
                return this.dtldStsField;
            }
            set
            {
                this.dtldStsField = value;
            }
        }

        /// <comentarios/>
        public decimal DtldCtrlSum
        {
            get
            {
                return this.dtldCtrlSumField;
            }
            set
            {
                this.dtldCtrlSumField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DtldCtrlSumSpecified
        {
            get
            {
                return this.dtldCtrlSumFieldSpecified;
            }
            set
            {
                this.dtldCtrlSumFieldSpecified = value;
            }
        }
    }
}
