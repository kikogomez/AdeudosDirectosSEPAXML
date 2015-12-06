namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class FinancialInstitutionIdentification7
    {
        private string bICField;
        private ClearingSystemMemberIdentification2 clrSysMmbIdField;
        private string nmField;
        private PostalAddress6 pstlAdrField;
        private GenericFinancialIdentification1 othrField;

        //Parameterless constructor for Serialization purpose
        private FinancialInstitutionIdentification7() { }

        public FinancialInstitutionIdentification7(
            string bIC,
            ClearingSystemMemberIdentification2 clearingSystemMemberIdentification,
            string name,
            PostalAddress6 postalAddress,
            GenericFinancialIdentification1 other)
        {
            this.bICField = bIC;
            this.clrSysMmbIdField = clearingSystemMemberIdentification;
            this.nmField = name;
            this.pstlAdrField = postalAddress;
            this.othrField = other;
        }

        /// <comentarios/>
        public string BIC
        {
            get
            {
                return this.bICField;
            }
            set
            {
                this.bICField = value;
            }
        }

        /// <comentarios/>
        public ClearingSystemMemberIdentification2 ClrSysMmbId
        {
            get
            {
                return this.clrSysMmbIdField;
            }
            set
            {
                this.clrSysMmbIdField = value;
            }
        }

        /// <comentarios/>
        public string Nm
        {
            get
            {
                return this.nmField;
            }
            set
            {
                this.nmField = value;
            }
        }

        /// <comentarios/>
        public PostalAddress6 PstlAdr
        {
            get
            {
                return this.pstlAdrField;
            }
            set
            {
                this.pstlAdrField = value;
            }
        }

        /// <comentarios/>
        public GenericFinancialIdentification1 Othr
        {
            get
            {
                return this.othrField;
            }
            set
            {
                this.othrField = value;
            }
        }
    }

}
