namespace ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public class ChargesInformation5
    {
        private ActiveOrHistoricCurrencyAndAmount amtField;
        private BranchAndFinancialInstitutionIdentification4 ptyField;

        //Parameterless constructor for Serialization purpose
        public ChargesInformation5() { }

        public ChargesInformation5(ActiveOrHistoricCurrencyAndAmount amount, BranchAndFinancialInstitutionIdentification4 party)
        {
            this.amtField = amount;
            this.ptyField = party;
        }

        /// <comentarios/>
        public ActiveOrHistoricCurrencyAndAmount Amt
        {
            get
            {
                return this.amtField;
            }
            set
            {
                this.amtField = value;
            }
        }

        /// <comentarios/>
        public BranchAndFinancialInstitutionIdentification4 Pty
        {
            get
            {
                return this.ptyField;
            }
            set
            {
                this.ptyField = value;
            }
        }
    }
}
