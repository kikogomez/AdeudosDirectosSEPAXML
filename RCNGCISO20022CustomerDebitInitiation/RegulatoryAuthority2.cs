namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class RegulatoryAuthority2
    {
        private string nmField;
        private string ctryField;

        //Parameterless constructor for Serialization purpose
        private RegulatoryAuthority2() { }

        public RegulatoryAuthority2(string name, string country)
        {
            this.nmField = name;
            this.ctryField = country;
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
        public string Ctry
        {
            get
            {
                return this.ctryField;
            }
            set
            {
                this.ctryField = value;
            }
        }
    }
}
