namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class RemittanceAmount1
    {
        private ActiveOrHistoricCurrencyAndAmount duePyblAmtField;
        private ActiveOrHistoricCurrencyAndAmount dscntApldAmtField;
        private ActiveOrHistoricCurrencyAndAmount cdtNoteAmtField;
        private ActiveOrHistoricCurrencyAndAmount taxAmtField;
        private DocumentAdjustment1[] adjstmntAmtAndRsnField;
        private ActiveOrHistoricCurrencyAndAmount rmtdAmtField;

        //Parameterless constructor for Serialization purpose
        private RemittanceAmount1() { }

        public RemittanceAmount1(
            ActiveOrHistoricCurrencyAndAmount duePayableAmount,
            ActiveOrHistoricCurrencyAndAmount discountApliedAmount,
            ActiveOrHistoricCurrencyAndAmount creditNoteAmount,
            ActiveOrHistoricCurrencyAndAmount taxAmount,
            DocumentAdjustment1[] adjustedAmountAndReason,
            ActiveOrHistoricCurrencyAndAmount remittedAmount
            )
        {
            this.duePyblAmtField = duePayableAmount;
            this.dscntApldAmtField = discountApliedAmount;
            this.cdtNoteAmtField=creditNoteAmount;
            this.taxAmtField = taxAmount;
            this.adjstmntAmtAndRsnField = (DocumentAdjustment1[])adjustedAmountAndReason.Clone();
            this.rmtdAmtField = remittedAmount;
        }

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount DuePyblAmt
        {
            get
            {
                return this.duePyblAmtField;
            }
            set
            {
                this.duePyblAmtField = value;
            }
        }

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount DscntApldAmt
        {
            get
            {
                return this.dscntApldAmtField;
            }
            set
            {
                this.dscntApldAmtField = value;
            }
        }

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount CdtNoteAmt
        {
            get
            {
                return this.cdtNoteAmtField;
            }
            set
            {
                this.cdtNoteAmtField = value;
            }
        }

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount TaxAmt
        {
            get
            {
                return this.taxAmtField;
            }
            set
            {
                this.taxAmtField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("AdjstmntAmtAndRsn")]
        public DocumentAdjustment1[] AdjstmntAmtAndRsn
        {
            get
            {
                return this.adjstmntAmtAndRsnField;
            }
            set
            {
                this.adjstmntAmtAndRsnField = value;
            }
        }

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount RmtdAmt
        {
            get
            {
                return this.rmtdAmtField;
            }
            set
            {
                this.rmtdAmtField = value;
            }
        }
    }
}
