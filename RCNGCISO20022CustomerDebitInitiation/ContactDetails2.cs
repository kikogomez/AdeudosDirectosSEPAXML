namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class ContactDetails2
    {
        private NamePrefix1Code nmPrfxField;
        private bool nmPrfxFieldSpecified;
        private string nmField;
        private string phneNbField;
        private string mobNbField;
        private string faxNbField;
        private string emailAdrField;
        private string othrField;

        //Parameterless constructor for Serialization purpose
        private ContactDetails2() { }

        public ContactDetails2(
            NamePrefix1Code namePrefix,
            bool namePrefixSpecified,
            string name,
            string phoneNumber,
            string mobileNumber,
            string faxNumber,
            string emailAddress,
            string other)
        {
            this.nmPrfxField= namePrefix;
            this.nmPrfxFieldSpecified = namePrefixSpecified;
            this.nmField = name;
            this.phneNbField = phoneNumber;
            this.mobNbField = mobileNumber;
            this.faxNbField = faxNumber;
            this.emailAdrField = emailAddress;
            this.othrField = other;
        }

        /// <comentarios/>
        public NamePrefix1Code NmPrfx
        {
            get
            {
                return this.nmPrfxField;
            }
            set
            {
                this.nmPrfxField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NmPrfxSpecified
        {
            get
            {
                return this.nmPrfxFieldSpecified;
            }
            set
            {
                this.nmPrfxFieldSpecified = value;
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
        public string PhneNb
        {
            get
            {
                return this.phneNbField;
            }
            set
            {
                this.phneNbField = value;
            }
        }

        /// <comentarios/>
        public string MobNb
        {
            get
            {
                return this.mobNbField;
            }
            set
            {
                this.mobNbField = value;
            }
        }

        /// <comentarios/>
        public string FaxNb
        {
            get
            {
                return this.faxNbField;
            }
            set
            {
                this.faxNbField = value;
            }
        }

        /// <comentarios/>
        public string EmailAdr
        {
            get
            {
                return this.emailAdrField;
            }
            set
            {
                this.emailAdrField = value;
            }
        }

        /// <comentarios/>
        public string Othr
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
