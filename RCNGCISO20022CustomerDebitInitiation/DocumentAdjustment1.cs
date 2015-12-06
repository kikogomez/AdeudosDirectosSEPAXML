namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class DocumentAdjustment1
    {
        private ActiveOrHistoricCurrencyAndAmount amtField;
        private CreditDebitCode cdtDbtIndField;
        private bool cdtDbtIndFieldSpecified;
        private string rsnField;
        private string addtlInfField;

        //Parameterless constructor for Serialization purpose
        private DocumentAdjustment1() { }

        public DocumentAdjustment1(
            ActiveOrHistoricCurrencyAndAmount adjustmentAmount,
            CreditDebitCode creditDebitIndicatorCode,
            bool creditDebitIndicatorCodeSpecified,
            string adjustmentReason,
            string additionalInformationForAdjustment
            )
        {
            this.amtField = adjustmentAmount;
            this.cdtDbtIndField = creditDebitIndicatorCode;
            this.cdtDbtIndFieldSpecified = creditDebitIndicatorCodeSpecified;
            this.rsnField = adjustmentReason;
            this.addtlInfField = additionalInformationForAdjustment;
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
        public CreditDebitCode CdtDbtInd
        {
            get
            {
                return this.cdtDbtIndField;
            }
            set
            {
                this.cdtDbtIndField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CdtDbtIndSpecified
        {
            get
            {
                return this.cdtDbtIndFieldSpecified;
            }
            set
            {
                this.cdtDbtIndFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public string Rsn
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
        public string AddtlInf
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
