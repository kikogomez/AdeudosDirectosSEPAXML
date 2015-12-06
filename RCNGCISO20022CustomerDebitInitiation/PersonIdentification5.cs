namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class PersonIdentification5
    {
        private DateAndPlaceOfBirth dtAndPlcOfBirthField;
        private GenericPersonIdentification1[] othrField;

        //Parameterless constructor for Serialization purpose
        private PersonIdentification5() { }

        public PersonIdentification5(
            DateAndPlaceOfBirth dateAndPlaceOfBirth,
            GenericPersonIdentification1[] otherGenericPersonIdentification)
        {
            this.dtAndPlcOfBirthField = dateAndPlaceOfBirth;
            this.othrField = (GenericPersonIdentification1[])otherGenericPersonIdentification.Clone();
        }

        /// <comentarios/>
        public DateAndPlaceOfBirth DtAndPlcOfBirth
        {
            get
            {
                return this.dtAndPlcOfBirthField;
            }
            set
            {
                this.dtAndPlcOfBirthField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("Othr")]
        public GenericPersonIdentification1[] Othr
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
