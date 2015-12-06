namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class BranchAndFinancialInstitutionIdentification4
    {
        private FinancialInstitutionIdentification7 finInstnIdField;
        private BranchData2 brnchIdField;

        //Parameterless constructor for Serialization purpose
        private BranchAndFinancialInstitutionIdentification4() { }

        public BranchAndFinancialInstitutionIdentification4(
            FinancialInstitutionIdentification7 financialInstitutionIdentification,
            BranchData2 branchIdentification)
        {
            this.finInstnIdField = financialInstitutionIdentification;
            this.brnchIdField = branchIdentification;
        }

        /// <comentarios/>
        public FinancialInstitutionIdentification7 FinInstnId
        {
            get
            {
                return this.finInstnIdField;
            }
            set
            {
                this.finInstnIdField = value;
            }
        }

        /// <comentarios/>
        public BranchData2 BrnchId
        {
            get
            {
                return this.brnchIdField;
            }
            set
            {
                this.brnchIdField = value;
            }
        }
    }
}
