namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class GroupHeader39
    {
        private string msgIdField;
        private System.DateTime creDtTmField;
        private Authorisation1Choice[] authstnField;
        private string nbOfTxsField;
        private decimal ctrlSumField;
        private bool ctrlSumFieldSpecified;
        private PartyIdentification32 initgPtyField;
        private BranchAndFinancialInstitutionIdentification4 fwdgAgtField;

        //Parameterless constructor for Serialization purpose
        private GroupHeader39() { }

        public GroupHeader39(
            string messageID,
            System.DateTime creationDateTime,
            Authorisation1Choice[] authorisationChoice,
            string numberOfTransactions,
            decimal controlSum,
            bool controlSumSpecified,
            PartyIdentification32 initiatingParty,
            BranchAndFinancialInstitutionIdentification4 forwardingAgent)
        {
            this.msgIdField = messageID;
            this.creDtTmField = creationDateTime;
            this.authstnField = (Authorisation1Choice[])authorisationChoice.Clone();
            this.nbOfTxsField = numberOfTransactions;
            this.ctrlSumField = controlSum;
            this.ctrlSumFieldSpecified = controlSumSpecified;
            this.initgPtyField = initiatingParty;
            this.fwdgAgtField = forwardingAgent;
        }

        /// <comentarios/>
        public string MsgId
        {
            get
            {
                return this.msgIdField;
            }
            set
            {
                this.msgIdField = value;
            }
        }

        /// <comentarios/>
        public System.DateTime CreDtTm
        {
            get
            {
                return this.creDtTmField;
            }
            set
            {
                this.creDtTmField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("Authstn")]
        public Authorisation1Choice[] Authstn
        {
            get
            {
                return this.authstnField;
            }
            set
            {
                this.authstnField = value;
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
        public PartyIdentification32 InitgPty
        {
            get
            {
                return this.initgPtyField;
            }
            set
            {
                this.initgPtyField = value;
            }
        }

        /// <comentarios/>
        public BranchAndFinancialInstitutionIdentification4 FwdgAgt
        {
            get
            {
                return this.fwdgAgtField;
            }
            set
            {
                this.fwdgAgtField = value;
            }
        }
    }
}
