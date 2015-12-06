namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class DirectDebitTransactionInformation9
    {
        private PaymentIdentification1 pmtIdField;
        private PaymentTypeInformation20 pmtTpInfField;
        private ActiveOrHistoricCurrencyAndAmount instdAmtField;
        private ChargeBearerType1Code chrgBrField;
        private bool chrgBrFieldSpecified;
        private DirectDebitTransaction6 drctDbtTxField;
        private PartyIdentification32 ultmtCdtrField;
        private BranchAndFinancialInstitutionIdentification4 dbtrAgtField;
        private CashAccount16 dbtrAgtAcctField;
        private PartyIdentification32 dbtrField;
        private CashAccount16 dbtrAcctField;
        private PartyIdentification32 ultmtDbtrField;
        private string instrForCdtrAgtField;
        private Purpose2Choice purpField;
        private RegulatoryReporting3[] rgltryRptgField;
        private TaxInformation3 taxField;
        private RemittanceLocation2[] rltdRmtInfField;
        private RemittanceInformation5 rmtInfField;

        //Parameterless constructor for Serialization purpose
        private DirectDebitTransactionInformation9() { }

        public DirectDebitTransactionInformation9(
            PaymentIdentification1 paymentIdentification,
            PaymentTypeInformation20 paymentTypeInformation,
            ActiveOrHistoricCurrencyAndAmount instructedAmount,
            ChargeBearerType1Code chargeBearer,
            bool chargeBearerSpecified,
            DirectDebitTransaction6 directDebitTransaction,
            PartyIdentification32 ultimateCreditor,
            BranchAndFinancialInstitutionIdentification4 debtorAgent,
            CashAccount16 debtorAgentAccount,
            PartyIdentification32 debtor,
            CashAccount16 debtorAccount,
            PartyIdentification32 ultimateDebtor,
            string instructionForCreditorAgent,
            Purpose2Choice purpose,
            RegulatoryReporting3[] regulatoryReporting,
            TaxInformation3 tax,
            RemittanceLocation2[] relatedRemittanceInformation,
            RemittanceInformation5 remittanceInformation)
        {
            this.pmtIdField = paymentIdentification;
            this.pmtTpInfField = paymentTypeInformation;
            this.instdAmtField = instructedAmount;
            this.chrgBrField = chargeBearer;
            this.chrgBrFieldSpecified = chargeBearerSpecified;
            this.drctDbtTxField=directDebitTransaction;
            this.ultmtCdtrField=ultimateCreditor;
            this.dbtrAgtField=debtorAgent;
            this.dbtrAgtAcctField=debtorAgentAccount;
            this.dbtrField = debtor;
            this.dbtrAcctField = debtorAccount;
            this.ultmtDbtrField = ultimateDebtor;
            this.instrForCdtrAgtField = instructionForCreditorAgent;
            this.purpField = purpose;
            this.rgltryRptgField = (RegulatoryReporting3[])regulatoryReporting.Clone();
            this.taxField = tax;
            this.rltdRmtInfField=(RemittanceLocation2[])relatedRemittanceInformation.Clone();
            this.rmtInfField = remittanceInformation;
        }
        /// <comentarios/>
        public PaymentIdentification1 PmtId
        {
            get
            {
                return this.pmtIdField;
            }
            set
            {
                this.pmtIdField = value;
            }
        }

        /// <comentarios/>
        public PaymentTypeInformation20 PmtTpInf
        {
            get
            {
                return this.pmtTpInfField;
            }
            set
            {
                this.pmtTpInfField = value;
            }
        }

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount InstdAmt
        {
            get
            {
                return this.instdAmtField;
            }
            set
            {
                this.instdAmtField = value;
            }
        }

        /// <comentarios/>
        public ChargeBearerType1Code ChrgBr
        {
            get
            {
                return this.chrgBrField;
            }
            set
            {
                this.chrgBrField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ChrgBrSpecified
        {
            get
            {
                return this.chrgBrFieldSpecified;
            }
            set
            {
                this.chrgBrFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public DirectDebitTransaction6 DrctDbtTx
        {
            get
            {
                return this.drctDbtTxField;
            }
            set
            {
                this.drctDbtTxField = value;
            }
        }

        /// <comentarios/>
        public PartyIdentification32 UltmtCdtr
        {
            get
            {
                return this.ultmtCdtrField;
            }
            set
            {
                this.ultmtCdtrField = value;
            }
        }

        /// <comentarios/>
        public BranchAndFinancialInstitutionIdentification4 DbtrAgt
        {
            get
            {
                return this.dbtrAgtField;
            }
            set
            {
                this.dbtrAgtField = value;
            }
        }

        /// <comentarios/>
        public CashAccount16 DbtrAgtAcct
        {
            get
            {
                return this.dbtrAgtAcctField;
            }
            set
            {
                this.dbtrAgtAcctField = value;
            }
        }

        /// <comentarios/>
        public PartyIdentification32 Dbtr
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
        public CashAccount16 DbtrAcct
        {
            get
            {
                return this.dbtrAcctField;
            }
            set
            {
                this.dbtrAcctField = value;
            }
        }

        /// <comentarios/>
        public PartyIdentification32 UltmtDbtr
        {
            get
            {
                return this.ultmtDbtrField;
            }
            set
            {
                this.ultmtDbtrField = value;
            }
        }

        /// <comentarios/>
        public string InstrForCdtrAgt
        {
            get
            {
                return this.instrForCdtrAgtField;
            }
            set
            {
                this.instrForCdtrAgtField = value;
            }
        }

        /// <comentarios/>
        public Purpose2Choice Purp
        {
            get
            {
                return this.purpField;
            }
            set
            {
                this.purpField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("RgltryRptg")]
        public RegulatoryReporting3[] RgltryRptg
        {
            get
            {
                return this.rgltryRptgField;
            }
            set
            {
                this.rgltryRptgField = value;
            }
        }

        /// <comentarios/>
        public TaxInformation3 Tax
        {
            get
            {
                return this.taxField;
            }
            set
            {
                this.taxField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("RltdRmtInf")]
        public RemittanceLocation2[] RltdRmtInf
        {
            get
            {
                return this.rltdRmtInfField;
            }
            set
            {
                this.rltdRmtInfField = value;
            }
        }

        /// <comentarios/>
        public RemittanceInformation5 RmtInf
        {
            get
            {
                return this.rmtInfField;
            }
            set
            {
                this.rmtInfField = value;
            }
        }
    }
}
