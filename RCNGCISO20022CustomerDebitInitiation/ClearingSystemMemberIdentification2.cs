namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class ClearingSystemMemberIdentification2
    {
        private ClearingSystemIdentification2Choice clrSysIdField;
        private string mmbIdField;

        //Parameterless constructor for Serialization purpose
        private ClearingSystemMemberIdentification2() { }

        public ClearingSystemMemberIdentification2(
            ClearingSystemIdentification2Choice clearingSystemIdentification,
            string memberIdentification)
        {
            this.clrSysIdField = clearingSystemIdentification;
            this.mmbIdField = memberIdentification;
        }

        /// <comentarios/>
        public ClearingSystemIdentification2Choice ClrSysId
        {
            get
            {
                return this.clrSysIdField;
            }
            set
            {
                this.clrSysIdField = value;
            }
        }

        /// <comentarios/>
        public string MmbId
        {
            get
            {
                return this.mmbIdField;
            }
            set
            {
                this.mmbIdField = value;
            }
        }
    }
}
