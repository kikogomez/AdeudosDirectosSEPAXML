namespace ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public class GroupHeader36
    {
        private string msgIdField;
        private System.DateTime creDtTmField;
        private PartyIdentification32 initgPtyField;
        private BranchAndFinancialInstitutionIdentification4 fwdgAgtField;
        private BranchAndFinancialInstitutionIdentification4 dbtrAgtField;
        private BranchAndFinancialInstitutionIdentification4 cdtrAgtField;

        //Parameterless constructor for Serialization purpose
        public GroupHeader36() { }

        public GroupHeader36(
            string messageIdentification,
            System.DateTime creationDateTime,
            PartyIdentification32 initiatingParty,
            BranchAndFinancialInstitutionIdentification4 forwardingAgent,
            BranchAndFinancialInstitutionIdentification4 debtorAgent,
            BranchAndFinancialInstitutionIdentification4 creditorAgent)
        {
            this.msgIdField = messageIdentification;
            this.creDtTmField = creationDateTime;
            this.initgPtyField = initiatingParty;
            this.fwdgAgtField = forwardingAgent;
            this.dbtrAgtField = debtorAgent;
            this.cdtrAgtField = creditorAgent;
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
    }
}
