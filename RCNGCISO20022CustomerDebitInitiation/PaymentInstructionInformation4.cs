namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class PaymentInstructionInformation4
    {
        private string pmtInfIdField;
        private PaymentMethod2Code pmtMtdField;
        private bool btchBookgField;
        private bool btchBookgFieldSpecified;
        private string nbOfTxsField;
        private decimal ctrlSumField;
        private bool ctrlSumFieldSpecified;
        private PaymentTypeInformation20 pmtTpInfField;
        private System.DateTime reqdColltnDtField;
        private PartyIdentification32 cdtrField;
        private CashAccount16 cdtrAcctField;
        private BranchAndFinancialInstitutionIdentification4 cdtrAgtField;
        private CashAccount16 cdtrAgtAcctField;
        private PartyIdentification32 ultmtCdtrField;
        private ChargeBearerType1Code chrgBrField;
        private bool chrgBrFieldSpecified;
        private CashAccount16 chrgsAcctField;
        private BranchAndFinancialInstitutionIdentification4 chrgsAcctAgtField;
        private PartyIdentification32 cdtrSchmeIdField;
        private DirectDebitTransactionInformation9[] drctDbtTxInfField;

        //Parameterless constructor for Serialization purpose
        private PaymentInstructionInformation4() { }

        public PaymentInstructionInformation4(
            string paymentInformationIdentification,
            PaymentMethod2Code paymentMethod,
            bool batchBooking,
            bool batchBookingSpecified,
            string numberOfTransactions,
            decimal controlSum,
            bool controlSumSpecified,
            PaymentTypeInformation20 paymentTypeInformation,
            System.DateTime requestedCollectionDate,
            PartyIdentification32 creditor,
            CashAccount16 creditorAccount,
            BranchAndFinancialInstitutionIdentification4 creditorAgent,
            CashAccount16 creditorAgentAccount,
            PartyIdentification32 ultimateCreditor,
            ChargeBearerType1Code chargeBearer,
            bool chargeBearerSpecified,
            CashAccount16 chargesAccount,
            BranchAndFinancialInstitutionIdentification4 chargesAccountAgent,
            PartyIdentification32 creditorSchemeIdentification,
            DirectDebitTransactionInformation9[] directDebitTransactionInformation)
        {
            this.pmtInfIdField = paymentInformationIdentification;
            this.pmtMtdField = paymentMethod;
            this.btchBookgField = batchBooking;
            this.btchBookgFieldSpecified = batchBookingSpecified;
            this.nbOfTxsField = numberOfTransactions;
            this.ctrlSumField = controlSum;
            this.ctrlSumFieldSpecified = controlSumSpecified;
            this.pmtTpInfField = paymentTypeInformation;
            this.reqdColltnDtField = requestedCollectionDate;
            this.cdtrField = creditor;
            this.cdtrAcctField = creditorAccount;
            this.cdtrAgtField = creditorAgent;
            this.cdtrAgtAcctField = creditorAgentAccount;
            this.ultmtCdtrField = ultimateCreditor;
            this.chrgBrField = chargeBearer;
            this.chrgBrFieldSpecified = chargeBearerSpecified;
            this.chrgsAcctField = chargesAccount;
            this.chrgsAcctAgtField = chargesAccountAgent;
            this.cdtrSchmeIdField = creditorSchemeIdentification;
            this.drctDbtTxInfField = (DirectDebitTransactionInformation9[])directDebitTransactionInformation.Clone();
        }

        /// <comentarios/>
        public string PmtInfId
        {
            get
            {
                return this.pmtInfIdField;
            }
            set
            {
                this.pmtInfIdField = value;
            }
        }

        /// <comentarios/>
        public PaymentMethod2Code PmtMtd
        {
            get
            {
                return this.pmtMtdField;
            }
            set
            {
                this.pmtMtdField = value;
            }
        }

        /// <comentarios/>
        public bool BtchBookg
        {
            get
            {
                return this.btchBookgField;
            }
            set
            {
                this.btchBookgField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BtchBookgSpecified
        {
            get
            {
                return this.btchBookgFieldSpecified;
            }
            set
            {
                this.btchBookgFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public string NbOfTxs
        {
            get
            {
                return this.nbOfTxsField;
            }
            set
            {
                this.nbOfTxsField = value;
            }
        }

        /// <comentarios/>
        public decimal CtrlSum
        {
            get
            {
                return this.ctrlSumField;
            }
            set
            {
                this.ctrlSumField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CtrlSumSpecified
        {
            get
            {
                return this.ctrlSumFieldSpecified;
            }
            set
            {
                this.ctrlSumFieldSpecified = value;
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
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime ReqdColltnDt
        {
            get
            {
                return this.reqdColltnDtField;
            }
            set
            {
                this.reqdColltnDtField = value;
            }
        }

        /// <comentarios/>
        public PartyIdentification32 Cdtr
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
        public CashAccount16 CdtrAcct
        {
            get
            {
                return this.cdtrAcctField;
            }
            set
            {
                this.cdtrAcctField = value;
            }
        }

        /// <comentarios/>
        public BranchAndFinancialInstitutionIdentification4 CdtrAgt
        {
            get
            {
                return this.cdtrAgtField;
            }
            set
            {
                this.cdtrAgtField = value;
            }
        }

        /// <comentarios/>
        public CashAccount16 CdtrAgtAcct
        {
            get
            {
                return this.cdtrAgtAcctField;
            }
            set
            {
                this.cdtrAgtAcctField = value;
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
        public CashAccount16 ChrgsAcct
        {
            get
            {
                return this.chrgsAcctField;
            }
            set
            {
                this.chrgsAcctField = value;
            }
        }

        /// <comentarios/>
        public BranchAndFinancialInstitutionIdentification4 ChrgsAcctAgt
        {
            get
            {
                return this.chrgsAcctAgtField;
            }
            set
            {
                this.chrgsAcctAgtField = value;
            }
        }

        /// <comentarios/>
        public PartyIdentification32 CdtrSchmeId
        {
            get
            {
                return this.cdtrSchmeIdField;
            }
            set
            {
                this.cdtrSchmeIdField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("DrctDbtTxInf")]
        public DirectDebitTransactionInformation9[] DrctDbtTxInf
        {
            get
            {
                return this.drctDbtTxInfField;
            }
            set
            {
                this.drctDbtTxInfField = value;
            }
        }
    }
}
