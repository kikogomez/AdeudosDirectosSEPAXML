namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class GenericPersonIdentification1
    {
        private string idField;
        private PersonIdentificationSchemeName1Choice schmeNmField;
        private string issrField;

        //Parameterless constructor for Serialization purpose
        private GenericPersonIdentification1() { }

        public GenericPersonIdentification1(
            string identification,
            PersonIdentificationSchemeName1Choice personIdentificationSchemeName,
            string issuer)
        {
            this.idField = identification;
            this.schmeNmField = personIdentificationSchemeName;
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
        public PersonIdentificationSchemeName1Choice SchmeNm
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
