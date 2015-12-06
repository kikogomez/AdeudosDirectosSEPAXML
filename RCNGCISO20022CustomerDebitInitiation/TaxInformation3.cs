namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class TaxInformation3
    {
        private TaxParty1 cdtrField;
        private TaxParty2 dbtrField;
        private string admstnZnField;
        private string refNbField;
        private string mtdField;
        private ActiveOrHistoricCurrencyAndAmount ttlTaxblBaseAmtField;
        private ActiveOrHistoricCurrencyAndAmount ttlTaxAmtField;
        private System.DateTime dtField;
        private bool dtFieldSpecified;
        private decimal seqNbField;
        private bool seqNbFieldSpecified;
        private TaxRecord1[] rcrdField;

        //Parameterless constructor for Serialization purpose
        private TaxInformation3() { }

        public TaxInformation3(
            TaxParty1 creditor,
            TaxParty2 debtor,
            string administrationZone,
            string referenceNumber,
            string method,
            ActiveOrHistoricCurrencyAndAmount totalTaxableBaseAmount,
            ActiveOrHistoricCurrencyAndAmount totalTaxAmount,
            System.DateTime date,
            bool dateSepecified,
            decimal sequenceNumber,
            bool sequenceNumberSpecified,
            TaxRecord1[] record)
        {
            this.cdtrField = creditor;
            this.dbtrField = debtor;
            this.admstnZnField = administrationZone;
            this.refNbField = referenceNumber;
            this.mtdField = method;
            this.ttlTaxblBaseAmtField = totalTaxableBaseAmount;
            this.ttlTaxAmtField = totalTaxAmount;
            this.dtField = date;
            this.dtFieldSpecified = dateSepecified;
            this.seqNbField = sequenceNumber;
            this.seqNbFieldSpecified = sequenceNumberSpecified;
            this.rcrdField = (TaxRecord1[])record.Clone(); ;
        }

        /// <comentarios/>
        public TaxParty1 Cdtr
        {
            get
            {
                return this.cdtrField;
            }
            set
            {
                this.cdtrField = value;
            }
        }

        /// <comentarios/>
        public TaxParty2 Dbtr
        {
            get
            {
                return this.dbtrField;
            }
            set
            {
                this.dbtrField = value;
            }
        }

        /// <comentarios/>
        public string AdmstnZn
        {
            get
            {
                return this.admstnZnField;
            }
            set
            {
                this.admstnZnField = value;
            }
        }

        /// <comentarios/>
        public string RefNb
        {
            get
            {
                return this.refNbField;
            }
            set
            {
                this.refNbField = value;
            }
        }

        /// <comentarios/>
        public string Mtd
        {
            get
            {
                return this.mtdField;
            }
            set
            {
                this.mtdField = value;
            }
        }

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount TtlTaxblBaseAmt
        {
            get
            {
                return this.ttlTaxblBaseAmtField;
            }
            set
            {
                this.ttlTaxblBaseAmtField = value;
            }
        }

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount TtlTaxAmt
        {
            get
            {
                return this.ttlTaxAmtField;
            }
            set
            {
                this.ttlTaxAmtField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Dt
        {
            get
            {
                return this.dtField;
            }
            set
            {
                this.dtField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DtSpecified
        {
            get
            {
                return this.dtFieldSpecified;
            }
            set
            {
                this.dtFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public decimal SeqNb
        {
            get
            {
                return this.seqNbField;
            }
            set
            {
                this.seqNbField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SeqNbSpecified
        {
            get
            {
                return this.seqNbFieldSpecified;
            }
            set
            {
                this.seqNbFieldSpecified = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("Rcrd")]
        public TaxRecord1[] Rcrd
        {
            get
            {
                return this.rcrdField;
            }
            set
            {
                this.rcrdField = value;
            }
        }
    }
}
