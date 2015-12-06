namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class BranchData2
    {
        private string idField;
        private string nmField;
        private PostalAddress6 pstlAdrField;

        //Parameterless constructor for Serialization purpose
        private BranchData2() { }

        public BranchData2(
            string identification,
            string name,
            PostalAddress6 postalAddress)
        {
            this.idField = identification;
            this.nmField = name;
            this.pstlAdrField = postalAddress;
        }

        /// <comentarios/>
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
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
    }

}
