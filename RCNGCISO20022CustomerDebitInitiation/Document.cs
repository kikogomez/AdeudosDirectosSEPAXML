namespace RCNGCISO20022CustomerDebitInitiation
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02", IsNullable = false)]
    public class Document
    {
        private CustomerDirectDebitInitiationV02 cstmrDrctDbtInitnField;

        //Parameterless constructor for Serialization purpose
        private Document() { }

        public Document(CustomerDirectDebitInitiationV02 customerDirectDebitInitiation)
        {
            this.cstmrDrctDbtInitnField = customerDirectDebitInitiation;
        }

        /// Customer Direct Debit Initiation Message
        /// Used to request single or bulk collection(s) of funds from one or various debtor's account(s) for a creditor.
        public CustomerDirectDebitInitiationV02 CstmrDrctDbtInitn
        {
            get
            {
                return this.cstmrDrctDbtInitnField;
            }
            set
            {
                this.cstmrDrctDbtInitnField = value;
            }
        }
    }
}
