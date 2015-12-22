namespace ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public class SettlementInformation13
    {
        private SettlementMethod1Code sttlmMtdField;
        private CashAccount16 sttlmAcctField;
        private ClearingSystemIdentification3Choice clrSysField;
        private BranchAndFinancialInstitutionIdentification4 instgRmbrsmntAgtField;
        private CashAccount16 instgRmbrsmntAgtAcctField;
        private BranchAndFinancialInstitutionIdentification4 instdRmbrsmntAgtField;
        private CashAccount16 instdRmbrsmntAgtAcctField;
        private BranchAndFinancialInstitutionIdentification4 thrdRmbrsmntAgtField;
        private CashAccount16 thrdRmbrsmntAgtAcctField;

        //Parameterless constructor for Serialization purposes
        public SettlementInformation13() { }

        public SettlementInformation13(
            SettlementMethod1Code settlementMethod,
            CashAccount16 settlementAccount,
            ClearingSystemIdentification3Choice clearingSystem,
            BranchAndFinancialInstitutionIdentification4 instructingReimbursementAgent,
            CashAccount16 instructingReimbursementAgentAccount,
            BranchAndFinancialInstitutionIdentification4 instructedReimbursementAgent,
            CashAccount16 instructedReimbursementAgentAccount,
            BranchAndFinancialInstitutionIdentification4 thirdReimbursementAgent,
            CashAccount16 thirdReimbursementAgentAccount
            )
        {
            this.sttlmMtdField = settlementMethod;
            this.sttlmAcctField = settlementAccount;
            this.clrSysField = clearingSystem;
            this.instgRmbrsmntAgtField = instructingReimbursementAgent;
            this.instgRmbrsmntAgtAcctField = instructingReimbursementAgentAccount;
            this.instdRmbrsmntAgtField = instructedReimbursementAgent;
            this.instdRmbrsmntAgtAcctField = instructedReimbursementAgentAccount;
            this.thrdRmbrsmntAgtField = thirdReimbursementAgent;
            this.thrdRmbrsmntAgtAcctField = thirdReimbursementAgentAccount;

    }

        /// <comentarios/>
        public SettlementMethod1Code SttlmMtd
        {
            get
            {
                return this.sttlmMtdField;
            }
            set
            {
                this.sttlmMtdField = value;
            }
        }

        /// <comentarios/>
        public CashAccount16 SttlmAcct
        {
            get
            {
                return this.sttlmAcctField;
            }
            set
            {
                this.sttlmAcctField = value;
            }
        }

        /// <comentarios/>
        public ClearingSystemIdentification3Choice ClrSys
        {
            get
            {
                return this.clrSysField;
            }
            set
            {
                this.clrSysField = value;
            }
        }

        /// <comentarios/>
        public BranchAndFinancialInstitutionIdentification4 InstgRmbrsmntAgt
        {
            get
            {
                return this.instgRmbrsmntAgtField;
            }
            set
            {
                this.instgRmbrsmntAgtField = value;
            }
        }

        /// <comentarios/>
        public CashAccount16 InstgRmbrsmntAgtAcct
        {
            get
            {
                return this.instgRmbrsmntAgtAcctField;
            }
            set
            {
                this.instgRmbrsmntAgtAcctField = value;
            }
        }

        /// <comentarios/>
        public BranchAndFinancialInstitutionIdentification4 InstdRmbrsmntAgt
        {
            get
            {
                return this.instdRmbrsmntAgtField;
            }
            set
            {
                this.instdRmbrsmntAgtField = value;
            }
        }

        /// <comentarios/>
        public CashAccount16 InstdRmbrsmntAgtAcct
        {
            get
            {
                return this.instdRmbrsmntAgtAcctField;
            }
            set
            {
                this.instdRmbrsmntAgtAcctField = value;
            }
        }

        /// <comentarios/>
        public BranchAndFinancialInstitutionIdentification4 ThrdRmbrsmntAgt
        {
            get
            {
                return this.thrdRmbrsmntAgtField;
            }
            set
            {
                this.thrdRmbrsmntAgtField = value;
            }
        }

        /// <comentarios/>
        public CashAccount16 ThrdRmbrsmntAgtAcct
        {
            get
            {
                return this.thrdRmbrsmntAgtAcctField;
            }
            set
            {
                this.thrdRmbrsmntAgtAcctField = value;
            }
        }
    }
}
