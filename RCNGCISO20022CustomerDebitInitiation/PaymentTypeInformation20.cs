namespace RCNGCISO20022CustomerDebitInitiation
{
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")]
    public class PaymentTypeInformation20
    {
        private Priority2Code instrPrtyField;
        private bool instrPrtyFieldSpecified;
        private ServiceLevel8Choice svcLvlField;
        private LocalInstrument2Choice lclInstrmField;
        private SequenceType1Code seqTpField;
        private bool seqTpFieldSpecified;
        private CategoryPurpose1Choice ctgyPurpField;

        //Parameterless constructor for Serialization purpose
        private PaymentTypeInformation20() { }

        public PaymentTypeInformation20(
            Priority2Code instructionPriority,
            bool instructionPrioritySpecified,
            ServiceLevel8Choice serviceLevel,
            LocalInstrument2Choice localInstrument,
            SequenceType1Code sequenceType,
            bool sequenceTypeSpecified,
            CategoryPurpose1Choice categoryPurpose)
        {
            this.instrPrtyField = instructionPriority;
            this.instrPrtyFieldSpecified = instructionPrioritySpecified;
            this.svcLvlField = serviceLevel;
            this.lclInstrmField = localInstrument;
            this.seqTpField = sequenceType;
            this.seqTpFieldSpecified = sequenceTypeSpecified;
            this.ctgyPurpField = categoryPurpose;
        }

        /// <comentarios/>
        public Priority2Code InstrPrty
        {
            get
            {
                return this.instrPrtyField;
            }
            set
            {
                this.instrPrtyField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool InstrPrtySpecified
        {
            get
            {
                return this.instrPrtyFieldSpecified;
            }
            set
            {
                this.instrPrtyFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public ServiceLevel8Choice SvcLvl
        {
            get
            {
                return this.svcLvlField;
            }
            set
            {
                this.svcLvlField = value;
            }
        }

        /// <comentarios/>
        public LocalInstrument2Choice LclInstrm
        {
            get
            {
                return this.lclInstrmField;
            }
            set
            {
                this.lclInstrmField = value;
            }
        }

        /// <comentarios/>
        public SequenceType1Code SeqTp
        {
            get
            {
                return this.seqTpField;
            }
            set
            {
                this.seqTpField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SeqTpSpecified
        {
            get
            {
                return this.seqTpFieldSpecified;
            }
            set
            {
                this.seqTpFieldSpecified = value;
            }
        }

        /// <comentarios/>
        public CategoryPurpose1Choice CtgyPurp
        {
            get
            {
                return this.ctgyPurpField;
            }
            set
            {
                this.ctgyPurpField = value;
            }
        }
    }
}
