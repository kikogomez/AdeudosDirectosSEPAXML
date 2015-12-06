namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class DateAndPlaceOfBirth
    {
        private System.DateTime birthDtField;
        private string prvcOfBirthField;
        private string cityOfBirthField;
        private string ctryOfBirthField;

        //Parameterless constructor for Serialization purpose
        private DateAndPlaceOfBirth() { }

        public DateAndPlaceOfBirth(
            System.DateTime birthDate,
            string provinceOfBirth,
            string cityOfBirth,
            string countryOfBirth)
        {
            this.birthDtField = birthDate;
            this.prvcOfBirthField = provinceOfBirth;
            this.cityOfBirthField = cityOfBirth;
            this.ctryOfBirthField = countryOfBirth;
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime BirthDt
        {
            get
            {
                return this.birthDtField;
            }
            set
            {
                this.birthDtField = value;
            }
        }

        /// <comentarios/>
        public string PrvcOfBirth
        {
            get
            {
                return this.prvcOfBirthField;
            }
            set
            {
                this.prvcOfBirthField = value;
            }
        }

        /// <comentarios/>
        public string CityOfBirth
        {
            get
            {
                return this.cityOfBirthField;
            }
            set
            {
                this.cityOfBirthField = value;
            }
        }

        /// <comentarios/>
        public string CtryOfBirth
        {
            get
            {
                return this.ctryOfBirthField;
            }
            set
            {
                this.ctryOfBirthField = value;
            }
        }
    }
}
