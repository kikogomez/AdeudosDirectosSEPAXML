using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISO20022PaymentInitiations.SchemaSerializableClasses.PaymentStatusReport
{
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:iso:std:iso:20022:tech:xsd:pain.002.001.03")]
    public partial class PaymentTypeInformation22
    {

        private Priority2Code instrPrtyField;

        private bool instrPrtyFieldSpecified;

        private ClearingChannel2Code clrChanlField;

        private bool clrChanlFieldSpecified;

        private ServiceLevel8Choice svcLvlField;

        private LocalInstrument2Choice lclInstrmField;

        private SequenceType1Code seqTpField;

        private bool seqTpFieldSpecified;

        private CategoryPurpose1Choice ctgyPurpField;

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
        public ClearingChannel2Code ClrChanl
        {
            get
            {
                return this.clrChanlField;
            }
            set
            {
                this.clrChanlField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ClrChanlSpecified
        {
            get
            {
                return this.clrChanlFieldSpecified;
            }
            set
            {
                this.clrChanlFieldSpecified = value;
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
