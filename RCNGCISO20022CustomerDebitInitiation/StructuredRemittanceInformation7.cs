namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class StructuredRemittanceInformation7
    {
        private ReferredDocumentInformation3[] rfrdDocInfField;
        private RemittanceAmount1 rfrdDocAmtField;
        private CreditorReferenceInformation2 cdtrRefInfField;
        private PartyIdentification32 invcrField;
        private PartyIdentification32 invceeField;
        private string[] addtlRmtInfField;

        //Parameterless constructor for Serialization purpose
        private StructuredRemittanceInformation7() { }

        public StructuredRemittanceInformation7(
            ReferredDocumentInformation3[] referredDocumentInformation,
            RemittanceAmount1 referredDocumentAmount,
            CreditorReferenceInformation2 creditorReferenceInformation,
            PartyIdentification32 invoicer,
            PartyIdentification32 invoicee,
            string[] additionalRemittanceInformation
            )
        {
            this.rfrdDocInfField = (ReferredDocumentInformation3[])referredDocumentInformation.Clone();
            this.rfrdDocAmtField = referredDocumentAmount;
            this.cdtrRefInfField = creditorReferenceInformation;
            this.invcrField = invoicer;
            this.invceeField = invoicee;
            this.addtlRmtInfField = (string[])additionalRemittanceInformation.Clone();
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("RfrdDocInf")]
        public ReferredDocumentInformation3[] RfrdDocInf
        {
            get
            {
                return this.rfrdDocInfField;
            }
            set
            {
                this.rfrdDocInfField = value;
            }
        }

        /// <comentarios/>
        public RemittanceAmount1 RfrdDocAmt
        {
            get
            {
                return this.rfrdDocAmtField;
            }
            set
            {
                this.rfrdDocAmtField = value;
            }
        }

        /// <comentarios/>
        public CreditorReferenceInformation2 CdtrRefInf
        {
            get
            {
                return this.cdtrRefInfField;
            }
            set
            {
                this.cdtrRefInfField = value;
            }
        }

        /// <comentarios/>
        public PartyIdentification32 Invcr
        {
            get
            {
                return this.invcrField;
            }
            set
            {
                this.invcrField = value;
            }
        }

        /// <comentarios/>
        public PartyIdentification32 Invcee
        {
            get
            {
                return this.invceeField;
            }
            set
            {
                this.invceeField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("AddtlRmtInf")]
        public string[] AddtlRmtInf
        {
            get
            {
                return this.addtlRmtInfField;
            }
            set
            {
                this.addtlRmtInfField = value;
            }
        }
    }
}
