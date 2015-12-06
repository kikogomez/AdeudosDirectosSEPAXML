namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class GenericOrganisationIdentification1
    {
        private string idField;
        private OrganisationIdentificationSchemeName1Choice schmeNmField;
        private string issrField;

        //Parameterless constructor for Serialization purpose
        private GenericOrganisationIdentification1() { }

        public GenericOrganisationIdentification1(
            string identification,
            OrganisationIdentificationSchemeName1Choice schemeName,
            string issuer)
        {
            this.idField = identification;
            this.schmeNmField = schemeName;
            this.issrField = issuer;
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
        public OrganisationIdentificationSchemeName1Choice SchmeNm
        {
            get
            {
                return this.schmeNmField;
            }
            set
            {
                this.schmeNmField = value;
            }
        }

        /// <comentarios/>
        public string Issr
        {
            get
            {
                return this.issrField;
            }
            set
            {
                this.issrField = value;
            }
        }
    }
}
